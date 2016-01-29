// <copyright file=IntRangeAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Utils.RNG;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.RangeAttributes
{
	/// <summary>
	/// 	IntRangeAttribute provides different ways of yielding an int,
	/// 	for example a random int from a range.
	/// </summary>
	[Serializable]
	public class IntRangeAttribute : HydraPropertyAttribute
	{
		public enum RangeMode
		{
			Constant,
			RandomBetweenTwoConstants,
		}

		[SerializeField] private HydraRNG m_RandomNumberGenerator;

		[SerializeField] private RangeMode m_RangeMode = RangeMode.Constant;

		[SerializeField] private int m_ConstValueA;
		[SerializeField] private int m_ConstValueB;

		#region Properties

		/// <summary>
		/// 	Gets or sets the range mode.
		/// </summary>
		/// <value>The range mode.</value>
		public RangeMode rangeMode { get { return m_RangeMode; } set { m_RangeMode = value; } }

		/// <summary>
		/// 	Gets or sets the const value A.
		/// </summary>
		/// <value>The const value A.</value>
		public int constValueA { get { return m_ConstValueA; } set { m_ConstValueA = value; } }

		/// <summary>
		/// 	Gets or sets the const value B.
		/// </summary>
		/// <value>The const value B.</value>
		public int constValueB { get { return m_ConstValueB; } set { m_ConstValueB = value; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public override void Enable()
		{
			if (m_RandomNumberGenerator == null)
				m_RandomNumberGenerator = new HydraRNG();

			base.Enable();
		}

		/// <summary>
		/// 	This method sets the given float value for each bound (A and B) for all range modes.
		/// 	This method is useful when wanting to ensure GetValue will return something specific.
		/// 
		/// 	For example, calling SetValue(1) would mean that GetValue(Time.time) will always return 1.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetValue(int value)
		{
			m_ConstValueA = value;
			m_ConstValueB = value;
		}

		/// <summary>
		/// 	Gets a value based on RangeMode.
		/// </summary>
		/// <returns>A value based on RangeMode.</returns>
		public int GetValue(float time)
		{
			switch (rangeMode)
			{
				case RangeMode.Constant:
					return m_ConstValueA;
				case RangeMode.RandomBetweenTwoConstants:
					return m_RandomNumberGenerator.Range(m_ConstValueA, m_ConstValueB);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Seeds the random number generator and gets the value at the given time.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="time">Time.</param>
		/// <param name="seed">Seed.</param>
		public int GetValue(int time, uint seed)
		{
			m_RandomNumberGenerator.SetSeed(seed);

			return GetValue(time);
		}

		#endregion
	}
}
