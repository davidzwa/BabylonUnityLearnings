using System.Runtime.Serialization;

namespace BabylonExport.Entities
{
    [DataContract]
    public class BabylonShadowGenerator
    {
        [DataMember]
        public int mapSize { get; set; }

        [DataMember]
        public float bias { get; set; }

        [DataMember]
        public float normalBias { get; set; }

        [DataMember]
        public string lightId { get; set; }

        [DataMember]
        public float? depthScale { get; set; }

        [DataMember]
        public float darkness { get; set; }

        [DataMember]
        public float blurScale { get; set; }

        [DataMember]
        public float blurKernel { get; set; }

        [DataMember]
        public bool useKernelBlur { get; set; }

        [DataMember]
        public float blurBoxOffset { get; set; }

        [DataMember]
        public int filteringQuality { get; set; }

        [DataMember]
        public float frustumEdgeFalloff { get; set; }

        [DataMember]
        public bool transparencyShadow { get; set; }

        [DataMember]
        public bool forceBackFacesOnly { get; set; }

        [DataMember]
        public bool usePoissonSampling { get; set; }

        [DataMember]
        public bool useExponentialShadowMap { get; set; }

        [DataMember]
        public bool useBlurExponentialShadowMap { get; set; }

        [DataMember]
        public bool useCloseExponentialShadowMap { get; set; }

        [DataMember]
        public bool useBlurCloseExponentialShadowMap { get; set; }

        [DataMember]
        public bool usePercentageCloserFiltering { get; set; }
        
        [DataMember]
        public bool useContactHardeningShadow { get; set; }

        [DataMember]
        public float contactHardeningLightSizeUVRatio { get; set; }

        [DataMember]
        public string[] renderList { get; set; }

        public BabylonShadowGenerator()
        {
            bias = 0.00005f;
            normalBias = 0.04f;
            darkness = 0;
            blurScale = 2;
            blurKernel = 1;
            blurBoxOffset = 0;
            depthScale = null;
            forceBackFacesOnly = false;
            filteringQuality = 0;
            transparencyShadow = false;
            frustumEdgeFalloff = 0.0f;
            useKernelBlur = false;
            useBlurCloseExponentialShadowMap = false;
            useBlurExponentialShadowMap = false;
            useCloseExponentialShadowMap = false;
            useExponentialShadowMap = false;
            usePercentageCloserFiltering = false;
            usePoissonSampling = false;
            useContactHardeningShadow = false;
            contactHardeningLightSizeUVRatio = 0.1f;
        }
    }
}
