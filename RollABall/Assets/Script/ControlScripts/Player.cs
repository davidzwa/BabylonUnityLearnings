/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class Player : EditorScriptComponent
	{
        [Header("-Script Properties-")]

        [BabylonProperty]
        public string hello = "Hello World";

		protected Player()
		{
			this.babylonClass = "PROJECT.Player";
		}
	}
}