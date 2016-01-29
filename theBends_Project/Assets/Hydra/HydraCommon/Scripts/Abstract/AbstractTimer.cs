// <copyright file=AbstractTimer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	Abstract timer simply keeps track of the progression of time.
	/// </summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public abstract class AbstractTimer : HydraMonoBehaviour
	{
		public const float PREWARM_STEP = 1.0f / 10.0f;
		public const float MAX_PREWARM = 10.0f;

		public const float DEFAULT_START_DELAY = 0.0f;
		public const float DEFAULT_TIME_SCALE = 1.0f;
		public const bool DEFAULT_PLAY_ON_AWAKE = true;
		public const bool DEFAULT_PREWARM = false;

		private float m_Time;
		private bool m_IsPlaying;

		[SerializeField, HideInInspector] private bool m_PlayOnAwake = DEFAULT_PLAY_ON_AWAKE;
		[SerializeField, HideInInspector] private float m_StartDelay = DEFAULT_START_DELAY;
		[SerializeField, HideInInspector] private float m_TimeScale = DEFAULT_TIME_SCALE;
		[SerializeField, HideInInspector] private bool m_Prewarm = DEFAULT_PREWARM;

		#region Properties

		/// <summary>
		/// 	The amount of time in seconds to delay the simulation.
		/// </summary>
		/// <value>The start delay.</value>
		public float startDelay { get { return m_StartDelay; } set { m_StartDelay = ValidateStartDelay(value); } }

		/// <summary>
		/// 	When enabled the timer will start playing as soon as it loads.
		/// </summary>
		/// <value><c>true</c> if play on awake; otherwise, <c>false</c>.</value>
		public bool playOnAwake { get { return m_PlayOnAwake; } set { m_PlayOnAwake = value; } }

		/// <summary>
		/// 	Gets or sets the time scale.
		/// </summary>
		/// <value>The time scale.</value>
		public float timeScale { get { return m_TimeScale; } set { m_TimeScale = ValidateTimeScale(value); } }

		/// <summary>
		/// 	If enabled the emitter will immediately simulate a full particle lifetime's
		/// 	worth of emissions.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool prewarm { get { return m_Prewarm; } set { m_Prewarm = value; } }

		/// <summary>
		/// 	Gets the time.
		/// </summary>
		/// <value>The time.</value>
		public float time { get { return m_Time; } }

		/// <summary>
		/// 	Gets a value indicating whether this
		/// 	<see cref="AbstractTimer"/> is playing.
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool isPlaying { get { return m_IsPlaying; } }

		/// <summary>
		/// 	Gets a value indicating whether this
		/// 	<see cref="AbstractTimer"/> is paused.
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
		public bool isPaused { get { return !isPlaying && m_Time > 0.0f - m_StartDelay; } }

		/// <summary>
		/// 	Gets a value indicating whether this
		/// 	<see cref="AbstractTimer"/> is stopped.
		/// </summary>
		/// <value><c>true</c> if is stopped; otherwise, <c>false</c>.</value>
		public bool isStopped { get { return !isPlaying && !isPaused; } }

		#endregion

		#region Messages

		/// <summary>
		/// 	Called when the component is enabled.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			ResetTimeAlive();

			if (m_PlayOnAwake)
				Restart();
		}

		/// <summary>
		/// 	Called every physics timestep.
		/// </summary>
		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			ProcessTimeIncrement(Time.fixedDeltaTime);
		}

		/// <summary>
		/// 	Called when the editor updates.
		/// </summary>
		/// <param name="updateTime">Update time.</param>
		protected override void OnEditorUpdate(float updateTime)
		{
			base.OnEditorUpdate(updateTime);

			// If the application is playing then the update is already ocurring in FixedUpdate
			if (Application.isPlaying)
				return;

			ProcessTimeIncrement(updateTime);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Plays or pauses the timer.
		/// </summary>
		/// <returns>The new isPlaying state.</returns>
		public bool Play()
		{
			if (m_IsPlaying)
				return Pause();

			bool previouslyStopped = isStopped;

			m_IsPlaying = true;

			if (previouslyStopped)
			{
				if (m_Prewarm)
					Prewarm();
				OnStartPlay();
			}

			return m_IsPlaying;
		}

		/// <summary>
		/// 	Pauses or unpauses the timer.
		/// </summary>
		/// <returns>The new isPlaying state.</returns>
		public bool Pause()
		{
			m_IsPlaying = false;

			return m_IsPlaying;
		}

		/// <summary>
		/// 	Stops the timer.
		/// </summary>
		/// <returns>The new isPlaying state.</returns>
		public bool Stop()
		{
			ResetTimeAlive();
			m_IsPlaying = false;

			OnStop();

			return m_IsPlaying;
		}

		/// <summary>
		/// 	Stops then starts the timer.
		/// </summary>
		/// <returns>The new isPlaying state.</returns>
		public bool Restart()
		{
			Stop();
			return Play();
		}

		#endregion

		#region Virtual Methods

		/// <summary>
		/// 	Gets the length of the prewarm.
		/// </summary>
		/// <returns>The prewarm length.</returns>
		protected virtual float GetPrewarmLength()
		{
			return 0.0f;
		}

		/// <summary>
		/// 	Called when the internal time has changed.
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		protected virtual void OnTimeAliveChanged(float deltaTime)
		{
			RepaintScheduler.RepaintAllViews();
		}

		/// <summary>
		/// 	Called before prewarm begins.
		/// </summary>
		protected virtual void OnStartPrewarm() {}

		/// <summary>
		/// 	OnStartPlay is called when Play() is called to start the timer from 0.0f.
		/// </summary>
		protected virtual void OnStartPlay() {}

		/// <summary>
		/// 	Called when the timer stops.
		/// </summary>
		protected virtual void OnStop() {}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Processes the time increment.
		/// </summary>
		/// <param name="offset">Offset.</param>
		private void ProcessTimeIncrement(float offset)
		{
			offset *= m_TimeScale;

			// Timer isn't playing
			if (!m_IsPlaying)
				return;

			// No change
			if (HydraMathUtils.Approximately(offset, 0.0f))
				return;

			m_Time += offset;

			OnTimeAliveChanged(offset);
		}

		/// <summary>
		/// 	Resets the time alive.
		/// </summary>
		private void ResetTimeAlive()
		{
			m_Time = 0.0f - m_StartDelay;
		}

		/// <summary>
		/// 	Prewarms the timer.
		/// </summary>
		private void Prewarm()
		{
			float length = GetPrewarmLength();
			length = HydraMathUtils.Min(length, MAX_PREWARM);

			m_Time = length * -1.0f;

			OnStartPrewarm();

			float cumulative;
			for (cumulative = 0.0f; cumulative < length; cumulative += PREWARM_STEP)
				ProcessTimeIncrement(PREWARM_STEP);

			ProcessTimeIncrement(length - cumulative);
		}

		#endregion

		/// <summary>
		/// 	Validates the start delay.
		/// </summary>
		/// <returns>The start delay.</returns>
		/// <param name="delay">Delay.</param>
		public static float ValidateStartDelay(float delay)
		{
			return HydraMathUtils.Max(delay, 0.0f);
		}

		/// <summary>
		/// 	Validates the time scale.
		/// </summary>
		/// <returns>The time scale.</returns>
		/// <param name="scale">Scale.</param>
		public static float ValidateTimeScale(float scale)
		{
			return HydraMathUtils.Clamp(scale, 0.0f, 10.0f);
		}
	}
}
