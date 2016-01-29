// <copyright file=AbstractAxesInputDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers.InputDrawers
{
	/// <summary>
	/// 	Abstract axes input drawer.
	/// </summary>
	public abstract class AbstractAxesInputDrawer : AbstractInputDrawer
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
											HydraEditorUtils.AxisInputField(position, GUIContent.none, m_InputProperty, HydraEditorGUIStyles.enumStyle));
		}

		#endregion
	}
}
