// <copyright file=AbstractInputDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers.InputDrawers
{
	/// <summary>
	/// 	AbstractInputDrawer is the base class for the input drawers.
	/// </summary>
	public abstract class AbstractInputDrawer : PropertyDrawer
	{
		#region Override Methods

		/// <summary>
		/// 	Override this method to make your own GUI for the property
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="prop">Property</param>
		/// <param name="label">Label</param>
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			Rect contentPosition = EditorGUI.PrefixLabel(position, label);

			FindProperties(prop);

			DrawContent(contentPosition);

			EditorGUI.EndProperty();
		}

		#endregion

		#region Virtual Methods

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected virtual void FindProperties(SerializedProperty prop) {}

		/// <summary>
		/// 	Draws the content.
		/// </summary>
		/// <param name="position">Position.</param>
		protected virtual void DrawContent(Rect position) {}

		#endregion
	}
}
