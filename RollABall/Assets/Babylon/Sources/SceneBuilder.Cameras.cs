using System;
using System.IO;
using System.Collections.Generic;
using BabylonExport.Entities;
using UnityEngine;
using UnityEditor;

namespace Unity3D2Babylon
{
    partial class SceneBuilder
    {
        private void ConvertUnityCameraToBabylon(Camera camera, GameObject gameObject, float progress, ref UnityMetaData metaData, ref List<UnityFlareSystem> lensFlares, ref string componentTags)
        {
            ExporterWindow.ReportProgress(progress, "Exporting camera: " + camera.name);

            BabylonUniversalCamera babylonCamera = new BabylonUniversalCamera
            {
                name = camera.name,
                id = GetID(camera.gameObject),
                fov = camera.fieldOfView * (float)Math.PI / 180,
                minZ = camera.nearClipPlane,
                maxZ = camera.farClipPlane,
                parentId = GetParentID(camera.transform),
                position = camera.transform.localPosition.ToFloat()
            };
            
            if (camera.transform.parent != null) {
                babylonCamera.rotation = new float[3];
                babylonCamera.rotation[0] = camera.transform.localRotation.eulerAngles.x * (float)Math.PI / 180;
                babylonCamera.rotation[1] = camera.transform.localRotation.eulerAngles.y * (float)Math.PI / 180;
                babylonCamera.rotation[2] = camera.transform.localRotation.eulerAngles.z * (float)Math.PI / 180;
            } else {
                var target = new Vector3(0, 0, 1);
                var transformedTarget = camera.transform.TransformDirection(target);
                babylonCamera.target = (camera.transform.position + transformedTarget).ToFloat();
            }
            
            if (camera.orthographic)
            {
                babylonCamera.tags += " [ORTHOGRAPHIC]";
                babylonCamera.mode = 1;
            }
            else
            {
                babylonCamera.mode = 0;
            }

            bool preventDefault = false;
            bool virtualJoystick = false;

            string followTarget = null;
            float followRadius = 20.0f;
            float followHeightOffset = 4.0f;
            float followRotationOffset = 0.0f;
            float followCameraAcceleration = 0.5f;
            float followMaxCameraSpeed = 50.0f;

            float arcRotateAlpha = (float)Math.PI / 2.0f;
            float arcRotateBeta = (float)Math.PI / 4.0f;
            float arcRotateRadius = 3.0f;
            float[] arcRotateTarget = new float[] { 0.0f, 1.0f, 0.0f };
            float arcRotateLowerRadiusLimit = 1;
            float arcRotateUpperRadiusLimit = 10;
            float[] arcRotateCollisionRadius = new float[] { 1.0f, 1.0f, 1.0f };
            float arcRotateWheelDeltaPercentage = 0.01f;
            
            bool enableVirtualReality = false;
            bool displayGazeIcon = false;
            bool displayLaserBeam = true;
            bool enableInteractions = false;
            bool enableTeleportation = false;
            bool useControllerPrefabs = false;
            int initialController = 0;
            string leftControllerPrefab = "LeftController";
            string rightControllerPrefab = "RightController";
            bool deviceOrientationCamera = false;
            bool fallbackFreeCamera = true;
            bool useCustomButton = false;
            string buttonElementID = null;
            float rayCastLength = 100f;
            float defaultHeight = 1.7f;
            float positionScale = 1.0f;
            string floorMeshesTags = "Navigation";

            bool stereoSideBySide = false;
            int cameraRigInput = 0;
            float cameraMoveSpeed = 1.0f;
            float cameraRotateSpeed = 0.005f;
            string cameraRigType = "UniversalCamera";
            bool localMultiPlayer = false;
            bool cameraCollisions = true;
            bool useMovementKeys = true;
            bool applyCamerGravity = true;
            var rigger = gameObject.GetComponent<CameraRig>();
            if (rigger != null && rigger.isActiveAndEnabled) {
                localMultiPlayer = (rigger.cameraType == BabylonCameraOptions.LocalMultiPlayerViewCamera);
                cameraRigType = (localMultiPlayer == true) ? "UniversalCamera" : rigger.cameraType.ToString();
                cameraRigInput = (int)rigger.cameraInput;
                cameraMoveSpeed = rigger.inputMoveSpeed;
                cameraRotateSpeed = rigger.inputRotateSpeed;
                babylonCamera.speed = rigger.cameraSpeed;   
                babylonCamera.inertia = rigger.inertiaScaleFactor;
                babylonCamera.interaxialDistance = rigger.interaxialDistance;     
                preventDefault = rigger.preventDefaultEvents;
                stereoSideBySide = rigger.stereoSideBySide;
                virtualJoystick = (rigger.cameraType == BabylonCameraOptions.VirtualJoysticksCamera);
                cameraCollisions = rigger.checkCameraCollision;
                useMovementKeys = rigger.useMovementKeys;
                applyCamerGravity = rigger.applyCameraGravity;

                if (rigger.followCameraOptions.target != null) followTarget = GetID(rigger.followCameraOptions.target);
                followRadius = rigger.followCameraOptions.radius;
                followHeightOffset = rigger.followCameraOptions.heightOffset;
                followRotationOffset = rigger.followCameraOptions.rotationOffset;
                followCameraAcceleration = rigger.followCameraOptions.cameraAcceleration;
                followMaxCameraSpeed = rigger.followCameraOptions.maxCameraSpeed;

                arcRotateAlpha = rigger.arcRotateCameraOptions.rotateAlpha;
                arcRotateBeta = rigger.arcRotateCameraOptions.rotateBeta;
                arcRotateRadius = rigger.arcRotateCameraOptions.rotateRadius;
                arcRotateTarget = rigger.arcRotateCameraOptions.rotateTarget.ToFloat();
                arcRotateLowerRadiusLimit = rigger.arcRotateCameraOptions.lowerRadiusLimit;
                arcRotateUpperRadiusLimit = rigger.arcRotateCameraOptions.upperRadiusLimit;
                arcRotateCollisionRadius = rigger.arcRotateCameraOptions.collisionRadius.ToFloat();
                arcRotateWheelDeltaPercentage = rigger.arcRotateCameraOptions.wheelDeltaPercentage;

                enableVirtualReality = rigger.virtualRealityWebPlatform.experienceHelper;
                displayGazeIcon = rigger.virtualRealityWebPlatform.displayGazeIcon;
                displayLaserBeam = rigger.virtualRealityWebPlatform.displayLaserBeam;
                enableInteractions = rigger.virtualRealityWebPlatform.enableInteractions;
                enableTeleportation = rigger.virtualRealityWebPlatform.enableTeleportation;
                useControllerPrefabs = rigger.virtualRealityWebPlatform.customControllerPrefabs.enablePrefabs;
                initialController = (int)rigger.virtualRealityWebPlatform.customControllerPrefabs.initialController;
                leftControllerPrefab = rigger.virtualRealityWebPlatform.customControllerPrefabs.leftPrefabName;
                rightControllerPrefab = rigger.virtualRealityWebPlatform.customControllerPrefabs.rightPrefabName;
                deviceOrientationCamera = (rigger.virtualRealityWebPlatform.experienceHelperOptions.defaultCamera == VirtualRealityCamera.DeviceOrientationCamera);
                fallbackFreeCamera = rigger.virtualRealityWebPlatform.experienceHelperOptions.fallbackCamera;
                useCustomButton = rigger.virtualRealityWebPlatform.experienceHelperOptions.useCustomButton;
                buttonElementID = rigger.virtualRealityWebPlatform.experienceHelperOptions.buttonElementID;
                rayCastLength = rigger.virtualRealityWebPlatform.experienceHelperOptions.rayCastLength;
                defaultHeight = rigger.virtualRealityWebPlatform.experienceHelperOptions.defaultHeight;
                positionScale = rigger.virtualRealityWebPlatform.experienceHelperOptions.positionScale;
                floorMeshesTags = rigger.virtualRealityWebPlatform.experienceHelperOptions.floorMeshesTags;
            }
            SceneBuilder.Metadata.properties["virtualJoystickAttached"] = virtualJoystick;

            metaData.type = "Camera";
            metaData.properties.Add("mainCamera", (Camera.main == camera));
            metaData.properties.Add("cameraType", cameraRigType);
            metaData.properties.Add("cameraInput", cameraRigInput);
            metaData.properties.Add("clearFlags", camera.clearFlags.ToString());
            metaData.properties.Add("clearColor", babylonScene.clearColor);
            metaData.properties.Add("cullingMask", camera.cullingMask);
            metaData.properties.Add("movementKeys", useMovementKeys);
            metaData.properties.Add("isOrthographic", camera.orthographic);
            metaData.properties.Add("orthographicSize", camera.orthographicSize);
            metaData.properties.Add("cameraMoveSpeed", cameraMoveSpeed);
            metaData.properties.Add("cameraRotateSpeed", cameraRotateSpeed);
            metaData.properties.Add("useOcclusionCulling", camera.useOcclusionCulling);
            metaData.properties.Add("preventDefaultEvents", preventDefault);
            metaData.properties.Add("stereoscopicSideBySide", stereoSideBySide);
            metaData.properties.Add("localMultiPlayerViewCamera", localMultiPlayer);

            metaData.properties.Add("followTarget", followTarget);
            metaData.properties.Add("followRadius", followRadius);
            metaData.properties.Add("followHeightOffset", followHeightOffset);
            metaData.properties.Add("followRotationOffset", followRotationOffset);
            metaData.properties.Add("followCameraAcceleration", followCameraAcceleration);
            metaData.properties.Add("followMaxCameraSpeed", followMaxCameraSpeed);

            metaData.properties.Add("arcRotateAlpha", arcRotateAlpha);
            metaData.properties.Add("arcRotateBeta", arcRotateBeta);
            metaData.properties.Add("arcRotateRadius", arcRotateRadius);
            metaData.properties.Add("arcRotateTarget", arcRotateTarget);
            metaData.properties.Add("arcRotateLowerRadiusLimit", arcRotateLowerRadiusLimit);
            metaData.properties.Add("arcRotateUpperRadiusLimit", arcRotateUpperRadiusLimit);
            metaData.properties.Add("arcRotateCollisionRadius", arcRotateCollisionRadius);
            metaData.properties.Add("arcRotateWheelDeltaPercentage", arcRotateWheelDeltaPercentage);

            metaData.properties.Add("webvrUniversalCamera", enableVirtualReality);
            metaData.properties.Add("webvrDisplayGazeIcon", displayGazeIcon);
            metaData.properties.Add("webvrDisplayLaserBeam", displayLaserBeam);
            metaData.properties.Add("webvrEnableInteractions", enableInteractions);
            metaData.properties.Add("webvrEnableTeleportation", enableTeleportation);
            metaData.properties.Add("webvrControllerPrefabs", useControllerPrefabs);
            metaData.properties.Add("webvrInitialController", initialController);
            metaData.properties.Add("webvrLeftControllerPrefab", leftControllerPrefab);
            metaData.properties.Add("webvrRightControllerPrefab", rightControllerPrefab);
            metaData.properties.Add("webvrDeviceOrientationCamera", deviceOrientationCamera);
            metaData.properties.Add("webvrFallbackFreeCamera", fallbackFreeCamera);
            metaData.properties.Add("webvrUseCustomButton", useCustomButton);
            metaData.properties.Add("webvrButtonElementID", buttonElementID);
            metaData.properties.Add("webvrRayCastLength", rayCastLength);
            metaData.properties.Add("webvrDefaultHeight", defaultHeight);
            metaData.properties.Add("webvrPositionScale", positionScale);
            metaData.properties.Add("webvrFloorMeshesTags", floorMeshesTags);

            babylonCamera.isStereoscopicSideBySide = stereoSideBySide;
            babylonCamera.applyGravity = applyCamerGravity;
            babylonCamera.type = cameraRigType;   
            babylonCamera.tags = componentTags;

            // Animations
            ExportTransformAnimationClips(camera.transform, babylonCamera, ref metaData);

            // Tagging
            if (!String.IsNullOrEmpty(babylonCamera.tags))
            {
                babylonCamera.tags = babylonCamera.tags.Trim();
            }

            babylonCamera.metadata = metaData;
            babylonScene.CamerasList.Add(babylonCamera);

            if (Camera.main == camera)
            {
                babylonScene.activeCameraID = babylonCamera.id;
            }

            // Collisions
            if (exportationOptions.ExportCollisions)
            {
                if (camera.transform.parent != null) {
                    babylonCamera.checkCollisions = false;
                } else {
                    babylonCamera.checkCollisions = cameraCollisions;
                }
            }

            // Lens Flares
            ParseLensFlares(gameObject, babylonCamera.id, ref lensFlares);

            // Greavity Ellispoid
            if (SceneController != null)
            {
                babylonCamera.ellipsoid = SceneController.sceneOptions.defaultEllipsoid.ToFloat();
            }

            // Particles Systems
            if (!exportationOptions.ExportMetadata) babylonCamera.metadata = null;
        }

