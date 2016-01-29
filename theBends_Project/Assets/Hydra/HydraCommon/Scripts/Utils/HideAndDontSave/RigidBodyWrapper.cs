// <copyright file=RigidBodyWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.HideAndDontSave
{
	/// <summary>
	/// 	Rigid body wrapper.
	/// </summary>
	public class RigidBodyWrapper : AbstractHideAndDontSaveWrapper
	{
		private Rigidbody m_RigidBody;

		#region Properties

		/// <summary>
		/// 	Gets or sets the rigid body.
		/// </summary>
		/// <value>The rigid body.</value>
		public Rigidbody rigidBody { get { return m_RigidBody; } set { m_RigidBody = value; } }

		#endregion

		/// <summary>
		/// 	Override this method to configure the child upon instantiation, e.g. to add components.
		/// </summary>
		/// <param name="child">Child.</param>
		protected override void ConfigureChild(GameObject child)
		{
			base.ConfigureChild(child);

			m_RigidBody = child.AddComponent<Rigidbody>();
		}
	}
}
