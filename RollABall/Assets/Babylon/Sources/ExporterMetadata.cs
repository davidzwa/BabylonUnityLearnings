using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using BabylonExport.Entities;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Object = UnityEngine.Object;
using Unity3D2Babylon;

namespace Unity3D2Babylon
{
    #region Unity Components
    [DataContract, Serializable]
    public class UnityNameString
    {
        [DataMember]
        public string name;

        [DataMember]
        public string value;
    }
    
    [DataContract, Serializable]
    public class UnityScriptFile
    {
        [DataMember]
        public int order;

        [DataMember]
        public string name;

        [DataMember]
        public string script;
    }

    [DataContract, Serializable]
    public class UnityFlareSystem
    {
        [DataMember]
        public string name;

        [DataMember]
        public string emitterId;

        [DataMember]
        public int borderLimit;

        [DataMember]
        public UnityFlareItem[] lensFlares;
    }

    [DataContract, Serializable]
    public class UnityFlareItem
    {
        [DataMember]
        public float size;

        [DataMember]
        public float position;

        [DataMember]
        public float[] color;

        [DataMember]
        public string textureName;
    }

    [DataContract, Serializable]
    public class UnityScriptComponent
    {
        [DataMember]
        public int order;

        [DataMember]
        public string name;

        [DataMember]
        public string klass;

        [DataMember]
        public bool update;

        [DataMember]
        public Dictionary<string, object> properties;

        [DataMember]
        public object instance;

        [DataMember]
        public object tag;

        public UnityScriptComponent()
        {
            this.order = 0;
            this.name = String.Empty;
            this.klass = String.Empty;
            this.update = true;
            this.properties = new Dictionary<string, object>();
            this.instance = null;
            this.tag = null;
        }
    }
    #endregion

    #region Read Only Attribute
    public class ReadOnlyAttribute : PropertyAttribute { }
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    #endregion
}

namespace UnityEditor
{
    [DataContract, Serializable]
    public class SocketData
    {
        [DataMember]
        public int boneIndex;
        [DataMember]
        public string boneName;
        [DataMember]
        public object socketMesh;
        [DataMember]
        public float positionX;
        [DataMember]
        public float positionY;
        [DataMember]
        public float positionZ;
        [DataMember]
        public float rotationX;
        [DataMember]
        public float rotationY;
        [DataMember]
        public float rotationZ;
    }

    [DataContract, Serializable]
    public class EmbeddedAsset
    {
        [DataMember]
        public BabylonTextEncoding encoding = BabylonTextEncoding.RawBytes;
        [DataMember]
        public TextAsset textAsset;
    }

    [DataContract, Serializable]
    public class SceneMetaData
    {
        [DataMember]
        public bool api;
        [DataMember]
        public bool parsed;
        [DataMember]
        public List<string> imports;
        [DataMember]
        public Dictionary<string, object> properties;

        public SceneMetaData()
        {
            this.api = true;
            this.parsed = false;
            this.imports = new List<string>();
            this.properties = new Dictionary<string, object>();
        }
    }

    [DataContract, Serializable]
    public class UnityMetaData
    {
        [DataMember]
        public bool api;
        [DataMember]
        public string type;
        [DataMember]
        public bool parsed;
        [DataMember]
        public bool prefab;
        [DataMember]
        public object state;
        [DataMember]
        public string objectName;
        [DataMember]
        public string objectId;
        [DataMember]
        public string tagName;
        [DataMember]
        public int layerIndex;
        [DataMember]
        public string layerName;
        [DataMember]
        public int areaIndex;
        [DataMember]
        public object navAgent;
        [DataMember]
        public object meshLink;
        [DataMember]
        public object meshObstacle;
        [DataMember]
        public int shadowCastingMode;
        [DataMember]
        public List<SocketData> socketList;
        [DataMember]
        public List<object> animationClips;
        [DataMember]
        public List<object> animationEvents;
        [DataMember]
        public object collisionEvent;
        [DataMember]
        public List<object> components;
        [DataMember]
        public Dictionary<string, object> properties;

        public UnityMetaData()
        {
            this.api = true;
            this.type = "Unity";
            this.parsed = false;
            this.prefab = false;
            this.state = new object();
            this.objectName = String.Empty;
            this.objectId = String.Empty;
            this.tagName = String.Empty;
            this.layerIndex = -1;
            this.layerName = String.Empty;
            this.areaIndex = -1;
            this.navAgent = null;
            this.meshLink = null;
            this.meshObstacle = null;
            this.shadowCastingMode = 0;
            this.socketList = new List<SocketData>();
            this.animationClips = new List<object>();
            this.animationEvents = new List<object>();
            this.collisionEvent = null;
            this.components = new List<object>();
            this.properties = new Dictionary<string, object>();
        }
    }

    [DataContract, Serializable]
    public class BoneMetaData
    {
        [DataMember]
        public bool api;
        [DataMember]
        public string transformPath;

        public BoneMetaData()
        {
            this.transformPath = null;
        }
    }

    [DataContract, Serializable]
    public sealed class UnityLensFlareItem
    {
        [DataMember]
        public float size;
        [DataMember]
        public float position;
        [DataMember]
        public Color color = Color.white;
        [DataMember]
        public Texture2D texture;
    }

