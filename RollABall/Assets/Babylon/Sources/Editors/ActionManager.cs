using System;
using UnityEngine;
using Unity3D2Babylon;
using System.Collections.Generic;

namespace UnityEditor
{
    [AddComponentMenu("Babylon/System Components/Action Manager", 11)]
    public sealed class ActionManager : EditorScriptComponent
    {
        [Header("-Action Properties-")]

        [BabylonProperty]
        public bool enableActions = false;

        public ActionManager()
        {
            this.babylonClass = "BABYLON.ActionManagerComponent";
            this.OnExportProperties = this.OnExportPropertiesHandler;
        }

        public void OnExportPropertiesHandler(SceneBuilder sceneBuilder, GameObject unityGameObject, Dictionary<string, object> propertyBag)
        {
            bool onCustomAction = false;
            propertyBag.Add("onCustomAction", onCustomAction);
        }
    }
}
