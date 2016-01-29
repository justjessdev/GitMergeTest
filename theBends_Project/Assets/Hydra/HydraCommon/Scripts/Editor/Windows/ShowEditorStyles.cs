// <copyright file=ShowEditorStyles company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Windows
{
	public class ShowEditorStyles : HydraEditorWindow
	{
		public const string TITLE = "Show Editor Styles";

		private static Vector2 s_Scroll;
		private static bool s_IsSelected;
		private static GUISkin s_Skin;

		private static bool s_IsInspector = true;
		private static bool s_IsScene;
		private static bool s_IsGame;

		private static float s_Width = 150.0f;
		private static float s_Height = 20.0f;
		private static string s_Text = "Hello World";

		/// <summary>
		/// 	Shows the window.
		/// </summary>
		[MenuItem(MENU + TITLE)]
		public static void Init()
		{
			GetWindow<ShowEditorStyles>(false, TITLE, true);
		}

		#region Messages

		/// <summary>
		/// 	Called to draw the window contents.
		/// </summary>
		protected override void OnGUI()
		{
			base.OnGUI();

			s_Width = EditorGUILayout.Slider("Width", s_Width, 0, 200);
			s_Height = EditorGUILayout.Slider("Height", s_Height, 0, 50);
			s_Text = EditorGUILayout.TextField("Text", s_Text);

			PrintHeader();
			s_Scroll = GUILayout.BeginScrollView(s_Scroll);

			s_IsInspector = EditorGUILayout.Foldout(s_IsInspector, "Inspector skin");
			if (s_IsInspector)
				PrintStyles(LoadGUISkin(EditorSkin.Inspector));

			s_IsScene = EditorGUILayout.Foldout(s_IsScene, "Scene skin");
			if (s_IsScene)
				PrintStyles(LoadGUISkin(EditorSkin.Scene));

			s_IsGame = EditorGUILayout.Foldout(s_IsGame, "Game skin");
			if (s_IsGame)
				PrintStyles(LoadGUISkin(EditorSkin.Game));

			GUILayout.EndScrollView();
		}

		#endregion

		private static void PrintHeader()
		{
			GUILayout.BeginHorizontal();

			GUILayout.Label("Name", GUILayout.Width(200));

			GUILayout.Label("Label", GUILayout.Width(150));

			GUILayout.Label("Button", GUILayout.Width(150));

			GUILayout.Label("Textfield", GUILayout.Width(150));

			GUILayout.Label("Toggle", GUILayout.Width(150));

			EditorGUILayout.EndHorizontal();
		}

		private static void PrintStyles(GUISkin skin)
		{
			EditorGUILayout.BeginVertical();
			foreach (GUIStyle style in skin.customStyles)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label("[" + style.fixedWidth + "," + style.fixedHeight + "] " + style.name, GUILayout.MinWidth(200),
								GUILayout.Width(200), GUILayout.MaxHeight(s_Height));

				GUILayout.Label(s_Text, style, GUILayout.MaxWidth(s_Width), GUILayout.MaxHeight(s_Height));

				GUILayout.Space(150 - s_Width);

				GUILayout.Button(s_Text, style, GUILayout.MaxWidth(s_Width), GUILayout.MaxHeight(s_Height));

				GUILayout.Space(150 - s_Width);

				EditorGUILayout.TextField(s_Text, style, GUILayout.MaxWidth(s_Width), GUILayout.MaxHeight(s_Height));

				GUILayout.Space(150 - s_Width);

				s_IsSelected = GUILayout.Toggle(s_IsSelected, s_Text, style, GUILayout.MaxWidth(s_Width),
												GUILayout.MaxHeight(s_Height));

				GUILayout.Space(150 - s_Width);

				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		public static GUISkin LoadGUISkin(EditorSkin editorSkin)
		{
			if (s_Skin != null)
				return s_Skin;

			s_Skin = EditorGUIUtility.GetBuiltinSkin(editorSkin);

			return s_Skin;
		}
	}
}
