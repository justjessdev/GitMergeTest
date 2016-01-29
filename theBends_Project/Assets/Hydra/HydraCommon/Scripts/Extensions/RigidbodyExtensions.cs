// <copyright file=RigidbodyExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// Rigidbody extensions.
	/// </summary>
	public static class RigidbodyExtensions
	{
		/// <summary>
		/// Moves the local position.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="localPositon">Local positon.</param>
		public static void MoveLocalPosition(this Rigidbody extends, Vector3 localPositon)
		{
			Vector3 worldPosition = extends.transform.TransformPoint(localPositon);
			extends.MovePosition(worldPosition);
		}
	}
}
