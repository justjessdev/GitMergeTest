// <copyright file=AudioSourceFactory company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.API;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.Audio
{
	public class AudioSourceFactory : IFactory<AudioSourceWrapper>
	{
		/// <summary>
		/// 	Creates a new AudioSourceWrapper instance.
		/// </summary>
		public AudioSourceWrapper Create()
		{
			GameObject gameObject = new GameObject(typeof(AudioSourceWrapper).Name, typeof(AudioSourceWrapper));
			gameObject.hideFlags = HideFlags.HideAndDontSave;

			return gameObject.GetComponent<AudioSourceWrapper>();
		}
	}
}