    [DataContract, Serializable]
    public class BabylonConfigOptions
    {
        [DataMember]
        public bool enableInput = true;
        [DataMember]
        public bool pointerLock = false;
        [DataMember]
        public bool captureInput = false;
        [DataMember]
        public bool preventDefault = false;
        [DataMember]
        public BabylonInputOptions inputProperties = null;
    }

    [DataContract, Serializable]
    public class BabylonInputOptions
    {
        [DataMember, Range(0.0f, 0.9f)]
        public float padDeadStick = 0.25f;
        [DataMember]
        public bool padLStickXInvert = false;
        [DataMember]
        public bool padLStickYInvert = false;
        [DataMember]
        public bool padRStickXInvert = false;
        [DataMember]
        public bool padRStickYInvert = false;
        [DataMember, Range(0.0f, 10.0f)]
        public float padLStickLevel = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float padRStickLevel = 1.0f;
        [DataMember, Range(0.0f, 5.0f)]
        public float wheelDeadZone = 0.1f;
        [DataMember, Range(0.0f, 10.0f)]
        public float mouseAngularLevel = 1.0f;
        
        [Header("-Virtual Joystick-")]

        [IgnoreDataMember]
        public BabylonJoystickOptions joystickInputMode = BabylonJoystickOptions.None;
        [DataMember]
        public bool disableRightStick = false;
        [DataMember, Range(0.0f, 0.9f)]
        public float joystickDeadStick = 0.1f;
        [DataMember, Range(0.0f, 10.0f)]
        public float joystickLeftLevel = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float joystickRightLevel = 1.0f;
        [IgnoreDataMember]
        public Color joystickRightColor = Color.yellow;

        // Hidden Properties

        [DataMember, HideInInspector]
        public string joystickRightColorText = null;
        [DataMember, HideInInspector]
        public int joystickInputValue = 0;
    }

    [DataContract, Serializable]
    public class BabylonSceneOptions
    {
        [DataMember]
        public bool clearCanvas = true;
        [DataMember]
        public Vector3 defaultGravity = new Vector3(0.0f, -9.81f, 0.0f);
        [DataMember]
        public Vector3 defaultEllipsoid = new Vector3(0.5f, 1.0f, 0.5f);
        [DataMember]
        public BabylonNavigationMesh navigationMesh = BabylonNavigationMesh.EnableNavigation;
        [DataMember]
        public bool resizeCameras = true;
        [DataMember]
        public SceneAsset[] importSceneMeshes = null;
    }

    [DataContract, Serializable]
    public class BabylonSkyboxOptions
    {
        [DataMember]
        public BabylonSkyboxType skyboxType = BabylonSkyboxType.UnitySkyboxEnvironment;
        [DataMember, Range(0.0f, 1.0f)]
        public float skyboxGradient = 0.5f;
        [DataMember]
        public string skyboxMeshTags = "WATER_TAG_0";
        [DataMember, Range(100, 2000)]
        public int skyboxMeshSize = 1000;
        [DataMember]
        public bool globalEnvironment = true;
        [DataMember]
        public Vector3 localCubemapPos = Vector3.zero;
        [DataMember]
        public Vector3 localCubemapBox = Vector3.zero;
        [DataMember]
        public bool enableReflections = true;
        [DataMember]
        public Color reflectionColor = Color.white;
        [DataMember]
        public BabylonSkyboxRendering directDrawSurface = null;
    }

    [DataContract, Serializable]
    public class BabylonLightOptions
    {
        [DataMember]
        public BabylonAmbientLighting ambientLight = BabylonAmbientLighting.UnityAmbientLighting;
        [DataMember, Range(0.0f, 10.0f)]
        public float ambientScale = 1.0f;
        [DataMember]
        public Color ambientSpecular = Color.black;
        [DataMember]
        public BabylonPhysicalRendering defaultShaderOptions = null;
    }

    [DataContract, Serializable]
    public class BabylonSkyboxRendering
    {
        [DataMember]
        public bool physicalBased = false;
        [DataMember, Range(0.0f, 1.0f)]
        public float microSurface = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float cameraContrast = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float cameraExposure = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float directIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float emissiveIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float specularIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float environmentIntensity = 1.0f;
    }

    [DataContract, Serializable]
    public class BabylonPhysicalRendering
    {
        [DataMember, Range(0.0f, 10.0f)]
        public float cameraContrast = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float cameraExposure = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float directIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float emissiveIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float specularIntensity = 1.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float environmentIntensity = 1.0f;
        [DataMember, Range(0.0f, 1.0f)]
        public float microSurfaceScaling = 1.0f;
        [DataMember, Range(1.0f, 25.0f)]
        public float specularHighlighting = 10.0f;
    }

    [DataContract, Serializable]
    public class BabylonArcRotateOptions
    {
        [DataMember, Range(0.0f, 10.0f)]
        public float rotateAlpha = (float)Math.PI / 2.0f;
        [DataMember, Range(0.0f, 10.0f)]
        public float rotateBeta = (float)Math.PI / 4.0f;
        [DataMember, Range(0.0f, 100.0f)]
        public float rotateRadius = 3.0f;
        [DataMember]
        public Vector3 rotateTarget = new Vector3(0.0f, 1.0f, 0.0f);
        [DataMember]
        public Vector3 collisionRadius = new Vector3(1.0f, 1.0f, 1.0f);
        [DataMember, Range(0.01f, 100.0f)]
        public float lowerRadiusLimit = 1;
        [DataMember, Range(0.1f, 1000.0f)]
        public float upperRadiusLimit = 10;
        [DataMember, Range(0.0f, 1.0f)]
        public float wheelDeltaPercentage = 0.01f;
    }

