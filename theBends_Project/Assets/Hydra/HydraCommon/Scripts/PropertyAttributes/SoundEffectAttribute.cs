// <copyright file=SoundEffectAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	Stores sound effect information such as the audio clip and volume.
	/// </summary>
	[Serializable]
	public class SoundEffectAttribute : HydraPropertyAttribute
	{
		[SerializeField] private AudioClip m_AudioClip;
		[SerializeField] private float m_VolumeScale = 1.0f;

		#region Properties

		/// <summary>
		/// 	Gets the audio clip.
		/// </summary>
		/// <value>The audio clip.</value>
		public AudioClip audioClip { get { return m_AudioClip; } }

		/// <summary>
		/// 	Gets the volume scale.
		/// </summary>
		/// <value>The volume scale.</value>
		public float volumeScale { get { return m_VolumeScale; } }

		#endregion
	}
}
