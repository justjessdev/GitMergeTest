// <copyright file=Matrix4x4Utils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Matrix4x4Utils provides utility methods for working with matrices.
	/// </summary>
	public static class Matrix4x4Utils
	{
		/// <summary>
		/// 	Creates a translation matrix.
		/// </summary>
		/// <param name="translation">Translation.</param>
		public static Matrix4x4 Translate(Vector3 translation)
		{
			return Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);
		}

		/// <summary>
		/// 	Creates a rotation matrix.
		/// </summary>
		/// <param name="rotation">Rotation.</param>
		public static Matrix4x4 Rotate(Quaternion rotation)
		{
			return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
		}

		/// <summary>
		/// 	Creates a rotation matrix.
		/// </summary>
		/// <param name="forwards">Forwards.</param>
		/// <param name="up">Up.</param>
		public static Matrix4x4 Rotate(Vector3 forwards, Vector3 up)
		{
			Quaternion rotation = Quaternion.LookRotation(forwards, up);

			return Rotate(rotation);
		}

		/// <summary>
		/// 	Creates a rotation taking normal as up.
		/// </summary>
		/// <returns>The normal.</returns>
		/// <param name="normal">Normal.</param>
		public static Matrix4x4 FromNormal(Vector3 normal)
		{
			Vector3 tangent = HydraMathUtils.Tangent(normal);

			return Rotate(tangent, normal);
		}
	}
}