    [DataContract, Serializable]
    public class BabylonFollowOptions
    {
        [DataMember]
        public GameObject target = null;
        
        [DataMember, Range(0.0f, 100.0f)]
        public float radius = 20.0f;

        [DataMember, Range(0.0f, 100.0f)]
        public float heightOffset = 4.0f;

        [DataMember, Range(0.0f, 360.0f)]
        public float rotationOffset = 0.0f;

        [DataMember, Range(0.0f, 10.0f)]
        public float cameraAcceleration = 0.5f;

        [DataMember, Range(0.0f, 200.0f)]
        public float maxCameraSpeed = 50.0f;
    }

    [DataContract, Serializable]
    public class BabylonSoundTrack
    {
        [DataMember]
        public float volume = 1.0f;
        [DataMember]
        public float playbackRate = 1.0f;
        [DataMember]
        public bool autoplay = false;
        [DataMember]
        public bool loop = false;
        [DataMember]
        public int soundTrackId = -1;
        [DataMember]
        public bool spatialSound = false;
        [DataMember]
        public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        [DataMember]
        public float refDistance = 1.0f;
        [DataMember]
        public float rolloffFactor = 1.0f;
        [DataMember]
        public float maxDistance = 100.0f;
        [DataMember]
        public string distanceModel = "linear";
        [DataMember]
        public string panningModel = "equalpower";
        [DataMember]
        public bool isDirectional = false;
        [DataMember, Range(0.0f, 360.0f)]
        public float coneInnerAngle = 360.0f;
        [DataMember, Range(0.0f, 360.0f)]
        public float coneOuterAngle = 360.0f;
        [DataMember]
        public float coneOuterGain = 0.0f;
        [DataMember]
        public Vector3 directionToMesh = new Vector3(1.0f, 0.0f, 0.0f);
    }

    [DataContract, Serializable]
    public class BabylonManifestOptions
    {
        [DataMember]
        public bool exportManifest = false;
        [DataMember]
        public int manifestVersion = 1;
        [DataMember]
        public bool storeSceneOffline = false;
        [DataMember]
        public bool storeTextureOffline = false;
        [DataMember]
        public bool enableOfflineSupport = false;
    }

    [DataContract, Serializable]
    public class BabylonSplitterOptions
    {
        [DataMember]
        public bool progress = true;
        [DataMember]
        public bool bilinear = true;
        [DataMember]
        public int resolution = 0;
    }

    [DataContract, Serializable]
    public class BabylonVirtualReality
    {
        [DataMember]
        public bool experienceHelper = false;

        [DataMember]         
        public bool displayGazeIcon = false;

        [DataMember]         
        public bool displayLaserBeam = false;

        [DataMember]
        public bool enableInteractions = false;

        [DataMember]
        public bool enableTeleportation = false;

        [DataMember]         
        public BabylonVirtualRealityControllers customControllerPrefabs = null;

        public BabylonVirtualRealityOptions experienceHelperOptions = null;
    }

    [DataContract, Serializable]
    public class BabylonVirtualRealityOptions
    {
        [DataMember]         
        public VirtualRealityCamera defaultCamera = VirtualRealityCamera.PrimaryCameraRig;
        [DataMember]         
        public bool fallbackCamera = true;
        [DataMember]         
        public bool useCustomButton = false;
        [DataMember]         
        public string buttonElementID = null;
        [DataMember, Range(1.0f, 10000.0f)]         
        public float rayCastLength = 100f;
        [DataMember, Range(0.0f, 10.0f)]         
        public float defaultHeight = 1.7f;
        [DataMember, Range(0.0f, 10.0f)]         
        public float positionScale = 1.0f;
        [DataMember]         
        public string floorMeshesTags = "Navigation";
    }

    [DataContract, Serializable]
    public class BabylonVirtualRealityControllers
    {
        [DataMember]         
        public bool enablePrefabs = false;
        [DataMember]
        public VirtualRealityController initialController = VirtualRealityController.CustomControllers;
        [DataMember]         
        public string leftPrefabName = "LeftController";
        [DataMember]         
        public string rightPrefabName = "RightController";
    }

