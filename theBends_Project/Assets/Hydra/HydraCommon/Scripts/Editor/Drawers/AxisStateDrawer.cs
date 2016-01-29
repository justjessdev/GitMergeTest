// <copyright file=AxisStateDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	/// <summary>
	/// 	Axis state drawer.
	/// </summary>
	[CustomPropertyDrawer(typeof(AxisStateAttribute))]
	public class AxisStateDrawer : PropertyDrawer
	{
		public const float HORIZONTAL_SPACING = 32.0f;

		private static readonly GUIContent s_XLabel = new GUIContent("X");
		private static readonly GUIContent s_YLabel = new GUIContent("Y");
		private static readonly GUIContent s_ZLabel = new GUIContent("Z");

		private SerializedProperty m_XProperty;
		private SerializedProperty m_YProperty;
		private SerializedProperty m_ZProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		private void FindProperties(SerializedProperty prop)
		{
			m_XProperty = prop.FindPropertyRelative("m_XState");
			m_YProperty = prop.FindPropertyRelative("m_YState");
			m_ZProperty = prop.FindPropertyRelative("m_ZState");
		}

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

			position.width = HORIZONTAL_SPACING;

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_XLabel, m_XProperty));

			position.x += HORIZONTAL_SPACING;
			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_YLabel, m_YProperty));

			position.x += HORIZONTAL_SPACING;
			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.ToggleLeftField(position, s_ZLabel, m_ZProperty));

			EditorGUI.EndProperty();
		}
	}
}
