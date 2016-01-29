// <copyright file=Hit2DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	Hit2DWrapper provides collision data.
	/// </summary>
	public struct Hit2DWrapper : IHitWrapper
	{
		private readonly RaycastHit2D m_Hit;
		private readonly Vector3 m_Origin;
		private readonly Vector3 m_Direction;
		private readonly float m_Distance;

		#region Properties

		/// <summary>
		/// 	Gets a value indicating whether the raycast collided.
		/// </summary>
		/// <value><c>true</c> if collided; otherwise, <c>false</c>.</value>
		public bool collided { get { return m_Hit.collider != null; } }

		/// <summary>
		/// 	Gets the collision point.
		/// </summary>
		/// <value>The collision point.</value>
		public Vector3 point { get { return new Vector3(m_Hit.point.x, m_Hit.point.y, GetZHit()); } }

		/// <summary>
		/// 	Gets the centroid.
		/// </summary>
		/// <value>The centroid.</value>
		public Vector3 centroid { get { return new Vector3(m_Hit.centroid.x, m_Hit.centroid.y, GetZHit()); } }

		/// <summary>
		/// 	Gets the normal at the collision surface.
		/// </summary>
		/// <value>The normal.</value>
		public Vector3 normal { get { return m_Hit.normal; } }

		/// <summary>
		/// 	Gets the distance from the start of the raycast to the collision.
		/// </summary>
		/// <value>The distance.</value>
		public float distance { get { return Vector3.Distance(m_Origin, centroid); } }

		/// <summary>
		/// 	Gets the fraction from the start of the raycast to the collision.
		/// </summary>
		/// <value>The fraction.</value>
		public float fraction { get { return distance / m_Distance; } }

		/// <summary>
		/// 	Gets the collider.
		/// </summary>
		/// <value>The collider.</value>
		public IColliderWrapper collider { get { return new Collider2DWrapper(m_Hit.collider); } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Hit2DWrapper struct.
		/// </summary>
		/// <param name="hit">Hit.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		public Hit2DWrapper(RaycastHit2D hit, Vector3 origin, float radius, Vector3 direction, float distance)
		{
			m_Hit = hit;
			m_Origin = origin;
			m_Direction = direction.normalized;
			m_Distance = distance;
		}

		/// <summary>
		/// 	Extrapolates the z co-ordinate of the 2D collision.
		/// </summary>
		/// <returns>The Z co-ordinate.</returns>
		private float GetZHit()
		{
			return m_Origin.z + m_Hit.fraction * m_Direction.z * m_Distance;
		}
	}
}
