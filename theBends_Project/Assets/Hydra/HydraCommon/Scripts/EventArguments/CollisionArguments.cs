// <copyright file=CollisionArguments company=Hydra>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.EventArguments
{
	/// <summary>
	/// 	Provides params used in collision events.
	/// </summary>
	public class CollisionArguments : EventArgs
	{
		private readonly Collision m_Collision;

		public Collision collision { get { return m_Collision; } }

		/// <summary>
		/// 	Initializes a new instance of the CollisionArguments class.
		/// </summary>
		/// <param name="collision">Collision.</param>
		public CollisionArguments(Collision collision)
		{
			m_Collision = collision;
		}
	}
}