    [DataContract, Serializable]
    public class BabylonCollisionMask
    {
        [DataMember]
        public bool collisionGroup1 = true;
        [DataMember]
        public bool collisionGroup2 = false;
        [DataMember]
        public bool collisionGroup3 = false;
        [DataMember]
        public bool collisionGroup4 = false;
        [DataMember]
        public bool collisionGroup5 = false;
        [DataMember]
        public bool collisionGroup6 = false;
        [DataMember]
        public bool collisionGroup7 = false;
        [DataMember]
        public bool collisionGroup8 = false;
        [DataMember]
        public bool collisionGroup9 = false;
        [DataMember]
        public bool collisionGroup10 = false;
        [DataMember]
        public bool collisionGroup11 = false;
        [DataMember]
        public bool collisionGroup12 = false;
        [DataMember]
        public bool collisionGroup13 = false;
        [DataMember]
        public bool collisionGroup14 = false;
        [DataMember]
        public bool collisionGroup15 = false;
        [DataMember]
        public bool collisionGroup16 = false;
        [DataMember]
        public bool collisionGroup17 = false;
        [DataMember]
        public bool collisionGroup18 = false;
        [DataMember]
        public bool collisionGroup19 = false;
        [DataMember]
        public bool collisionGroup20 = false;
        [DataMember]
        public bool collisionGroup21 = false;
        [DataMember]
        public bool collisionGroup22 = false;
        [DataMember]
        public bool collisionGroup23 = false;
        [DataMember]
        public bool collisionGroup24 = false;
        [DataMember]
        public bool collisionGroup25 = false;
        [DataMember]
        public bool collisionGroup26 = false;
        [DataMember]
        public bool collisionGroup27 = false;
        [DataMember]
        public bool collisionGroup28 = false;
        [DataMember]
        public bool collisionGroup29 = false;
        [DataMember]
        public bool collisionGroup30 = false;
    }

    [Serializable]
    public enum BabylonCollisionFilter {
        GROUP1 	= 1,
        GROUP2 	= 2,
        GROUP3 	= 4,
        GROUP4 	= 8,
        GROUP5 	= 16,
        GROUP6 	= 32,
        GROUP7 	= 64,
        GROUP8 	= 128,
        GROUP9 	= 256,
        GROUP10 = 512,
        GROUP11	= 1024,
        GROUP12	= 2048,
        GROUP13	= 4096,
        GROUP14	= 8192,
        GROUP15	= 16384,
        GROUP16	= 32768,
        GROUP17	= 65536,
        GROUP18	= 131072,
        GROUP19	= 262144,
        GROUP20	= 524288,
        GROUP21	= 1048576,
        GROUP22	= 2097152,
        GROUP23	= 4194304,
        GROUP24	= 8388608,
        GROUP25 = 16777216,
        GROUP26 = 33554432,
        GROUP27 = 67108864,
        GROUP28 = 134217728,
        GROUP29 = 268435456,
        GROUP30 = 536870912
    }

    [Serializable]
    public enum EcmaScript
    {
        ES5 = 0,
        ES6 = 1,
    }

