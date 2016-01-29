// <copyright file=Triangle2d company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Shapes._2d
{
	/// <summary>
	/// 	Triangle2d is made up of 3 Vector2s.
	/// </summary>
	public struct Triangle2d
	{
		private readonly Vector2 m_PointA;
		private readonly Vector2 m_PointB;
		private readonly Vector2 m_PointC;

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
		/// 	Gets the point c.
		/// </summary>
		/// <value>The point c.</value>
		public Vector2 pointC { get { return m_PointB; } }

		/// <summary>
		/// 	Gets the line a.
		/// </summary>
		/// <value>The line a.</value>
		public Line2d lineA { get { return new Line2d(m_PointB, m_PointC); } }

		/// <summary>
		/// 	Gets the line b.
		/// </summary>
		/// <value>The line b.</value>
		public Line2d lineB { get { return new Line2d(m_PointC, m_PointA); } }

		/// <summary>
		/// 	Gets the line c.
		/// </summary>
		/// <value>The line c.</value>
		public Line2d lineC { get { return new Line2d(m_PointA, m_PointB); } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Triangle2d struct.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		public Triangle2d(Vector2 pointA, Vector2 pointB, Vector2 pointC)
		{
			m_PointA = pointA;
			m_PointB = pointB;
			m_PointC = pointC;
		}
	}
}
