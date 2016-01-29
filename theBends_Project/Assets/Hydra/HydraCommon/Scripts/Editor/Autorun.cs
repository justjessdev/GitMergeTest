// <copyright file=Autorun company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.IO;
using Hydra.HydraCommon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor
{
	/// <summary>
	/// 	Autorun allows us to do some automatic maintenance.
	/// </summary>
	[InitializeOnLoad]
	public static class Autorun
	{
		/// <summary>
		/// 	Called when the editor starts, and every time the assembly is recompiled.
		/// </summary>
		static Autorun()
		{
			string path = Path.Combine(Application.dataPath, "Hydra");
			MaintenanceUtils.RemoveEmptyDirectories(path);
		}
	}
}
