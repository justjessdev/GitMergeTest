// <copyright file=Hit3DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	Hit3DWrapper provides collision data.
	/// </summary>
	public struct Hit3DWrapper : IHitWrapper
	{
		/// <summary>
		/// 	The centroid is slightly fudged away from the surface since 3d raycast
		/// 	doesn't hit things at 0 distance.
		/// </summary>
		private const float CENTROID_FUDGE = 0.001f;

		private readonly RaycastHit m_Hit;
		private readonly Vector3 m_Origin;
		private readonly float m_Radius;
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
		public Vector3 point { get { return m_Hit.point; } }

		/// <summary>
		/// 	Gets the centroid.
		/// </summary>
		/// <value>The centroid.</value>
		public Vector3 centroid { get { return point + normal * (m_Radius + CENTROID_FUDGE); } }

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
		public IColliderWrapper collider { get { return new Collider3DWrapper(m_Hit.collider); } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Hit3DWrapper struct.
		/// </summary>
		/// <param name="hit">Hit.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		public Hit3DWrapper(RaycastHit hit, Vector3 origin, float radius, Vector3 direction, float distance)
		{
			m_Hit = hit;
			m_Origin = origin;
			m_Radius = radius;
			m_Distance = distance;
		}
	}
}
