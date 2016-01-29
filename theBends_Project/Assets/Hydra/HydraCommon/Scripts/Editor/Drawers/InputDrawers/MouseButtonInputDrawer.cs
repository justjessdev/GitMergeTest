// <copyright file=MouseButtonInputDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes.InputAttributes;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers.InputDrawers
{
	/// <summary>
	/// 	MouseButtonInputDrawer draws the inpspector elements for the MouseButtonInputAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(MouseButtonInputAttribute))]
	public class MouseButtonInputDrawer : AbstractInputDrawer
	{
		private SerializedProperty m_InputProperty;

		#region Override Methods

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected override void FindProperties(SerializedProperty prop)
		{
			base.FindProperties(prop);

			m_InputProperty = prop.FindPropertyRelative("m_Input");
		}

		/// <summary>
		/// 	Draws the content.
		/// </summary>
		/// <param name="position">Position.</param>
		protected override void DrawContent(Rect position)
		{
			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<MouseButtonInputAttribute.Buttons>(position, GUIContent.none, m_InputProperty,
																											   HydraEditorGUIStyles.enumStyle));
		}

		#endregion
	}
}
