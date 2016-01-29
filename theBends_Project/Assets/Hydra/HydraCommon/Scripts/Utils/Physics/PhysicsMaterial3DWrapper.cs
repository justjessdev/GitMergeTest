// <copyright file=PhysicsMaterial3DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	PhysicsMaterial3DWrapper provides physics material information.
	/// </summary>
	public class PhysicsMaterial3DWrapper : IPhysicsMaterialWrapper
	{
		private readonly PhysicMaterial m_Material;

		#region Properties

		/// <summary>
		/// 	Gets the bounciness.
		/// </summary>
		/// <value>The bounciness.</value>
		public float bounciness { get { return (m_Material != null) ? m_Material.bounciness : PhysicsUtils.DEFAULT_BOUNCE; } }

		/// <summary>
		/// 	Gets the static friction.
		/// </summary>
		/// <value>The static friction.</value>
		public float staticFriction
		{
			get { return (m_Material != null) ? m_Material.staticFriction : PhysicsUtils.DEFAULT_FRICTION; }
		}

		/// <summary>
		/// 	Gets the bounce combine.
		/// </summary>
		/// <value>The bounce combine.</value>
		public PhysicMaterialCombine bounceCombine
		{
			get { return (m_Material != null) ? m_Material.bounceCombine : PhysicsUtils.DEFAULT_BOUNCE_COMBINE; }
		}

		/// <summary>
		/// 	Gets the friction combine.
		/// </summary>
		/// <value>The friction combine.</value>
		public PhysicMaterialCombine frictionCombine
		{
			get { return (m_Material != null) ? m_Material.frictionCombine : PhysicsUtils.DEFAULT_FRICTION_COMBINE; }
		}

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the PhysicsMaterial3DWrapper class.
		/// </summary>
		/// <param name="material">Material.</param>
		public PhysicsMaterial3DWrapper(PhysicMaterial material)
		{
			m_Material = material;
		}
	}
}
