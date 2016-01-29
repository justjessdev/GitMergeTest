// <copyright file=ResourceCache company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;
using System.IO;
using Hydra.HydraCommon.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	The ResourceCache contains methods for loading and caching resources.
	/// </summary>
	public static class ResourceCache
	{
		public const string MATERIAL_PATH = "Materials";

		// Cache in the format [path][name]
		private static readonly Dictionary<string, Dictionary<string, Object>> s_LoadedObjects;

		/// <summary>
		/// 	Initializes the ResourceCache class.
		/// </summary>
		static ResourceCache()
		{
			s_LoadedObjects = new Dictionary<string, Dictionary<string, Object>>();
		}

		#region Methods

		/// <summary>
		/// 	Gets the resource.
		/// </summary>
		/// <returns>The resource.</returns>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The resource type.</typeparam>
		public static T GetResource<T>(string path, string name) where T : Object
		{
			if (!ResourceLoaded(path, name))
			{
				string assetPath = Path.Combine(path, name);
				assetPath = StringUtils.ToUnixPath(assetPath);

				T resource = Resources.Load<T>(assetPath);
				if (resource == null)
					throw new Exception(string.Format("No resource at {0}", assetPath));

				StoreResource(resource, path, name);
			}

			return GetCachedResource<T>(path, name);
		}

		/// <summary>
		/// 	Returns true if the resource has been loaded.
		/// </summary>
		/// <returns><c>true</c> if the resource has been loaded; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		public static bool ResourceLoaded(string path, string name)
		{
			if (!s_LoadedObjects.ContainsKey(path))
				return false;

			return s_LoadedObjects.Get(path).Get(name) != null;
		}

		/// <summary>
		/// 	Gets the cached resource.
		/// </summary>
		/// <returns>The cached resource.</returns>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The resource type.</typeparam>
		public static T GetCachedResource<T>(string path, string name) where T : Object
		{
			return s_LoadedObjects[path][name] as T;
		}

		/// <summary>
		/// 	Stores the resource.
		/// </summary>
		/// <param name="resource">Resource.</param>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		public static void StoreResource(Object resource, string path, string name)
		{
			if (!s_LoadedObjects.ContainsKey(path))
				s_LoadedObjects[path] = new Dictionary<string, Object>();
			s_LoadedObjects[path][name] = resource;
		}

		#endregion
	}
}