    [Serializable]
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }

    [Serializable]
    public enum VirtualRealityCamera
    {
        PrimaryCameraRig = 0,
        DeviceOrientationCamera = 1,
    }

    [Serializable]
    public enum VirtualRealityController
    {
        CustomControllers = 0,
        DefaultControllers = 1
    }

    [Serializable]
    public enum PlayerNumber
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4 
    }

    [Serializable]
    public enum TerrainExport
    {
        SceneFile = 0,
        ExternalFile = 1
    }

    [Serializable]
    public enum HostingType
    {
        BuiltInServer = 0,
        RemoteServer = 1
    }

    [Serializable]
    public enum FirelightAudio
    {
        StudioInterface = 0,
        LowLevelInterface = 1
    }

    [Serializable]
    public enum XboxLive
    {
        UniversalWindowsPlatform = 0
    }

    [Serializable]
    public enum TimelineStepping
    {
        ThirtyFramesPerSecond = 30,
        SixtyFramesPerSecond = 60
    }

    [Serializable]
    public enum LightmapBakingMode
    {
        None = -1,
        Indiret = 0,
        Subtractive = 1,
        Shadows = 2
    }

    [Serializable]
    public enum MaterialExportType
    {
        Standard = 0,
        Metallic = 1,
        Roughness = 2,
        Specular = 3
    }
    

    [Serializable]
    public enum MultiPlayerStartup
    {
        StartSinglePlayerView = 1,
        StartTwoPlayerView = 2,
        StartThreePlayerView = 3,
        StartFourPlayerView = 4
    }

    [Serializable]
    public enum PlayerType
    {
        NonPlayerControlled = 0,
        PlayerOneControlled = 1,
        PlayerTwoControlled = 2,
        PlayerThreeControlled = 3,
        PlayerFourControlled = 4
    }

    [Serializable]
    public enum LaunchMode
    {
        DefaultWindow = 0,
        FullScreenWindow = 1
    }

    [Serializable]
    public enum SandboxType
    {
        Retail = 0,
        Custom = 1
    }

    [Serializable]
    public enum BuildType
    {
        Scene = 0,
        Script = 1,
        Project = 2
    }

    [Serializable]
    public enum MotionType {
        Clip = 0,
        Tree = 1
    }

    [Serializable]
    public enum BabylonCameraOptions
    {
        UniversalCamera = 0,
        ArcRotateCamera = 1,
        FollowCamera = 2,
        ArcFollowCamera = 3,
        HolographicCamera = 4,
        DeviceOrientationCamera = 5,
        VirtualJoysticksCamera = 6,
        AnaglyphArcRotateCamera = 7,
        AnaglyphUniversalCamera = 8,
        StereoscopicArcRotateCamera = 9,
        StereoscopicUniversalCamera = 10,
        LocalMultiPlayerViewCamera = 11
    }

    [Serializable]
    public enum BabylonEnabled
    {
        Enabled = 0,
        Disabled = 1
    }

    [Serializable]
    public enum BabylonSkyboxOption
    {
        SixSidedCubemap = 0,
        DirectDrawSurface = 1
    }

    [Serializable]
    public enum BabylonLightIntensity
    {
        Automatic = 0,
        LuminousPower = 1,
        LuminousItensity = 2,
        Illuminance = 3,
        Luminance = 4,
    }

    [Serializable]
    public enum BabylonRenderStats
    {
        FramesPerSecond = 0,
        FrameMilliseconds = 1
    }

    [Serializable]
    public enum BabylonAreaLights
    {
        ExcludeAreaBakingLights = 0,
        IncludeAreaBakingLights = 1
    }

    [Serializable]
    public enum BabylonLargeEnabled
    {
        ENABLED = 0,
        DISABLED = 1
    }

    [Serializable]
    public enum BabylonPhysicsEngine
    {
        CANNON = 0
    }

    [Serializable]
    public enum BabylonImageFormat
    {
        PNG = 0,
        JPG = 1
    }

    [Serializable]
    public enum BabylonHeightmapFormat
    {
        PNG = 0,
        RAW = 1
    }

    [Serializable]
    public enum BabylonSplitterFormat
    {
        PNG = 0,
    }

    [Serializable]
    public enum BabylonFilterFormat
    {
        DDS = 0
    }
    
    [Serializable]
    public enum BabylonCubemapFormat
    {
        RGBA16F = 0,
        RGBA32F = 1
    }

    [Serializable]
    public enum BabylonCubemapLight
    {
        PHONG = 0,
        PHONGBRDF = 1,
        BLINN = 2,
        BLINNBRDF = 3
    }

    [Serializable]
    public enum BabylonReflectionProbe
    {
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
    }
    
    [Serializable]
    public enum MeshSimplyfycationType
    {
        QUADRATIC = 0
    }    
    
    [Serializable]
    public enum BabylonJoystickOptions
    {
        None = 0,
        Always = 1,
        Mobile = 2
    }

    [Serializable]
    public enum BabylonFogMode
    {
        None = 0,
        Exponential = 1,
        FastExponential = 2,
        Linear = 3
    }

    [Serializable]
    public enum BabylonGuiMode
    {
        None = 0,
        Html = 1
    }

    [Serializable]
    public enum BabylonSkyboxType
    {
        UnitySkyboxEnvironment = 0
    }

    [Serializable]
    public enum BabylonTickOptions
    {
        EnableTick = 0,
        DisableTick = 1
    }

    [Serializable]
    public enum BabylonTextEncoding
    {
        RawBytes = 0,
        EncodedText = 1
    }

    [Serializable]
    public enum BabylonShadowOptions
    {
        Baked = 0,
        Realtime = 1
    }

    [Serializable]
    public enum BabylonLightmapBaking
    {
        Automatic = 0
    }

    [Serializable]
    public enum BabylonLoopBehavior
    {
        Relative = 0,
        Cycle = 1,
        Constant = 2
    }

    [Serializable]
    public enum BabylonParticleBlend
    {
        OneOne = 0,
        Standard = 1
    }

    [Serializable]
    public enum BabylonTextureExport
    {
        AlwaysExport = 0,
        IfNotExists = 1
    }

    [Serializable]
    public enum BabylonToolkitType
    {
        CombineMeshes = 0,
        SeperateMeshes = 1,
        BlockingVolumes = 2,
        BakeTextureAtlas = 3
    }

    [Serializable]
    public enum BabylonBlockingVolume
    {
        BakeColliders = 0,
        RemoveColliders = 1
    }

    [Serializable]
    public enum BabylonPrimitiveType
    {
        Ground = 0,
        Cube = 1,
        Cone = 2,
        Tube = 3,
        Wheel = 4,
        Torus = 5,
        Sphere = 6,
        Capsule = 7
    }

    [Serializable]
    public enum BabylonTextureMode
    {
        CombineMeshes = 0,
        SeperateMeshes = 1
    }

    [Serializable]
    public enum BabylonTextureScale
    {
        Point = 0,
        Bilinear = 1
    }
    
    [Serializable]
    public enum BabylonAnimationMode
    {
        DisabledAnimation = 0,
        TransformAnimation = 1,
        SkeletonAnimation = 2
    }

    [Serializable]
    public enum BabylonProgramSection
    {
        Babylon = 0,
        Vertex = 1,
        Fragment = 2
    }


    [Serializable]
    public enum BabylonPhysicsImposter
    {
        None = 0,
        Sphere = 1,
        Box = 2,
        Plane = 3,
        Mesh = 4,
        Cylinder = 7,
        Particle = 8,
        HeightMap = 9
    }

    [Serializable]
    public enum BabylonPhysicsRotation
    {
        Normal = 0,
        Fixed = 1
    }

    [Serializable]
    public enum BabylonMovementType
    {
        DirectVelocity = 0,
        AppliedForces = 1,
        CheckCollision = 2
    }

    [Serializable]
    public enum BabylonCollisionType
    {
        Collider = 0,
        Trigger = 1
    }

    [Serializable]
    public enum BabylonAmbientLighting
    {
        NoAmbientLighting = 0,
        UnityAmbientLighting = 1
    }

    [Serializable]
    public enum BabylonNavigationMesh
    {
        DisableNavigation = 0,
        EnableNavigation = 1
    }

    [Serializable]
    public enum BabylonFreeImageLibrary
    {
        FreeImageLibrary = 0
    }

    [Serializable]
    public enum BabylonToneMapping
    {
        Drago = 0,
        Fattal = 1,
        Reinhard = 2
    }

    [Serializable]
    public enum BabylonUpdateOptions
    {
        StableProduction = 0,
        PreviewRelease = 1,
    }

    [Serializable]
    public enum BabylonLightingFilter
    {
        NoShadowMapFilter = 0,
        ExponentialShadowMap = 1,
        PoissonSampling = 2,
        BlurExponentialShadowMap = 3,
        CloseExponentialShadowMap = 4,
        BlurCloseExponentialShadowMap = 5,
        PercentageCloserFiltering = 6,
        ContactHardeningShadowMap = 7
    }

    [Serializable]
    public enum BabylonFilteringQuality
    {
        High = 0,
        Medium = 1,
        Low = 2
    }

    [Serializable]
    public class BabylonTerrainData
    {
        public int width;
        public int height;
        public Vector3[] vertices;
        public Vector3[] normals;
        public Vector2[] uvs;
        public int[] triangles;
    }

    [Serializable]
    public enum BabylonProbeFormat
    {
        Probe128 = 128,
        Probe256 = 256,
        Probe512 = 512
    }

    [Serializable]
    public enum BabylonImageLibrary
    {
        UnityImageLibrary = 0
    }

    [Serializable]
    public enum BabylonTerrainFormat
    {
        Triangles = 0
    }

    [Serializable]
    public enum BabylonHeightmapExport
    {
        SceneFile = 0,
        ExternalFile = 1 
    }


    [Serializable]
    public enum BabylonTerrainSegments
    {
        SingleMesh = 1,
        TwoByTwo = 2,
        ThreeByThree = 3,
        FourByFour = 4,
        FiveByFive = 5,
        SixBySix = 6,
        SevenBySeven = 7,
        EightByEight = 8,
        NineByNine = 9,
        TenByTen = 10,
        ElevenByEleven = 11,
        TwelveByTwelve = 12,
        ThirteenByThirteen = 13,
        FourteenByFourteen = 14,
        FifteenByFifteen = 15,
        SixteenBySixteen = 16,
    }

    [Serializable]
    public enum BabylonColliderDetail
    {
        HighResolution = 0,
        MediumResolution = 1,
        LowResolution = 2,
        VeryLowResolution = 3,
        MinimumResolution = 4
    }

    [Serializable]
    public enum BabylonPreviewWindow
    {
        OpenDefaultBrowser = 0,
        AttachUnityBrowser = 1
    }

    [Serializable]
    public enum BabylonCameraInput
    {
        NoCameraUserInput = 0,
        AutoCameraUserInput = 1,
        AttachRenderCanvasInput = 2
    }

    [Serializable]
    public enum BabylonTransparencyMode
    {
        Opaque = 0,
        AlphaTest = 1,
        AlphaBlend = 2,
        AlphaTestAndBlend = 3
    }

    [DataContract, Serializable]
    public class BabylonEllipsoidProperties
    {
        [DataMember]
        public Vector3 defaultEllipsoid = new Vector3(0.5f, 1.0f, 0.5f);

        [DataMember]
        public Vector3 ellipsoidOffset = new Vector3(0.0f, 0.0f, 0.0f);
    }

    [DataContract, Serializable]
    public class BabylonRuntimeProperties
    {
        [DataMember]
        public bool pickableWithRay = true;

        [DataMember]
        public bool delayLoadingFile = false;

        [DataMember]
        public bool freezeWorldMatrix = false;

        [DataMember]
        public bool convertToUnIndexed = false;

        [DataMember]
        public bool convertToFlatShaded = false;
    }

    [DataContract, Serializable]
    public class BabylonTriggerActions
    {
        [DataMember]
        public bool pickTrigger = false;
        [DataMember]
        public bool doublePickTrigger = false;
        [DataMember]
        public bool pickDownTrigger = false;
        [DataMember]
        public bool pickUpTrigger = false;
        [DataMember]
        public bool pickOutTrigger = false;
        [DataMember]
        public bool leftPickTrigger = false;
        [DataMember]
        public bool rightPickTrigger = false;
        [DataMember]
        public bool centerPickTrigger = false;
        [DataMember]
        public bool longPressTrigger = false;
        [DataMember]
        public bool pointerOverTrigger = false;
        [DataMember]
        public bool pointerOutTrigger = false;
    }

    [DataContract, Serializable]
    public class BabylonOverrideVisibility
    {
        [DataMember]
        public bool overrideVisibility = false;
        [DataMember, Range(0.0f, 1.0f)]
        public float newVisibilityLevel = 1.0f;
    }

    [DataContract, Serializable]
    public class BabylonCurveKeyframe
    {
        [DataMember]
        public float time { get; set; }
        [DataMember]
        public float value { get; set; }
        [DataMember]
        public float inTangent { get; set; }
        [DataMember]
        public float outTangent { get; set; }
        [DataMember]
        public int tangentMode { get; set; }
    }

    [DataContract, Serializable]
    public class BabylonTerrainSplat
    {
        [DataMember]
        public Color32[] Splat { get; private set; }
        [DataMember]
        public int Width { get; private set; }
        [DataMember]
        public int Height { get; private set; }
        [DataMember]
        public Vector2 TileSize { get; private set; }
        [DataMember]
        public Vector2 TileOffset { get; private set; }
        public BabylonTerrainSplat(Texture2D splat, Vector2 tile, Vector2 offset)
        {
            this.Splat = (splat != null) ? splat.GetPixels32() : null;
            this.Width = (splat != null) ? splat.width : 0;
            this.Height = (splat != null) ? splat.height : 0;
            this.TileSize = tile;
            this.TileOffset = offset;
        }
    }

    [DataContract, Serializable]
    public class BabylonCombineInstance
    {
        [DataMember]
        public MeshFilter filter { get; private set; }
        [DataMember]
        public string material { get; private set; }
        [DataMember]
        public Matrix4x4 transform { get; private set; }
        [DataMember]
        public Matrix4x4[] bindposes { get; private set; }
        [DataMember]
        public BoneWeight[] boneWeights { get; private set; }
        [DataMember]
        public int subMeshCount { get; private set; }
        [DataMember]
        public Bounds bounds { get; private set; }
        [DataMember]
        public int[] triangles { get; private set; }
        [DataMember]
        public Color32[] colors32 { get; private set; }
        [DataMember]
        public Color[] colors { get; private set; }
        [DataMember]
        public Vector2[] uv4 { get; private set; }
        [DataMember]
        public Vector2[] uv3 { get; private set; }
        [DataMember]
        public Vector2[] uv2 { get; private set; }
        [DataMember]
        public Vector2[] uv { get; private set; }
        [DataMember]
        public Vector4[] tangents { get; private set; }
        [DataMember]
        public Vector3[] normals { get; private set; }
        [DataMember]
        public Vector3[] vertices { get; private set; }

        public BabylonCombineInstance(Mesh source, Matrix4x4 transform, MeshFilter filter)
        {
            this.filter = filter;
            this.transform = transform;
            this.vertices = source.vertices;
            this.triangles = source.triangles;
            this.uv = source.uv;
            this.uv2 = source.uv2;
            this.uv3 = source.uv3;
            this.uv4 = source.uv4;
            this.bounds = source.bounds;
            this.normals = source.normals;
            this.tangents = source.tangents;
            this.colors = source.colors;
            this.colors32 = source.colors32;
            this.bindposes = source.bindposes;
            this.boneWeights = source.boneWeights;
            this.subMeshCount = source.subMeshCount;
        }

        public CombineInstanceFilter CreateCombineInstance()
        {
            Mesh mesh = new Mesh();
            mesh.name = this.filter.name;
            mesh.vertices = this.vertices;
            mesh.triangles = this.triangles;
            mesh.uv = this.uv;
            mesh.uv2 = this.uv2;
            mesh.uv3 = this.uv3;
            mesh.uv4 = this.uv4;
            mesh.bounds = this.bounds;
            mesh.normals = this.normals;
            mesh.tangents = this.tangents;
            mesh.colors = this.colors;
            mesh.colors32 = this.colors32;
            mesh.bindposes = this.bindposes;
            mesh.boneWeights = this.boneWeights;
            mesh.subMeshCount = this.subMeshCount;

            CombineInstance result = new CombineInstance();
            result.mesh = mesh;
            result.transform = this.transform;
            return new CombineInstanceFilter(result, this.filter);
        }
    }    

    [DataContract, Serializable]
    public class CombineInstanceFilter
    {
        [DataMember]
        public CombineInstance combine { get; private set; }
        [DataMember]
        public MeshFilter filter { get; private set; }

        public CombineInstanceFilter(CombineInstance combine, MeshFilter filter)
        {
            this.combine = combine;
            this.filter = filter;
        }
    }

    [DataContract, Serializable]
    public class BabylonGroundMesh: BabylonMesh
    {
        [DataMember]
        public string customType { get; private set; }
        
        public BabylonGroundMesh() : base()
        {
            this.customType = "BABYLON.GroundMesh";
        }
    }

    [DataContract, Serializable]
    public class BabylonDefaultMaterial: BabylonStandardMaterial
    {
        public BabylonDefaultMaterial() : base()
        {
            this.SetCustomType("BABYLON.StandardMaterial");
            this.ambient = new[] {1.0f, 1.0f, 1.0f};
            this.diffuse = new[] { 1.0f, 1.0f, 1.0f };
            this.specular = new[] { 1.0f, 1.0f, 1.0f };
            this.emissive = new[] { 0f, 0f, 0f };
            this.specularPower = 64;
            this.useSpecularOverAlpha = true;
            this.useAlphaFromDiffuseTexture = false;
            this.useEmissiveAsIllumination = false;
            this.linkEmissiveWithDiffuse = false;
            this.twoSidedLighting = false;
            this.maxSimultaneousLights = 4;
        }
    }

    [DataContract, Serializable]
    public class BabylonSystemMaterial : BabylonPBRMaterial
    {
        public BabylonSystemMaterial() : base()
        {
            this.SetCustomType("BABYLON.PBRMaterial");
            this.cameraExposure = 1.0f;
            this.cameraContrast = 1.0f;
            this.directIntensity = 1.0f;
            this.emissiveIntensity = 0.5f;
            this.specularIntensity = 0.5f;
            this.environmentIntensity = 1.0f;
            this.indexOfRefraction = 0.66f;
            this.twoSidedLighting = false;
            this.maxSimultaneousLights = 4;
            this.useRadianceOverAlpha = true;
            this.useSpecularOverAlpha = true;
            this.usePhysicalLightFalloff = true;
            this.useEmissiveAsIllumination = false;

            this.metallic = null;
            this.roughness = null;
            this.useRoughnessFromMetallicTextureAlpha = false;
            this.useRoughnessFromMetallicTextureGreen = true;
            this.useMetallnessFromMetallicTextureBlue = true;
            this.useAmbientOcclusionFromMetallicTextureRed = false;

            this.sideOrientation = 1;
            this.microSurface = 0.9f;
            this.useMicroSurfaceFromReflectivityMapAlpha = false;
            this.useAutoMicroSurfaceFromReflectivityMap = false;

            this.ambient = new[] { 0f, 0f, 0f };
            this.albedo = new[] { 1f, 1f, 1f };
            this.reflectivity = new[] { 1f, 1f, 1f };
            this.reflection = new[] { 1f, 1f, 1f };
            this.emissive = new[] { 0f, 0f, 0f };
        }
    }

    [DataContract, Serializable]
    public class BabylonRoughnessMaterial : BabylonPBRMetallicRoughnessMaterial
    {
        // Toolkit Wrapper Class
    }    

   [DataContract, Serializable]
    public class BabylonUniversalMaterial : BabylonDefaultMaterial
    {
        [DataMember]
        public Dictionary<string, object> textures;

        [DataMember]
        public Dictionary<string, object[]> textureArrays;

        [DataMember]
        public Dictionary<string, object> floats;

        [DataMember]
        public Dictionary<string, object[]> floatArrays;

        [DataMember]
        public Dictionary<string, object> colors3;

        [DataMember]
        public Dictionary<string, object> colors4;

        [DataMember]
        public Dictionary<string, object> vectors2;

        [DataMember]
        public Dictionary<string, object> vectors3;

        [DataMember]
        public Dictionary<string, object> vectors4;

        [DataMember]
        public Dictionary<string, object> matrices;

        [DataMember]
        public Dictionary<string, object> matrices2x2;

        [DataMember]
        public Dictionary<string, object> matrices3x3;

        [DataMember]
        public Dictionary<string, object[]> vectors3Arrays;

        public BabylonUniversalMaterial()
        {
            this.SetCustomType("BABYLON.UniversalShaderMaterial");
            this.textures = new Dictionary<string, object>();
            this.textureArrays = new Dictionary<string, object[]>();
            this.floats = new Dictionary<string, object>();
            this.floatArrays = new Dictionary<string, object[]>();
            this.colors3 = new Dictionary<string, object>();
            this.colors4 = new Dictionary<string, object>();
            this.vectors2 = new Dictionary<string, object>();
            this.vectors3 = new Dictionary<string, object>();
            this.vectors4 = new Dictionary<string, object>();
            this.matrices = new Dictionary<string, object>();
            this.matrices2x2 = new Dictionary<string, object>();
            this.matrices3x3 = new Dictionary<string, object>();
            this.vectors3Arrays = new Dictionary<string, object[]>();
        }
    }
    
   [DataContract, Serializable]
    public class BabylonLDRCubeTexture : BabylonTexture
    {
        [DataMember]
        public string customType { get; private set; }

        [DataMember]
        public int size { get; set; }

        [DataMember]
        public bool useInGammaSpace { get; set; }

        public BabylonLDRCubeTexture()
        {
            this.customType = "BABYLON.LDRCubeTexture";
            this.size = 0;
            this.isCube = true;
            this.useInGammaSpace = false;
        }
    }

    [Serializable]
    public class MachineState
    {
        public int hash;
        public string name;
        public string tag;
        public float time;
        public int type;
        public float rate;
        public float length;
        public string layer;
        public int layerIndex;
        public int played;
        public string machine;
        public bool interrupted;
        public float apparentSpeed;
        public float averageAngularSpeed;
        public float averageDuration;
        public float[] averageSpeed;
        public float cycleOffset;
        public string cycleOffsetParameter;
        public bool cycleOffsetParameterActive;
        public bool iKOnFeet;
        public bool mirror;
        public string mirrorParameter;
        public bool mirrorParameterActive;
        public float speed;
        public string speedParameter;
        public bool speedParameterActive;
        public Dictionary<string, object> blendtree;
        public List<Dictionary<string, object>> transitions;
        public List<Dictionary<string, object>> behaviours;
        public List<object> animations;
    }

    [Serializable]
    public class AnimationParameters
    {
        public bool defaultBool;
        public float defaultFloat;
        public int defaultInt;
        public string name;
        public int type;
        public bool curve;
    }

    [DataContract, Serializable, AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BabylonClassAttribute : Attribute { }

    [DataContract, Serializable, AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class BabylonPropertyAttribute : PropertyAttribute { }

    /////////////////////////////////////////////////////
    // Note: The Only Supported Toolkit Script Component
    /////////////////////////////////////////////////////

    [Serializable, CanEditMultipleObjects]
    public abstract class EditorScriptComponent : MonoBehaviour
    {
        [Unity3D2Babylon.ReadOnly]
        public string babylonClass;
        [HideInInspector]
        public Action<SceneBuilder, GameObject, Dictionary<string, object>> OnExportProperties { get; set; }
        void Start(){}
        protected EditorScriptComponent()
        {
            this.babylonClass = "BABYLON.SceneComponent";
        }
    }
}
