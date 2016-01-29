// <copyright file=PhysicsMaterial2DWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	PhysicsMaterial2DWrapper provides physics material information.
	/// </summary>
	public class PhysicsMaterial2DWrapper : IPhysicsMaterialWrapper
	{
		private readonly PhysicsMaterial2D m_Material;

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
			get { return (m_Material != null) ? m_Material.friction : PhysicsUtils.DEFAULT_FRICTION; }
		}

		/// <summary>
		/// 	Gets the bounce combine.
		/// </summary>
		/// <value>The bounce combine.</value>
		public PhysicMaterialCombine bounceCombine { get { return PhysicsUtils.DEFAULT_BOUNCE_COMBINE; } }

		/// <summary>
		/// 	Gets the friction combine.
		/// </summary>
		/// <value>The friction combine.</value>
		public PhysicMaterialCombine frictionCombine { get { return PhysicsUtils.DEFAULT_FRICTION_COMBINE; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the PhysicsMaterial2DWrapper class.
		/// </summary>
		/// <param name="material">Material.</param>
		public PhysicsMaterial2DWrapper(PhysicsMaterial2D material)
		{
			m_Material = material;
		}
	}
}
