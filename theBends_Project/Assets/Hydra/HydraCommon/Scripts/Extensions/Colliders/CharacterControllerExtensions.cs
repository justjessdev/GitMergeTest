// <copyright file=CharacterControllerExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions.Colliders
{
	public static class CharacterControllerExtensions
	{
		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetVolume(this CharacterController extends)
		{
			return extends.ToCapsule().GetVolume();
		}

		/// <summary>
		/// 	Gets the capsule.
		/// </summary>
		/// <returns>The capsule.</returns>
		/// <param name="extends">Extends.</param>
		public static Capsule ToCapsule(this CharacterController extends)
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
		private static Vector3 GetPointGeneric(CharacterController extends, bool point1)
		{
			float scalar = point1 ? -1.0f : 1.0f;
			float distance = (extends.height / 2.0f) - extends.radius;

			Vector3 output = extends.center + scalar * Vector3.up * distance;
			output = extends.transform.TransformPoint(output);

			return output;
		}
	}
}
