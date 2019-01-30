using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;
using BabylonHosting;

namespace Unity3D2Babylon
{
    [ExecuteInEditMode]
    public class ExporterWindow : EditorWindow
    {
        /* Global Variables */

        public const string ToolkitVersion = "4.0.0 - Alpha 10 - 2018";
        public const string DefaultHost = "localhost";
        public const string IgnoreLabel = "Babylon Void";
        public const string StaticLabel = "Babylon Static";
        public const string PrefabLabel = "Babylon Prefab";
        public const int IgnoreIndex = 29;
        public const int StaticIndex = 30;
        public const int PrefabIndex = 31;
        public const int MaxVerticies = 65535;
        public const int MaxTerrainTiles = 6;
        public const int DefaultPort = 8888;
        public const float NoScaling = 1.0f;
        public const float DefaultAmbientScale = 1.0f;
        public const float DefaultAmbientGradient = 0.5f;
	    public static Texture2D BabylonHeader = null;

        /* Local Variables */

        public static ExportationOptions exportationOptions = null;
        public static readonly List<string> logs = new List<string>();
        int adaptToDeviceRatio = 0;
        Vector2 scrollPosMain;
        bool showEngine = false;
        bool showTerrain = false;
        bool showLighting = false;
        bool showCollision = false;
        bool showPreview = false;
        bool showCompiler = false;
        bool showWindows = false;
        //bool showMobile = false;
        bool showModule = false;
        bool showExport = false;
        bool showMenu = false;
        bool showUnity = false;
        int buildResult = 0;
        GUIStyle labelGuiStyle = new GUIStyle();
        string toolkitLabel = "Babylon Toolkit - Version " + ExporterWindow.ToolkitVersion + " (www.babylontoolkit.com)";
        public bool previewThread = false;
        private static Color splashScreenColor = new Color(0.23137255f, 0.470588237f, 0.6039216f, 1.0f);
        private static Color splashTextColor = new Color(0.9529412f, 0.9529412f, 0.9529412f, 1.0f);
        private static Color splashRingColor = new Color(0.9529412f, 0.9529412f, 0.9529412f, 1.0f);
        private static Color splashSpinColor = new Color(0.8784314f, 0.129411772f, 0.152941182f, 1.0f);
        private static Color splashDataColor = new Color(0.172549024f, 0.698039234f, 0.294117659f, 1.0f);

        public static string DefaultProjectFolder = String.Empty;
        /* Exporter Window */

        public static void ReportProgress(float value, string message = "")
        {
            EditorUtility.DisplayProgressBar("Babylon.js", message, value);

            if (!string.IsNullOrEmpty(message))
            {
                logs.Add(message);
            }
        }

        public static bool ShowMessage(string message, string title = "Babylon.js", string ok = "OK", string cancel = "")
        {
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }

        public ExportationOptions CreateSettings()
        {
            ExportationOptions result = new ExportationOptions();
            string ufile = Path.Combine(Application.dataPath, "Babylon/Template/Config/settings.json");
            if (File.Exists(ufile))
            {
                string json = FileTools.ReadAllText(ufile);
                result = Tools.FromJson<ExportationOptions>(json);
            }
            Color? splash = Tools.ColorFromFloats(result.SplashScreenColor);
            if (splash != null) splashScreenColor = splash.Value;

            Color? text = Tools.ColorFromFloats(result.SplashTextColor);
            if (text != null) splashTextColor = text.Value;

            Color? ring = Tools.ColorFromFloats(result.SplashRingColor);
            if (ring != null) splashRingColor = ring.Value;

            Color? spin = Tools.ColorFromFloats(result.SplashSpinColor);
            if (spin != null) splashSpinColor = spin.Value;

            Color? data = Tools.ColorFromFloats(result.SplashDataColor);
            if (data != null) splashDataColor = data.Value;
            return result;
        }

        public void SaveSettings(bool refresh = false)
        {
            if (exportationOptions != null) {
                exportationOptions.SplashScreenColor = splashScreenColor.ToFloat();
                exportationOptions.SplashTextColor = splashTextColor.ToFloat();
                exportationOptions.SplashRingColor = splashRingColor.ToFloat();
                exportationOptions.SplashSpinColor = splashSpinColor.ToFloat();
                exportationOptions.SplashDataColor = splashDataColor.ToFloat();
                if (refresh) GetSceneInfomation(false);
                string ufile = Path.Combine(Application.dataPath, "Babylon/Template/Config/settings.json");
                if (File.Exists(ufile))
                {
                    string json = Tools.ToJson(exportationOptions, true);
                    FileTools.WriteAllText(ufile, json);
                }
            }
        }

        [MenuItem("Babylon/Scene Exporter", false, 0)]
        public static void InitExporter()
        {
            var exporter = (ExporterWindow)GetWindow(typeof(ExporterWindow));
            exporter.minSize = new Vector2(420.0f, 520.0f);
        }

        [MenuItem("Babylon/Output Window", false, 5)]
        public static void InitOutput()
        {
            var output = (ExporterOutput)GetWindow(typeof(ExporterOutput));
            output.OnInitialize();
        }

