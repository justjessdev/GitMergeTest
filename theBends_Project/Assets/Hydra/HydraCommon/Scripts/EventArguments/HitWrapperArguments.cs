// <copyright file=HitWrapperArguments company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils.Physics;

namespace Hydra.HydraCommon.EventArguments
{
	/// <summary>
	/// 	Provides params used in 2d or 3d hit detection events.
	/// </summary>
	public class HitWrapperArguments : EventArgs
	{
		private readonly IHitWrapper m_HitWrapper;

		#region Properties

		/// <summary>
		/// 	Gets the hit wrapper.
		/// </summary>
		/// <value>The hit wrapper.</value>
		public IHitWrapper hitWrapper { get { return m_HitWrapper; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the HitWrapperArguments class.
		/// </summary>
		/// <param name="hitWrapper">Hit wrapper.</param>
		public HitWrapperArguments(IHitWrapper hitWrapper)
		{
			m_HitWrapper = hitWrapper;
		}
	}
}
