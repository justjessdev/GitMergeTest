// <copyright file=AnimationEventReceiver company=Hydra>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.EventArguments;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	[RequireComponent(typeof(Animator))]
	public class AnimationEventReceiver : HydraMonoBehaviour
	{
		public event EventHandler<AnimationEventArguments> onAnimationEventCallback;

		private Animator m_CachedAnimator;

		#region Properties

		/// <summary>
		/// 	Gets the animator.
		/// </summary>
		/// <value>The animator.</value>
		public Animator animator { get { return m_CachedAnimator ?? (m_CachedAnimator = GetComponent<Animator>()); } }

		#endregion

		#region Messages

		/// <summary>
		/// 	Called when the component is enabled.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			Subscribe();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Subscribe to the animator events.
		/// </summary>
		private void Subscribe()
		{
			string functionName = (OnEventFired as Action<AnimationEvent>).Method.Name;
			AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

			for (int index = 0; index < clips.Length; index++)
			{
				AnimationEvent[] events = clips[index].events;

				for (int eventIndex = 0; eventIndex < events.Length; eventIndex++)
				{
					AnimationEvent animEvent = events[eventIndex];

					if (animEvent.functionName == functionName)
						continue;

					animEvent.stringParameter = animEvent.functionName;
					animEvent.functionName = functionName;
				}

				clips[index].events = events;
			}
		}

		/// <summary>
		/// 	Called when an animation event is fired.
		/// </summary>
		/// <param name="animationEvent">Animation event.</param>
		public void OnEventFired(AnimationEvent animationEvent)
		{
			EventHandler<AnimationEventArguments> handler = onAnimationEventCallback;
			if (handler != null)
				handler(this, new AnimationEventArguments(animationEvent));
		}

		#endregion
	}
}
