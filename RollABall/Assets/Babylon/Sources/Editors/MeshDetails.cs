using System;
using UnityEngine;
using Unity3D2Babylon;

namespace UnityEditor
{
    [AddComponentMenu("Babylon/System Components/Mesh Details", 7)]
    public sealed class MeshDetails: EditorScriptComponent
    {
        [Header("-Entity Properties-")]
        public bool enabledMeshEntity = true;
        public bool forceCheckCollision = false;
        public string meshComponentTags = null;

        [Header("-Detail Properties-")]

        public BabylonRuntimeProperties meshRuntimeProperties = null;
        public BabylonEllipsoidProperties meshEllipsoidProperties = null;
        public BabylonOverrideVisibility meshVisibilityProperties = null;
    }

    [CustomEditor(typeof(MeshDetails)), CanEditMultipleObjects]
    public class MeshDetailsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            MeshDetails myScript = (MeshDetails)target;
        }
    }
}