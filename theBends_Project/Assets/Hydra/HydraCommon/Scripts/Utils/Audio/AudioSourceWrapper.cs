// <copyright file=AudioSourceWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.API;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Audio
{
	/// <summary>
	/// 	AudioSourceWrapper provides a callback after playing an audio clip.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class AudioSourceWrapper : HydraMonoBehaviour, IRecyclable
	{
		public event EventHandler onClipsFinishedCallback;

		private AudioSource m_CachedAudioSource;
		private float m_CompletionTime;
		private bool m_Completed = true;

		#region Properties

		/// <summary>
		/// 	Gets the audio source.
		/// </summary>
		/// <value>The audio source.</value>
		public AudioSource audioSource
		{
			get { return m_CachedAudioSource ?? (m_CachedAudioSource = GetComponent<AudioSource>()); }
		}

		#endregion

		#region Messages

		/// <summary>
		/// 	Called once every frame.
		/// </summary>
		protected override void Update()
		{
			base.Update();

			// The callback has already been executed.
			if (m_Completed)
				return;

			if (m_CompletionTime > Time.time)
				return;

			m_Completed = true;

			EventHandler handler = onClipsFinishedCallback;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Plays the audio clip.
		/// </summary>
		/// <param name="clip">Clip.</param>
		/// <param name="volumeScale">Volume scale.</param>
		public void PlayOneShot(AudioClip clip, float volumeScale)
		{
			audioSource.PlayOneShot(clip, volumeScale);

			float clipLength = AudioSourceUtils.GetClipLength(clip, audioSource.pitch);

			m_CompletionTime = Time.time + clipLength;
			m_Completed = false;
		}

		#endregion
	}
}
