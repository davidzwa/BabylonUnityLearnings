using System;
using UnityEngine;
using Unity3D2Babylon;

namespace UnityEditor
{
    [RequireComponent(typeof(UnityEngine.Light))]
    [AddComponentMenu("Babylon/System Components/Shadow Map Generator", 99)]
    public sealed class ShadowGenerator : EditorScriptComponent
    {
        [Header("-Shadow Properties-")]
       
        public BabylonLightingFilter shadowMapFilter = BabylonLightingFilter.BlurCloseExponentialShadowMap;
        [Range(128, 8192)]
        public int shadowMapSize = 1024;
        [Range(0.0f, 1.0f)]
        public float shadowMapBias = 0.00005f;
        [Range(0.0f, 1.0f)]
        public float shadowNormBias = 0.0f;
        [Range(0.0f, 100.0f)]
        public float shadowNearPlane = 0.1f;
        [Range(0.0f, 1000.0f)]
        public float shadowFarPlane = 100.0f;

        [Header("-Process Properties-")]
        
        public bool shadowKernelBlur = false;
        [Range(0.0f, 64.0f)]
        public float shadowBlurKernel = 1.0f;
        [Range(0.0f, 10.0f)]
        public float shadowBlurScale = 2.0f;
        [Range(0.0f, 10.0f)]
        public float shadowBlurOffset = 0.0f;
        [Range(0.0f, 1.0f)]
        public float shadowOrthoScale = 0.1f;
        [Range(0.0f, 1.0f)]
        public float shadowStrengthScale = 1.0f;
        public float shadowDepthScale = 50.0f;
        [Range(0.0f, 1.0f)]
        public float contactHardening = 0.1f;

        [Header("-Render Properties-")]
        
        public BabylonFilteringQuality shadowFilterQuality = BabylonFilteringQuality.High;
        [Range(0.0f, 1.0f)]
        public float frustumEdgeFalloff = 0.0f;
        public bool forceBackFacesOnly = true;
        public bool transparencyShadow = false;
    }
}
