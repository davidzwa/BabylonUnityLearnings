/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class BallScene : EditorScriptComponent
	{
        [Header("-Script Properties-")]

        [BabylonProperty]
        public string hello = "Hello World";

		protected BallScene()
		{
			this.babylonClass = "PROJECT.BallScene";
		}
	}
}