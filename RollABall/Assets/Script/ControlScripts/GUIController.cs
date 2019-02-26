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

        [BabylonProperty] public string Title = "Title";
        [BabylonProperty] public string TitleText = "Awesome game";

        [BabylonProperty] public string ScoreName = "Score";
		[BabylonProperty] public string Score = "Score";

        [BabylonProperty] public string MenuButtonName = "MenuButton";
		[BabylonProperty] public string MenuButton = "Menu";
		
		
		
		
		protected GUIController()
		{
			this.babylonClass = "PROJECT.GUIController";
		}
	}
}