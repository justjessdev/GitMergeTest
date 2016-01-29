// <copyright file=MaintenanceUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using System.IO;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	Util methods for cleaning up the project.
	/// </summary>
	public class MaintenanceUtils
	{
		public const string META_EXT = ".meta";
		public const string GUID_PREFIX = "guid:";
		public const string FBX_EXT = ".fbx";
		public const string ASSETS_DIR_NAME = "Assets";

		/// <summary>
		/// 	Recursively searches for paths in the root that match the given extension.
		/// </summary>
		/// <param name="root">Root.</param>
		/// <param name="ext">Ext.</param>
		/// <param name="output">Output.</param>
		public static void GetPathsRecursive(string root, string ext, List<string> output)
		{
			if (IsFile(root))
			{
				string pathExt = Path.GetExtension(root);
				if (pathExt == ext)
					output.Add(root);
				return;
			}

			string[] contents = GetDirectoryContents(root);
			for (int index = 0; index < contents.Length; index++)
				GetPathsRecursive(contents[index], ext, output);
		}

		/// <summary>
		/// 	Gets the files and directories at the given path.
		/// </summary>
		/// <returns>The directory contents.</returns>
		/// <param name="path">Path.</param>
		public static string[] GetDirectoryContents(string path)
		{
			string[] output = Directory.GetDirectories(path);
			ArrayUtils.AddRange(ref output, Directory.GetFiles(path));
			return output;
		}

		/// <summary>
		/// 	Removes any empty directories, clearing any orphaned meta files.
		/// </summary>
		[MenuItem("Assets/Remove Empty Directories")]
		public static void RemoveEmptyDirectories()
		{
			RemoveEmptyDirectories(Application.dataPath);
		}

		/// <summary>
		/// 	Removes any empty directories, clearing any orphaned meta files.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void RemoveEmptyDirectories(string path)
		{
			path = Path.GetFullPath(path);
			if (!Directory.Exists(path))
				return;

			if (!InAssetsDir(path))
				return;

			string[] directories = Directory.GetDirectories(path);

			for (int index = 0; index < directories.Length; index++)
				RemoveEmptyDirectories(directories[index]);

			if (IsAssetsDir(path))
				return;

			if (GetDirectoryContents(path).Length != 0)
				return;

			Debug.Log(string.Format("Removing empty dir {0}", path));
			Directory.Delete(path);
			AssetDatabase.Refresh();

			RemoveOrphanedMeta(path);
		}

		/// <summary>
		/// 	Removes the orphaned meta files at the given path.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void RemoveOrphanedMeta(string path)
		{
			path = Path.GetFullPath(path);
			if (!Directory.Exists(path) && !File.Exists(path))
				return;

			if (IsDirectory(path))
			{
				string[] files = Directory.GetFiles(path);
				for (int index = 0; index < files.Length; index++)
					RemoveOrphanedMeta(files[index]);
				return;
			}

			if (!IsOrphanedMeta(path))
				return;

			Debug.Log(string.Format("Removing orphaned metadata {0}", path));
			File.Delete(path);
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// 	Returns true if the meta file at the given path is orphaned.
		/// </summary>
		/// <returns><c>true</c> if orphaned; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		public static bool IsOrphanedMeta(string path)
		{
			if (!IsMeta(path))
				return false;

			string assetPath = AssetDatabase.GetAssetPathFromTextMetaFilePath(path);

			if (File.Exists(assetPath))
				return false;

			if (Directory.Exists(assetPath))
				return false;

			return true;
		}

		/// <summary>
		/// 	Returns true if the file at the path is a meta file.
		/// </summary>
		/// <returns><c>true</c> if the specified path is a meta file; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		public static bool IsMeta(string path)
		{
			if (!IsFile(path))
				return false;

			return Path.GetExtension(path) == META_EXT;
		}

		/// <summary>
		/// 	Returns true if the path is a directory.
		/// </summary>
		/// <returns><c>true</c> if the specified path is a directory; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		public static bool IsDirectory(string path)
		{
			path = Path.GetFullPath(path);
			FileAttributes attr = File.GetAttributes(path);
			return (attr & FileAttributes.Directory) == FileAttributes.Directory;
		}

		/// <summary>
		/// 	Determines if the path is a file.
		/// </summary>
		/// <returns><c>true</c> if the path is a file; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		public static bool IsFile(string path)
		{
			return !IsDirectory(path);
		}

		/// <summary>
		/// 	Returns true if the path tree contains a directory with the given name.
		/// 	E.g. "/this/is/a/test" and "is" return true. 
		/// </summary>
		/// <returns><c>true</c>, if path contains dir name, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		/// <param name="dirName">Dir name.</param>
		public static bool ContainsDirName(string path, string dirName)
		{
			if (string.IsNullOrEmpty(path))
				return false;

			if (IsDirectory(path) && Path.GetFileName(path) == dirName)
				return true;

			return ContainsDirName(Path.GetDirectoryName(path), dirName);
		}

		/// <summary>
		/// 	Returns true if the given path is in the assets directory.
		/// </summary>
		/// <returns><c>true</c>, if in assets dir, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public static bool InAssetsDir(string path)
		{
			return NormalizePath(path).StartsWith(NormalizePath(Application.dataPath));
		}

		/// <summary>
		/// 	Returns true if the given path is the Assets directory.
		/// </summary>
		/// <returns><c>true</c> if path is assets dir; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		public static bool IsAssetsDir(string path)
		{
			return NormalizePath(path) == NormalizePath(Application.dataPath);
		}

		/// <summary>
		/// 	Normalizes the path, taken from
		/// 	http://stackoverflow.com/questions/2281531/how-can-i-compare-directory-paths-in-c
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="path">Path.</param>
		public static string NormalizePath(string path)
		{
			return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
		}

		/// <summary>
		/// 	Gets the relative asset path.
		/// </summary>
		/// <returns>The relative asset path.</returns>
		/// <param name="path">Path.</param>
		public static string GetRelativeAssetPath(string path)
		{
			if (path.StartsWith(Application.dataPath))
				path = "Assets" + path.Substring(Application.dataPath.Length);
			return path;
		}
	}
}
