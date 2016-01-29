// <copyright file=IntRangeDrawer company=Hydra>
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
	/// 	IntRangeDrawer presents the IntRangeAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(IntRangeAttribute))]
	public class IntRangeDrawer : PropertyDrawer
	{
		private SerializedProperty m_RangeModeProperty;
		private SerializedProperty m_ConstValueAProperty;
		private SerializedProperty m_ConstValueBProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_RangeModeProperty = prop.FindPropertyRelative("m_RangeMode");
			m_ConstValueAProperty = prop.FindPropertyRelative("m_ConstValueA");
			m_ConstValueBProperty = prop.FindPropertyRelative("m_ConstValueB");
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

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			Rect rangeModePosition = new Rect(position);
			rangeModePosition.x += (rangeModePosition.width - 12);
			rangeModePosition.width = 12;

			Rect rangeModeFieldsPosition = new Rect(position);
			rangeModeFieldsPosition.width -= (rangeModePosition.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING);

			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<IntRangeAttribute.RangeMode>(rangeModePosition, GUIContent.none,
																										 m_RangeModeProperty, HydraEditorGUIStyles.enumStyle));

			switch (m_RangeModeProperty.GetEnumValue<IntRangeAttribute.RangeMode>())
			{
				case IntRangeAttribute.RangeMode.Constant:
					OnConstant(rangeModeFieldsPosition);
					break;

				case IntRangeAttribute.RangeMode.RandomBetweenTwoConstants:
					OnRandomBetweenTwoConstants(rangeModeFieldsPosition);
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
	}
}
