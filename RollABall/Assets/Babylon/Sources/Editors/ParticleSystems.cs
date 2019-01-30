using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEditor
{
    [AddComponentMenu("Babylon/System Components/Particle Systems", 30)]
	public sealed class ParticleSystems : EditorScriptComponent
	{
        [BabylonProperty]
        public BabylonSystemMode systemMode = BabylonSystemMode.CentralProcessingUnit;
        [BabylonProperty, Range(1024, 16384)]
        public int randomSize = 4096;
        [BabylonProperty]
        public int activeCount = 100000;
        
        [Header("-System Properties-")]

        [BabylonProperty]
        public Texture2D textureImage = null;
        [BabylonProperty]
        public string particleName = null;
        [BabylonProperty, Range(0.0f, 100000.0f)]
        public float duration = 0.0f;
        [BabylonProperty]
        public bool autoStart = true;
        [BabylonProperty]
        public bool loopPlay = false;
        [BabylonProperty]
        public float delayTime = 0.0f;
        [BabylonProperty]
        public BabylonParticleBlend blendMode = BabylonParticleBlend.OneOne;

        [Header("-Particle Properties-")]
        
        [BabylonProperty]
        public int capacity = 1000;
        [BabylonProperty]
        public float startSpeed = 0.01f;
        [BabylonProperty]
        public BabylonParticleEmission emitType = BabylonParticleEmission.Rate;
        [BabylonProperty]
        public float emitRate = 10.0f;
        [BabylonProperty]
        public BabylonParticleBusrt[] emitBurst = null;
        [BabylonProperty]
        public Vector2 emitPower = new Vector2(1.0f, 1.0f);
        [BabylonProperty]
        public Vector2 lifeTime = new Vector2(1.0f, 1.0f);
        [BabylonProperty]
        public Vector2 particleSize = new Vector2(1.0f, 1.0f);
        [BabylonProperty]
        public Vector2 angularSpeed = new Vector2(0.0f, 0.0f);

        [Header("-Color Properties-")]
        
        [BabylonProperty]
        public Color color1 = Color.white;
        [BabylonProperty]
        public Color color2 = Color.white;
        [BabylonProperty]
        public Color colorDead = Color.black;
        [BabylonProperty]
        public Color textureMask = Color.white;

        [Header("-Shape Properties-")]

        [BabylonProperty]
        public BabylonShapePreset preset = BabylonShapePreset.BoxVolume;
        [BabylonProperty, Range(0.0f, 10.0f)]
        public float angle = 1.0f;
        [BabylonProperty, Range(0.0f, 10.0f)]
        public float radius = 1.0f;
        [BabylonProperty]
        public Vector3 direction1 = new Vector3(0.0f, 1.0f, 0.0f);
        [BabylonProperty]
        public Vector3 direction2 = new Vector3(0.0f, 1.0f, 0.0f);
        [BabylonProperty, Range(0.0f, 1.0f)]
        public float randomizer = 0.0f;
        [BabylonProperty]
        public Vector3 minEmitBox = new Vector3(0.0f, 0.0f, 0.0f);
        [BabylonProperty]
        public Vector3 maxEmitBox = new Vector3(0.0f, 0.0f, 0.0f);

        [Header("-Gravity Properties-")]

        [BabylonProperty]
        public BabylonGravityMode gravityMode = BabylonGravityMode.ManualVector;
        [BabylonProperty]
        public Vector3 gravityVector = new Vector3(0.0f, 0.0f, 0.0f);
        [Range(-10.0f, 10.0f)]
        [BabylonProperty]
        public float gravityMultiplier = 0.0f;
        
		public ParticleSystems()
		{
			this.babylonClass = "BABYLON.UniversalParticleSystem";
            this.OnExportProperties = this.OnExportPropertiesHandler;
		}

        public void OnExportPropertiesHandler(SceneBuilder sceneBuilder, GameObject unityGameObject, Dictionary<string, object> propertyBag)
		{
            string textureImagePath = ExporterWindow.exportationOptions.DefaultScenePath + "/";
			propertyBag.Add("textureImagePath", textureImagePath);
		}

        public void SetSceneGravityVector()
        {
            if (this.gravityMultiplier != 0.0f) this.gravityMultiplier = 0.0f;
        }

        public void SetSceneGravityMultiplier()
        {
            if (this.gravityVector.x != 0.0f) this.gravityVector.x = 0.0f;
            if (this.gravityVector.y != 0.0f) this.gravityVector.y = 0.0f;
            if (this.gravityVector.z != 0.0f) this.gravityVector.z = 0.0f;
        }
	}

    [CustomEditor(typeof(ParticleSystems)), CanEditMultipleObjects]
    public class ParticleSystemsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ParticleSystems myScript = (ParticleSystems)target;
            // Validate Duration Based Properties
            if (myScript.duration <= 0.0f) {
                if (myScript.loopPlay == true) {
                    myScript.loopPlay = false;
                }
                if (myScript.emitType == BabylonParticleEmission.Burst) {
                    myScript.emitType = BabylonParticleEmission.Rate;
                }
            }
            if (myScript.systemMode == BabylonSystemMode.GraphicsProcessingUnit) {
                if (myScript.emitType == BabylonParticleEmission.Burst) {
                    myScript.emitType = BabylonParticleEmission.Rate;
                }
            }
            // Force Scene Gravity Vector And Multiplier Property Values
            if (myScript.gravityMode == BabylonGravityMode.ManualVector) {
                myScript.SetSceneGravityVector();
            } else if (myScript.gravityMode == BabylonGravityMode.SceneMultiplier) {
                myScript.SetSceneGravityMultiplier();
            }
        }
    }

    [System.Serializable]
    public enum BabylonSystemMode
    {
        CentralProcessingUnit = 0,
        GraphicsProcessingUnit = 1
    }

    [System.Serializable]
    public enum BabylonParticleEmission {
        Rate = 0,
        Burst = 1
    }

    [System.Serializable]
    public enum BabylonGravityMode
    {
        ManualVector = 0,
        SceneMultiplier = 1
    }

    [System.Serializable]
    public enum BabylonShapePreset
    {
        BoxVolume = 0,
        ConeVolume = 1,
        SphereRadius = 2,
        SphereDirection = 3,
    }

    [System.Serializable]
    public class BabylonParticleBusrt
    {
        [BabylonProperty]
        public float time = 0.0f;

        [BabylonProperty]
        public int minCount = 100;
        
        [BabylonProperty]
        public int maxCount = 100;
    }
}
