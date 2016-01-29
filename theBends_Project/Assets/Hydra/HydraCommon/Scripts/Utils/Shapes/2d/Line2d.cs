// <copyright file=Line2d company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Extensions;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Shapes._2d
{
	/// <summary>
	/// 	Line2d provides functionality for working with lines made of two Vector2s.
	/// </summary>
	public struct Line2d
	{
		private readonly Vector2 m_PointA;
		private readonly Vector2 m_PointB;

		#region Properties

		/// <summary>
		/// 	Gets the point a.
		/// </summary>
		/// <value>The point a.</value>
		public Vector2 pointA { get { return m_PointA; } }

		/// <summary>
		/// 	Gets the point b.
		/// </summary>
		/// <value>The point b.</value>
		public Vector2 pointB { get { return m_PointB; } }

		/// <summary>
		/// 	Gets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public Vector2 direction { get { return m_PointB - m_PointA; } }

		/// <summary>
		/// 	Gets the length.
		/// </summary>
		/// <value>The length.</value>
		public float length { get { return direction.magnitude; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Line2d struct.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		public Line2d(Vector2 pointA, Vector2 pointB)
		{
			m_PointA = pointA;
			m_PointB = pointB;
		}

		#region Methods

		/// <summary>
		/// 	Returns true if the line intersects the given line. Both lines are
		/// 	considered finite.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="intersection">Intersection.</param>
		public bool Intersects(Line2d other, out Vector2 intersection)
		{
			return Intersects(other, true, true, out intersection);
		}

		/// <summary>
		/// 	Calculates the intersection of this line with the given line.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="finiteThis">If set to <c>true</c> this line is finite.</param>
		/// <param name="finiteOther">If set to <c>true</c> the other line is finite.</param>
		/// <param name="intersection">Intersection.</param>
		public bool Intersects(Line2d other, bool finiteThis, bool finiteOther, out Vector2 intersection)
		{
			intersection = Vector2.zero;

			Vector2 b = m_PointB - m_PointA;
			Vector2 d = other.pointB - other.pointA;
			float bDotDPerp = b.x * d.y - b.y * d.x;

			// If bDotDPerp is 0 it means the lines are parallel.
			if (HydraMathUtils.Approximately(bDotDPerp, 0.0f))
				return false;

			Vector2 c = other.pointA - m_PointA;
			float t = (c.x * d.y - c.y * d.x) / bDotDPerp;
			if (finiteThis && (t < 0.0f || t > 1.0f))
				return false;

			float u = (c.x * b.y - c.y * b.x) / bDotDPerp;
			if (finiteOther && (u < 0.0f || u > 1.0f))
				return false;

			intersection = m_PointA + t * b;

			return true;
		}

		/// <summary>
		/// 	Returns the number of intersections with the given rectangle. This line
		/// 	is considered finite.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="intersections">Intersections.</param>
		public int Intersects(Rect rect, ref Vector2[] intersections)
		{
			return Intersects(rect, true, ref intersections);
		}

		/// <summary>
		/// 	Returns the number of intersections with the given rectangle.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="finiteThis">If set to <c>true</c> this line is finite.</param>
		/// <param name="intersections">Intersections.</param>
		public int Intersects(Rect rect, bool finiteThis, ref Vector2[] intersections)
		{
			int output = 0;

			Array.Resize(ref intersections, 4);

			Vector2 topLeft = rect.GetTopLeft();
			Vector2 topRight = rect.GetTopRight();
			Vector2 bottomLeft = rect.GetBottomLeft();
			Vector2 bottomRight = rect.GetBottomRight();

			Line2d top = new Line2d(topLeft, topRight);
			Line2d bottom = new Line2d(bottomLeft, bottomRight);
			Line2d left = new Line2d(topLeft, bottomLeft);
			Line2d right = new Line2d(topRight, bottomRight);

			Vector2 intersection;

			if (Intersects(top, finiteThis, true, out intersection))
			{
				intersections[output] = intersection;
				output++;
			}

			if (Intersects(bottom, finiteThis, true, out intersection))
			{
				intersections[output] = intersection;
				output++;
			}

			if (Intersects(left, finiteThis, true, out intersection))
			{
				intersections[output] = intersection;
				output++;
			}

			if (Intersects(right, finiteThis, true, out intersection))
			{
				intersections[output] = intersection;
				output++;
			}

			return output;
		}

		#endregion
	}
}
