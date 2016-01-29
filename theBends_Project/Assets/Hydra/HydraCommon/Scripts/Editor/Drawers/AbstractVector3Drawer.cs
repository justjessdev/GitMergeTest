// <copyright file=AbstractVector3Drawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	/// <summary>
	/// 	Provides base functionality for presenting AbstractVector3Attributes in the inspector.
	/// </summary>
	public abstract class AbstractVector3Drawer : PropertyDrawer
	{
		public const string PADLOCK_CLOSED_NAME = "LinkClosed.tif";
		public const string PADLOCK_OPEN_NAME = "LinkOpen.tif";

		private SerializedProperty m_LockedProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected virtual void FindProperties(SerializedProperty prop)
		{
			m_LockedProperty = prop.FindPropertyRelative("m_Locked");
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

			// Draw the lock
			Rect lockPosition = new Rect(position);
			lockPosition.width = lockPosition.height;
			HydraEditorUtils.DrawUnindented(() => LockField(lockPosition, m_LockedProperty));

			// Draw the contents
			Rect contentPosition = new Rect(position);
			float offset = lockPosition.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING;
			contentPosition.x += offset;
			contentPosition.width -= offset;

			DrawContent(contentPosition, m_LockedProperty.boolValue);

			EditorGUI.EndProperty();
		}

		#endregion

		/// <summary>
		/// 	Draws the lock field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="prop">Property.</param>
		private void LockField(Rect position, SerializedProperty prop)
		{
			Texture2D trueTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures", PADLOCK_CLOSED_NAME);
			Texture2D falseTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures", PADLOCK_OPEN_NAME);

			HydraEditorUtils.ToggleTexturesField(position, GUIContent.none, prop, trueTexture, falseTexture);
		}

		/// <summary>
		/// 	Draws the content.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="locked">Locked.</param>
		protected abstract void DrawContent(Rect position, bool locked);
	}
}