        [MenuItem("Babylon/Developer Portal", false, 50)]
        public static void InitEditor()
        {
            Application.OpenURL("https://www.babylontoolkit.com");
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/TypeScript Class", false, 10)]
        public static void CreateTypescript_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewScript.ts");
            string template = "Assets/Babylon/Template/Sources/ts_typescript.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon TypeScript File";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Scene Controller", false, 1001)]
        public static void CreateSceneController_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewSceneController.ts");
            string template = "Assets/Babylon/Template/Sources/ts_scene.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Mesh Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Mesh Component", false, 1101)]
        public static void CreateMeshComponent_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewMeshComponent.ts");
            string template = "Assets/Babylon/Template/Sources/ts_mesh.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Mesh Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Light Component", false, 1102)]
        public static void CreateLightComponent_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewLightComponent.ts");
            string template = "Assets/Babylon/Template/Sources/ts_light.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Light Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Camera Component", false, 1103)]
        public static void CreateCameraComponent_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewCameraComponent.ts");
            string template = "Assets/Babylon/Template/Sources/ts_camera.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Camera Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Shader Controller", false, 1202)]
        public static void CreateShaderController_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewShaderController.ts");
            string template = "Assets/Babylon/Template/Sources/ts_shader.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Shader Controller";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }
        

        [MenuItem("Assets/Create/Babylon/Babylon TypeScript/Global Startup Script", false, 1301)]
        public static void CreateApplicationScript_TS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "Global.app.ts");
            string template = "Assets/Babylon/Template/Sources/ts_global.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Global Application Script";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/New JavaScript File", false, 2001)]
        public static void CreateJavacriptFile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewScript.bjs");
            string template = "Assets/Babylon/Template/Sources/js_script.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon JavaScript File";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Custom Shader Material", false, 2002)]
        public static void CreateAmigaShader()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewShaderMaterial.shader");
            string template = "Assets/Babylon/Template/Sources/ux_shader.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Shader Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Assets/Create/Babylon/Editor Script Component (C#)", false, 9001)]
        public static void CreateEditorMeshComponent_CS()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (String.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string filename = Path.Combine(path, "NewScriptComponent.cs");
            string template = "Assets/Babylon/Template/Sources/editora.template";
            if (!File.Exists(template))
            {
                string defaultTemplate = "// Babylon Script Class";
                FileTools.WriteAllText(template, defaultTemplate);
            }
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon") as Texture2D;
            var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction, filename, icon, template);
        }

        [MenuItem("Babylon/Babylon Dashboard", false, 9999)]
        public static void InitWebsite()
        {
            Application.OpenURL("https://www.babylonjs.com");
        }

        public void OnEnable()
        {
            this.titleContent = new GUIContent("Exporter");
            this.labelGuiStyle.hover.textColor = Color.green;
            this.labelGuiStyle.normal.textColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            this.labelGuiStyle.alignment = TextAnchor.MiddleCenter;
            this.labelGuiStyle.fontSize = 9;
            
            // Validate exporter options
            if (ExporterWindow.exportationOptions == null) {
                ExporterWindow.exportationOptions = CreateSettings();
            }

            // Validate project folder
            ExporterWindow.DefaultProjectFolder = Tools.GetDefaultProjectFolder();

            // Validate project layers
            Tools.ValidateProjectLayers();
			
            // Validate project extensions
            Tools.ValidateEditorProjectFile("md");
            Tools.ValidateEditorProjectFile("ts");
            Tools.ValidateEditorProjectFile("fx");
            Tools.ValidateEditorProjectFile("bjs");
            Tools.ValidateEditorProjectFile("json");
            Tools.ValidateEditorProjectFile("html");
            Tools.ValidateEditorProjectFile("shader");
            Tools.ValidateEditorProjectFile("config");
            Tools.ValidateEditorProjectFile("template");
            Tools.ValidateEditorProjectFile("javascript");
            
            // Attach unity editor buttons
            UnityEditor.EditorApplication.playModeStateChanged += (PlayModeStateChange change) =>
            {
                if (exportationOptions != null && exportationOptions.AttachUnityEditor)
                {
                    bool wantsToPlay = UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
                    bool wantsToPause = UnityEditor.EditorApplication.isPaused;
                    if (wantsToPlay || wantsToPause)
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                        UnityEditor.EditorApplication.isPaused = false;
                    }
                    if (wantsToPlay) Execute(false);
                }
            };
        }

        public void InitServer()
        {
            if (WebServer.IsStarted == false && exportationOptions.HostPreviewType == 0)
            {
                // Validate default project folder selected
                if (String.IsNullOrEmpty(ExporterWindow.DefaultProjectFolder))
                {
                    UnityEngine.Debug.LogWarning("No default project file selected. Web server not started.");
                    return;
                }

                // Validate default project folder exists
                if (!Directory.Exists(ExporterWindow.DefaultProjectFolder))
                {
                    UnityEngine.Debug.LogWarning("No default project file created. Web server not started.");
                    return;
                }

                // Validate local web server supported
                if (HttpListener.IsSupported)
                {
                    string prefix = "http://*:";
                    string unity = Tools.GetAssetsRootPath();
                    string root = ExporterWindow.DefaultProjectFolder;
                    int port = exportationOptions.DefaultServerPort;
                    bool started = WebServer.Activate(prefix, root, port, unity);
                    if (started) UnityEngine.Debug.Log("Babylon.js web server started on port: " + port.ToString());
                    else UnityEngine.Debug.LogWarning("Babylon.js web server failed to start on port: " + port.ToString());
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Http listener services are not supported.");   
                }
            }
        }

        public void OnGUI()
        {
		    if(ExporterWindow.BabylonHeader == null) {
                string header = Path.Combine(Application.dataPath, "Babylon/Images/Header.png");
                if (File.Exists(header)) {
                    ExporterWindow.BabylonHeader = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    ExporterWindow.BabylonHeader.LoadImage(File.ReadAllBytes(header));
                    ExporterWindow.BabylonHeader.alphaIsTransparency = true;
                }
            }
            if (ExporterWindow.BabylonHeader != null) {
                Tools.DrawGuiBox(new Rect(0, 0, this.position.width, 76), Color.white);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(ExporterWindow.BabylonHeader);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            EditorGUI.BeginDisabledGroup(true);
            ExporterWindow.DefaultProjectFolder = EditorGUILayout.TextField("", ExporterWindow.DefaultProjectFolder);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Change Export Folder"))
            {
                string exportFolder = EditorUtility.SaveFolderPanel("Select Export Folder", String.Empty, String.Empty);
                if (!String.IsNullOrEmpty(exportFolder)) {
                    exportationOptions.AlternateExport = exportFolder;
                    ExporterWindow.DefaultProjectFolder = exportationOptions.AlternateExport;
                }
            }
            if (GUILayout.Button("Reset Export Folder"))
            {
                exportationOptions.AlternateExport = null;
                ExporterWindow.DefaultProjectFolder = Tools.GetDefaultProjectFolder();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save Project Export Settings"))
            {
                SaveSettings();
                ShowMessage("Export settings saved.");
            }
            scrollPosMain = EditorGUILayout.BeginScrollView(scrollPosMain, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            EditorGUILayout.Space();
            exportationOptions.DefaultBinPath = EditorGUILayout.TextField("Project Bin Path", exportationOptions.DefaultBinPath);
            EditorGUILayout.Space();
            exportationOptions.DefaultBuildPath = EditorGUILayout.TextField("Project Build Path", exportationOptions.DefaultBuildPath);
            EditorGUILayout.Space();
            exportationOptions.DefaultScenePath = EditorGUILayout.TextField("Project Scene Path", exportationOptions.DefaultScenePath);
            EditorGUILayout.Space();
            exportationOptions.DefaultScriptPath = EditorGUILayout.TextField("Project Script Path", exportationOptions.DefaultScriptPath);
            EditorGUILayout.Space();
            exportationOptions.DefaultIndexPage = EditorGUILayout.TextField("Project Index Page", exportationOptions.DefaultIndexPage);
            EditorGUILayout.Space();
            showEngine = EditorGUILayout.Foldout(showEngine, "Default Engine Options");
            if (showEngine)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                exportationOptions.ExportPhysics = EditorGUILayout.Toggle("Enable Physics Engine", exportationOptions.ExportPhysics);
                exportationOptions.DefaultPhysicsEngine = (int)(BabylonPhysicsEngine)EditorGUILayout.EnumPopup("Physics Engine Library", (BabylonPhysicsEngine)exportationOptions.DefaultPhysicsEngine, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                exportationOptions.EnableAntiAliasing = EditorGUILayout.Toggle("Set Engine Antialias", exportationOptions.EnableAntiAliasing);
                this.adaptToDeviceRatio = (int)(BabylonLargeEnabled)EditorGUILayout.EnumPopup("Adaptive Device Ratio", (BabylonLargeEnabled)this.adaptToDeviceRatio, GUILayout.ExpandWidth(true));
                exportationOptions.AdaptToDeviceRatio = (this.adaptToDeviceRatio == 0);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                exportationOptions.EnforceImageEncoding = EditorGUILayout.Toggle("Set Image Encoding", exportationOptions.EnforceImageEncoding);
                exportationOptions.ImageEncodingOptions = (int)(BabylonImageFormat)EditorGUILayout.EnumPopup("Default Image Format", (BabylonImageFormat)exportationOptions.ImageEncodingOptions, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                exportationOptions.DefaultTextureQuality = (int)EditorGUILayout.Slider("Set Texture Quality", exportationOptions.DefaultTextureQuality, 1, 100);
                EditorGUILayout.Space();
                exportationOptions.StaticVertexLimit = EditorGUILayout.Toggle("Set Mesh Vertex Limit", exportationOptions.StaticVertexLimit);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            showTerrain = EditorGUILayout.Foldout(showTerrain, "Terrain Builder Options");
            if (showTerrain)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                exportationOptions.TerrainExportFile = (int)(TerrainExport)EditorGUILayout.EnumPopup("Terrain Export File", (TerrainExport)exportationOptions.TerrainExportFile, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                exportationOptions.TerrainAtlasSize = (int)EditorGUILayout.Slider("Texture Atlas Size", exportationOptions.TerrainAtlasSize, 128, 8192);
                EditorGUILayout.Space();
                exportationOptions.TerrainImageScaling = (int)(BabylonTextureScale)EditorGUILayout.EnumPopup("Texture Image Scale", (BabylonTextureScale)exportationOptions.TerrainImageScaling, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                exportationOptions.TerrainCoordinatesIndex = (int)EditorGUILayout.Slider("Shadow Map Index", exportationOptions.TerrainCoordinatesIndex, 0, 1);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            showCollision = EditorGUILayout.Foldout(showCollision, "Collision System Options");
            if (showCollision)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                exportationOptions.ExportCollisions = EditorGUILayout.Toggle("Enable Collisions", exportationOptions.ExportCollisions);
                EditorGUILayout.Space();
                exportationOptions.GenerateColliders = EditorGUILayout.Toggle("Generate Colliders", exportationOptions.GenerateColliders);
                EditorGUILayout.Space();
                exportationOptions.ColliderVisibility = (float)EditorGUILayout.Slider("Collision Visibility", exportationOptions.ColliderVisibility, 0.1f, 1.0f);
                EditorGUILayout.Space();
                exportationOptions.ShowPhysicsImposter = EditorGUILayout.Toggle("Show Physics Imposter", exportationOptions.ShowPhysicsImposter);
                EditorGUILayout.Space();
                exportationOptions.ShowDebugColliders = EditorGUILayout.Toggle("Show Debug Colliders", exportationOptions.ShowDebugColliders);
                EditorGUILayout.Space();
                exportationOptions.ShowDebugSockets = EditorGUILayout.Toggle("Show Debug Sockets", exportationOptions.ShowDebugSockets);
                EditorGUILayout.Space();
                exportationOptions.SocketColliderSize = (float)EditorGUILayout.Slider("Socket Collider Size", exportationOptions.SocketColliderSize, 0.01f, 1.0f);
                EditorGUILayout.Space();
                exportationOptions.DefaultColliderDetail = (int)(BabylonColliderDetail)EditorGUILayout.EnumPopup("Default Collider Detail", (BabylonColliderDetail)exportationOptions.DefaultColliderDetail, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            showLighting = EditorGUILayout.Foldout(showLighting, "Lightmap Baking Options");
            if (showLighting)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                exportationOptions.ExportLightmaps = EditorGUILayout.Toggle("Export Lightmaps", exportationOptions.ExportLightmaps);
                exportationOptions.DefaultLightmapBaking = (int)(BabylonLightmapBaking)EditorGUILayout.EnumPopup("Bake Iterative Maps", (BabylonLightmapBaking)exportationOptions.DefaultLightmapBaking, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                exportationOptions.DefaultCoordinatesIndex = (int)EditorGUILayout.Slider("Coordinates Index", exportationOptions.DefaultCoordinatesIndex, 0, 1);
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                exportationOptions.CreateMaterialInstance = EditorGUILayout.Toggle("Use Material Instances", exportationOptions.CreateMaterialInstance, GUILayout.ExpandWidth(false));
                exportationOptions.BakedLightsMode = (int)(BabylonAreaLights)EditorGUILayout.EnumPopup("", (BabylonAreaLights)exportationOptions.BakedLightsMode, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUI.indentLevel -= 1;
            }

            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                exportationOptions.RunWindowsPlatform = false;
            }
            showWindows = EditorGUILayout.Foldout(showWindows, "Windows Platform Options");
            if (showWindows)
            {
                int xboxlive = 0;
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();

                exportationOptions.DefaultWindowsLaunchMode = (int)(LaunchMode)EditorGUILayout.EnumPopup("Launching Mode", (LaunchMode)exportationOptions.DefaultWindowsLaunchMode, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(Application.platform != RuntimePlatform.WindowsEditor);
                    EditorGUILayout.BeginHorizontal();
                        exportationOptions.RunWindowsPlatform = EditorGUILayout.Toggle("Run App Protocol", exportationOptions.RunWindowsPlatform, GUILayout.ExpandWidth(false));
                        EditorGUI.BeginDisabledGroup(exportationOptions.RunWindowsPlatform == false);
                            exportationOptions.DefaultPlatformApp = EditorGUILayout.TextField("", exportationOptions.DefaultPlatformApp, GUILayout.ExpandWidth(true));
                        EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.BeginHorizontal();
                    exportationOptions.EnableXboxLive = EditorGUILayout.Toggle("Xbox Live Services", exportationOptions.EnableXboxLive, GUILayout.ExpandWidth(false));
                    EditorGUI.BeginDisabledGroup(exportationOptions.EnableXboxLive == false);
                        xboxlive = (int)(XboxLive)EditorGUILayout.EnumPopup("", (XboxLive)xboxlive, GUILayout.ExpandWidth(true));
                    EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Xbox Live Plugin"))
                    {
                        Application.OpenURL("https://www.nuget.org/packages/BabylonToolkit.XboxLive/");
                    }
                    if (GUILayout.Button("Switch Local Windows Sandbox"))
                    {
                        if (Application.platform != RuntimePlatform.WindowsEditor)
                        {
                            ExporterWindow.ShowMessage("This feature is only supported on Windows.");
                        }
                        else
                        {
                            ExporterSandbox sandbox = ScriptableObject.CreateInstance<ExporterSandbox>();
                            sandbox.OnInitialize();
                            sandbox.ShowUtility();
                        }
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                exportationOptions.RunWindowsPlatform = false;
            }

            exportationOptions.BuildJavaScript = true;
            showCompiler = EditorGUILayout.Foldout(showCompiler, "Script Compilation Options");
            if (showCompiler)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                EditorGUI.BeginDisabledGroup(true);
                exportationOptions.BuildJavaScript = EditorGUILayout.Toggle("Build Javascript Files", exportationOptions.BuildJavaScript);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();
                exportationOptions.CompileTypeScript = EditorGUILayout.Toggle("Build Typescript Files", exportationOptions.CompileTypeScript);
                EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(exportationOptions.CompileTypeScript == false);
                exportationOptions.EcmaScriptVersion = (int)(EcmaScript)EditorGUILayout.EnumPopup("Ecmascript Version", (EcmaScript)exportationOptions.EcmaScriptVersion, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                exportationOptions.DefaultTypeScriptPath = EditorGUILayout.TextField("Typescript Compiler", exportationOptions.DefaultTypeScriptPath);
                EditorGUILayout.Space();
                exportationOptions.DefaultNodeRuntimePath = EditorGUILayout.TextField("Node Runtime System", exportationOptions.DefaultNodeRuntimePath);
                EditorGUILayout.Space();
                if (GUILayout.Button("Detect Runtime Compiler Locations"))
                {
                    exportationOptions.DefaultTypeScriptPath = Tools.GetDefaultTypeScriptPath();
                    exportationOptions.DefaultNodeRuntimePath = Tools.GetDefaultNodeRuntimePath();
                }
                EditorGUILayout.Space();
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel -= 1;
            }

            showPreview = EditorGUILayout.Foldout(showPreview, "Toolkit Exportation Options");
            if (showPreview)
            {
                EditorGUI.indentLevel += 1;
                //EditorGUILayout.Space();

                showUnity = EditorGUILayout.Foldout(showUnity, "Unity Build Options");
                if (showUnity)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    exportationOptions.AttachUnityEditor = EditorGUILayout.Toggle("Attach Unity Editor", exportationOptions.AttachUnityEditor, GUILayout.ExpandWidth(false));
                    exportationOptions.HostPreviewType = (int)(HostingType)EditorGUILayout.EnumPopup("", (HostingType)exportationOptions.HostPreviewType, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    if (exportationOptions.HostPreviewType == 0) {
                        exportationOptions.DefaultServerPort = (int)EditorGUILayout.Slider("Local Server Port", exportationOptions.DefaultServerPort, 1025, 41000);
                        EditorGUILayout.Space();
                    } else {
                        exportationOptions.RemoteServerPath = EditorGUILayout.TextField("Remote Server Path", exportationOptions.RemoteServerPath);
                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.BeginHorizontal();
                    exportationOptions.ShowRenderStats = EditorGUILayout.Toggle("Show Render Stats", exportationOptions.ShowRenderStats, GUILayout.ExpandWidth(false));
                    EditorGUI.BeginDisabledGroup(exportationOptions.ShowRenderStats == false);
                    exportationOptions.SceneRenderStats = (int)(BabylonRenderStats)EditorGUILayout.EnumPopup("", (BabylonRenderStats)exportationOptions.SceneRenderStats, GUILayout.ExpandWidth(true));
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    exportationOptions.TruevisionGraphics = EditorGUILayout.Toggle("Truevision Graphics", exportationOptions.TruevisionGraphics);
                    EditorGUILayout.Space();

                    EditorGUI.indentLevel -= 1;
                }

                showMenu = EditorGUILayout.Foldout(showMenu, "Game Project Options");
                if (showMenu)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    exportationOptions.EnableDefaultScene = EditorGUILayout.Toggle("Set Default Scene", exportationOptions.EnableDefaultScene, GUILayout.ExpandWidth(false));
                    EditorGUI.BeginDisabledGroup(exportationOptions.EnableDefaultScene == false);
                    exportationOptions.DefaultSceneName = EditorGUILayout.TextField("", exportationOptions.DefaultSceneName, GUILayout.ExpandWidth(true));
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    exportationOptions.EnableMainMenu = EditorGUILayout.Toggle("Enable Main Menu", exportationOptions.EnableMainMenu, GUILayout.ExpandWidth(false));
                    EditorGUI.BeginDisabledGroup(exportationOptions.EnableMainMenu == false);
                    exportationOptions.DefaultGamePage = EditorGUILayout.TextField("", exportationOptions.DefaultGamePage, GUILayout.ExpandWidth(true));
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    splashScreenColor = EditorGUILayout.ColorField("Background Color", splashScreenColor);
                    EditorGUILayout.Space();
                    splashTextColor = EditorGUILayout.ColorField("Splash Text Color", splashTextColor);
                    EditorGUILayout.Space();
                    splashRingColor = EditorGUILayout.ColorField("Splash Ring Color", splashRingColor);
                    EditorGUILayout.Space();
                    splashSpinColor = EditorGUILayout.ColorField("Splash Spin Color", splashSpinColor);
                    EditorGUILayout.Space();
                    splashDataColor = EditorGUILayout.ColorField("Splash Data Color", splashDataColor);
                    EditorGUILayout.Space();

                    EditorGUI.indentLevel -= 1;
                }

                /*
                showMobile = EditorGUILayout.Foldout(showMobile, "Mobile Package Options");
                if (showMobile)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();

                    // Device Orientation Options

                    EditorGUI.indentLevel -= 1;
                }
                */

                showModule = EditorGUILayout.Foldout(showModule, "Library Module Options");
                if (showModule)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    exportationOptions.FirelightAudio = EditorGUILayout.Toggle("Firelight Audio Lib", exportationOptions.FirelightAudio, GUILayout.ExpandWidth(false));
                    EditorGUI.BeginDisabledGroup(exportationOptions.FirelightAudio == false);
                    exportationOptions.FirelightAudioMode = (int)(FirelightAudio)EditorGUILayout.EnumPopup("", (FirelightAudio)exportationOptions.FirelightAudioMode, GUILayout.ExpandWidth(true));
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    exportationOptions.FirelightDebugMode = EditorGUILayout.Toggle("Firelight Debug Mode", exportationOptions.FirelightDebugMode);
                    EditorGUILayout.Space();

                    EditorGUI.indentLevel -= 1;
                }

                showExport = EditorGUILayout.Foldout(showExport, "Publish Content Options");
                if (showExport)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();

                    exportationOptions.ExportMetadata = EditorGUILayout.Toggle("Export Meta Data", exportationOptions.ExportMetadata);
                    EditorGUILayout.Space();
                    exportationOptions.ExportHttpModule = EditorGUILayout.Toggle("Export Web Module", exportationOptions.ExportHttpModule);
                    EditorGUILayout.Space();
                    exportationOptions.MinifyScriptFiles = EditorGUILayout.Toggle("Minify Project Script", exportationOptions.MinifyScriptFiles);
                    EditorGUILayout.Space();
                    exportationOptions.PrettyPrintExport = EditorGUILayout.Toggle("Debug Export Output", exportationOptions.PrettyPrintExport);
                    EditorGUILayout.Space();

                    EditorGUI.indentLevel -= 1;
                }

                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            if (GUILayout.Button(this.toolkitLabel, this.labelGuiStyle)) {
                Application.OpenURL("https://www.babylontoolkit.com");
            }
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Compile Script"))
            {
                Export(BuildType.Script);
            }
            if (GUILayout.Button("Export Scene File"))
            {
                Export(BuildType.Scene);
            }
            if (GUILayout.Button("Build And Preview"))
            {
                Export(BuildType.Project);
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Rebuild Editor Project"))
            {
                if (UnityEditor.EditorApplication.isCompiling == false) {
                    Tools.RebuildProjectSourceCode();
                    UnityEngine.Debug.Log("Queued project rebuild.");
                } else {
                    string msg = "There is a project compile in progress.";
                    UnityEngine.Debug.LogWarning(msg);
                    ShowMessage(msg);
                }
            }
            if (GUILayout.Button("Launch Preview Window"))
            {
                Execute(true);
            }
            EditorGUILayout.Space();
        }

        public void OnInspectorUpdate()
        {
            this.Repaint();
        }

        public string Build(string[] info = null)
        {
            string javascriptFile = null;
            try
            {
                var sceneInfo = info ?? GetSceneInfomation(false);
                string scriptPath = sceneInfo[2];
                string projectScript = sceneInfo[4];

                // Assemble javascript files
                javascriptFile = Tools.FormatProjectJavaScript(scriptPath, projectScript);
                if (exportationOptions.BuildJavaScript)
                {
                    ReportProgress(1, "Building project javascript files...");
                    Tools.BuildProjectJavaScript(scriptPath, javascriptFile);
                }

                // Compile typescript files
                if (exportationOptions.CompileTypeScript)
                {
                    ReportProgress(1, "Compiling project typescript files... This may take a while.");
                    string config = String.Empty;
                    string options = Path.Combine(Application.dataPath, "Babylon/Template/Config/options.json");
                    string ecmascript = (exportationOptions.EcmaScriptVersion == 1) ? "ES6" : "ES5";
                    if (File.Exists(options)) config = FileTools.ReadAllText(options);
                    this.buildResult = Tools.BuildProjectTypeScript(exportationOptions.DefaultNodeRuntimePath, exportationOptions.DefaultTypeScriptPath, scriptPath, javascriptFile, ecmascript, config);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
            return javascriptFile;
        }

        public void Export(BuildType build)
        {
            // Clear Console Log
             Tools.ClearConsoleLog(true);
            
            // Validate Project Platform
            if (!Tools.ValidateProjectPlatform()) return;

            // Validate lightmap bake in progress
            if (UnityEditor.EditorApplication.isCompiling == true)
            {
                string msg = "There is a project compile in progress.";
                UnityEngine.Debug.LogWarning(msg);
                ShowMessage(msg);
                return;
            }
            
            // Validate lightmap bake in progress
            if (Lightmapping.isRunning)
            {
                string msg = "There is a lightmap bake in progress.";
                UnityEngine.Debug.LogWarning(msg);
                ShowMessage(msg);
                return;
            }

            // Validate default project folder selected
            if (String.IsNullOrEmpty(ExporterWindow.DefaultProjectFolder))
            {
                string msg = "No default project file selected.";
                UnityEngine.Debug.LogWarning(msg);
                ShowMessage(msg);
                return;
            }

            // Validate default project folder exists
            if (!Directory.Exists(ExporterWindow.DefaultProjectFolder))
            {
                Directory.CreateDirectory(ExporterWindow.DefaultProjectFolder);
                if (!Directory.Exists(ExporterWindow.DefaultProjectFolder))
                {
                    string msg = "Failed to create default project file created.";
                    UnityEngine.Debug.LogWarning(msg);
                    ShowMessage(msg);
                    return;
                }
            }
            this.previewThread = (build == BuildType.Project);
            if (build == BuildType.Script) {
                this.CompileScriptFiles();
            } else {
                this.ExportSceneFiles();
            }
        }

        public void CompileScriptFiles()
        {
            string projectName = Application.productName;
            if (String.IsNullOrEmpty(projectName))
            {
                projectName = "Application";
            }
            GetSceneInfomation(true); // Note: Validate project folders
            string projectScript = projectName.Replace(" ", "");
            // if (!ExporterWindow.ShowMessage("Compile babylon project script: " + projectScript, "Babylon.js", "Compile", "Cancel"))
            // {
            //     return;
            // }
            try
            {
                this.buildResult = 0;
                string javascriptFile = null;
                // Save current scene info
                SaveSettings();
                ExporterWindow.logs.Clear();
                Stopwatch watch = new Stopwatch();
                watch.Start();
                ReportProgress(0, "Preparing script compilers...");
                if (exportationOptions.BuildJavaScript || exportationOptions.CompileTypeScript)
                {
                    javascriptFile = Build();
                }
                if (this.buildResult == 0)
                {
                    double compiled = watch.Elapsed.TotalSeconds;
                    string finished = String.Format("Compilation complete in {0:0.00} seconds.", compiled);
                    ReportProgress(1, finished);
                    EditorUtility.ClearProgressBar();
                    UnityEngine.Debug.Log(String.Format("Compiled {0} in {1:0.00} seconds.", Path.GetFileName(javascriptFile), compiled));
                    // ShowMessage(finished, "Compile Complete", "Done");
                }
                else
                {
                    watch.Stop();
                    string failed = String.Format("Failed to compile project in {0:0.00} seconds.", watch.Elapsed.TotalSeconds);
                    ReportProgress(1, failed);
                    EditorUtility.ClearProgressBar();
                    UnityEngine.Debug.LogWarning(failed);
                    ShowMessage(failed, "Compile Failed", "Cancel");
                }
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                ShowMessage("A problem occurred: " + ex.Message + ex.StackTrace, "Error");
            }
        }

        public void ExportSceneFiles()
        {
            bool preview = this.previewThread;
            try
            {
                // Get validate scene path info
                string[] sceneInfo = GetSceneInfomation(true);
                string sceneName = sceneInfo[0];
                string scenePath = sceneInfo[1];
                string scriptPath = sceneInfo[2];
                string outputFile = sceneInfo[3];
                string projectScript = sceneInfo[4];
                string exportMessage = "Export current babylon scene: " + sceneName;
                if (preview) exportMessage = "Build current babylon scene: " + sceneName;
                // if (!ExporterWindow.ShowMessage(exportMessage, "Babylon.js", (preview) ? "Build" : "Export", "Cancel"))
                // {
                //     return;
                // }

                // Validate preview server initialized
                if (preview == true && exportationOptions.HostPreviewType == 0)
                {
                    this.InitServer();
                }

                // Save current scene info
                SaveSettings();
                ExporterWindow.logs.Clear();
                Stopwatch watch = new Stopwatch();
                watch.Start();
                ReportProgress(0, "Preparing scene assets: " + scenePath);
                AssetDatabase.Refresh(ImportAssetOptions.Default);
                ReportProgress(0, "Exporting scene assets: " + scenePath);
                Tools.ValidateColorSpaceSettings();
                // Generate light and reflection data
                if (exportationOptions.ExportLightmaps) Tools.ValidateLightmapSettings();
                if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.Iterative)
                {
                    ReportProgress(1, "Generating lightmap textures... This may take a while.");
                    try {
                        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
                        Lightmapping.Bake();
                    } catch(Exception baker) {
                        UnityEngine.Debug.LogException(baker);
                    } finally {
                        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
                    }
                }

                // Save all open scenes
                ReportProgress(1, "Saving open scene information...");
                UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
                SceneController sceneController = Tools.GetSceneController();

                // Build project preview
                ReportProgress(1, "Generating index page libraries...  This may take a while.");
                this.buildResult = 0;
                string javascriptFile = null;
                // Full build and preview 
                if (preview)
                {
                    string sceneFilename = Path.GetFileName(outputFile);
                    if (exportationOptions.EnableDefaultScene && !String.IsNullOrEmpty(exportationOptions.DefaultSceneName)) {
                        sceneFilename = Path.GetFileNameWithoutExtension(exportationOptions.DefaultSceneName) + Path.GetExtension(outputFile);
                    }
                    string splash = "#" + ColorUtility.ToHtmlStringRGB(splashScreenColor);
                    string text = "#" + ColorUtility.ToHtmlStringRGB(splashTextColor);
                    string ring = "#" + ColorUtility.ToHtmlStringRGB(splashRingColor);
                    string spin = "#" + ColorUtility.ToHtmlStringRGB(splashSpinColor);
                    string data = "#" + ColorUtility.ToHtmlStringRGB(splashDataColor);
                    bool offline = (sceneController != null && sceneController.manifestOptions.exportManifest == true && sceneController.manifestOptions.enableOfflineSupport == true);
                    Tools.GenerateProjectIndexPage(ExporterWindow.DefaultProjectFolder, offline, exportationOptions.EnableMainMenu, exportationOptions.DefaultScenePath, sceneFilename, exportationOptions.DefaultScriptPath, Path.GetFileName(projectScript), exportationOptions.DefaultBinPath, exportationOptions.EnableAntiAliasing, exportationOptions.AdaptToDeviceRatio, splash, text, ring, spin, data);
                    if (exportationOptions.BuildJavaScript || exportationOptions.CompileTypeScript)
                    {
                        javascriptFile = Build(sceneInfo);
                    }
                }
                if (this.buildResult == 0)
                {
                    // Build current scene
                    ReportProgress(1, "Parsing scene object information...");
                    var sceneBuilder = new SceneBuilder(scenePath, sceneName, exportationOptions, sceneController, scriptPath);
                    sceneBuilder.ConvertFromUnity();
                    
                    // Write babylon scenes file(s)
                    sceneBuilder.WriteToBabylonFile(outputFile, javascriptFile);
                    watch.Stop();
                    double exported = watch.Elapsed.TotalSeconds;
                    string finished = String.Format("Exportation complete in {0:0.00} seconds.", exported);
                    ReportProgress(1, finished);
                    EditorUtility.ClearProgressBar();
                    sceneBuilder.GenerateStatus(logs);
                    UnityEngine.Debug.Log(String.Format("Exported {0} in {1:0.00} seconds.", Path.GetFileName(outputFile), exported));
                    string finish = preview ? "Preview" : "OK";
                    string close = preview ? "Done" : "";
                    // bool ok = ShowMessage(finished, "Export Complete", finish, close);
                    // if (preview && ok)
                    // {
                    Preview();
                    // }
                }
                else
                {
                    watch.Stop();
                    string failed = String.Format("Failed to build project in {0:0.00} seconds.", watch.Elapsed.TotalSeconds);
                    ReportProgress(1, failed);
                    EditorUtility.ClearProgressBar();
                    UnityEngine.Debug.LogWarning(failed);
                    ShowMessage(failed, "Build Failed", "Cancel");
                }
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                ShowMessage("A problem occurred: " + ex.Message + ex.StackTrace, "Error");
            }
        }

        public string[] GetSceneInfomation(bool validate)
        {
            string[] result = new string[6];
            string sceneName = SceneManager.GetActiveScene().name;
            if (String.IsNullOrEmpty(exportationOptions.DefaultBinPath))
            {
                exportationOptions.DefaultBinPath = "bin";
            }
            string binPath = Tools.FormatSafePath(Path.Combine(ExporterWindow.DefaultProjectFolder, exportationOptions.DefaultBinPath));
            if (validate && !Directory.Exists(binPath))
            {
                Directory.CreateDirectory(binPath);
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultBuildPath))
            {
                exportationOptions.DefaultBuildPath = "debug";
            }
            string buildPath = Tools.FormatSafePath(Path.Combine(ExporterWindow.DefaultProjectFolder, exportationOptions.DefaultBuildPath));
            if (validate && !Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultScenePath))
            {
                exportationOptions.DefaultScenePath = "scenes";
            }
            string scenePath = Tools.FormatSafePath(Path.Combine(ExporterWindow.DefaultProjectFolder, exportationOptions.DefaultScenePath));
            if (validate && !Directory.Exists(scenePath))
            {
                Directory.CreateDirectory(scenePath);
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultScriptPath))
            {
                exportationOptions.DefaultScriptPath = "scripts";
            }
            string scriptPath = Tools.FormatSafePath(Path.Combine(ExporterWindow.DefaultProjectFolder, exportationOptions.DefaultScriptPath));
            if (validate && !Directory.Exists(scriptPath))
            {
                Directory.CreateDirectory(scriptPath);
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultIndexPage))
            {
                exportationOptions.DefaultIndexPage = "index.html";
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultGamePage))
            {
                exportationOptions.DefaultGamePage = "game.html";
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultSceneName))
            {
                exportationOptions.DefaultSceneName = "default";
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultPlatformApp))
            {
                exportationOptions.DefaultPlatformApp = "my-babylontoolkit://app";
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultTypeScriptPath))
            {
                exportationOptions.DefaultTypeScriptPath = Tools.GetDefaultTypeScriptPath();
            }
            if (String.IsNullOrEmpty(exportationOptions.DefaultNodeRuntimePath))
            {
                exportationOptions.DefaultNodeRuntimePath = Tools.GetDefaultNodeRuntimePath();
            }
            if (exportationOptions.DefaultServerPort != ExporterWindow.DefaultPort)
            {
                exportationOptions.DefaultServerPort = ExporterWindow.DefaultPort;
            }
            string projectName = Application.productName;
            if (String.IsNullOrEmpty(projectName))
            {
                projectName = "Application";
            }
            string outputFile = Tools.FormatSafePath(Path.Combine(scenePath, sceneName.Replace(" ", "") + Tools.GetSceneFileExtension()));
            string projectScript = Tools.FormatSafePath(Path.Combine(scenePath, projectName.Replace(" ", "") + ".babylon"));
            result[0] = sceneName;
            result[1] = scenePath;
            result[2] = buildPath;
            result[3] = outputFile;
            result[4] = projectScript;
            return result;
        }

        public void Preview()
        {
            if (exportationOptions.HostPreviewType == 0)
            {
                this.InitServer();
            }
            if (Application.platform == RuntimePlatform.WindowsEditor && exportationOptions.RunWindowsPlatform == true)
            {
                Application.OpenURL(exportationOptions.DefaultPlatformApp);
            }
            else
            {
                string hostProtocol = "http://";
                string previewUrl = hostProtocol + "localhost:" + exportationOptions.DefaultServerPort.ToString() + "/" + exportationOptions.DefaultIndexPage;
                if (exportationOptions.HostPreviewType == 1) {
                    previewUrl = exportationOptions.RemoteServerPath.TrimEnd('/') + "/" + exportationOptions.DefaultIndexPage;
                }
                Application.OpenURL(previewUrl);
            }
        }

        public void Execute(bool preview)
        {
            if (preview)
            {
                Preview();
            }
            else
            {
                Export(BuildType.Project);
            }
        }
    }
}

