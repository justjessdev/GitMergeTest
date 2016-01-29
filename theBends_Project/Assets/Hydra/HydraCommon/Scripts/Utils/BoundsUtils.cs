// <copyright file=BoundsUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	BoundsUtils provides util methods for working with Bounds.
	/// </summary>
	public static class BoundsUtils
	{
		/// <summary>
		/// 	Gets the bounds.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="positions">Positions.</param>
		public static Bounds GetBounds(Vector3[] positions)
		{
			Vector3 min = (positions.Length > 0) ? positions[0] : Vector3.zero;
			Vector3 max = min;

			float xMin = min.x;
			float xMax = max.x;
			float yMin = min.y;
			float yMax = max.y;
			float zMin = min.z;
			float zMax = max.z;

			for (int index = 1; index < positions.Length; index++)
			{
				Vector3 thisCenter = positions[index];

				xMin = HydraMathUtils.Min(xMin, thisCenter.x);
				xMax = HydraMathUtils.Max(xMax, thisCenter.x);
				yMin = HydraMathUtils.Min(yMin, thisCenter.y);
				yMax = HydraMathUtils.Max(yMax, thisCenter.y);
				zMin = HydraMathUtils.Min(zMin, thisCenter.z);
				zMax = HydraMathUtils.Max(zMax, thisCenter.z);
			}

			return MinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
		}

		/// <summary>
		/// 	Gets the corners.
		/// </summary>
		/// <param name="bounds">Bounds.</param>
		/// <param name="corners">Corners.</param>
		public static void GetCorners(Bounds bounds, ref Vector3[] corners)
		{
			Array.Resize(ref corners, 8);

			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			corners[0] = min;
			corners[1] = new Vector3(max.x, min.y, min.z);
			corners[2] = new Vector3(max.x, min.y, max.z);
			corners[3] = new Vector3(min.x, min.y, max.z);

			corners[4] = max;
			corners[5] = new Vector3(max.x, max.y, min.z);
			corners[6] = new Vector3(min.x, max.y, min.z);
			corners[7] = new Vector3(min.x, max.y, max.z);
		}

		/// <summary>
		/// 	Creates a Bounds from a minimum and maximum point.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static Bounds MinMax(Vector3 min, Vector3 max)
		{
			Vector3 center = (min + max) / 2.0f;
			Vector3 size = max - min;

			return new Bounds(center, size);
		}

		/// <summary>
		/// 	Returns the bounds of the given Bounds list
		/// </summary>
		/// <returns>The combined bounds.</returns>
		/// <param name="bounds">Bounds.</param>
		public static Bounds CombineBounds(Bounds[] bounds)
		{
			if (bounds.Length == 0)
				return new Bounds();

			Bounds first = bounds[0];

			Vector3 firstCenter = first.center;
			Vector3 firstExtents = first.extents;

			Vector3 min = firstCenter - firstExtents;
			Vector3 max = firstCenter + firstExtents;

			float xMin = min.x;
			float xMax = max.x;
			float yMin = min.y;
			float yMax = max.y;
			float zMin = min.z;
			float zMax = max.z;

			for (int index = 1; index < bounds.Length; index++)
			{
				Bounds thisBounds = bounds[index];

				Vector3 thisCenter = thisBounds.center;
				Vector3 thisExtents = thisBounds.extents;

				Vector3 thisMin = thisCenter - thisExtents;
				Vector3 thisMax = thisCenter + thisExtents;

				xMin = HydraMathUtils.Min(xMin, thisMin.x);
				xMax = HydraMathUtils.Max(xMax, thisMax.x);
				yMin = HydraMathUtils.Min(yMin, thisMin.y);
				yMax = HydraMathUtils.Max(yMax, thisMax.y);
				zMin = HydraMathUtils.Min(zMin, thisMin.z);
				zMax = HydraMathUtils.Max(zMax, thisMax.z);
			}

			return MinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
		}

		/// <summary>
		/// 	Calculates the axis-aligned bounding box for a transformed bounding box.
		/// </summary>
		/// <returns>The axis aligned bounds.</returns>
		/// <param name="mesh">Mesh.</param>
		/// <param name="transform">Transform.</param>
		public static Bounds AABB(Mesh mesh, Matrix4x4 transform)
		{
			Bounds bounds = (mesh != null) ? mesh.bounds : new Bounds();
			return AABB(bounds, transform);
		}

		/// <summary>
		/// 	Calculates the axis-aligned bounding box for a transformed bounding box.
		/// </summary>
		/// <returns>The axis aligned bounds.</returns>
		/// <param name="input">Input.</param>
		/// <param name="transform">Transform.</param>
		public static Bounds AABB(Bounds input, Matrix4x4 transform)
		{
			Vector3 center = input.center;
			Vector3 extents = input.extents;

			Vector3 min = transform.MultiplyPoint3x4(center - extents);
			Vector3 max = transform.MultiplyPoint3x4(center + extents);

			return MinMax(min, max);
		}

		/// <summary>
		/// Finds the base.
		/// </summary>
		/// <returns>The base.</returns>
		/// <param name="input">Input.</param>
		public static Vector3 FindBase(Bounds input)
		{
			Vector3 center = input.center;

			return new Vector3(center.x, input.min.y, center.z);
		}
	}
}
