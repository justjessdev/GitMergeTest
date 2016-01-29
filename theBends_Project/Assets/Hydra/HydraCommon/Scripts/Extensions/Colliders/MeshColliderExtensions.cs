// <copyright file=MeshColliderExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions.Colliders
{
	public static class MeshColliderExtensions
	{
		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetVolume(this MeshCollider extends)
		{
			Mesh mesh = extends.sharedMesh;
			if (mesh == null)
				return 0.0f;

			return MeshUtils.GetVolume(mesh);
		}
	}
}
