// <copyright file=BoxColliderExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Extensions.Colliders
{
	public static class BoxColliderExtensions
	{
		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetVolume(this BoxCollider extends)
		{
			Vector3 size = extends.size;
			return size.x * size.y * size.z;
		}
	}
}
