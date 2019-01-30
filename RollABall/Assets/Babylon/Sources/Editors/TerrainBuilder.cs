using System;
using UnityEngine;
using Unity3D2Babylon;

namespace UnityEditor
{
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(UnityEngine.TerrainCollider))]
    [AddComponentMenu("Babylon/System Components/Terrain Builder", 18)]
    public sealed class TerrainBuilder : EditorScriptComponent
    {
        [Header("-Terrain Properties-")]
        
        [Range(0.0f, 100.0f)]
        public float textureScaling = 1.0f;
        public bool receiveShadows = true;

        [Header("-World Properties-")]
        
        public float offsetPositionX = 0.0f;
        public float offsetPositionZ = 0.0f;

        [Header("-Collider Properties-")]
        
        public BabylonTerrainSegments terrainCollisions = BabylonTerrainSegments.SingleMesh;
        public BabylonTerrainSegments lodGroundSegments = BabylonTerrainSegments.SingleMesh;
    }
}