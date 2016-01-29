// <copyright file=Vector2Extensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	Vector2Extensions provides extension methods for working with Vector2s.
	/// </summary>
	public static class Vector2Extensions
	{
		/// <summary>
		/// 	Calculates the cross product with the other vector.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static float Cross(this Vector2 extends, Vector2 other)
		{
			return (extends.x * other.y) - (extends.y * other.x);
		}

		/// <summary>
		/// 	Rotates the Vector2 about the origin.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="degrees">Degrees.</param>
		public static Vector2 Rotate(this Vector2 extends, float degrees)
		{
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tempX = extends.x;
			float tempY = extends.y;

			extends.x = (cos * tempX) - (sin * tempY);
			extends.y = (sin * tempX) + (cos * tempY);

			return extends;
		}
	}
}
