// <copyright file=AudioSourcePool company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Audio
{
	/// <summary>
	/// 	AudioSourcePool allows for playing sound effects on temporary AudioSources.
	/// </summary>
	public static class AudioSourcePool
	{
		private static AudioSourceFactory m_Factory;
		private static ObjectPool<AudioSourceWrapper, AudioSourceFactory> m_Pool;

		/// <summary>
		/// 	Initializes the <see cref="Hydra.HydraCommon.Utils.Audio.AudioSourcePool"/> class.
		/// </summary>
		static AudioSourcePool()
		{
			m_Factory = new AudioSourceFactory();
			m_Pool = new ObjectPool<AudioSourceWrapper, AudioSourceFactory>(m_Factory);
		}

		#region Methods

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="clip">Clip.</param>
		public static AudioSource PlayOneShot(GameObject parent, AudioClip clip)
		{
			return PlayOneShot(parent, clip, 1.0f);
		}

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="clip">Clip.</param>
		/// <param name="volumeScale">Volume scale.</param>
		public static AudioSource PlayOneShot(GameObject parent, AudioClip clip, float volumeScale)
		{
			return PlayOneShot(parent, clip, volumeScale, 1.0f);
		}

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="clip">Clip.</param>
		/// <param name="volumeScale">Volume scale.</param>
		/// <param name="pitch">Pitch.</param>
		public static AudioSource PlayOneShot(GameObject parent, AudioClip clip, float volumeScale, float pitch)
		{
			AudioSource output = PlayOneShot(parent.transform.position, clip, volumeScale, pitch);

			output.transform.parent = parent.transform;

			return output;
		}

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="clip">Clip.</param>
		public static AudioSource PlayOneShot(Vector3 position, AudioClip clip)
		{
			return PlayOneShot(position, clip, 1.0f);
		}

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="clip">Clip.</param>
		/// <param name="volumeScale">Volume scale.</param>
		public static AudioSource PlayOneShot(Vector3 position, AudioClip clip, float volumeScale)
		{
			return PlayOneShot(position, clip, volumeScale, 1.0f);
		}

		/// <summary>
		/// 	Plays the clip.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="clip">Clip.</param>
		/// <param name="volumeScale">Volume scale.</param>
		/// <param name="pitch">Pitch.</param>
		public static AudioSource PlayOneShot(Vector3 position, AudioClip clip, float volumeScale, float pitch)
		{
			AudioSourceWrapper wrapper = New();

			wrapper.transform.parent = null;
			wrapper.transform.position = position;
			wrapper.audioSource.pitch = pitch;

			wrapper.onClipsFinishedCallback += WrapperFinished;
			wrapper.PlayOneShot(clip, volumeScale);

			return wrapper.audioSource;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Called when a wrapper is finished.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private static void WrapperFinished(object sender, EventArgs eventArgs)
		{
			AudioSourceWrapper wrapper = sender as AudioSourceWrapper;
			wrapper.onClipsFinishedCallback -= WrapperFinished;
			Store(wrapper);
		}

		/// <summary>
		/// 	Retrieves a stored wrapper, or makes a new one. Enables the GameObject.
		/// </summary>
		private static AudioSourceWrapper New()
		{
			AudioSourceWrapper output = m_Pool.New();

			try
			{
				output.gameObject.SetActive(true);
			}
			catch (MissingReferenceException)
			{
				// The GameObject has been destroyed so return a different one.
				return New();
			}

			return output;
		}

		/// <summary>
		/// 	Stores and disables the wrapper.
		/// </summary>
		/// <param name="wrapper">Wrapper.</param>
		private static void Store(AudioSourceWrapper wrapper)
		{
			wrapper.gameObject.SetActive(false);
			m_Pool.Store(wrapper);
		}

		#endregion
	}
}
