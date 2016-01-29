// <copyright file=QuaternionExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	QuaternionExtensions provides extension methods for working with Quaternions.
	/// </summary>
	public static class QuaternionExtensions
	{
		/// <summary>
		/// 	Returns a normalized copy of the quaternion.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static Quaternion Normalized(this Quaternion extends)
		{
			float sum = 0.0f;

			float x = extends.x;
			float y = extends.y;
			float z = extends.z;
			float w = extends.w;

			sum += x * x;
			sum += y * y;
			sum += z * z;
			sum += w * w;

			float magnitudeInverse = 1.0f / Mathf.Sqrt(sum);

			x *= magnitudeInverse;
			y *= magnitudeInverse;
			z *= magnitudeInverse;
			w *= magnitudeInverse;

			return new Quaternion(x, y, z, w);
		}
	}
}
