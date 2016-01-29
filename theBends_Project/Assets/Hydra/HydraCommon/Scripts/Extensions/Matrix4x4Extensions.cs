// <copyright file=Matrix4x4Extensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	Matrix4x4Extensions contains extension methods for working with Matrix4x4s.
	/// </summary>
	public static class Matrix4x4Extensions
	{
		/// <summary>
		/// 	Returns the matrix as a quaternion rotation.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="extends">Extends.</param>
		public static Quaternion ToQuaternion(this Matrix4x4 extends)
		{
			return Quaternion.LookRotation(extends.GetColumn(2), extends.GetColumn(1));
		}
	}
}
