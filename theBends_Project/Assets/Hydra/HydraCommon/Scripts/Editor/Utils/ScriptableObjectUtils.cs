// <copyright file=ScriptableObjectUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	ScriptableObject utility methods.
	/// </summary>
	public static class ScriptableObjectUtils
	{
		/// <summary>
		/// 	Creates the asset from selected script.
		/// </summary>
		[MenuItem("Assets/Create/Asset From Selected Script")]
		public static void CreateAssetFromSelectedScript()
		{
			MonoScript script = Selection.objects[0] as MonoScript;
			string path;

			if (IsSingleton(script))
				path = GetSingletonPath(script);
			else
				path = EditorUtility.SaveFilePanel("Save location", "Assets", "New " + script.name, "asset");

			if (string.IsNullOrEmpty(path))
				return;

			// Get project relative path and ensure path is within project
			string projectRelative = FileUtil.GetProjectRelativePath(path);
			if (string.IsNullOrEmpty(projectRelative))
			{
				EditorUtility.DisplayDialog("Error", "Please select somewhere within your assets folder.", "OK");
				return;
			}

			CreateAsset(script.GetClass(), path);
		}

		/// <summary>
		/// 	Determines if the menu item should be enabled or not.
		/// </summary>
		/// <returns><c>true</c>, if menu item should be enabled, <c>false</c> otherwise.</returns>
		[MenuItem("Assets/Create/Asset From Selected Script", true)]
		public static bool CreateAssetFromSelectedScript_Validator()
		{
			if (Selection.objects == null || Selection.objects.Length != 1)
				return false;

			Object selected = Selection.objects[0];
			if (!(selected is MonoScript))
				return false;

			Type type = (selected as MonoScript).GetClass();
			if (!(type.IsSubclassOf(typeof(ScriptableObject))) || type.IsSubclassOf(typeof(UnityEditor.Editor)))
				return false;

			if (type.IsAbstract)
				return false;

			return true;
		}

		/// <summary>
		/// 	Creates the asset.
		/// </summary>
		/// <returns>The asset.</returns>
		/// <param name="type">Type.</param>
		/// <param name="path">Path.</param>
		public static ScriptableObject CreateAsset(Type type, string path)
		{
			string projectRelative = FileUtil.GetProjectRelativePath(path);

			if (string.IsNullOrEmpty(projectRelative))
				throw new Exception(string.Format("Path {0} is invalid.", projectRelative));

			ScriptableObject scriptableObject = CreateAndSave(type, projectRelative);

			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = scriptableObject;
			}

			return scriptableObject;
		}

		/// <summary>
		/// 	Creates and saves an instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="type">Type.</param>
		/// <param name="path">Path.</param>
		private static ScriptableObject CreateAndSave(Type type, string path)
		{
			ScriptableObject instance = ScriptableObject.CreateInstance(type);

			// Saving during Awake() will crash Unity, delay saving until next editor frame
			if (EditorApplication.isPlayingOrWillChangePlaymode)
				EditorApplication.delayCall += () => SaveAsset(instance, path);
			else
				SaveAsset(instance, path);

			return instance;
		}

		/// <summary>
		/// 	Saves the asset.
		/// </summary>
		/// <param name="instance">Instance.</param>
		/// <param name="path">Path.</param>
		private static void SaveAsset(ScriptableObject instance, string path)
		{
			string dirName = Path.GetDirectoryName(path);
			if (!Directory.Exists(dirName))
				Directory.CreateDirectory(dirName);

			AssetDatabase.CreateAsset(instance, path);
			AssetDatabase.SaveAssets();
		}

		/// <summary>
		/// 	Determines if the script is a singleton scriptable object.
		/// </summary>
		/// <returns><c>true</c> if is singleton; otherwise, <c>false</c>.</returns>
		/// <param name="script">Script.</param>
		private static bool IsSingleton(MonoScript script)
		{
			Type toCheck = script.GetClass();
			Type generic = typeof(SingletonHydraScriptableObject<>);

			return ReflectionUtils.IsSubclassOfRawGeneric(generic, toCheck);
		}

		/// <summary>
		/// 	Gets the asset path for the SingletonHydraMonoBehaviour.
		/// </summary>
		/// <returns>The singleton path.</returns>
		/// <param name="script">Script.</param>
		private static string GetSingletonPath(MonoScript script)
		{
			string dataPath = Application.dataPath;
			dataPath = Path.GetDirectoryName(dataPath);

			string local = ReflectionUtils.GetPropertyByName(script.GetClass(), "assetPath").GetValue(null, null) as string;
			return string.Format("{0}/{1}", dataPath, local);
		}
	}
}
