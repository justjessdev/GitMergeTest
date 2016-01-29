// <copyright file=ReplaceGameObjects company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Windows
{
	/// <summary>
	/// 	ReplaceGameObjects allows for batch replacement of scene instances with
	/// 	a prefab reference, keeping transform data and names.
	/// 	
	/// 	Based on -
	/// 		CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
	/// 		http://forum.unity3d.com/threads/replace-game-object-with-prefab.24311/
	/// </summary>
	public class ReplaceGameObjects : HydraEditorWindow
	{
		public const string TITLE = "Replace GameObjects";

		private static GameObject s_Prefab;
		private static List<GameObject> s_ObjectsToReplace;
		private static bool s_KeepOriginalNames = false;
		private static Vector2 s_ScrollPosition;

		/// <summary>
		/// 	Initializes the ReplaceGameObjects class.
		/// </summary>
		static ReplaceGameObjects()
		{
			s_ObjectsToReplace = new List<GameObject>();
		}

		/// <summary>
		/// 	Shows the window.
		/// </summary>
		[MenuItem(MENU + TITLE)]
		public static void Init()
		{
			GetWindow<ReplaceGameObjects>(false, TITLE, true);
			UpdateSelection();
		}

		#region Messages

		/// <summary>
		/// 	Called whenever the selection changes.
		/// </summary>
		protected override void OnSelectionChange()
		{
			base.OnSelectionChange();

			UpdateSelection();
		}

		/// <summary>
		/// 	Called to draw the window contents.
		/// </summary>
		protected override void OnGUI()
		{
			base.OnGUI();

			s_KeepOriginalNames = GUILayout.Toggle(s_KeepOriginalNames, "Keep names");

			GUILayout.Space(5);

			s_Prefab = EditorGUILayout.ObjectField("Prefab", s_Prefab, typeof(GameObject), false) as GameObject;

			GUILayout.Space(5);

			HydraEditorLayoutUtils.BeginBox(false);
			{
				s_ScrollPosition = GUILayout.BeginScrollView(s_ScrollPosition);
				{
					for (int index = 0; index < s_ObjectsToReplace.Count; index++)
					{
						GameObject original = s_ObjectsToReplace[index];

						GUIStyle style = HydraEditorGUIStyles.ArrayElementStyle(index);
						GUILayout.BeginHorizontal(style);
						GUILayout.Label(original.name);
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndScrollView();
			}
			HydraEditorLayoutUtils.EndBox(false);

			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			{
				bool oldEnabled = GUI.enabled;
				GUI.enabled = s_Prefab != null;

				if (GUILayout.Button("Apply"))
					Replace();

				GUI.enabled = oldEnabled;
			}
			GUILayout.EndHorizontal();
		}

		#endregion

		/// <summary>
		/// 	Updates the selection.
		/// </summary>
		private static void UpdateSelection()
		{
			s_ObjectsToReplace.Clear();
			GameObject[] selection = Selection.gameObjects;

			for (int index = 0; index < selection.Length; index++)
			{
				GameObject gameObject = selection[index];
				if (IgnoreGameObject(gameObject))
					continue;

				s_ObjectsToReplace.Add(gameObject);
			}

			//s_ObjectsToReplace = Selection.gameObjects;

			s_ObjectsToReplace.Sort(delegate(GameObject x, GameObject y) { return x.name.CompareTo(y.name); });
		}

		/// <summary>
		/// 	Returns true if the target GameObject should be ignored for replacement.
		/// </summary>
		/// <returns><c>true</c>, if game object is ignored, <c>false</c> otherwise.</returns>
		/// <param name="gameObject">Game object.</param>
		private static bool IgnoreGameObject(GameObject gameObject)
		{
			if (gameObject == null)
				return true;

			// Don't allow replacing prefabs with prefabs.
			if (PrefabUtility.GetPrefabType(gameObject) == PrefabType.Prefab)
				return true;

			return false;
		}

		/// <summary>
		/// 	Replaces the selected instances with the selected prefab.
		/// </summary>
		private static void Replace()
		{
			Replace(s_ObjectsToReplace, s_Prefab, s_KeepOriginalNames);
		}

		/// <summary>
		/// 	Replaces the original instances with the given prefab.
		/// </summary>
		/// <param name="originals">Originals.</param>
		/// <param name="prefab">Prefab.</param>
		/// <param name="keepNames">If set to <c>true</c> keep names.</param>
		public static void Replace(List<GameObject> originals, GameObject prefab, bool keepNames)
		{
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName(typeof(ReplaceGameObjects).Name);
			int undoIndex = Undo.GetCurrentGroup();

			for (int index = 0; index < originals.Count; index++)
			{
				Replace(originals[index], prefab, keepNames);
				originals[index] = null;
			}

			Undo.CollapseUndoOperations(undoIndex);
		}

		/// <summary>
		/// 	Replaces the original instance with the given prefab.
		/// </summary>
		/// <param name="original">Original.</param>
		/// <param name="prefab">Prefab.</param>
		/// <param name="keepNames">If set to <c>true</c> keep names.</param>
		public static GameObject Replace(GameObject original, GameObject prefab, bool keepNames)
		{
			if (IgnoreGameObject(original))
				return null;

			GameObject newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			newObject.transform.Copy(original);

			if (keepNames)
				newObject.name = original.name;

			Undo.RegisterCreatedObjectUndo(newObject, original.name + "replaced");
			Undo.DestroyObjectImmediate(original);

			return newObject;
		}
	}
}
