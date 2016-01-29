// <copyright file=Vector3RangeAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Utils.RNG;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.RangeAttributes
{
	[Serializable]
	public class Vector3RangeAttribute : HydraPropertyAttribute
	{
		public enum RangeMode
		{
			Constant,
			CurveSet,
			RandomBetweenTwoConstants,
			RandomBetweenTwoCurveSets
		}

		public const WrapMode DEFAULT_WRAP_MODE = WrapMode.Loop;

		[SerializeField] private HydraRNG m_RandomVector3Generator;

		[SerializeField] private RangeMode m_RangeMode = RangeMode.Constant;

		[SerializeField] private bool m_Linear = true;

		[SerializeField] private Vector3Attribute m_ConstValueA;
		[SerializeField] private Vector3Attribute m_ConstValueB;

		[SerializeField] private Vector3CurvesAttribute m_CurvesA;
		[SerializeField] private Vector3CurvesAttribute m_CurvesB;

		#region Properties

		/// <summary>
		/// 	Gets or sets the range mode.
		/// </summary>
		/// <value>The range mode.</value>
		public RangeMode rangeMode { get { return m_RangeMode; } set { m_RangeMode = value; } }

		/// <summary>
		/// 	Linearity is only used when picking a Vector3 between two values (constants or curves).
		/// 	When linearity is enabled we pick a Vector3 at a linear point between the two bounds,
		/// 	Otherwise we consider each axis separately.
		/// </summary>
		/// <value><c>true</c> if linear; otherwise, <c>false</c>.</value>
		public bool linear { get { return m_Linear; } set { m_Linear = value; } }

		/// <summary>
		/// 	Gets or sets the const value A.
		/// </summary>
		/// <value>The const value A.</value>
		public Vector3Attribute constValueA { get { return m_ConstValueA; } }

		/// <summary>
		/// 	Gets or sets the const value B.
		/// </summary>
		/// <value>The const value B.</value>
		public Vector3Attribute constValueB { get { return m_ConstValueB; } }

		/// <summary>
		/// 	Gets the curves A.
		/// </summary>
		/// <value>The curves A.</value>
		public Vector3CurvesAttribute curvesA { get { return m_CurvesA; } }

		/// <summary>
		/// 	Gets the curves B.
		/// </summary>
		/// <value>The curves B.</value>
		public Vector3CurvesAttribute curvesB { get { return m_CurvesB; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public override void Enable()
		{
			InitializeAttributes();

			if (m_RandomVector3Generator == null)
				m_RandomVector3Generator = new HydraRNG();

			base.Enable();
		}

		/// <summary>
		/// 	Initializes the attributes.
		/// </summary>
		private void InitializeAttributes()
		{
			CreateInstance(ref m_ConstValueA);
			CreateInstance(ref m_ConstValueB);

			if (CreateInstance(ref m_CurvesA))
				m_CurvesA.SetWrapMode(DEFAULT_WRAP_MODE, DEFAULT_WRAP_MODE);

			if (CreateInstance(ref m_CurvesB))
				m_CurvesB.SetWrapMode(DEFAULT_WRAP_MODE, DEFAULT_WRAP_MODE);
		}

		/// <summary>
		/// 	Call this method before the object goes out of scope to ensure
		/// 	any Object resources are destroyed.
		/// </summary>
		public override object Destroy()
		{
			m_ConstValueA = Destroy(m_ConstValueA);
			m_ConstValueB = Destroy(m_ConstValueB);

			m_CurvesA = Destroy(m_CurvesA);
			m_CurvesB = Destroy(m_CurvesB);

			return base.Destroy();
		}

		/// <summary>
		/// 	Sets the value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetValue(Vector3 value)
		{
			m_ConstValueA.vector = value;
			m_ConstValueB.vector = value;

			m_CurvesA.SetValue(value);
			m_CurvesB.SetValue(value);
		}

		/// <summary>
		/// 	Gets a value based on RangeMode.
		/// </summary>
		/// <returns>A value based on RangeMode.</returns>
		public Vector3 GetValue(float time)
		{
			switch (rangeMode)
			{
				case RangeMode.Constant:
					return m_ConstValueA.vector;

				case RangeMode.CurveSet:
					return m_CurvesA.GetValue(time);

				case RangeMode.RandomBetweenTwoConstants:
					return m_RandomVector3Generator.Range(m_ConstValueA.vector, m_ConstValueB.vector, m_Linear);

				case RangeMode.RandomBetweenTwoCurveSets:
					return GetRandomBetweenCurveSets(time);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Evaluates the curve sets and returns a random Vector3 between them.
		/// </summary>
		/// <returns>The random Vector3 between curve sets.</returns>
		/// <param name="time">Time.</param>
		private Vector3 GetRandomBetweenCurveSets(float time)
		{
			return m_RandomVector3Generator.Range(m_CurvesA.GetValue(time), m_CurvesB.GetValue(time), m_Linear);
		}

		/// <summary>
		/// 	Seeds the random number generator and gets the value at the given time.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="time">Time.</param>
		/// <param name="seed">Seed.</param>
		public Vector3 GetValue(float time, uint seed)
		{
			m_RandomVector3Generator.SetSeed(seed);

			return GetValue(time);
		}

		/// <summary>
		/// 	Copies the instance values and assigns them to the target.
		/// </summary>
		/// <param name="target">Target.</param>
		public void DeepCopyInto(Vector3RangeAttribute target)
		{
			target.rangeMode = m_RangeMode;

			target.linear = m_Linear;

			target.constValueA.vector = m_ConstValueA.vector;
			target.constValueB.vector = m_ConstValueB.vector;

			m_CurvesA.DeepCopyInto(target.curvesA);
			m_CurvesB.DeepCopyInto(target.curvesB);
		}

		#endregion
	}
}
