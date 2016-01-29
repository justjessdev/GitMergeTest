// <copyright file=Texture2DDrawer company=Hydra>
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
	/// 	Texture2DDrawer presents the Texture2DAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(Texture2DAttribute))]
	public class Texture2DDrawer : PropertyDrawer
	{
		public const int PREVIEW_TEXTURE_SIZE = 64;

		private SerializedProperty m_TextureProp;
		private SerializedProperty m_WrapProp;
		private SerializedProperty m_WrappedTextureProp;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected virtual void FindProperties(SerializedProperty prop)
		{
			m_TextureProp = prop.FindPropertyRelative("m_Texture");
			m_WrapProp = prop.FindPropertyRelative("m_Wrap");
			m_WrappedTextureProp = prop.FindPropertyRelative("m_WrappedTexture");
		}

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

			position.height = EditorGUIUtility.singleLineHeight;
			Rect contents = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			FindProperties(prop);

			// Draw the preview texture
			Rect textureRect = new Rect(contents);
			textureRect.x += textureRect.width - PREVIEW_TEXTURE_SIZE;
			textureRect.width = PREVIEW_TEXTURE_SIZE;
			textureRect.height = PREVIEW_TEXTURE_SIZE;

			EditorGUI.BeginChangeCheck();

			HydraEditorUtils.DrawUnindented(
										    () => HydraEditorUtils.TextureField(textureRect, GUIContent.none, m_TextureProp, true));

			// Draw the fields
			Rect contentRect = new Rect(contents);
			contentRect.width -= textureRect.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING * 2.0f;
			HydraEditorUtils.DrawUnindented(
										    () =>
											HydraEditorUtils.EnumPopupField<Texture2DAttribute.Wrap>(contentRect, GUIContent.none, m_WrapProp,
																									 HydraEditorGUIStyles.enumStyle));

			// Clear the cache if the texture changes
			if (EditorGUI.EndChangeCheck())
				m_WrappedTextureProp.objectReferenceValue = null;

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
			return PREVIEW_TEXTURE_SIZE + EditorGUIUtility.standardVerticalSpacing;
		}

		#endregion
	}
}
