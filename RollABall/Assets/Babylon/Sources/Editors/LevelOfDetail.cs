using System;
using UnityEngine;
using Unity3D2Babylon;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor
{
    [RequireComponent(typeof(UnityEngine.LODGroup))]
    [AddComponentMenu("Babylon/System Components/Level Of Detail", 15)]
    public sealed class LevelOfDetail : EditorScriptComponent
    {
        [Unity3D2Babylon.ReadOnly]
        public LODGroup levelDetailGroup = null;

        [Range(0.01f, 1.0f)]
        public float farClipPlaneScale = 0.1f;

        [HideInInspector]
        public bool showGroupLevels = true;
        
        [HideInInspector, Unity3D2Babylon.ReadOnly]
        public List<string> groupLevelDistances = new List<string>();
    }

    [CustomEditor(typeof(LevelOfDetail)), CanEditMultipleObjects]
    public class LevelDistanceEditor : Editor
    {
        public void OnEnable()
        {
            LevelOfDetail myScript = (LevelOfDetail)target;
            if (myScript.levelDetailGroup == null) {
                myScript.levelDetailGroup = myScript.gameObject.GetComponent<LODGroup>();
            }
            RecalculateLevelDistances();
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelOfDetail myScript = (LevelOfDetail)target;
            if (myScript.levelDetailGroup == null) {
                myScript.levelDetailGroup = myScript.gameObject.GetComponent<LODGroup>();
            }
            serializedObject.Update();
            var groupLevel = serializedObject.FindProperty("groupLevelDistances");
            if (groupLevel != null) LevelDistanceEditorList.Show(groupLevel, this, false, false, "", ref myScript.showGroupLevels);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
        }
        public void RecalculateLevelDistances()
        {
            LevelOfDetail myScript = (LevelOfDetail)target;
            if (myScript.levelDetailGroup != null) {
                myScript.groupLevelDistances.Clear();
                var lodGroup = myScript.levelDetailGroup;
                float farClipingPlane = (Camera.main != null) ? Camera.main.farClipPlane : 1000.0f;
                LOD[] lods = lodGroup.GetLODs();
                float startingPercent = -1f;
                float endingPercent = -1f;
                float lastPercent = 0f;
                int startRange = 0;
                int lodIndex = 0;
                foreach (var lod in lods) {
                    endingPercent = lod.screenRelativeTransitionHeight;
                    if (startingPercent == -1) startingPercent = 1;
                    else startingPercent = lastPercent;
                    float lodPercent = startingPercent - endingPercent;
                    string lodFormatted = String.Format("LOD{0} - Babylon Camera Distance: {1}", lodIndex.ToString(), startRange.ToString());
                    myScript.groupLevelDistances.Add(lodFormatted);
                    lastPercent = endingPercent;
                    startRange += Unity3D2Babylon.Tools.CalculateCameraDistance(farClipingPlane, myScript.farClipPlaneScale, lodPercent);
                    lodIndex++;
                }
                float startCulling = startRange;
                string cullingFormatted = String.Format("LODX - Babylon Camera Distance: {0}", startCulling.ToString());
                myScript.groupLevelDistances.Add(cullingFormatted);
            }
        }
    }
    public static class LevelDistanceEditorList
    {
        public static void Show(SerializedProperty item, LevelDistanceEditor editor, bool indent, bool foldout, string label, ref bool visibility)
        {
            if (foldout == true) {
                visibility = EditorGUILayout.Foldout(visibility, label);
                if (visibility) ShowItems(item, editor, indent);
            } else {
                EditorGUILayout.PropertyField(item);
                ShowItems(item, editor, indent);
            }
        }
        private static void ShowItems(SerializedProperty item, LevelDistanceEditor editor, bool indent)
        {
            if (item.isArray) {
                if (indent) EditorGUI.indentLevel += 1;
                if (item.isExpanded) {
                    if (item.name == "groupLevelDistances") {
                        editor.RecalculateLevelDistances();
                        //EditorGUILayout.PropertyField(item.FindPropertyRelative("Array.size"));                        
                        for (int i = 0; i < item.arraySize; i++) {
                            EditorGUILayout.PropertyField(item.GetArrayElementAtIndex(i), GUIContent.none);
                        }
                    } else {
                        for (int i = 0; i < item.arraySize; i++) {
                            EditorGUILayout.PropertyField(item.GetArrayElementAtIndex(i));
                        }
                    }
                }
                if (indent) EditorGUI.indentLevel -= 1;
            }
        }
    }
}