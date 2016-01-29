// <copyright file=ColliderExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions.Colliders
{
	public static class ColliderExtensions
	{
		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetVolume(this Collider extends)
		{
			if (extends is CapsuleCollider)
				return CapsuleColliderExtensions.GetVolume(extends as CapsuleCollider);

			if (extends is BoxCollider)
				return BoxColliderExtensions.GetVolume(extends as BoxCollider);

			if (extends is MeshCollider)
				return MeshColliderExtensions.GetVolume(extends as MeshCollider);

			if (extends is CharacterController)
				return CharacterControllerExtensions.GetVolume(extends as CharacterController);

			throw new NotImplementedException(string.Format("GetVolume for {0}", extends.GetType().Name));
		}

		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="scale">Scale.</param>
		public static float GetVolume(this Collider extends, Vector3 scale)
		{
			return extends.GetVolume() * scale.x * scale.y * scale.z;
		}

		/// <summary>
		/// 	Gets the extents along vector.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="position">Position.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public static void GetExtentsAlongVector(this Collider extends, Vector3 position, Vector3 direction, out Vector3 start,
												 out Vector3 end)
		{
			float colliderMagnitude = extends.bounds.size.magnitude;
			// Fudge it a little
			colliderMagnitude += 1.0f;

			direction = direction.normalized * colliderMagnitude;

			Ray startRay = new Ray(position - direction, direction);
			Ray endRay = new Ray(position + direction, direction * -1.0f);

			RaycastHit findStartCollision;
			extends.Raycast(startRay, out findStartCollision, colliderMagnitude);
			start = findStartCollision.point;

			RaycastHit findEndCollision;
			extends.Raycast(endRay, out findEndCollision, colliderMagnitude);
			end = findEndCollision.point;
		}
	}
}
