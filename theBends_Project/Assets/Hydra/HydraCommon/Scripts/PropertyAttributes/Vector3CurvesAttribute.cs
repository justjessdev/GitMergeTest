// <copyright file=Vector3CurvesAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Extensions;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	[Serializable]
	public class Vector3CurvesAttribute : AbstractVector3Attribute
	{
		[SerializeField] private AnimationCurve m_CurveX;
		[SerializeField] private AnimationCurve m_CurveY;
		[SerializeField] private AnimationCurve m_CurveZ;

		#region Properties

		/// <summary>
		/// 	Gets or sets the curve x.
		/// </summary>
		/// <value>The curve x.</value>
		public AnimationCurve curveX { get { return m_CurveX; } }

		/// <summary>
		/// 	Gets or sets the curve y.
		/// </summary>
		/// <value>The curve y.</value>
		public AnimationCurve curveY { get { return m_CurveY; } }

		/// <summary>
		/// 	Gets or sets the curve z.
		/// </summary>
		/// <value>The curve z.</value>
		public AnimationCurve curveZ { get { return m_CurveZ; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public override void Enable()
		{
			if (m_CurveX == null)
				m_CurveX = new AnimationCurve();

			if (m_CurveY == null)
				m_CurveY = new AnimationCurve();

			if (m_CurveZ == null)
				m_CurveZ = new AnimationCurve();

			base.Enable();
		}

		/// <summary>
		/// 	Sets the value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetValue(Vector3 value)
		{
			if (locked)
				value = Vector3.one * value.x;

			m_CurveX.SetValue(value.x);
			m_CurveY.SetValue(value.y);
			m_CurveZ.SetValue(value.z);
		}

		/// <summary>
		/// 	Gets a value based on RangeMode.
		/// </summary>
		/// <returns>A value based on RangeMode.</returns>
		public Vector3 GetValue(float time)
		{
			if (locked)
				return Vector3.one * m_CurveX.Evaluate(time);

			return new Vector3(m_CurveX.Evaluate(time), m_CurveY.Evaluate(time), m_CurveZ.Evaluate(time));
		}

		/// <summary>
		/// 	Sets the wrap mode.
		/// </summary>
		/// <param name="inMode">In mode.</param>
		/// <param name="outMode">Out mode.</param>
		public void SetWrapMode(WrapMode inMode, WrapMode outMode)
		{
			m_CurveX.SetWrapMode(inMode, outMode);
			m_CurveY.SetWrapMode(inMode, outMode);
			m_CurveZ.SetWrapMode(inMode, outMode);
		}

		/// <summary>
		/// 	Copies the instance values and assigns them to the target.
		/// </summary>
		/// <param name="target">Target.</param>
		public void DeepCopyInto(Vector3CurvesAttribute target)
		{
			target.locked = locked;

			m_CurveX.DeepCopyInto(target.curveX);
			m_CurveY.DeepCopyInto(target.curveY);
			m_CurveZ.DeepCopyInto(target.curveZ);
		}

		#endregion

		/// <summary>
		/// 	Called when the locked state is changed.
		/// </summary>
		protected override void OnLockedChanged()
		{
			if (!locked)
				return;

			m_CurveX.DeepCopyInto(m_CurveY);
			m_CurveX.DeepCopyInto(m_CurveZ);
		}
	}
}
