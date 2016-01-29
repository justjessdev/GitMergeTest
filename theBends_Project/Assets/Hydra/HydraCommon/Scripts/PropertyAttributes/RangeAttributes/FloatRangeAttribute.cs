// <copyright file=FloatRangeAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.Utils;
using Hydra.HydraCommon.Utils.RNG;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.RangeAttributes
{
	/// <summary>
	/// 	FloatRangeAttribute provides different ways of yielding a float,
	/// 	for example a random float from a range.
	/// </summary>
	[Serializable]
	public class FloatRangeAttribute : HydraPropertyAttribute
	{
		public enum RangeMode
		{
			Constant,
			Curve,
			RandomBetweenTwoConstants,
			RandomBetweenTwoCurves
		}

		public const WrapMode DEFAULT_WRAP_MODE = WrapMode.Loop;

		[SerializeField] private HydraRNG m_RandomNumberGenerator;

		[SerializeField] private RangeMode m_RangeMode = RangeMode.Constant;

		[SerializeField] private float m_ConstValueA;
		[SerializeField] private float m_ConstValueB;

		[SerializeField] private AnimationCurve m_CurveA;
		[SerializeField] private AnimationCurve m_CurveB;

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
		public float constValueA { get { return m_ConstValueA; } set { m_ConstValueA = value; } }

		/// <summary>
		/// 	Gets or sets the const value B.
		/// </summary>
		/// <value>The const value B.</value>
		public float constValueB { get { return m_ConstValueB; } set { m_ConstValueB = value; } }

		/// <summary>
		/// 	Gets or sets the curve A.
		/// </summary>
		/// <value>The curve A.</value>
		public AnimationCurve curveA { get { return m_CurveA; } set { m_CurveA = value; } }

		/// <summary>
		/// 	Gets or sets the curve B.
		/// </summary>
		/// <value>The curve B.</value>
		public AnimationCurve curveB { get { return m_CurveB; } set { m_CurveB = value; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public override void Enable()
		{
			if (m_RandomNumberGenerator == null)
				m_RandomNumberGenerator = new HydraRNG();

			if (m_CurveA == null)
			{
				m_CurveA = new AnimationCurve();
				m_CurveA.SetWrapMode(DEFAULT_WRAP_MODE, DEFAULT_WRAP_MODE);
			}

			if (m_CurveB == null)
			{
				m_CurveB = new AnimationCurve();
				m_CurveB.SetWrapMode(DEFAULT_WRAP_MODE, DEFAULT_WRAP_MODE);
			}

			base.Enable();
		}

		/// <summary>
		/// 	This method sets the given float value for each bound (A and B) for all range modes.
		/// 	This method is useful when wanting to ensure GetValue will return something specific.
		/// 
		/// 	For example, calling SetValue(1.0f) would mean that GetValue(Time.time) will always return 1.0f.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetValue(float value)
		{
			m_ConstValueA = value;
			m_ConstValueB = value;

			m_CurveA.SetValue(value);
			m_CurveB.SetValue(value);
		}

		/// <summary>
		/// 	Gets a value based on RangeMode.
		/// </summary>
		/// <returns>A value based on RangeMode.</returns>
		public float GetValue(float time)
		{
			switch (rangeMode)
			{
				case RangeMode.Constant:
					return m_ConstValueA;
				case RangeMode.Curve:
					return m_CurveA.Evaluate(time);
				case RangeMode.RandomBetweenTwoConstants:
					return m_RandomNumberGenerator.Range(m_ConstValueA, m_ConstValueB);
				case RangeMode.RandomBetweenTwoCurves:
					return m_RandomNumberGenerator.Range(m_CurveA.Evaluate(time), m_CurveB.Evaluate(time));
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
		public float GetValue(float time, uint seed)
		{
			m_RandomNumberGenerator.SetSeed(seed);

			return GetValue(time);
		}

		/// <summary>
		/// 	Returns the maximum value that can be returned given the const/curve settings.
		/// 	Note, in the case of curves, this will be inaccurate because there is no
		/// 	nice way of getting the bounding box for an animation curve.
		/// </summary>
		/// <returns>The max value.</returns>
		public float GetMaxValue()
		{
			switch (rangeMode)
			{
				case RangeMode.Constant:
					return m_ConstValueA;
				case RangeMode.Curve:
					return m_CurveA.GetMaxValue();
				case RangeMode.RandomBetweenTwoConstants:
					return HydraMathUtils.Max(m_ConstValueA, m_ConstValueB);
				case RangeMode.RandomBetweenTwoCurves:
					return HydraMathUtils.Max(m_CurveA.GetMaxValue(), m_CurveB.GetMaxValue());
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion
	}
}
