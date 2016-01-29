// <copyright file=IHitWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	IHitWrapper allows us to work with both 2D and 3D collisions.
	/// </summary>
	public interface IHitWrapper
	{
		/// <summary>
		/// 	Gets a value indicating whether the raycast collided.
		/// </summary>
		/// <value><c>true</c> if collided; otherwise, <c>false</c>.</value>
		bool collided { get; }

		/// <summary>
		/// 	Gets the collision point.
		/// </summary>
		/// <value>The collision point.</value>
		Vector3 point { get; }

		/// <summary>
		/// 	Gets the centroid.
		/// </summary>
		/// <value>The centroid.</value>
		Vector3 centroid { get; }

		/// <summary>
		/// 	Gets the normal at the collision surface.
		/// </summary>
		/// <value>The normal.</value>
		Vector3 normal { get; }

		/// <summary>
		/// 	Gets the distance from the start of the raycast to the collision.
		/// </summary>
		/// <value>The distance.</value>
		float distance { get; }

		/// <summary>
		/// 	Gets the fraction from the start of the raycast to the collision.
		/// </summary>
		/// <value>The fraction.</value>
		float fraction { get; }

		/// <summary>
		/// 	Gets the collider.
		/// </summary>
		/// <value>The collider.</value>
		IColliderWrapper collider { get; }
	}
}
