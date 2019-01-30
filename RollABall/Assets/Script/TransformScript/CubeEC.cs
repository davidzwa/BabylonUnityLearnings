/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class CubeEC : EditorScriptComponent
	{
        [Header("-Script Properties-")]

        [BabylonProperty]
        public Vector3 hello;

		protected CubeEC()
		{
			this.babylonClass = "PROJECT.CubeComponent";
		}
	}
}