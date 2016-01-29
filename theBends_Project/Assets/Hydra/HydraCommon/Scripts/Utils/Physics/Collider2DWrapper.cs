// <copyright file=Collider2DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	Collider2DWrapper provides collider information. 
	/// </summary>
	public struct Collider2DWrapper : IColliderWrapper
	{
		private readonly Collider2D m_Collider;

		#region Properties

		/// <summary>
		/// 	Gets the shared material.
		/// </summary>
		/// <value>The shared material.</value>
		public IPhysicsMaterialWrapper sharedMaterial
		{
			get { return new PhysicsMaterial2DWrapper(m_Collider.sharedMaterial); }
		}

		/// <summary>
		/// 	Gets the game object.
		/// </summary>
		/// <value>The game object.</value>
		public GameObject gameObject { get { return (m_Collider != null) ? m_Collider.gameObject : null; } }

		/// <summary>
		/// 	Gets the rigid body.
		/// </summary>
		/// <value>The rigid body.</value>
		public Rigidbody rigidBody { get { return (gameObject != null) ? gameObject.GetComponent<Rigidbody>() : null; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the Collider2DWrapper struct.
		/// </summary>
		/// <param name="collider">Collider.</param>
		public Collider2DWrapper(Collider2D collider)
		{
			m_Collider = collider;
		}
	}
}
