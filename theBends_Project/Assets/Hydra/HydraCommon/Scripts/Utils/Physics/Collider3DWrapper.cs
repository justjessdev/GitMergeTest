// <copyright file=Collider3DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	Collider3DWrapper provides collider information. 
	/// </summary>
	public struct Collider3DWrapper : IColliderWrapper
	{
		private readonly Collider m_Collider;

		#region Properties

		/// <summary>
		/// 	Gets the shared material.
		/// </summary>
		/// <value>The shared material.</value>
		public IPhysicsMaterialWrapper sharedMaterial
		{
			get { return new PhysicsMaterial3DWrapper(m_Collider.sharedMaterial); }
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
		/// 	Initializes a new instance of the Collider3DWrapper struct.
		/// </summary>
		/// <param name="collider">Collider.</param>
		public Collider3DWrapper(Collider collider)
		{
			m_Collider = collider;
		}
	}
}
