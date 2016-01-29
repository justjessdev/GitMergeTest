// <copyright file=ColorRangeAttribute company=Hydra>
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
	/// 	ColorRangeAttribute provides different ways of yielding a color,
	/// 	for example a random color from a range.
	/// </summary>
	[Serializable]
	public class ColorRangeAttribute : HydraPropertyAttribute
	{
		public static readonly Color defaultColor = Color.white;

		public enum RangeMode
		{
			Constant,
			Gradient,
			RandomInGradient,
			RandomBetweenTwoConstants,
			RandomBetweenTwoGradients
		}

		[SerializeField] private HydraRNG m_RandomColorGenerator;

		[SerializeField] private RangeMode m_RangeMode = RangeMode.Constant;

		[SerializeField] private bool m_Linear = true;

		[SerializeField] private GradientExtensions.GradientWrapMode m_GradientWrapMode =
			GradientExtensions.GradientWrapMode.Loop;

		[SerializeField] private float m_GradientLength = 1.0f;

		[SerializeField] private ColorUtils.Gamut m_RandomBlend = ColorUtils.Gamut.HSL;

		[SerializeField] private Color m_ConstValueA;
		[SerializeField] private Color m_ConstValueB;

		[SerializeField] private Gradient m_GradientA;
		[SerializeField] private Gradient m_GradientB;

		#region Properties

		/// <summary>
		/// 	Gets or sets the range mode.
		/// </summary>
		/// <value>The range mode.</value>
		public RangeMode rangeMode { get { return m_RangeMode; } set { m_RangeMode = value; } }

		/// <summary>
		/// 	Gets or sets a value indicating whether this HydraColorRangeAttribute is linear.
		/// </summary>
		/// <value><c>true</c> if linear; otherwise, <c>false</c>.</value>
		public bool linear { get { return m_Linear; } set { m_Linear = value; } }

		/// <summary>
		/// 	Gets or sets the gradient wrap mode.
		/// </summary>
		/// <value>The gradient wrap mode.</value>
		public GradientExtensions.GradientWrapMode gradientWrapMode
		{
			get { return m_GradientWrapMode; }
			set { m_GradientWrapMode = value; }
		}

		/// <summary>
		/// 	When getting a gradient value we scale by this number along the time axis.
		/// 	(e.g. a gradientLength of 2 means the last color in the gradient (100%) is at the 2 seconds mark)
		/// </summary>
		/// <value>The length of the gradient.</value>
		public float gradientLength { get { return m_GradientLength; } set { m_GradientLength = value; } }

		/// <summary>
		/// 	Gets or sets the random blend.
		/// </summary>
		/// <value>The random blend.</value>
		public ColorUtils.Gamut randomBlend { get { return m_RandomBlend; } set { m_RandomBlend = value; } }

		/// <summary>
		/// 	Gets or sets the const value A.
		/// </summary>
		/// <value>The const value A.</value>
		public Color constValueA { get { return m_ConstValueA; } set { m_ConstValueA = value; } }

		/// <summary>
		/// 	Gets or sets the const value B.
		/// </summary>
		/// <value>The const value B.</value>
		public Color constValueB { get { return m_ConstValueB; } set { m_ConstValueB = value; } }

		/// <summary>
		/// 	Gets or sets the gradient A.
		/// </summary>
		/// <value>The gradient A.</value>
		public Gradient gradientA { get { return m_GradientA; } set { m_GradientA = value; } }

		/// <summary>
		/// 	Gets or sets the gradient B.
		/// </summary>
		/// <value>The gradient A.</value>
		public Gradient gradientB { get { return m_GradientB; } set { m_GradientB = value; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public override void Enable()
		{
			if (m_RandomColorGenerator == null)
				m_RandomColorGenerator = new HydraRNG();

			if (m_GradientA == null)
				m_GradientA = new Gradient();

			if (m_GradientB == null)
				m_GradientB = new Gradient();

			base.Enable();
		}

		/// <summary>
		/// 	This method sets the given float value for each bound (A and B) for all range modes.
		/// 	This method is useful when wanting to ensure GetValue will return something specific.
		/// 
		/// 	For example, calling SetValue(1.0f) would mean that GetValue(Time.time) will always return 1.0f.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetValue(Color value)
		{
			m_ConstValueA = value;
			m_ConstValueB = value;

			m_GradientA.SetValue(value);
			m_GradientB.SetValue(value);
		}

		/// <summary>
		/// 	Gets a value based on RangeMode.
		/// </summary>
		/// <returns>A value based on RangeMode.</returns>
		public Color GetValue(float time)
		{
			switch (rangeMode)
			{
				case RangeMode.Constant:
					return constValueA;

				case RangeMode.Gradient:
					return GetColorFromGradientAtTime(m_GradientA, time);

				case RangeMode.RandomInGradient:
					return GetRandomColorFromGradient(m_GradientA);

				case RangeMode.RandomBetweenTwoConstants:
					return GetColorBetweenValues(m_ConstValueA, m_ConstValueB);

				case RangeMode.RandomBetweenTwoGradients:
					return GetRandomColorBetweenGradientsAtTime(time);

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
		public Color GetValue(float time, uint seed)
		{
			m_RandomColorGenerator.SetSeed(seed);

			return GetValue(time);
		}

		/// <summary>
		/// 	Gets a value based on RangeMode using a normalized value for curves.
		/// </summary>
		/// <returns>A value based on RangeMode using a normalized value for curves.</returns>
		/// <param name="normalized">Normalized.</param>
		public Color GetValueNormalized(float normalized)
		{
			return GetValue(normalized * m_GradientLength);
		}

		/// <summary>
		/// 	Seeds the random number generator and gets a value based on RangeMode using
		/// 	a normalized value for curves.
		/// </summary>
		/// <returns>A value based on RangeMode using a normalized value for curves.</returns>
		/// <param name="normalized">Normalized.</param>
		/// <param name="seed">Seed.</param>
		public Color GetValueNormalized(float normalized, uint seed)
		{
			return GetValue(normalized * m_GradientLength, seed);
		}

		#endregion

		/// <summary>
		/// 	Gets a random color from the gradient.
		/// </summary>
		/// <returns>The random color from gradient.</returns>
		/// <param name="gradient">Gradient.</param>
		private Color GetRandomColorFromGradient(Gradient gradient)
		{
			return gradient.Evaluate(m_RandomColorGenerator.value);
		}

		/// <summary>
		/// 	Gets the color between specified values.
		/// </summary>
		/// <returns>The color between values.</returns>
		/// <param name="colorA">Color a.</param>
		/// <param name="colorB">Color b.</param>
		private Color GetColorBetweenValues(Color colorA, Color colorB)
		{
			switch (m_RandomBlend)
			{
				case ColorUtils.Gamut.RGB:
					return m_RandomColorGenerator.Range(colorA, colorB, m_Linear);
				case ColorUtils.Gamut.HSL:
					return m_RandomColorGenerator.RangeHsl(colorA, colorB, m_Linear);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Gets the color from gradient at time.
		/// </summary>
		/// <returns>The color from gradient at time.</returns>
		/// <param name="gradient">Gradient.</param>
		/// <param name="time">Time.</param>
		private Color GetColorFromGradientAtTime(Gradient gradient, float time)
		{
			if (!HydraMathUtils.Approximately(m_GradientLength, 0.0f))
				time /= m_GradientLength;

			return gradient.Evaluate(time, m_GradientWrapMode);
		}

		/// <summary>
		/// 	Gets the random color between curves at time.
		/// </summary>
		/// <returns>The random color between curves at time.</returns>
		/// <param name="time">Time.</param>
		private Color GetRandomColorBetweenGradientsAtTime(float time)
		{
			Color a = GetColorFromGradientAtTime(m_GradientA, time);
			Color b = GetColorFromGradientAtTime(m_GradientB, time);

			return GetColorBetweenValues(a, b);
		}
	}
}
