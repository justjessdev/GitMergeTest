// <copyright file=HydraEditorGUIStyles company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	public static class HydraEditorGUIStyles
	{
		private static GUIStyle s_WindowStyle;
		private static GUIStyle s_CenteredLabelStyle;
		private static GUIStyle s_RightAlignedLabelStyle;
		private static GUIStyle s_RightAlignedWhiteLabelStyle;
		private static GUIStyle s_ButtonStyle;
		private static GUIStyle s_EnumStyle;
		private static GUIStyle s_BoxStyle;
		private static GUIStyle s_Vector3CurvesBackgroundStyle;
		private static GUIStyle s_Vector3CurvesForegroundStyle;
		private static GUIStyle s_PreviewTextureStyle;

		private static GUIStyle s_ArrayElementEvenStyle;
		private static GUIStyle s_ArrayElementOddStyle;
		private static GUIStyle s_ArrayElementSelectedStyle;
		private static GUIStyle s_PictureButtonStyle;
		private static GUIStyle s_TextureButtonStyle;

		#region Properties

		/// <summary>
		/// 	Gets the window style.
		/// </summary>
		/// <value>The window style.</value>
		public static GUIStyle windowStyle
		{
			get
			{
				if (s_WindowStyle == null)
				{
					s_WindowStyle = new GUIStyle(GUI.skin.window);
					s_WindowStyle.onNormal = s_WindowStyle.normal;
				}
				return s_WindowStyle;
			}
		}

		/// <summary>
		/// 	Gets the centered label style.
		/// </summary>
		/// <value>The centered label style.</value>
		public static GUIStyle centeredLabelStyle
		{
			get
			{
				if (s_CenteredLabelStyle == null)
				{
					s_CenteredLabelStyle = new GUIStyle(GUI.skin.label);
					s_CenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
				}
				return s_CenteredLabelStyle;
			}
		}

		/// <summary>
		/// 	Gets the right aligned label style.
		/// </summary>
		/// <value>The right aligned label style.</value>
		public static GUIStyle rightAlignedLabelStyle
		{
			get
			{
				if (s_RightAlignedLabelStyle == null)
				{
					s_RightAlignedLabelStyle = new GUIStyle(GUI.skin.label);
					s_RightAlignedLabelStyle.alignment = TextAnchor.MiddleRight;
				}
				return s_RightAlignedLabelStyle;
			}
		}

		/// <summary>
		/// 	Gets the right aligned white label style.
		/// </summary>
		/// <value>The right aligned white label style.</value>
		public static GUIStyle rightAlignedWhiteLabelStyle
		{
			get
			{
				if (s_RightAlignedWhiteLabelStyle == null)
				{
					s_RightAlignedWhiteLabelStyle = new GUIStyle(EditorStyles.whiteLabel);
					s_RightAlignedWhiteLabelStyle.alignment = TextAnchor.MiddleRight;
				}
				return s_RightAlignedWhiteLabelStyle;
			}
		}

		/// <summary>
		/// 	Gets the button style.
		/// </summary>
		/// <value>The button style.</value>
		public static GUIStyle buttonStyle
		{
			get
			{
				if (s_ButtonStyle == null)
				{
					s_ButtonStyle = new GUIStyle("PreButton");
					CorrectPreStyle(s_ButtonStyle);
				}
				return s_ButtonStyle;
			}
		}

		/// <summary>
		/// 	Gets the enum style.
		/// </summary>
		/// <value>The enum style.</value>
		public static GUIStyle enumStyle
		{
			get
			{
				if (s_EnumStyle == null)
				{
					s_EnumStyle = new GUIStyle("PreDropdown");
					CorrectPreStyle(s_EnumStyle);
				}
				return s_EnumStyle;
			}
		}

		/// <summary>
		/// 	Gets the box style.
		/// </summary>
		/// <value>The box style.</value>
		public static GUIStyle boxStyle { get { return s_BoxStyle ?? (s_BoxStyle = new GUIStyle(GUI.skin.box)); } }

		/// <summary>
		/// 	Gets the vector3 curves background style.
		/// </summary>
		/// <value>The vector3 curves background style.</value>
		public static GUIStyle vector3CurvesBackgroundStyle
		{
			get
			{
				string name = EditorGUIUtility.isProSkin ? "AS TextArea" : "AnimationCurveEditorBackground";
				return s_Vector3CurvesBackgroundStyle ?? (s_Vector3CurvesBackgroundStyle = new GUIStyle(name));
			}
		}

		/// <summary>
		/// 	Gets the vector3 curves foreground style.
		/// </summary>
		/// <value>The vector3 curves foreground style.</value>
		public static GUIStyle vector3CurvesForegroundStyle
		{
			get { return s_Vector3CurvesForegroundStyle ?? (s_Vector3CurvesForegroundStyle = new GUIStyle("ColorPickerBox")); }
		}

		/// <summary>
		/// 	Gets the preview texture style.
		/// </summary>
		/// <value>The preview texture style.</value>
		public static GUIStyle previewTextureStyle
		{
			get { return s_PreviewTextureStyle ?? (s_PreviewTextureStyle = new GUIStyle("ColorPickerBox")); }
		}

		/// <summary>
		/// 	Gets the array element even style.
		/// </summary>
		/// <value>The array element even style.</value>
		public static GUIStyle arrayElementEvenStyle
		{
			get { return s_ArrayElementEvenStyle ?? (s_ArrayElementEvenStyle = new GUIStyle("AnimationRowEven")); }
		}

		/// <summary>
		/// 	Gets the array element odd style.
		/// </summary>
		/// <value>The array element odd style.</value>
		public static GUIStyle arrayElementOddStyle
		{
			get { return s_ArrayElementOddStyle ?? (s_ArrayElementOddStyle = new GUIStyle()); }
		}

		/// <summary>
		/// 	Gets the array element selected even style.
		/// </summary>
		/// <value>The array element selected even style.</value>
		public static GUIStyle arrayElementSelectedStyle
		{
			get { return s_ArrayElementSelectedStyle ?? (s_ArrayElementSelectedStyle = new GUIStyle("LODSliderRangeSelected")); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Gets the array element style.
		/// </summary>
		/// <returns>The element style.</returns>
		/// <param name="index">Index.</param>
		public static GUIStyle ArrayElementStyle(int index)
		{
			bool even = index % 2 == 0;
			return even ? arrayElementEvenStyle : arrayElementOddStyle;
		}

		/// <summary>
		/// 	Gets the picture button style.
		/// </summary>
		/// <returns>The picture button style.</returns>
		/// <param name="position">Position.</param>
		public static GUIStyle PictureButtonStyle(Rect position)
		{
			return PictureButtonStyle(position.width, position.height);
		}

		/// <summary>
		/// 	Gets the picture button style.
		/// </summary>
		/// <returns>The picture button style.</returns>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static GUIStyle PictureButtonStyle(float width, float height)
		{
			if (s_PictureButtonStyle == null)
				s_PictureButtonStyle = new GUIStyle(buttonStyle);

			s_PictureButtonStyle.fixedWidth = width;
			s_PictureButtonStyle.fixedHeight = height;

			return s_PictureButtonStyle;
		}

		/// <summary>
		/// 	Gets the texture button style.
		/// </summary>
		/// <returns>The texture button style.</returns>
		/// <param name="position">Position.</param>
		public static GUIStyle TextureButtonStyle(Rect position)
		{
			return TextureButtonStyle(position.width, position.height);
		}

		/// <summary>
		/// 	Gets the texture button style.
		/// </summary>
		/// <returns>The texture button style.</returns>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static GUIStyle TextureButtonStyle(float width, float height)
		{
			if (s_TextureButtonStyle == null)
				s_TextureButtonStyle = new GUIStyle();

			s_TextureButtonStyle.fixedWidth = width;
			s_TextureButtonStyle.fixedHeight = height;

			return s_TextureButtonStyle;
		}

		/// <summary>
		/// 	The "pre" styles are a little chunky, so we slim them down.
		/// </summary>
		/// <param name="style">Style.</param>
		public static void CorrectPreStyle(GUIStyle style)
		{
			style.font = GUI.skin.label.font;
			style.fontSize = GUI.skin.label.fontSize;
			style.fontStyle = GUI.skin.label.fontStyle;

			style.fixedHeight = GUI.skin.button.fixedHeight;
			style.margin = GUI.skin.button.margin;
		}

		#endregion
	}
}
