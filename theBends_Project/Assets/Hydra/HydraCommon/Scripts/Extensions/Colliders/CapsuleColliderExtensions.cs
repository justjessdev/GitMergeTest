// <copyright file=CapsuleColliderExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions.Colliders
{
	/// <summary>
	/// 	CapsuleColliderExtensions provides extension methods for working with CapsuleColliders.
	/// </summary>
	public static class CapsuleColliderExtensions
	{
		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetVolume(this CapsuleCollider extends)
		{
			return extends.ToCapsule().GetVolume();
		}

		/// <summary>
		/// 	Gets the capsule.
		/// </summary>
		/// <returns>The capsule.</returns>
		/// <param name="extends">Extends.</param>
		public static Capsule ToCapsule(this CapsuleCollider extends)
		{
			float radius = extends.radius;
			Vector3 point1 = GetPointGeneric(extends, true);
			Vector3 point2 = GetPointGeneric(extends, false);

			return new Capsule(point1, point2, radius);
		}

		/// <summary>
		/// 	Returns either point1 or point2 of the capsule.
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="point1">If set to <c>true</c> point1.</param>
		private static Vector3 GetPointGeneric(CapsuleCollider extends, bool point1)
		{
			Vector3 direction = extends.GetDirectionVector();
			float scalar = point1 ? -1.0f : 1.0f;
			float distance = (extends.height / 2.0f) - extends.radius;

			Vector3 output = extends.center + scalar * direction * distance;
			output = extends.transform.TransformPoint(output);

			return output;
		}

		/// <summary>
		/// 	Returns a vector for the capsule direction.
		/// 	
		/// 	0 = Vector3.right
		/// 	1 = Vector3.up
		/// 	2 = Vector3.forward
		/// </summary>
		/// <returns>The direction vector.</returns>
		/// <param name="extends">Extends.</param>
		private static Vector3 GetDirectionVector(this CapsuleCollider extends)
		{
			switch (extends.direction)
			{
				case 0:
					return Vector3.right;
				case 1:
					return Vector3.up;
				case 2:
					return Vector3.forward;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
