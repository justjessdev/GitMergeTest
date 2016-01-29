// <copyright file=AnimationEventArguments company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.EventArguments
{
	/// <summary>
	/// 	Provides params used in animation events.
	/// </summary>
	public class AnimationEventArguments : EventArgs
	{
		private readonly AnimationEvent m_AnimationEvent;

		public AnimationEvent animationEvent { get { return m_AnimationEvent; } }

		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="Hydra.HydraCommon.EventArguments.AnimationEventArguments"/> class.
		/// </summary>
		/// <param name="animationEvent">Animation event.</param>
		public AnimationEventArguments(AnimationEvent animationEvent)
		{
			m_AnimationEvent = animationEvent;
		}
	}
}