        private void ExportMainCameraSkyboxToBabylon()
        {
            if (RenderSettings.sun != null) {
                var direction = new Vector3(0, 0, 1);
                var transformedDirection = RenderSettings.sun.transform.TransformDirection(direction);
                SceneBuilder.SunlightDirection = transformedDirection.ToFloat();
                SceneBuilder.SunlightIndentifier = GetID(RenderSettings.sun.gameObject);
            }
            if (Camera.main != null) {
                babylonScene.clearColor = Camera.main.backgroundColor.ToFloat(1.0f);
                if ((Camera.main.clearFlags & CameraClearFlags.Skybox) == CameraClearFlags.Skybox)
                {
                    if (RenderSettings.skybox != null)
                    {
                        bool dds = false;
                        BabylonTexture skytex = null;
                        if (RenderSettings.skybox.shader.name == "Skybox/Cubemap")
                        {
                            skytex = new BabylonCubeTexture();
                            skytex.name = String.Format("{0}_Skybox", SceneName);
                            skytex.coordinatesMode = 5;
                            Cubemap cubeMap = RenderSettings.skybox.GetTexture("_Tex") as Cubemap;
                            if (cubeMap != null) {
                                var srcTexturePath = AssetDatabase.GetAssetPath(cubeMap);
                                var srcTextureExt = Path.GetExtension(srcTexturePath);
                                if (srcTextureExt.Equals(".dds", StringComparison.OrdinalIgnoreCase)) {
                                    ExporterWindow.ReportProgress(1, "Exporting skybox direct draw surface... This may take a while.");
                                    // ..
                                    // Export Draw Surface Skybox Textures
                                    // ..
                                    dds = true;
                                    skytex.name += ".dds";
                                    skytex.extensions = null;
                                    ((BabylonCubeTexture)skytex).prefiltered = true;
                                    CopyCubemapTexture(skytex.name, cubeMap, skytex);
                                } else {
                                    ExporterWindow.ReportProgress(1, "Baking skybox environment textures... This may take a while.");
                                    var imageFormat = (BabylonImageFormat)ExporterWindow.exportationOptions.ImageEncodingOptions;
                                    // ..
                                    // Export Tone Mapped Cubemap To 6-Sided Skybox Textures
                                    // ..
                                    bool jpeg = (imageFormat == BabylonImageFormat.JPG);
                                    string faceTextureExt = (jpeg) ? ".jpg" : ".png";
                                    string frontTextureExt = "_pz" + faceTextureExt;
                                    string backTextureExt = "_nz" + faceTextureExt;
                                    string leftTextureExt = "_px" + faceTextureExt;
                                    string rightTextureExt = "_nx" + faceTextureExt;
                                    string upTextureExt = "_py" + faceTextureExt;
                                    string downTextureExt = "_ny" + faceTextureExt;
                                    skytex.extensions = new string[] { leftTextureExt, upTextureExt, frontTextureExt, rightTextureExt, downTextureExt, backTextureExt };
                                    Tools.SetTextureWrapMode(skytex, cubeMap);
                                    var outputFile = Path.Combine(babylonScene.OutputPath, skytex.name + faceTextureExt);
                                    var splitterOpts = new BabylonSplitterOptions();
                                    Tools.ExportSkybox(cubeMap, outputFile, splitterOpts, imageFormat);
                                }
                            }
                        }
                        else if (RenderSettings.skybox.shader.name == "Skybox/6 Sided" || RenderSettings.skybox.shader.name == "Mobile/Skybox")
                        {
                            skytex = new BabylonCubeTexture();
                            skytex.name = String.Format("{0}_Skybox", SceneName);
                            skytex.coordinatesMode = 5;
                            // ..
                            // 6-Sided Skybox Textures (Tone Mapped Image Formats Only)
                            // ..
                            var frontTexture = RenderSettings.skybox.GetTexture("_FrontTex") as Texture2D;
                            var backTexture = RenderSettings.skybox.GetTexture("_BackTex") as Texture2D;
                            var leftTexture = RenderSettings.skybox.GetTexture("_LeftTex") as Texture2D;
                            var rightTexture = RenderSettings.skybox.GetTexture("_RightTex") as Texture2D;
                            var upTexture = RenderSettings.skybox.GetTexture("_UpTex") as Texture2D;
                            var downTexture = RenderSettings.skybox.GetTexture("_DownTex") as Texture2D;
                            DumpSkyboxTextures( ref skytex, ref frontTexture, ref backTexture, ref leftTexture, ref rightTexture, ref upTexture, ref downTexture);
                        }
                        else if (RenderSettings.skybox.name.Equals("Default-Skybox"))
                        {
                            skytex = new BabylonCubeTexture();
                            skytex.name = String.Format("{0}_Skybox", SceneName);
                            skytex.coordinatesMode = 5;
                            // ..
                            // 6-Sided Skybox Textures (Toolkit Skybox Template Images)
                            // ..
                            string skyboxPath = "Assets/Babylon/Template/Skybox/"; 
                            var frontTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_pz.png");
                            var backTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_nz.png");
                            var leftTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_px.png");
                            var rightTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_nx.png");
                            var upTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_py.png");
                            var downTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(skyboxPath + "DefaultSkybox_ny.png");
                            DumpSkyboxTextures( ref skytex, ref frontTexture, ref backTexture, ref leftTexture, ref rightTexture, ref upTexture, ref downTexture);
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("SKYBOX: " + RenderSettings.skybox.shader.name + " shader type is unsupported. Skybox and reflections will be disabled.");
                        }
                        if (skytex != null) {
                            float size = (SceneController != null) ? SceneController.skyboxOptions.skyboxMeshSize : 1000;
                            string tags = (SceneController != null) ? SceneController.skyboxOptions.skyboxMeshTags : String.Empty;
                            // ..
                            // PBR Skybox Material Support
                            // ..
                            bool pbr = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.physicalBased : false;
                            float pbr_ms = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.microSurface : 1.0f;
                            float pbr_cc = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.cameraContrast : 1.0f;
                            float pbr_ce = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.cameraExposure : 1.0f;
                            float pbr_di = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.directIntensity : 1.0f;
                            float pbr_ei = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.emissiveIntensity : 1.0f;
                            float pbr_si = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.specularIntensity : 1.0f;
                            float pbr_ri = (SceneController != null) ? SceneController.skyboxOptions.directDrawSurface.environmentIntensity : 1.0f;
                            var skybox = new BabylonMesh();
                            skybox.id = Guid.NewGuid().ToString();
                            skybox.infiniteDistance = true;
                            skybox.numBoneInfluencers = Tools.GetMaxBoneInfluencers();
                            if (!String.IsNullOrEmpty(tags)) {
                                skybox.tags = tags;
                            }
                            skybox.name = "SceneSkyboxMesh";
                            Mesh boxMesh = Tools.CreateBoxMesh(size, size, size);
                            Tools.GenerateBabylonMeshData(boxMesh, skybox);
                            BabylonMaterial skyboxMaterial = null;
                            if (dds == true && pbr == true) {
                                var skyboxMaterialPbr = new BabylonSystemMaterial {
                                    name = "SceneSkyboxMaterial",
                                    id = Guid.NewGuid().ToString(),
                                    backFaceCulling = false,
                                    disableLighting = true,
                                    albedo = Color.white.ToFloat(),
                                    ambient = Color.black.ToFloat(),
                                    emissive = Color.black.ToFloat(),
                                    metallic = null,
                                    roughness = null,
                                    sideOrientation = 1,
                                    reflectivity = Color.white.ToFloat(),
                                    reflection = Color.white.ToFloat(),
                                    microSurface = pbr_ms,
                                    cameraContrast = pbr_cc,
                                    cameraExposure = pbr_ce,
                                    directIntensity = pbr_di,
                                    emissiveIntensity = pbr_ei,
                                    specularIntensity = pbr_si,
                                    environmentIntensity = pbr_ri,
                                    maxSimultaneousLights = 4,
                                    useSpecularOverAlpha = false,
                                    useRadianceOverAlpha = false,
                                    usePhysicalLightFalloff = false,
                                    useAlphaFromAlbedoTexture = false,
                                    useEmissiveAsIllumination = false,
                                    reflectionTexture = skytex
                                };
                                skyboxMaterial = skyboxMaterialPbr;
                            } else {
                                var skyboxMaterialStd = new BabylonDefaultMaterial {
                                    name = "SceneSkyboxMaterial",
                                    id = Guid.NewGuid().ToString(),
                                    backFaceCulling = false,
                                    disableLighting = true,
                                    diffuse = Color.black.ToFloat(),
                                    specular = Color.black.ToFloat(),
                                    ambient = Color.clear.ToFloat(),
                                    reflectionTexture = skytex
                                };
                                skyboxMaterial = skyboxMaterialStd;
                            }
                            if (skyboxMaterial != null) {
                                skybox.materialId = skyboxMaterial.id;
                                babylonScene.MeshesList.Add(skybox);
                                babylonScene.MaterialsList.Add(skyboxMaterial);
                                babylonScene.AddTextureCube("SceneSkyboxMaterial");
                            }
                        }
                    }
                }
            }
        }

