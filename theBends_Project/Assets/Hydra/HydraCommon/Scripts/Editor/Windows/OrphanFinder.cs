// <copyright file=OrphanFinder company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;
using System.IO;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Editor.Windows
{
	/// <summary>
	/// 	OrphanFinder is a utility for finding orphaned assets.
	/// </summary>
	public class OrphanFinder : HydraEditorWindow
	{
		public const string TITLE = "Orphan Finder";
		public const string GUID_PREFIX = "guid:";

		private static GUIContent s_SearchLabel = new GUIContent("Search");

		// Ignored directories
		private static string[] s_DirIgnore = {"ProjectSettings", "Resources", "ShaderForge"};
		private static string[] s_ExtIgnore = {".js", ".cs", ".unity", ".txt", ".md", ".dll"};

		private static Vector2 s_ScrollPosition;

		private static Dictionary<string, List<string>> s_CachedGuidDependencies;
		private static Dictionary<string, List<string>> s_CachedGuidParents;
		private static List<string> s_GuidOrphans;

		/// <summary>
		/// 	Initializes the class.
		/// </summary>
		static OrphanFinder()
		{
			s_CachedGuidDependencies = new Dictionary<string, List<string>>();
			s_CachedGuidParents = new Dictionary<string, List<string>>();
			s_GuidOrphans = new List<string>();
		}

		/// <summary>
		/// 	Shows the window.
		/// </summary>
		[MenuItem(MENU + TITLE)]
		public static void Init()
		{
			GetWindow<OrphanFinder>(false, TITLE, true);
		}

		#region Messages

		/// <summary>
		/// 	Called to draw the window contents.
		/// </summary>
		protected override void OnGUI()
		{
			base.OnGUI();

			EditorGUILayout.LabelField(string.Format("{0} orphaned assets", s_GuidOrphans.Count));

			HydraEditorLayoutUtils.BeginBox(false);
			{
				s_ScrollPosition = EditorGUILayout.BeginScrollView(s_ScrollPosition, false, true);
				{
					for (int index = 0; index < s_GuidOrphans.Count; index++)
						DrawGuidRow(s_GuidOrphans[index], index);
				}
				EditorGUILayout.EndScrollView();
			}
			HydraEditorLayoutUtils.EndBox(false);

			if (GUILayout.Button(s_SearchLabel))
				Search();
		}

		#endregion

		/// <summary>
		/// 	Draws the GUID row.
		/// </summary>
		private static void DrawGuidRow(string guid, int index)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);

			GUIStyle style = HydraEditorGUIStyles.ArrayElementStyle(index);

			Object selected = Selection.activeObject;
			string selectedPath = AssetDatabase.GetAssetPath(selected);

			if (selectedPath == path)
				style = HydraEditorGUIStyles.arrayElementSelectedStyle;

			GUILayout.BeginHorizontal(style);
			{
				if (!DrawDeleteButton(guid))
				{
					DrawIcon(guid);

					if (GUILayout.Button(path, GUI.skin.label))
					{
						Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

						Selection.activeObject = asset;
						EditorGUIUtility.PingObject(asset);
					}

					List<string> dependencies = s_CachedGuidDependencies[guid];
					if (dependencies.Count > 0)
					{
						string dependenciesLabel = string.Format("{0} dependencies", dependencies.Count);
						GUILayout.Label(dependenciesLabel, HydraEditorGUIStyles.rightAlignedLabelStyle);
					}
				}
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// 	Draws the delete button.
		/// </summary>
		/// <param name="guid">GUID.</param>
		private static bool DrawDeleteButton(string guid)
		{
			bool pressed = GUILayout.Button("X", new GUIStyle("sv_label_6"), GUILayout.Width(24.0f));

			if (pressed)
				Delete(guid);

			return pressed;
		}

		private static void DrawIcon(string guid)
		{
			string local = AssetDatabase.GUIDToAssetPath(guid);
			Texture icon = AssetDatabase.GetCachedIcon(local);

			if (icon == null)
				return;

			Rect position = EditorGUILayout.GetControlRect(false, GUILayout.Width(16.0f));
			EditorGUI.DrawTextureTransparent(position, icon);
		}

		/// <summary>
		/// 	Search.
		/// </summary>
		private static void Search()
		{
			CacheDependencies();
			CacheParents();
			CacheOrphans();

			EditorUtility.ClearProgressBar();
		}

		private static void UpdateProgressBar(string label, int count)
		{
			string title = "Searching for Orphaned Assets";
			label = string.Format("{0} {1}", label, count);
			float progress = ((float)EditorApplication.timeSinceStartup / 3.0f) % 1.0f;

			EditorUtility.DisplayProgressBar(title, label, progress);
		}

		/// <summary>
		/// 	Deletes the asset with the specified guid from disk.
		/// </summary>
		/// <param name="guid">GUID.</param>
		private static void Delete(string guid)
		{
			string local = AssetDatabase.GUIDToAssetPath(guid);

			// Remove from disk
			if (!AssetDatabase.DeleteAsset(local))
			{
				Debug.LogWarning(string.Format("Couldn't delete {0}", local));
				return;
			}

			// Clear empty directories
			string directory = Path.GetDirectoryName(local);
			MaintenanceUtils.RemoveEmptyDirectories(directory);

			// Update the cache
			List<string> dependencies = s_CachedGuidDependencies[guid];
			s_CachedGuidDependencies.Remove(guid);
			s_GuidOrphans.Remove(guid);

			if (dependencies.Count == 0)
				return;

			CacheParents();
			CacheOrphans();

			EditorUtility.ClearProgressBar();
		}

		/// <summary>
		/// 	Caches the dependencies.
		/// </summary>
		private static void CacheDependencies()
		{
			s_CachedGuidDependencies.Clear();

			string[] paths = AssetDatabase.GetAllAssetPaths();

			for (int index = 0; index < paths.Length; index++)
			{
				string path = paths[index];
				if (!File.Exists(path))
					continue;

				if (MaintenanceUtils.IsDirectory(path))
					continue;

				if (!MaintenanceUtils.InAssetsDir(path))
					continue;

				List<string> dependencies = new List<string>();
				GetDependencies(path, dependencies);
				string guid = AssetDatabase.AssetPathToGUID(path);

				s_CachedGuidDependencies[guid] = dependencies;

				UpdateProgressBar("Caching dependencies:", index + 1);
			}
		}

		/// <summary>
		/// 	Caches the parents.
		/// </summary>
		private static void CacheParents()
		{
			s_CachedGuidParents.Clear();
			int count = 0;

			foreach (KeyValuePair<string, List<string>> pair in s_CachedGuidDependencies)
			{
				string guid = pair.Key;
				if (!s_CachedGuidParents.ContainsKey(guid))
					s_CachedGuidParents[guid] = new List<string>();

				List<string> dependencies = pair.Value;

				for (int index = 0; index < dependencies.Count; index++)
				{
					string dependent = dependencies[index];
					if (dependent == guid)
						continue;

					if (!s_CachedGuidParents.ContainsKey(dependent))
						s_CachedGuidParents[dependent] = new List<string>();

					s_CachedGuidParents[dependent].Add(guid);
				}

				count++;
				UpdateProgressBar("Caching parents:", count);
			}
		}

		/// <summary>
		/// 	Caches the orphans.
		/// </summary>
		private static void CacheOrphans()
		{
			s_GuidOrphans.Clear();
			int count = 0;

			foreach (KeyValuePair<string, List<string>> pair in s_CachedGuidParents)
			{
				string guid = pair.Key;
				string path = AssetDatabase.GUIDToAssetPath(guid);

				if (IgnorePath(path))
					continue;

				List<string> parents = pair.Value;

				if (parents.Count == 0)
					s_GuidOrphans.Add(guid);

				count++;
				UpdateProgressBar("Caching orphans:", count);
			}

			SortGuidByExt(s_GuidOrphans);
		}

		/// <summary>
		/// 	Gets the dependencies.
		/// </summary>
		/// <returns>The dependencies.</returns>
		/// <param name="path">Path.</param>
		private static void GetDependencies(string path, List<string> output)
		{
			if (Path.GetExtension(path) == ".unity")
			{
				GetSceneDependencies(path, output);
				return;
			}

			string guid = AssetDatabase.AssetPathToGUID(path);
			Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
			Object[] dependencies = EditorUtility.CollectDependencies(assets);

			for (int dependencyIndex = 0; dependencyIndex < dependencies.Length; dependencyIndex++)
			{
				Object dependency = dependencies[dependencyIndex];
				string dependencyPath = AssetDatabase.GetAssetPath(dependency);

				if (!MaintenanceUtils.InAssetsDir(dependencyPath))
					continue;

				string dependencyGuid = AssetDatabase.AssetPathToGUID(dependencyPath);
				if (dependencyGuid == guid)
					continue;

				output.Add(dependencyGuid);
			}
		}

		/// <summary>
		/// 	We can't load the scene to collect dependencies, so let's get dirty and
		/// 	parse the file for GUIDs.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="output">Output.</param>
		private static void GetSceneDependencies(string path, List<string> output)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				while (reader.Peek() >= 0)
				{
					string line = reader.ReadLine();
					if (!line.Contains(GUID_PREFIX))
						continue;

					string guidPart = line.Split(',')[1];

					string guid = StringUtils.RemoveSubstring(guidPart, GUID_PREFIX).Trim();
					string assetPath = AssetDatabase.GUIDToAssetPath(guid);

					if (string.IsNullOrEmpty(assetPath))
						continue;

					if (!MaintenanceUtils.InAssetsDir(assetPath))
						continue;

					output.Add(guid);
				}
			}
		}

		/// <summary>
		/// 	Sorts the guid list by extension.
		/// </summary>
		private static void SortGuidByExt(List<string> collection)
		{
			collection.Sort(delegate(string guid1, string guid2)
			{
				string path1 = AssetDatabase.GUIDToAssetPath(guid1);
				string path2 = AssetDatabase.GUIDToAssetPath(guid2);

				string ext1 = Path.GetExtension(path1);
				string ext2 = Path.GetExtension(path2);

				int result = string.Compare(ext1, ext2);

				return result != 0 ? result : string.Compare(path1, path2);
			});
		}

		/// <summary>
		/// 	Returns true if the asset at the given path should be ignored.
		/// </summary>
		/// <returns><c>true</c>, if path should be ignored, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		private static bool IgnorePath(string path)
		{
			if (!MaintenanceUtils.InAssetsDir(path))
				return true;

			string ext = Path.GetExtension(path);
			if (Array.Exists(s_ExtIgnore, element => element == ext))
				return true;

			for (int index = 0; index < s_DirIgnore.Length; index++)
			{
				string dirName = s_DirIgnore[index];
				if (MaintenanceUtils.ContainsDirName(path, dirName))
					return true;
			}

			return false;
		}
	}
}
