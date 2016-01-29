// <copyright file=CurveEditorWindowWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Reflection;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Windows
{
	/// <summary>
	/// 	Unity's CurveEditorWindow is internal, so if we want to use it we need to get aggressive.
	/// </summary>
	public class CurveEditorWindowWrapper
	{
		public const string CURVE_EDITOR_WINDOW_TYPE_NAME = "CurveEditorWindow";
		public const string CURVE_EDITOR_WINDOW_SHARED_INSTANCE_FIELD_NAME = "s_SharedCurveEditor";
		public const string CURVE_EDITOR_WINDOW_CURVE_EDITOR_FIELD_NAME = "m_CurveEditor";
		public const string CURVE_EDITOR_WINDOW_COLOR_PROPERTY_NAME = "color";
		public const string CURVE_EDITOR_WINDOW_CURVE_PROPERTY_NAME = "curve";

		public const string CURVE_EDITOR_TYPE_NAME = "CurveEditor";
		public const string CURVE_EDITOR_FRAME_H_METHOD_NAME = "SetShownHRangeInsideMargins";
		public const string CURVE_EDITOR_FRAME_V_METHOD_NAME = "SetShownVRangeInsideMargins";

		public const string CURVE_EDITOR_WINDOW_POPUP_METHOD_NAME = "ShowCurvePopup";

		public const string BAD_MEMBER_NAMES_ERROR =
			"Can't use CurveEditorWindow in this version of Unity. " +
			"Dear Unity Technologies, please expose an API for the CurveEditorWindow.";

		private static readonly Type s_CurveEditorWindowType;
		private static readonly FieldInfo s_SharedInstanceField;
		private static readonly FieldInfo s_CurveEditorField;
		private static readonly PropertyInfo s_ColorProperty;
		private static readonly PropertyInfo s_CurveProperty;
		private static readonly MethodInfo s_PopupMethod;

		private static readonly MethodInfo s_FrameHMethod;
		private static readonly MethodInfo s_FrameVMethod;

		#region Properties

		/// <summary>
		/// 	Gets or sets the curve.
		/// </summary>
		/// <value>The curve.</value>
		public static AnimationCurve curve
		{
			get { return s_CurveProperty.GetValue(s_CurveEditorWindowType, null) as AnimationCurve; }
			set { s_CurveProperty.SetValue(s_CurveEditorWindowType, value, null); }
		}

		/// <summary>
		/// 	Gets or sets the color of the curve.
		/// </summary>
		/// <value>The color.</value>
		public static Color color
		{
			get { return (Color)s_ColorProperty.GetValue(s_CurveEditorWindowType, null); }
			set { s_ColorProperty.SetValue(s_CurveEditorWindowType, value, null); }
		}

		/// <summary>
		/// 	Gets the CurveEditorWindow instance.
		/// </summary>
		/// <value>The CurveEditorWindow instance.</value>
		public static EditorWindow curveEditorWindow { get { return s_SharedInstanceField.GetValue(null) as EditorWindow; } }

		/// <summary>
		/// 	Gets the curve editor.
		/// </summary>
		/// <value>The curve editor.</value>
		public static object curveEditor { get { return s_CurveEditorField.GetValue(curveEditorWindow); } }

		#endregion

		/// <summary>
		/// 	Initializes the <see cref="CurveEditorWindowWrapper"/> class.
		/// </summary>
		static CurveEditorWindowWrapper()
		{
			// Get the CurveEditorWindow type and members
			s_CurveEditorWindowType = ReflectionUtils.GetTypeByName(Assembly.GetAssembly(typeof(EditorWindow)),
																	CURVE_EDITOR_WINDOW_TYPE_NAME);
			if (s_CurveEditorWindowType == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_SharedInstanceField = ReflectionUtils.GetFieldByName(s_CurveEditorWindowType,
																   CURVE_EDITOR_WINDOW_SHARED_INSTANCE_FIELD_NAME);
			if (s_SharedInstanceField == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_CurveEditorField = ReflectionUtils.GetFieldByName(s_CurveEditorWindowType,
																CURVE_EDITOR_WINDOW_CURVE_EDITOR_FIELD_NAME);
			if (s_CurveEditorField == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_CurveProperty = ReflectionUtils.GetPropertyByName(s_CurveEditorWindowType, CURVE_EDITOR_WINDOW_CURVE_PROPERTY_NAME);
			if (s_CurveProperty == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_ColorProperty = ReflectionUtils.GetPropertyByName(s_CurveEditorWindowType, CURVE_EDITOR_WINDOW_COLOR_PROPERTY_NAME);
			if (s_ColorProperty == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			// Get the CurveEditor type and members
			Type curveEditorType = ReflectionUtils.GetTypeByName(Assembly.GetAssembly(typeof(EditorWindow)),
																 CURVE_EDITOR_TYPE_NAME);
			if (curveEditorType == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_FrameHMethod = ReflectionUtils.GetMethodByName(curveEditorType, CURVE_EDITOR_FRAME_H_METHOD_NAME);
			if (s_FrameHMethod == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_FrameVMethod = ReflectionUtils.GetMethodByName(curveEditorType, CURVE_EDITOR_FRAME_V_METHOD_NAME);
			if (s_FrameVMethod == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			// Finally the method for showing the curve editor window
			s_PopupMethod = ReflectionUtils.GetMethodByName(typeof(EditorGUI), CURVE_EDITOR_WINDOW_POPUP_METHOD_NAME);
			if (s_PopupMethod == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);
		}

		#region Methods

		/// <summary>
		/// 	Shows the curve editor window.
		/// </summary>
		/// <param name="title">Title.</param>
		public static void ShowCurveEditorWindow(string title)
		{
			s_PopupMethod.Invoke(null, new object[] {null, new Rect()});

			curveEditorWindow.titleContent = new GUIContent(title);
		}

		/// <summary>
		/// 	Frame the specified area.
		/// </summary>
		/// <param name="area">Area.</param>
		public static void Frame(Rect area)
		{
			s_FrameHMethod.Invoke(curveEditor, new object[] {area.xMin, area.xMax});
			s_FrameVMethod.Invoke(curveEditor, new object[] {area.yMin, area.yMax});
		}

		#endregion
	}
}
