using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using BabylonHosting;

namespace Unity3D2Babylon
{
    public class ExportationOptions
    {
        public string AlternateExport { get; set; }
        public int EcmaScriptVersion { get; set; }
        public int HostPreviewType { get; set; }
        public bool BuildJavaScript { get; set; }
        public bool CompileTypeScript { get; set; }
        public bool FirelightAudio { get; set; }
        public int FirelightAudioMode { get; set; }
        public bool FirelightDebugMode { get; set; }
        public bool TruevisionGraphics { get; set; }
        public bool PrettyPrintExport { get; set; }
        public bool EnableAntiAliasing { get; set; }
        public bool AdaptToDeviceRatio { get; set; }
        public string RemoteServerPath { get; set; }
        public bool AttachUnityEditor { get; set; }
        public bool ShowDebugSockets { get; set; }
        public bool ShowDebugColliders { get; set; }
        public bool ShowPhysicsImposter { get; set; }
        public float ColliderVisibility { get; set; }
        public float SocketColliderSize { get; set; }
        public bool StaticVertexLimit { get; set; }
        public bool MinifyScriptFiles { get; set; }
        public int TerrainCoordinatesIndex { get; set; }
        public int TerrainAtlasSize { get; set; }
        public int TerrainImageScaling { get; set; }
        public bool EnableDefaultScene { get; set; }
        public bool RunWindowsPlatform { get; set; }
        public bool EnableXboxLive { get; set; }
        public bool EnableMainMenu { get; set; }
        public bool ExportMetadata { get; set; }
        public bool ExportLightmaps { get; set; }
        public bool ExportCollisions { get; set; }
        public int BakedLightsMode { get; set; }
        public bool ExportPhysics { get; set; }
        public bool ExportHttpModule { get; set; }
        public bool GenerateColliders { get; set; }
        public int TerrainExportFile { get; set; }
        public bool EnforceImageEncoding { get; set; }
        public bool CreateMaterialInstance { get; set; }
        public string CustomWindowsSandbox { get; set; }
        public bool ShowRenderStats { get; set; }
        public int SceneRenderStats { get; set; }
        public int ImageEncodingOptions { get; set; }
        public int DefaultTextureQuality { get; set; }
        public int DefaultPhysicsEngine { get; set; }
        public int DefaultLightmapBaking { get; set; }
        public int DefaultCoordinatesIndex { get; set; }
        public int DefaultColliderDetail { get; set; }
        public string DefaultPlatformApp { get; set; }
        public string DefaultGamePage { get; set; }
        public string DefaultIndexPage { get; set; }
        public string DefaultBinPath { get; set; }
        public string DefaultBuildPath { get; set; }
        public string DefaultScenePath { get; set; }
        public string DefaultScriptPath { get; set; }
        public int DefaultServerPort { get; set; }
        public string DefaultSceneName { get; set; }
        public string DefaultTypeScriptPath { get; set; }
        public string DefaultNodeRuntimePath { get; set; }
        public int DefaultWindowsLaunchMode { get; set; }
        public float[] SplashScreenColor { get; set; }
        public float[] SplashTextColor { get; set; }
        public float[] SplashRingColor { get; set; }
        public float[] SplashSpinColor { get; set; }
        public float[] SplashDataColor { get; set; }

        public ExportationOptions()
        {
            HostPreviewType = 0;
            EcmaScriptVersion = 0;
            RemoteServerPath = "http://localhost/project";
            AlternateExport = null;
            FirelightAudio = false;
            FirelightAudioMode = 0;
            FirelightDebugMode = false;
            TruevisionGraphics = true;
            EnableDefaultScene = false;
            BuildJavaScript = true;
            CompileTypeScript = false;
            EnableMainMenu = false;
            EnableXboxLive = false;
            ShowRenderStats = false;
            SceneRenderStats = 0;
            RunWindowsPlatform = false;
            EnableAntiAliasing = true;
            AdaptToDeviceRatio = true;
            AttachUnityEditor = false;
            ShowDebugSockets = false;
            ShowDebugColliders = false;
            ShowPhysicsImposter = false;
            ColliderVisibility = 0.25f;
            SocketColliderSize = 0.125f;
            StaticVertexLimit = false;
            TerrainAtlasSize = 4096;
            TerrainCoordinatesIndex = 0;
            TerrainImageScaling = 1;
            ExportMetadata = true;
            ExportLightmaps = true;
            ExportCollisions = true;
            ExportPhysics = true;
            ExportHttpModule = true;
            BakedLightsMode = 0;
            TerrainExportFile = 1;
            GenerateColliders = true;
            EnforceImageEncoding = true;
            CreateMaterialInstance = true;
            ImageEncodingOptions = 0;
            MinifyScriptFiles = false;
            PrettyPrintExport = false;
            CustomWindowsSandbox = String.Empty;
            SplashScreenColor = null;
            SplashTextColor = null;
            SplashRingColor = null;
            SplashSpinColor = null;
            SplashDataColor = null;
            DefaultTextureQuality = 100;
            DefaultLightmapBaking = 0;
            DefaultCoordinatesIndex = 1;
            DefaultColliderDetail = 2;
            DefaultPhysicsEngine = 0;
            DefaultServerPort = 8888;
            DefaultBinPath = "bin";
            DefaultBuildPath = "debug";
            DefaultScenePath = "scenes";
            DefaultScriptPath = "scripts";
            DefaultIndexPage = "index.html";
            DefaultGamePage = "game.html";
            DefaultSceneName = "default";
            DefaultPlatformApp = "my-babylontoolkit://app";
            DefaultTypeScriptPath = Tools.GetDefaultTypeScriptPath();
            DefaultNodeRuntimePath = Tools.GetDefaultNodeRuntimePath();
            DefaultWindowsLaunchMode = 0;
        }
    }
}
