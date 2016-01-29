// <copyright file=BezierPointSerializedProperty company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.API;
using Hydra.HydraCommon.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Inspector.Concrete
{
	/// <summary>
	/// 	BezierPointSerializedProperty is a transient class that lets us feed a serialized BezierPointAttribute
	/// 	property to IBezierPoint methods.
	/// </summary>
	public class BezierPointSerializedProperty : IBezierPoint
	{
		private SerializedProperty m_SerializedProperty;

		private SerializedProperty m_PositionProp;
		private SerializedProperty m_TangentModeProp;
		private SerializedProperty m_InTangentProp;
		private SerializedProperty m_OutTangentProp;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		private void FindProperties()
		{
			m_PositionProp = m_SerializedProperty.FindPropertyRelative("m_Position");
			m_TangentModeProp = m_SerializedProperty.FindPropertyRelative("m_TangentMode");
			m_InTangentProp = m_SerializedProperty.FindPropertyRelative("m_InTangent");
			m_OutTangentProp = m_SerializedProperty.FindPropertyRelative("m_OutTangent");
		}

		#region Properties

		/// <summary>
		/// 	Gets or sets the serialized property.
		/// </summary>
		/// <value>The serialized property.</value>
		public SerializedProperty serializedProperty
		{
			get { return m_SerializedProperty; }
			set
			{
				if (value == m_SerializedProperty)
					return;

				m_SerializedProperty = value;

				FindProperties();
			}
		}

		/// <summary>
		/// 	Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Vector3 position
		{
			get { return m_PositionProp.vector3Value; }
			set
			{
				if (value != m_PositionProp.vector3Value)
					m_PositionProp.vector3Value = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the tangent mode.
		/// </summary>
		/// <value>The tangent mode.</value>
		public TangentMode tangentMode
		{
			get { return m_TangentModeProp.GetEnumValue<TangentMode>(); }
			set
			{
				if (value != m_TangentModeProp.GetEnumValue<TangentMode>())
					m_TangentModeProp.SetEnumValue(value);
			}
		}

		/// <summary>
		/// 	Gets or sets the in tangent.
		/// </summary>
		/// <value>The in tangent.</value>
		public Vector3 inTangent
		{
			get { return m_InTangentProp.vector3Value; }
			set
			{
				if (value != m_InTangentProp.vector3Value)
				{
					if (tangentMode == TangentMode.Smooth)
						m_OutTangentProp.vector3Value = value * -1.0f;
					m_InTangentProp.vector3Value = value;
				}
			}
		}

		/// <summary>
		/// 	Gets or sets the out tangent.
		/// </summary>
		/// <value>The out tangent.</value>
		public Vector3 outTangent
		{
			get { return m_OutTangentProp.vector3Value; }
			set
			{
				if (value != m_OutTangentProp.vector3Value)
				{
					if (tangentMode == TangentMode.Smooth)
						m_InTangentProp.vector3Value = value * -1.0f;
					m_OutTangentProp.vector3Value = value;
				}
			}
		}

		#endregion
	}
}
