// <copyright file=PhysicsUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	Provides util methods for working with Unity's physics objects.
	/// </summary>
	public static class PhysicsUtils
	{
		public const float DEFAULT_BOUNCE = 0.0f;
		public const float DEFAULT_FRICTION = 0.4f;
		public const PhysicMaterialCombine DEFAULT_BOUNCE_COMBINE = PhysicMaterialCombine.Average;
		public const PhysicMaterialCombine DEFAULT_FRICTION_COMBINE = PhysicMaterialCombine.Average;

		/// <summary>
		/// 	Returns the resulting PhysicMaterialCombine for two colliding physics materials.
		/// 
		/// 	Maximum > Multiply > Minimum > Average
		/// </summary>
		/// <returns>The resulting physic material combine.</returns>
		/// <param name="combineA">Combine a.</param>
		/// <param name="combineB">Combine b.</param>
		public static PhysicMaterialCombine ResultingPhysicMaterialCombine(PhysicMaterialCombine combineA,
																		   PhysicMaterialCombine combineB)
		{
			if (combineA == PhysicMaterialCombine.Maximum || combineB == PhysicMaterialCombine.Maximum)
				return PhysicMaterialCombine.Maximum;

			if (combineA == PhysicMaterialCombine.Multiply || combineB == PhysicMaterialCombine.Multiply)
				return PhysicMaterialCombine.Multiply;

			if (combineA == PhysicMaterialCombine.Minimum || combineB == PhysicMaterialCombine.Minimum)
				return PhysicMaterialCombine.Minimum;

			return PhysicMaterialCombine.Average;
		}

		/// <summary>
		/// 	Calculates the bounce of two surfaces colliding. Returns zero if the
		/// 	bounce is below the bounce threshold.
		/// </summary>
		/// <param name="bounceA">Bounce a.</param>
		/// <param name="bounceB">Bounce b.</param>
		/// <param name="velocityA">Velocity a.</param>
		/// <param name="velocityB">Velocity b.</param>
		/// <param name="combine">Combine.</param>
		public static float CombineBounce(float bounceA, float bounceB, Vector3 velocityA, Vector3 velocityB,
										  PhysicMaterialCombine combine)
		{
			Vector3 relativeVelocity = velocityA - velocityB;
			if (relativeVelocity.magnitude < UnityEngine.Physics.bounceThreshold)
				return 0.0f;

			return Combine(bounceA, bounceB, combine);
		}

		/// <summary>
		/// 	Combines the friction of two surfaces.
		/// </summary>
		/// <returns>The combined friction.</returns>
		/// <param name="frictionA">Friction a.</param>
		/// <param name="frictionB">Friction b.</param>
		/// <param name="combine">Combine.</param>
		public static float CombineFriction(float frictionA, float frictionB, PhysicMaterialCombine combine)
		{
			return Combine(frictionA, frictionB, combine);
		}

		/// <summary>
		/// 	Combines the surface values.
		/// </summary>
		/// <returns>The combined surface value.</returns>
		/// <param name="surfaceA">Surface a.</param>
		/// <param name="surfaceB">Surface b.</param>
		/// <param name="combine">Combine.</param>
		public static float Combine(float surfaceA, float surfaceB, PhysicMaterialCombine combine)
		{
			switch (combine)
			{
				case PhysicMaterialCombine.Average:
					return (surfaceA + surfaceB) / 2.0f;

				case PhysicMaterialCombine.Maximum:
					return HydraMathUtils.Max(surfaceA, surfaceB);

				case PhysicMaterialCombine.Minimum:
					return HydraMathUtils.Min(surfaceA, surfaceB);

				case PhysicMaterialCombine.Multiply:
					return surfaceA * surfaceB;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Performs a sphere cast in 3D and a circle cast in 2D.
		/// </summary>
		/// <returns>The closest collision information.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="minDepth">Minimum depth.</param>
		/// <param name="maxDepth">Max depth.</param>
		public static IHitWrapper SphereCirclecast2D3D(Vector3 origin, float radius, Vector3 direction, float distance,
													   int layerMask, float minDepth, float maxDepth)
		{
			IHitWrapper hit3d = Spherecast3D(origin, radius, direction, distance, layerMask);
			IHitWrapper hit2d = Circlecast2D(origin, radius, direction, distance, layerMask, minDepth, maxDepth);

			if (!hit3d.collided)
				return hit2d;

			if (!hit2d.collided)
				return hit3d;

			return (hit2d.distance < hit3d.distance) ? hit2d : hit3d;
		}

		/// <summary>
		/// 	Performs a sphere cast in 3D.
		/// </summary>
		/// <returns>The collision information.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static Hit3DWrapper Spherecast3D(Vector3 origin, float radius, Vector3 direction, float distance, int layerMask)
		{
			if (layerMask == LayerMaskUtils.EMPTY)
				return default(Hit3DWrapper);

			RaycastHit hit;
			UnityEngine.Physics.SphereCast(origin, radius, direction, out hit, distance, layerMask);

			return new Hit3DWrapper(hit, origin, radius, direction, distance);
		}

		/// <summary>
		/// 	Performs a circle cast in 2D.
		/// </summary>
		/// <returns>The collision information.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="minDepth">Minimum depth.</param>
		/// <param name="maxDepth">Max depth.</param>
		public static Hit2DWrapper Circlecast2D(Vector3 origin, float radius, Vector3 direction, float distance, int layerMask,
												float minDepth, float maxDepth)
		{
			if (layerMask == LayerMaskUtils.EMPTY)
				return default(Hit2DWrapper);

			RaycastHit2D collision = Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);

			return new Hit2DWrapper(collision, origin, radius, direction, distance);
		}

		/// <summary>
		/// 	Performs a raycast in both 2D and 3D. Returns data for the closest collision.
		/// </summary>
		/// <returns>The data for the closest collision.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="minDepth">Minimum depth.</param>
		/// <param name="maxDepth">Max depth.</param>
		public static IHitWrapper Raycast2D3D(Vector3 origin, Vector3 direction, float distance, int layerMask, float minDepth,
											  float maxDepth)
		{
			IHitWrapper hit3d = Raycast3D(origin, direction, distance, layerMask);
			IHitWrapper hit2d = Raycast2D(origin, direction, distance, layerMask, minDepth, maxDepth);

			if (!hit3d.collided)
				return hit2d;

			if (!hit2d.collided)
				return hit3d;

			return (hit2d.distance < hit3d.distance) ? hit2d : hit3d;
		}

		/// <summary>
		/// 	Performs a 3D raycast and returns the collision data as a Hit3DWrapper.
		/// </summary>
		/// <returns>The collision data.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static Hit3DWrapper Raycast3D(Vector3 origin, Vector3 direction, float distance, int layerMask)
		{
			if (layerMask == LayerMaskUtils.EMPTY)
				return default(Hit3DWrapper);

			RaycastHit hit;
			UnityEngine.Physics.Raycast(origin, direction, out hit, distance, layerMask);

			return new Hit3DWrapper(hit, origin, 0.0f, direction, distance);
		}

		/// <summary>
		/// 	Performs a 2D raycast without discarding the Z component.
		/// 	
		/// 	This method also ignores any collision at the start of the ray. This is consistent
		/// 	with the 3D raycast behaviour.
		/// </summary>
		/// <returns>The collision data.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="minDepth">Minimum depth.</param>
		/// <param name="maxDepth">Max depth.</param>
		public static Hit2DWrapper Raycast2D(Vector3 origin, Vector3 direction, float distance, int layerMask, float minDepth,
											 float maxDepth)
		{
			if (layerMask == LayerMaskUtils.EMPTY)
				return default(Hit2DWrapper);

			RaycastHit2D[] collisions = Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);

			for (int index = 0; index < collisions.Length; index++)
			{
				RaycastHit2D hit2D = collisions[index];

				if (HydraMathUtils.Approximately(hit2D.fraction, 0.0f))
					continue;

				return new Hit2DWrapper(hit2D, origin, 0.0f, direction, distance);
			}

			return default(Hit2DWrapper);
		}
	}
}
