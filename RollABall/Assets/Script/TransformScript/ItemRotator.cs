/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class ItemRotator : EditorScriptComponent
	{
        [Header("-Script Properties-")]

        [BabylonProperty, Range(0.1f, 0.1f)]
        public float speed = 0.1f;

		protected ItemRotator()
		{
			this.babylonClass = "PROJECT.ItemRotator";
		}
	}
}