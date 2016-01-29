// <copyright file=BezierPointDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.API;
using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	[CustomPropertyDrawer(typeof(BezierPointAttribute))]
	public class BezierPointDrawer : PropertyDrawer
	{
		public const float LABEL_WIDTH = 72.0f;

		public static readonly GUIContent s_PositionLabel = new GUIContent("Position", "The position of the point.");
		public static readonly GUIContent s_SmoothingLabel = new GUIContent("Smoothing", "The tangent mode of the point.");
		public static readonly GUIContent s_InLabel = new GUIContent("In", "The position of the In-Tangent handle.");
		public static readonly GUIContent s_OutLabel = new GUIContent("Out", "The position of the Out-Tangent handle.");

		private SerializedProperty m_PositionProp;
		private SerializedProperty m_TangentModeProp;
		private SerializedProperty m_InTangentProp;
		private SerializedProperty m_OutTangentProp;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_PositionProp = prop.FindPropertyRelative("m_Position");
			m_TangentModeProp = prop.FindPropertyRelative("m_TangentMode");
			m_InTangentProp = prop.FindPropertyRelative("m_InTangent");
			m_OutTangentProp = prop.FindPropertyRelative("m_OutTangent");
		}

		#region Methods

		/// <summary>
		/// 	Override this method to make your own GUI for the property
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="prop">Property</param>
		/// <param name="label">Label</param>
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			FindProperties(prop);

			position.height = EditorGUIUtility.singleLineHeight;
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = LABEL_WIDTH;

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.Vector3Field2Dp(position, s_PositionLabel, m_PositionProp));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<TangentMode>(position, s_SmoothingLabel, m_TangentModeProp,
																						 HydraEditorGUIStyles.enumStyle));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			switch (m_TangentModeProp.GetEnumValue<TangentMode>())
			{
				case TangentMode.Auto:
					break;
				case TangentMode.Corner:
					DrawTangents(position);
					break;
				case TangentMode.Smooth:
					DrawTangents(position);
					break;
				case TangentMode.Symmetric:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			EditorGUIUtility.labelWidth = oldLabelWidth;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Gets the height of the property.
		/// </summary>
		/// <returns>The property height.</returns>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			FindProperties(property);

			int lines;

			switch (m_TangentModeProp.GetEnumValue<TangentMode>())
			{
				case TangentMode.Auto:
					lines = 2;
					break;
				case TangentMode.Corner:
					lines = 4;
					break;
				case TangentMode.Smooth:
					lines = 4;
					break;
				case TangentMode.Symmetric:
					lines = 2;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
		}

		#endregion

		/// <summary>
		/// 	Draws the tangents.
		/// </summary>
		private void DrawTangents(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.Vector3Field2Dp(position, s_InLabel, m_InTangentProp));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.Vector3Field2Dp(position, s_OutLabel, m_OutTangentProp));
		}
	}
}
