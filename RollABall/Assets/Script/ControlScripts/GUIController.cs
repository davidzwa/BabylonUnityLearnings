/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class GUIController : EditorScriptComponent
	{
        [Header("-GUI Properties-")]

        [BabylonProperty]
        public string GUITitle = "Title";

		protected GUIController()
		{
			this.babylonClass = "PROJECT.GUIController";
		}
	}
}