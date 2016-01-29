// <copyright file=NoiseAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Utils.RNG;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	[Serializable]
	public class NoiseAttribute : HydraPropertyAttribute
	{
		public const float X_OFFSET = 0.0f;
		public const float Y_OFFSET = 1.0f;
		public const float Z_OFFSET = 2.0f;

		[SerializeField] private Vector3 m_Frequency = Vector3.one;
		[SerializeField] private Vector3 m_Phase = Vector3.zero;

		#region Properties

		/// <summary>
		/// 	Gets or sets the frequency.
		/// </summary>
		/// <value>The frequency.</value>
		public Vector3 frequency { get { return m_Frequency; } set { m_Frequency = value; } }

		/// <summary>
		/// 	Gets or sets the phase.
		/// </summary>
		/// <value>The phase.</value>
		public Vector3 phase { get { return m_Phase; } set { m_Phase = value; } }

		#endregion

		/// <summary>
		/// 	Returns the noise at the given position.
		/// </summary>
		/// <returns>The noise at the position.</returns>
		/// <param name="position">Position.</param>
		public Vector3 NoiseForPosition(Vector3 position)
		{
			Vector3 scaled = Vector3.Scale(position, m_Frequency);
			Vector3 offset = scaled + m_Phase;

			// I'm cheating. SimplexNoise returns floats for given positions, so I offset each axis by
			// an arbitrary value to get asynchronous results.
			return new Vector3((float)SimplexNoise.Noise(offset.x, offset.y, offset.z, X_OFFSET),
							   (float)SimplexNoise.Noise(offset.x, offset.y, offset.z, Y_OFFSET),
							   (float)SimplexNoise.Noise(offset.x, offset.y, offset.z, Z_OFFSET));
		}
	}
}