        private void DumpSkyboxTextures(ref BabylonTexture skytex, ref Texture2D frontTexture, ref Texture2D backTexture, ref Texture2D leftTexture, ref Texture2D rightTexture, ref Texture2D upTexture, ref Texture2D downTexture)
        {
            if (frontTexture != null && backTexture != null && leftTexture != null && rightTexture != null && upTexture != null && downTexture != null)
            {
                ExporterWindow.ReportProgress(1, "Exporting skybox environment textures... This may take a while.");
                string frontTextureExt = "_pz.jpg";
                string backTextureExt = "_nz.jpg";
                string leftTextureExt = "_px.jpg";
                string rightTextureExt = "_nx.jpg";
                string upTextureExt = "_py.jpg";
                string downTextureExt = "_ny.jpg";
                Tools.SetTextureWrapMode(skytex, frontTexture);
                var faceTextureFile = AssetDatabase.GetAssetPath(frontTexture);
                var faceTextureExt = Path.GetExtension(faceTextureFile);
                var faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    frontTextureExt = "_pz" + faceTextureExt;
                    var frontTextureName = String.Format("{0}_pz{1}", skytex.name, faceTextureExt);
                    var frontTexturePath = Path.Combine(babylonScene.OutputPath, frontTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(frontTexturePath, frontTextureName, frontTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                faceTextureFile = AssetDatabase.GetAssetPath(backTexture);
                faceTextureExt = Path.GetExtension(faceTextureFile);
                faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    backTextureExt = "_nz" + faceTextureExt;
                    var backTextureName = String.Format("{0}_nz{1}", skytex.name, faceTextureExt);
                    var backTexturePath = Path.Combine(babylonScene.OutputPath, backTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(backTexturePath, backTextureName, backTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                faceTextureFile = AssetDatabase.GetAssetPath(leftTexture);
                faceTextureExt = Path.GetExtension(faceTextureFile);
                faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    leftTextureExt = "_px" + faceTextureExt;
                    var leftTextureName = String.Format("{0}_px{1}", skytex.name, faceTextureExt);
                    var leftTexturePath = Path.Combine(babylonScene.OutputPath, leftTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(leftTexturePath, leftTextureName, leftTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                faceTextureFile = AssetDatabase.GetAssetPath(rightTexture);
                faceTextureExt = Path.GetExtension(faceTextureFile);
                faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    rightTextureExt = "_nx" + faceTextureExt;
                    var rightTextureName = String.Format("{0}_nx{1}", skytex.name, faceTextureExt);
                    var rightTexturePath = Path.Combine(babylonScene.OutputPath, rightTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(rightTexturePath, rightTextureName, rightTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                faceTextureFile = AssetDatabase.GetAssetPath(upTexture);
                faceTextureExt = Path.GetExtension(faceTextureFile);
                faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    upTextureExt = "_py" + faceTextureExt;
                    var upTextureName = String.Format("{0}_py{1}", skytex.name, faceTextureExt);
                    var upTexturePath = Path.Combine(babylonScene.OutputPath, upTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(upTexturePath, upTextureName, upTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                faceTextureFile = AssetDatabase.GetAssetPath(downTexture);
                faceTextureExt = Path.GetExtension(faceTextureFile);
                faceImportTool = new BabylonTextureImporter(faceTextureFile);
                if (faceTextureExt.Equals(".png", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || faceTextureExt.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                    downTextureExt = "_ny" + faceTextureExt;
                    var downTextureName = String.Format("{0}_ny{1}", skytex.name, faceTextureExt);
                    var downTexturePath = Path.Combine(babylonScene.OutputPath, downTextureName);
                    faceImportTool.SetReadable();
                    CopyTextureFace(downTexturePath, downTexturePath, downTexture);
                } else {
                    UnityEngine.Debug.LogWarning("SKYBOX: Unsupported cube face texture type of " + faceTextureExt + " for " + Path.GetFileName(faceTextureFile));
                }
                skytex.extensions = new string[] { leftTextureExt, upTextureExt, frontTextureExt, rightTextureExt, downTextureExt, backTextureExt };
            }
        }
    }
}
