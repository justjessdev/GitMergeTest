// <copyright file=AudioSourceUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Audio
{
	/// <summary>
	/// 	AudioSourceUtils provides util methods for working with audio.
	/// </summary>
	public static class AudioSourceUtils
	{
		public const float SEMITONE = 1.05946f;
		public const int SEMITONES_PER_OCTAVE = 12;

		/// <summary>
		/// 	Gets the length of the clip.
		/// </summary>
		/// <returns>The clip length.</returns>
		/// <param name="clip">Clip.</param>
		/// <param name="pitch">Pitch.</param>
		public static float GetClipLength(AudioClip clip, float pitch)
		{
			return clip.length / pitch;
		}

		/// <summary>
		/// 	Increases the pitch by the given number of semitones.
		/// </summary>
		/// <returns>The pitch.</returns>
		/// <param name="pitch">Pitch.</param>
		/// <param name="semitones">Semitones.</param>
		public static float IncreaseSemitones(float pitch, int semitones)
		{
			return pitch * Mathf.Pow(SEMITONE, semitones);
		}

		/// <summary>
		/// 	Increases the pitch by the given number of octaves.
		/// </summary>
		/// <returns>The pitch.</returns>
		/// <param name="pitch">Pitch.</param>
		/// <param name="octaves">Octaves.</param>
		public static float IncreaseOctaves(float pitch, int octaves)
		{
			return IncreaseSemitones(pitch, octaves * SEMITONES_PER_OCTAVE);
		}
	}
}
