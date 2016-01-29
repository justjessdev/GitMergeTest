// <copyright file=Triangle3d company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Shapes._3d
{
	/// <summary>
	/// 	Triangle3d is made up of 3 Vector3s.
	/// </summary>
	public struct Triangle3d
	{
		private readonly Vector3 m_PointA;
		private readonly Vector3 m_PointB;
		private readonly Vector3 m_PointC;

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
		/// 	Gets the point c.
		/// </summary>
		/// <value>The point c.</value>
		public Vector3 pointC { get { return m_PointC; } }

		/// <summary>
		/// 	Gets the line a.
		/// </summary>
		/// <value>The line a.</value>
		public Line3d lineA { get { return new Line3d(m_PointB, m_PointC); } }

		/// <summary>
		/// 	Gets the line b.
		/// </summary>
		/// <value>The line b.</value>
		public Line3d lineB { get { return new Line3d(m_PointC, m_PointA); } }

		/// <summary>
		/// 	Gets the line c.
		/// </summary>
		/// <value>The line c.</value>
		public Line3d lineC { get { return new Line3d(m_PointA, m_PointB); } }

		/// <summary>
		/// 	Gets the normal.
		/// </summary>
		/// <value>The normal.</value>
		public Vector3 normal { get { return GetNormal(m_PointA, m_PointB, m_PointC); } }

		/// <summary>
		/// 	Gets the centroid.
		/// </summary>
		/// <value>The centroid.</value>
		public Vector3 centroid { get { return (m_PointA + m_PointB + m_PointC) / 3.0f; } }

		/// <summary>
		/// 	Gets the area.
		/// </summary>
		/// <value>The area.</value>
		public float area
		{
			get
			{
				float sideA = lineA.length;
				float sideB = lineB.length;
				float sideC = lineC.length;

				float halfPerim = (sideA + sideB + sideC) * 0.5f;

				return Mathf.Sqrt(halfPerim * (halfPerim - sideA) * (halfPerim - sideB) * (halfPerim - sideC));
			}
		}

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Triangle3d struct.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		public Triangle3d(Vector3 pointA, Vector3 pointB, Vector3 pointC)
		{
			m_PointA = pointA;
			m_PointB = pointB;
			m_PointC = pointC;
		}

		#region Methods

		/// <summary>
		/// 	Gets the lines.
		/// </summary>
		/// <param name="output">Output.</param>
		public void GetLines(ref Line3d[] output)
		{
			Array.Resize(ref output, 3);

			output[0] = lineA;
			output[1] = lineB;
			output[2] = lineC;
		}

		/// <summary>
		/// 	Given the triangle a,b,c normals aN,bN,cN this method returns
		/// 	an interpolated normal at the point.
		/// </summary>
		/// <returns>The interpolated normal.</returns>
		/// <param name="nA">Corner A normal.</param>
		/// <param name="nB">Corner B normal.</param>
		/// <param name="nC">Corner C normal.</param>
		/// <param name="point">Point.</param>
		public Vector3 InterpolatedNormal(Vector3 nA, Vector3 nB, Vector3 nC, Vector3 point)
		{
			float totalArea = area;
			float areaB = new Triangle3d(m_PointA, point, m_PointC).area;
			float areaC = new Triangle3d(m_PointA, point, m_PointB).area;
			float areaA = totalArea - areaB - areaC;

			// Calculate coefficients
			float c1 = areaA / totalArea;
			float c2 = areaB / totalArea;
			float c3 = areaC / totalArea;

			return new Vector3(nA.x * c1 + nB.x * c2 + nC.x * c3, nA.y * c1 + nB.y * c2 + nC.y * c3,
							   nA.z * c1 + nB.z * c2 + nC.z * c3);
		}

		/// <summary>
		/// 	Returns true if the point is in the triangle.
		/// 
		/// 	Taken from http://www.blackpawn.com/texts/pointinpoly/
		/// 	
		/// 	TODO - this method assumes that the given point is on the same plane as the triangle.
		/// </summary>
		/// <returns><c>true</c>, if the point is in triangle, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		public bool Contains(Vector3 point)
		{
			// First collapse everything to 2D 
			Matrix4x4 transform = GetRotationMatrix(m_PointA, m_PointB, m_PointC);
			Vector3 transformA = transform * m_PointA;

			// Compute vectors        
			Vector3 v0 = (Vector3)(transform * m_PointC) - transformA;
			Vector3 v1 = (Vector3)(transform * m_PointB) - transformA;
			Vector3 v2 = (Vector3)(transform * point) - transformA;

			// Compute dot products
			float dot00 = Vector3.Dot(v0, v0);
			float dot01 = Vector3.Dot(v0, v1);
			float dot02 = Vector3.Dot(v0, v2);
			float dot11 = Vector3.Dot(v1, v1);
			float dot12 = Vector3.Dot(v1, v2);

			// Compute barycentric coordinates
			float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
			float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
			float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

			// Check if point is in triangle
			return (u >= 0) && (v >= 0) && (u + v < 1);
		}

		/// <summary>
		/// 	Calculates the signed volume of the triangle.
		/// 	
		/// 	http://answers.unity3d.com/questions/52664/how-would-one-calculate-a-3d-mesh-volume-in-unity.html
		/// </summary>
		/// <returns>The volume.</returns>
		public float SignedVolume()
		{
			float v321 = m_PointC.x * m_PointB.y * m_PointA.z;
			float v231 = m_PointB.x * m_PointC.y * m_PointA.z;
			float v312 = m_PointC.x * m_PointA.y * m_PointB.z;
			float v132 = m_PointA.x * m_PointC.y * m_PointB.z;
			float v213 = m_PointB.x * m_PointA.y * m_PointC.z;
			float v123 = m_PointA.x * m_PointB.y * m_PointC.z;

			return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// 	Gets the rotation matrix such that the normal faces forwards.
		/// </summary>
		/// <returns>The rotation matrix.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		public static Matrix4x4 GetRotationMatrix(Vector3 pointA, Vector3 pointB, Vector3 pointC)
		{
			Vector3 normal = GetNormal(pointA, pointB, pointC);
			return Matrix4x4Utils.Rotate(normal, pointB - pointA).inverse;
		}

		/// <summary>
		/// 	Gets the normal.
		/// </summary>
		/// <returns>The normal.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		public static Vector3 GetNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC)
		{
			return Vector3.Cross(pointB - pointA, pointC - pointA).normalized;
		}

		#endregion
	}
}
