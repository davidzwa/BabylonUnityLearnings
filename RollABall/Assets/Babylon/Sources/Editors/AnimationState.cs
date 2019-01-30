using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Unity3D2Babylon;

namespace UnityEditor
{
    [AddComponentMenu("Babylon/System Components/Animation State", 25)]
    public sealed class AnimationState : EditorScriptComponent
    {
        [Header("-Animation Properties-")]
        
        [BabylonProperty]
        public BabylonAnimationMode controlType = BabylonAnimationMode.DisabledAnimation;
        [BabylonProperty]
        public TimelineStepping timelineStep = TimelineStepping.ThirtyFramesPerSecond;
        [BabylonProperty, Range(0.1f, 10.0f)]
        public float playbackSpeed = 1.0f;
        [BabylonProperty]
        public bool enableTransitions = true;
        [BabylonProperty]
        public bool enableEvents = true;

        [Header("-Machine Properties-")]
        
        [BabylonProperty]
        public bool enableStateMachine = true;
        [Unity3D2Babylon.ReadOnly]
        public string stateMachineSource = null;

        public AnimationState()
        {
            this.babylonClass = "BABYLON.AnimationState";
            this.OnExportProperties = this.OnExportPropertiesHandler;
        }

        void OnAnimatorMove()
        {
            // *************************************** //
            // Handle Scripting Root Motion Reference  //
            // *************************************** //
        }        
        public void OnExportPropertiesHandler(SceneBuilder sceneBuilder, GameObject unityGameObject, Dictionary<string, object> propertyBag)
        {
            string stateMachineName = "Unknown";
            if (!String.IsNullOrEmpty(this.stateMachineSource)) stateMachineName = this.stateMachineSource;
            propertyBag.Add("stateMachineName", stateMachineName);
        }
    }

    [CustomEditor(typeof(AnimationState)), CanEditMultipleObjects]
    public class AnimationStateEditor : Editor
    {
        public void OnEnable()
        {
            AnimationState myScript = (AnimationState)target;
            var animator = myScript.GetComponent<Animator>();
            var legacy = myScript.GetComponent<Animation>();
            if (animator != null) {
                myScript.stateMachineSource = "" + animator.name;
            } else if (legacy != null) {
                myScript.stateMachineSource = "" + legacy.name;
            } else {
                myScript.stateMachineSource = "";
            }
        }
    }
}
