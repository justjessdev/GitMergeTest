// <copyright file=SimpleRNG company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.RNG
{
	/// <summary>
	/// 	SimpleRNG is a simple random number generator based on 
	/// 	George Marsaglia's MWC (multiply with carry) generator.
	/// 	Although it is very simple, it passes Marsaglia's DIEHARD
	/// 	series of random number generator tests.
	/// 
	/// 	Written by John D. Cook 
	/// 	http://www.johndcook.com
	/// </summary>
	[Serializable]
	public class SimpleRNG
	{
		// These values are not magical, just the default values Marsaglia used.
		// Any pair of unsigned integers should be fine.
		public const uint DEFAULT_W = 521288629;
		public const uint DEFAULT_Z = 362436069;

		[SerializeField] private uint m_W;
		[SerializeField] private uint m_Z;

		/// <summary>
		/// 	Initializes a new instance of the SimpleRNG class.
		/// </summary>
		public SimpleRNG()
		{
			SetSeedFromSystemTime();
		}

		#region Methods

		/// <summary>
		/// 	Sets the seed.
		/// </summary>
		/// <param name="u">U.</param>
		/// <param name="v">V.</param>
		public void SetSeed(uint u, uint v)
		{
			m_W = (u == 0) ? DEFAULT_W : u;
			m_Z = (v == 0) ? DEFAULT_Z : v;
		}

		/// <summary>
		/// 	Sets the seed.
		/// </summary>
		/// <param name="u">U.</param>
		public void SetSeedA(uint u)
		{
			SetSeed(u, m_Z);
		}

		/// <summary>
		/// 	Sets the seed.
		/// </summary>
		/// <param name="v">V.</param>
		public void SetSeedB(uint v)
		{
			SetSeed(m_W, v);
		}

		/// <summary>
		/// 	Sets the seed from system time.
		/// </summary>
		public void SetSeedFromSystemTime()
		{
			DateTime dt = DateTime.Now;
			long x = dt.ToFileTime();
			SetSeed((uint)(x >> 16), (uint)(x % 4294967296));
		}

		/// <summary>
		/// 	Produce a uniform random sample from the open interval (0, 1).
		/// 	The method will not return either end point.
		/// </summary>
		/// <returns>The uniform.</returns>
		public double GetUniform()
		{
			// 0 <= u < 2^32
			uint u = GetUint();
			// The magic number below is 1/(2^32 + 2).
			// The result is strictly between 0 and 1.
			return (u + 1.0) * 2.328306435454494e-10;
		}

		/// <summary>
		/// 	This is the heart of the generator.
		/// 	It uses George Marsaglia's MWC algorithm to produce an unsigned integer.
		/// 	See http://www.bobwheeler.com/statistics/Password/MarsagliaPost.txt.
		/// </summary>
		/// <returns>The uint.</returns>
		private uint GetUint()
		{
			m_Z = 36969 * (m_Z & 65535) + (m_Z >> 16);
			m_W = 18000 * (m_W & 65535) + (m_W >> 16);
			return (m_Z << 16) + m_W;
		}

		#endregion
	}
}
