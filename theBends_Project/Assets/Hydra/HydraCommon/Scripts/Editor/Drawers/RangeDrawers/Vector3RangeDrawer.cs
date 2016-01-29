// <copyright file=Vector3RangeDrawer company=Hydra>
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
	/// 	Vector3RangeDrawer presents the Vector3RangeAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(Vector3RangeAttribute))]
	public class Vector3RangeDrawer : PropertyDrawer
	{
		private static readonly GUIContent s_LinearLabel = new GUIContent("Linear",
																		  "When enabled a random point on the line between point " + "A and B will be chosen.");

		private SerializedProperty m_RangeModeProperty;
		private SerializedProperty m_LinearProperty;
		private SerializedProperty m_ConstValueAProperty;
		private SerializedProperty m_ConstValueBProperty;
		private SerializedProperty m_CurvesAProperty;
		private SerializedProperty m_CurvesBProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_RangeModeProperty = prop.FindPropertyRelative("m_RangeMode");
			m_LinearProperty = prop.FindPropertyRelative("m_Linear");
			m_ConstValueAProperty = prop.FindPropertyRelative("m_ConstValueA");
			m_ConstValueBProperty = prop.FindPropertyRelative("m_ConstValueB");
			m_CurvesAProperty = prop.FindPropertyRelative("m_CurvesA");
			m_CurvesBProperty = prop.FindPropertyRelative("m_CurvesB");
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
											HydraEditorUtils.EnumPopupField<Vector3RangeAttribute.RangeMode>(rangeModePosition, GUIContent.none,
																											 m_RangeModeProperty, HydraEditorGUIStyles.enumStyle));

			// Range Mode Fields
			Rect rangeModeFieldsPosition = new Rect(position);
			rangeModeFieldsPosition.width -= (rangeModePosition.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING);

			switch (m_RangeModeProperty.GetEnumValue<Vector3RangeAttribute.RangeMode>())
			{
				case Vector3RangeAttribute.RangeMode.Constant:
					OnConstant(rangeModeFieldsPosition);
					break;

				case Vector3RangeAttribute.RangeMode.CurveSet:
					OnCurve(rangeModeFieldsPosition);
					break;

				case Vector3RangeAttribute.RangeMode.RandomBetweenTwoConstants:
					OnRandomBetweenTwoConstants(rangeModeFieldsPosition);
					break;

				case Vector3RangeAttribute.RangeMode.RandomBetweenTwoCurveSets:
					OnRandomBetweenTwoCurves(rangeModeFieldsPosition);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

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

			switch (m_RangeModeProperty.GetEnumValue<Vector3RangeAttribute.RangeMode>())
			{
				case Vector3RangeAttribute.RangeMode.Constant:
					lines = 1;
					break;
				case Vector3RangeAttribute.RangeMode.CurveSet:
					lines = 1;
					break;
				case Vector3RangeAttribute.RangeMode.RandomBetweenTwoConstants:
					lines = 3;
					break;
				case Vector3RangeAttribute.RangeMode.RandomBetweenTwoCurveSets:
					lines = 3;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
		}

		#endregion

		/// <summary>
		/// 	Raises when constant has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnConstant(Rect position)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_ConstValueAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when curve has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnCurve(Rect position)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_CurvesAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random Between Two Constants has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoConstants(Rect position)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_LinearLabel, m_LinearProperty));

			Rect vector3Rect = new Rect(position);
			vector3Rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(vector3Rect, m_ConstValueAProperty, GUIContent.none));

			vector3Rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(vector3Rect, m_ConstValueBProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random Between Two Curves has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoCurves(Rect position)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_LinearLabel, m_LinearProperty));

			Rect vector3Rect = new Rect(position);

			vector3Rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(vector3Rect, m_CurvesAProperty, GUIContent.none));

			vector3Rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(vector3Rect, m_CurvesBProperty, GUIContent.none));
		}
	}
}
