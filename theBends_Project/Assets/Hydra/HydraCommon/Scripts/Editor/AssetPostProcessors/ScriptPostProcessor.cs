// <copyright file=ScriptPostProcessor company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using Hydra.HydraCommon.Editor.Utils;
using UnityEditor;

namespace Hydra.HydraCommon.Editor.AssetPostProcessors
{
	public class ScriptPostProcessor : AssetPostprocessor
	{
		/// <summary>
		/// 	Adds the header to new scripts.
		/// </summary>
		/// <param name="imported">Imported.</param>
		/// <param name="deleted">Deleted.</param>
		/// <param name="moved">Moved.</param>
		/// <param name="movedFromAssetPaths">Moved from asset paths.</param>
		private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved,
												   string[] movedFromAssetPaths)
		{
			for (int index = 0; index < imported.Length; index++)
			{
				string path = imported[index];
				if (!MaintenanceUtils.ContainsDirName(path, "Hydra"))
					continue;

				string ext = Path.GetExtension(path);
				if (ext != ".cs")
					continue;

				WriteHeader(imported[index]);
			}
		}

		/// <summary>
		/// 	Writes the header.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void WriteHeader(string path)
		{
			string tempFile = Path.GetFullPath(path) + ".tmp";

			using (StreamReader reader = new StreamReader(path))
			{
				using (StreamWriter writer = new StreamWriter(tempFile))
				{
					writer.WriteLine(@"// <copyright file=" + Path.GetFileNameWithoutExtension(path) + @" company=Hydra>
// Copyright (c) " + DateTime.Now.Year + @" All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>
");

					bool write = false;
					string line = string.Empty;

					while ((line = reader.ReadLine()) != null)
					{
						if (!line.StartsWith("//") && !string.IsNullOrEmpty(line))
							write = true;

						if (!write)
							continue;

						writer.WriteLine(line);
					}
				}
			}

			File.Delete(path);
			File.Move(tempFile, path);
		}
	}
}
