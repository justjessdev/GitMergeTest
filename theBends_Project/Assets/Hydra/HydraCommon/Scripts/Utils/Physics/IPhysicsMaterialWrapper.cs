// <copyright file=IPhysicsMaterialWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Physics
{
	/// <summary>
	/// 	IPhysicsMaterialWrapper allows us to work with both 2D and 3D physics materials.
	/// </summary>
	public interface IPhysicsMaterialWrapper
	{
		/// <summary>
		/// 	Gets the bounciness.
		/// </summary>
		/// <value>The bounciness.</value>
		float bounciness { get; }

		/// <summary>
		/// 	Gets the static friction.
		/// </summary>
		/// <value>The static friction.</value>
		float staticFriction { get; }

		/// <summary>
		/// 	Gets the bounce combine.
		/// </summary>
		/// <value>The bounce combine.</value>
		PhysicMaterialCombine bounceCombine { get; }

		/// <summary>
		/// 	Gets the friction combine.
		/// </summary>
		/// <value>The friction combine.</value>
		PhysicMaterialCombine frictionCombine { get; }
	}
}
