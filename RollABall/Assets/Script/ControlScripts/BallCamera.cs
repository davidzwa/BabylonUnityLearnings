/* Babylon Editor Script Component (C# UnityScript) */

using System;
using UnityEditor;
using UnityEngine;
using Unity3D2Babylon;

namespace MyProject
{
	public class BallCamera : EditorScriptComponent
	{
        [Header("-Script Properties-")]

        [BabylonProperty]
        public string hello = "Hello World";

		[BabylonProperty]
		public GameObject player;

		protected BallCamera()
		{
			this.babylonClass = "PROJECT.BallCamera";
		}
	}
}