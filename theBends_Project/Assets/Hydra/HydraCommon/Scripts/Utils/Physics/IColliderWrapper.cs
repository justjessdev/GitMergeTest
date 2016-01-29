// <copyright file=IColliderWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	IColliderWrapper allows us to work with both 2D and 3D colliders.
	/// </summary>
	public interface IColliderWrapper
	{
		/// <summary>
		/// 	Gets the shared material.
		/// </summary>
		/// <value>The shared material.</value>
		IPhysicsMaterialWrapper sharedMaterial { get; }

		/// <summary>
		/// 	Gets the game object.
		/// </summary>
		/// <value>The game object.</value>
		GameObject gameObject { get; }

		/// <summary>
		/// 	Gets the rigid body.
		/// </summary>
		/// <value>The rigid body.</value>
		Rigidbody rigidBody { get; }
	}
}
