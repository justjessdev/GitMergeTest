// <copyright file=ColorRangeDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.PropertyAttributes.RangeAttributes;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers.RangeDrawers
{
	/// <summary>
	/// 	ColorRangeDrawer presents the ColorRangeAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(ColorRangeAttribute))]
	public class ColorRangeDrawer : PropertyDrawer
	{
		private static readonly GUIContent s_LinearLabel = new GUIContent("Linear",
																		  "When enabled a random color on the line between color " +
																		  "A and B will be chosen. When disabled each channel is randomised.");

		private static readonly GUIContent s_RandomBlendLabel = new GUIContent("Random Blend",
																			   "Determines the color-space in which random colors " +
																			   "are picked. HSL is good for yielding visually similar colors, " +
																			   "whereas RGB is useful when individual channels are important.");

		private static readonly GUIContent s_GradientWrapModeLabel = new GUIContent("Wrap Mode",
																					"Determines how the gradient is extrapolated.");

		private static readonly GUIContent s_GradientLengthLabel = new GUIContent("Gradient Length",
																				  "Important when the gradient does not represent " +
																				  "a simple scale, for example a distance or time value.");

		private SerializedProperty m_RangeModeProperty;
		private SerializedProperty m_LinearProperty;
		private SerializedProperty m_GradientWrapModeProperty;
		private SerializedProperty m_GradientLengthProperty;
		private SerializedProperty m_RandomBlendProperty;
		private SerializedProperty m_ConstValueAProperty;
		private SerializedProperty m_ConstValueBProperty;
		private SerializedProperty m_GradientAProperty;
		private SerializedProperty m_GradientBProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_RangeModeProperty = prop.FindPropertyRelative("m_RangeMode");
			m_LinearProperty = prop.FindPropertyRelative("m_Linear");
			m_GradientWrapModeProperty = prop.FindPropertyRelative("m_GradientWrapMode");
			m_GradientLengthProperty = prop.FindPropertyRelative("m_GradientLength");
			m_RandomBlendProperty = prop.FindPropertyRelative("m_RandomBlend");
			m_ConstValueAProperty = prop.FindPropertyRelative("m_ConstValueA");
			m_ConstValueBProperty = prop.FindPropertyRelative("m_ConstValueB");
			m_GradientAProperty = prop.FindPropertyRelative("m_GradientA");
			m_GradientBProperty = prop.FindPropertyRelative("m_GradientB");
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

			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<ColorRangeAttribute.RangeMode>(position, GUIContent.none, m_RangeModeProperty,
																										   HydraEditorGUIStyles.enumStyle));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			switch (m_RangeModeProperty.GetEnumValue<ColorRangeAttribute.RangeMode>())
			{
				case ColorRangeAttribute.RangeMode.Constant:
					OnConstant(position);
					break;

				case ColorRangeAttribute.RangeMode.Gradient:
					OnGradient(position);
					break;

				case ColorRangeAttribute.RangeMode.RandomInGradient:
					OnRandomInGradient(position);
					break;

				case ColorRangeAttribute.RangeMode.RandomBetweenTwoConstants:
					OnRandomBetweenTwoConstants(position);
					break;

				case ColorRangeAttribute.RangeMode.RandomBetweenTwoGradients:
					OnRandomBetweenTwoGradients(position);
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

			switch (m_RangeModeProperty.GetEnumValue<ColorRangeAttribute.RangeMode>())
			{
				case ColorRangeAttribute.RangeMode.Constant:
					lines = 2;
					break;
				case ColorRangeAttribute.RangeMode.Gradient:
					lines = 4;
					break;
				case ColorRangeAttribute.RangeMode.RandomInGradient:
					lines = 2;
					break;
				case ColorRangeAttribute.RangeMode.RandomBetweenTwoConstants:
					lines = 4;
					break;
				case ColorRangeAttribute.RangeMode.RandomBetweenTwoGradients:
					lines = 6;
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
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_ConstValueAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Gradient has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnGradient(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => GradientWrapField(position));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(
										    () => EditorGUI.PropertyField(position, m_GradientLengthProperty, s_GradientLengthLabel));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_GradientAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random In Gradient has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomInGradient(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(position, m_GradientAProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Raises when Random Between Two Constants has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoConstants(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_LinearLabel, m_LinearProperty));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => RandomBlendField(position));

			Rect gradientRect = new Rect(position);
			gradientRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			gradientRect.width = (gradientRect.width - HydraEditorUtils.STANDARD_HORIZONTAL_SPACING) / 2.0f;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_ConstValueAProperty, GUIContent.none));

			gradientRect.x += gradientRect.width;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_ConstValueBProperty, GUIContent.none));
		}

		/// <summary>
		/// Raises when Random Between Two Gradients has been selected.
		/// </summary>
		/// <param name="position">Position.</param>
		private void OnRandomBetweenTwoGradients(Rect position)
		{
			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_LinearLabel, m_LinearProperty));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => RandomBlendField(position));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(() => GradientWrapField(position));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.DrawUnindented(
										    () => EditorGUI.PropertyField(position, m_GradientLengthProperty, s_GradientLengthLabel));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			Rect gradientRect = new Rect(position);
			gradientRect.width = (gradientRect.width - HydraEditorUtils.STANDARD_HORIZONTAL_SPACING) / 2.0f;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_GradientAProperty, GUIContent.none));

			gradientRect.x += gradientRect.width;
			HydraEditorUtils.DrawUnindented(() => EditorGUI.PropertyField(gradientRect, m_GradientBProperty, GUIContent.none));
		}

		/// <summary>
		/// 	Gets the wrap mode.
		/// </summary>
		/// <returns>The wrap mode.</returns>
		/// <param name="property">Property.</param>
		private GradientExtensions.GradientWrapMode GetWrapMode(SerializedProperty property)
		{
			return (GradientExtensions.GradientWrapMode)property.enumValueIndex;
		}

		/// <summary>
		/// 	Draws the random blend field.
		/// </summary>
		/// <param name="position">Position.</param>
		private void RandomBlendField(Rect position)
		{
			HydraEditorUtils.EnumPopupField<ColorUtils.Gamut>(position, s_RandomBlendLabel, m_RandomBlendProperty,
															  HydraEditorGUIStyles.enumStyle);
		}

		/// <summary>
		/// 	Draws the gradient wrap field.
		/// </summary>
		/// <param name="position">Position.</param>
		private void GradientWrapField(Rect position)
		{
			HydraEditorUtils.EnumPopupField<GradientExtensions.GradientWrapMode>(position, s_GradientWrapModeLabel,
																				 m_GradientWrapModeProperty, HydraEditorGUIStyles.enumStyle);
		}
	}
}
