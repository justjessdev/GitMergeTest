// <copyright file=EditorSceneUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using UnityEditor;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	Editor scene utils.
	/// </summary>
	public static class EditorSceneUtils
	{
		/// <summary>
		/// 	Gets the enabled scenes.
		/// </summary>
		/// <returns>The enabled scenes.</returns>
		public static EditorBuildSettingsScene[] GetEnabledScenes()
		{
			EditorBuildSettingsScene[] allScenes = EditorBuildSettings.scenes;

			int enabledCount = 0;

			for (int index = 0; index < allScenes.Length; index++)
			{
				if (allScenes[index].enabled)
					enabledCount++;
			}

			EditorBuildSettingsScene[] output = new EditorBuildSettingsScene[enabledCount];

			int enabledIndex = 0;

			for (int index = 0; index < allScenes.Length; index++)
			{
				if (!allScenes[index].enabled)
					continue;

				output[enabledIndex] = allScenes[index];
				enabledIndex++;
			}

			return output;
		}

		/// <summary>
		/// 	Returns an array of enabled scene paths.
		/// </summary>
		/// <returns>The scene paths.</returns>
		public static string[] GetScenePaths()
		{
			EditorBuildSettingsScene[] scenes = GetEnabledScenes();
			string[] output = new string[scenes.Length];

			for (int index = 0; index < scenes.Length; index++)
				output[index] = scenes[index].path;

			return output;
		}

		/// <summary>
		/// 	Returns an array of enabled scene names.
		/// </summary>
		/// <returns>The scene names.</returns>
		public static string[] GetSceneNames()
		{
			EditorBuildSettingsScene[] scenes = GetEnabledScenes();
			return GetSceneNames(scenes);
		}

		/// <summary>
		/// 	Returns an array of scene names.
		/// </summary>
		/// <returns>The scene names.</returns>
		public static string[] GetSceneNames(EditorBuildSettingsScene[] scenes)
		{
			string[] output = new string[scenes.Length];

			for (int index = 0; index < scenes.Length; index++)
				output[index] = GetSceneName(scenes[index]);

			return output;
		}

		/// <summary>
		/// 	Gets the name of the enabled scene.
		/// </summary>
		/// <returns>The scene name.</returns>
		/// <param name="index">Index.</param>
		public static string GetSceneName(int index)
		{
			EditorBuildSettingsScene scene = GetEnabledScenes()[index];
			return GetSceneName(scene);
		}

		/// <summary>
		/// 	Gets the name of the scene.
		/// </summary>
		/// <returns>The scene name.</returns>
		/// <param name="scene">Scene.</param>
		public static string GetSceneName(EditorBuildSettingsScene scene)
		{
			return GetSceneName(scene.path);
		}

		/// <summary>
		/// 	Gets the name of the scene.
		/// </summary>
		/// <returns>The scene name.</returns>
		/// <param name="path">Path.</param>
		public static string GetSceneName(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		/// <summary>
		/// 	Gets the index of the scene.
		/// </summary>
		/// <returns>The scene index.</returns>
		/// <param name="path">Path.</param>
		public static int GetSceneIndex(string path)
		{
			string normalized = MaintenanceUtils.NormalizePath(path);
			EditorBuildSettingsScene[] scenes = GetEnabledScenes();

			for (int index = 0; index < scenes.Length; index++)
			{
				if (normalized == MaintenanceUtils.NormalizePath(scenes[index].path))
					return index;
			}

			string error = string.Format("{0} is not an enabled scene.", path);
			throw new ArgumentOutOfRangeException(error);
		}
	}
}
