// <copyright file=EditorResourceCache company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using Hydra.HydraCommon.Utils;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Editor.Utils
{
	public static class EditorResourceCache
	{
		public const string HYDRA_PATH = "Assets/Hydra";
		public const string EDITOR_RESOURCES_PATH = "Editor/Resources";

		#region Methods

		/// <summary>
		/// 	Gets a resource from the editor resource directory.
		/// </summary>
		/// <returns>The resource.</returns>
		/// <param name="package">Package.</param>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The resource type.</typeparam>
		public static T GetResource<T>(string package, string path, string name) where T : Object
		{
			string editorPath = GetEditorPath(package, path);

			if (!ResourceCache.ResourceLoaded(editorPath, name))
			{
				string assetPath = Path.Combine(editorPath, name);
				assetPath = StringUtils.ToUnixPath(assetPath);

				T resource = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
				if (resource == null)
					throw new Exception(string.Format("No resource at {0}", assetPath));

				ResourceCache.StoreResource(resource, editorPath, name);
			}

			return ResourceCache.GetCachedResource<T>(editorPath, name);
		}

		#endregion

		private static string GetEditorPath(string package, string path)
		{
			string editorPath = Path.Combine(HYDRA_PATH, package);
			editorPath = Path.Combine(editorPath, EDITOR_RESOURCES_PATH);
			return Path.Combine(editorPath, path);
		}
	}
}
