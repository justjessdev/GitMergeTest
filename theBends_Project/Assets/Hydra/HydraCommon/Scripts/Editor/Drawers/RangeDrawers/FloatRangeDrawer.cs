// <copyright file=FloatRangeDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes.RangeAttributes;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers.RangeDrawers
{
	/// <summary>
	/// 	FloatRangeDrawer presents the FloatRangeAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(FloatRangeAttribute))]
	public class FloatRangeDrawer : PropertyDrawer
	{
		private SerializedProperty m_RangeModeProperty;
		private SerializedProperty m_ConstValueAProperty;
		private SerializedProperty m_ConstValueBProperty;
		private SerializedProperty m_CurveAProperty;
		private SerializedProperty m_CurveBProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_RangeModeProperty = prop.FindPropertyRelative("m_RangeMode");
			m_ConstValueAProperty = prop.FindPropertyRelative("m_ConstValueA");
			m_ConstValueBProperty = prop.FindPropertyRelative("m_ConstValueB");
			m_CurveAProperty = prop.FindPropertyRelative("m_CurveA");
			m_CurveBProperty = prop.FindPropertyRelative("m_CurveB");
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

			// Range Mode
			Rect rangeModePosition = new Rect(position);
			rangeModePosition.x += (rangeModePosition.width - 12);
			rangeModePosition.width = 12;

			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<FloatRangeAttribute.RangeMode>(rangeModePosition, GUIContent.none,
																										   m_RangeModeProperty, HydraEditorGUIStyles.enumStyle));

			// Range Mode Fields
			Rect rangeModeFieldsPosition = new Rect(position);
			rangeModeFieldsPosition.width -= (rangeModePosition.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING);

			switch (m_RangeModeProperty.GetEnumValue<FloatRangeAttribute.RangeMode>())
			{
				case FloatRangeAttribute.RangeMode.Constant:
					OnConstant(rangeModeFieldsPosition);
					break;

				case FloatRangeAttribute.RangeMode.Curve:
					OnCurve(rangeModeFieldsPosition);
					break;

				case FloatRangeAttribute.RangeMode.RandomBetweenTwoConstants:
					OnRandomBetweenTwoConstants(rangeModeFieldsPosition);
					break;

				case FloatRangeAttribute.RangeMode.RandomBetweenTwoCurves:
					OnRandomBetweenTwoCurves(rangeModeFieldsPosition);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			EditorGUI.EndProperty();
		}

		#endregion

		/// <summary>
		/// 	Raises when constant has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnConstant(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_ConstValueAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when curve has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnCurve(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_CurveAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random Between Two Constants has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoConstants(Rect position)
		{
			Rect gradientRect = new Rect(position);
			gradientRect.width = (position.width - HydraEditorUtils.STANDARD_HORIZONTAL_SPACING) / 2.0f;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_ConstValueAProperty, GUIContent.none));

			gradientRect.x += gradientRect.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_ConstValueBProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random Between Two Curves has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoCurves(Rect position)
		{
			Rect gradientRect = new Rect(position);
			gradientRect.width = (position.width - HydraEditorUtils.STANDARD_HORIZONTAL_SPACING) / 2.0f;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_CurveAProperty, GUIContent.none));

			gradientRect.x += gradientRect.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_CurveBProperty, GUIContent.none));
		}
	}
}
