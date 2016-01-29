// <copyright file=BezierPointAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.API;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	Bezier point attribute.
	/// </summary>
	[Serializable]
	public class BezierPointAttribute : HydraPropertyAttribute, IBezierPoint
	{
		[SerializeField] private Vector3 m_Position;
		[SerializeField] private TangentMode m_TangentMode;
		[SerializeField] private Vector3 m_InTangent;
		[SerializeField] private Vector3 m_OutTangent;

		#region Properties

		/// <summary>
		/// 	Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Vector3 position { get { return m_Position; } set { m_Position = value; } }

		/// <summary>
		/// 	Gets or sets the tangent mode.
		/// </summary>
		/// <value>The tangent mode.</value>
		public TangentMode tangentMode { get { return m_TangentMode; } set { m_TangentMode = value; } }

		/// <summary>
		/// 	Gets or sets the in tangent.
		/// </summary>
		/// <value>The in tangent.</value>
		public Vector3 inTangent
		{
			get { return m_InTangent; }
			set
			{
				if (m_TangentMode == TangentMode.Smooth)
					m_OutTangent = value * -1.0f;

				m_InTangent = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the out tangent.
		/// </summary>
		/// <value>The out tangent.</value>
		public Vector3 outTangent
		{
			get { return m_OutTangent; }
			set
			{
				if (m_TangentMode == TangentMode.Smooth)
					m_InTangent = value * -1.0f;

				m_OutTangent = value;
			}
		}

		#endregion
	}
}
