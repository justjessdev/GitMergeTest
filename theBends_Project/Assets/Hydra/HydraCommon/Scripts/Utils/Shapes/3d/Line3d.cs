// <copyright file=Line3d company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Utils.Shapes._2d;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Shapes._3d
{
	/// <summary>
	/// 	Line3d provides functionality for working with lines made of two Vector3s.
	/// </summary>
	public struct Line3d
	{
		private readonly Vector3 m_PointA;
		private readonly Vector3 m_PointB;

		#region Properties

		/// <summary>
		/// 	Gets the point a.
		/// </summary>
		/// <value>The point a.</value>
		public Vector3 pointA { get { return m_PointA; } }

		/// <summary>
		/// 	Gets the point b.
		/// </summary>
		/// <value>The point b.</value>
		public Vector3 pointB { get { return m_PointB; } }

		/// <summary>
		/// 	Gets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public Vector3 direction { get { return m_PointB - m_PointA; } }

		/// <summary>
		/// 	Gets the length.
		/// </summary>
		/// <value>The length.</value>
		public float length { get { return direction.magnitude; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Line3d struct.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		public Line3d(Vector3 pointA, Vector3 pointB)
		{
			m_PointA = pointA;
			m_PointB = pointB;
		}

		#region Methods

		/// <summary>
		/// 	Returns true if the lines are identical ignoring .
		/// </summary>
		/// <returns><c>true</c> if this instance is equal unordered the specified other; otherwise, <c>false</c>.</returns>
		/// <param name="other">Other.</param>
		public bool IsEqualUnordered(Line3d other)
		{
			return this == other || this == new Line3d(other.pointB, other.pointA);
		}

		/// <summary>
		/// 	Flattens the line in the y axis.
		/// </summary>
		/// <returns>The 2d line in the x/z plane.</returns>
		public Line2d FlattenY()
		{
			Vector2 flatA = new Vector2(m_PointA.x, m_PointA.z);
			Vector2 flatB = new Vector2(m_PointB.x, m_PointB.z);

			return new Line2d(flatA, flatB);
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// 	Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hydra.HydraCommon.Utils.Shapes._3d.Line3d"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Hydra.HydraCommon.Utils.Shapes._3d.Line3d"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hydra.HydraCommon.Utils.Shapes._3d.Line3d"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Line3d))
				return false;

			Line3d other = (Line3d)obj;

			return (m_PointA == other.pointA) && (m_PointB == other.pointB);
		}

		/// <summary>
		/// 	Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return m_PointA.GetHashCode() ^ m_PointB.GetHashCode();
		}

		#endregion

		/// <summary>
		/// 	Determines if the two items are equal.
		/// </summary>
		/// <param name="lineA">Line a.</param>
		/// <param name="lineB">Line b.</param>
		public static bool operator ==(Line3d lineA, Line3d lineB)
		{
			return lineA.Equals(lineB);
		}

		/// <summary>
		/// 	Determines if the two items are inequal.
		/// </summary>
		/// <param name="lineA">Line a.</param>
		/// <param name="lineB">Line b.</param>
		public static bool operator !=(Line3d lineA, Line3d lineB)
		{
			return !lineA.Equals(lineB);
		}
	}
}
