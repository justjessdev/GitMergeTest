// <copyright file=HydraEditorUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	public static class HydraEditorUtils
	{
		public const string BROWSE_BUTTON_TEXTURE_NAME = "Browse.tif";

		public const float STANDARD_HORIZONTAL_SPACING = 2.0f;

		public const float AXIS_LABEL_WIDTH = 16.0f;
		public const float BROWSE_BUTTON_WIDTH = 32.0f;

		public static readonly GUIContent s_AxisXLabel = new GUIContent("X");
		public static readonly GUIContent s_AxisYLabel = new GUIContent("Y");

		private static string[] s_LayerNames;
		private static AxisInfo[] s_AxesInfo;
		private static string[] s_AxesNames;

		#region Methods

		/// <summary>
		/// 	Creates a rect that prevents the mouse from effecting the scene.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="controlId">Control identifier.</param>
		public static void EatMouseInput(Rect position, int controlId)
		{
			controlId = GUIUtility.GetControlID(controlId, FocusType.Native, position);

			switch (Event.current.GetTypeForControl(controlId))
			{
				case EventType.MouseDown:
					if (position.Contains(Event.current.mousePosition))
					{
						GUIUtility.hotControl = controlId;
						Event.current.Use();
					}
					break;

				case EventType.MouseUp:
					if (GUIUtility.hotControl == controlId)
					{
						GUIUtility.hotControl = 0;
						Event.current.Use();
					}
					break;

				case EventType.MouseDrag:
					if (GUIUtility.hotControl == controlId)
						Event.current.Use();
					break;

				case EventType.ScrollWheel:
					if (position.Contains(Event.current.mousePosition))
						Event.current.Use();
					break;
			}
		}

		/// <summary>
		/// 	Draws a Save File field.
		/// </summary>
		/// <returns>The save path.</returns>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="path">Path.</param>
		/// <param name="extention">Extention.</param>
		public static string SaveFileField(Rect position, GUIContent label, string path, string extention)
		{
			Rect content = new Rect(position);
			content.width -= BROWSE_BUTTON_WIDTH;
			content.width -= STANDARD_HORIZONTAL_SPACING;

			EditorGUI.TextField(content, label, path);

			Rect buttonRect = new Rect(content);
			buttonRect.x += content.width + STANDARD_HORIZONTAL_SPACING;
			buttonRect.width = BROWSE_BUTTON_WIDTH;

			Texture2D buttonTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures",
																				 BROWSE_BUTTON_TEXTURE_NAME);
			GUIStyle style = HydraEditorGUIStyles.PictureButtonStyle(buttonRect);

			bool pressed = DrawUnindented(() => GUI.Button(buttonRect, buttonTexture, style));
			if (pressed)
				path = SaveFilePanel(label, path, extention);

			return path;
		}

		/// <summary>
		/// 	Shows the Save File panel.
		/// </summary>
		/// <returns>The new path.</returns>
		/// <param name="title">Title.</param>
		/// <param name="path">Path.</param>
		/// <param name="extention">Extention.</param>
		public static string SaveFilePanel(GUIContent title, string path, string extention)
		{
			path = Path.ChangeExtension(path, extention);

			string directory = string.IsNullOrEmpty(path) ? string.Empty : Path.GetDirectoryName(path);
			string defaultName = string.IsNullOrEmpty(path) ? string.Empty : Path.GetFileName(path);

			return EditorUtility.SaveFilePanel(title.text, directory, defaultName, extention);
		}

		/// <summary>
		/// 	Draws a scene field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="style">Style.</param>
		public static void SceneField(Rect position, GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorBuildSettingsScene[] scenes = EditorSceneUtils.GetEnabledScenes();
			string[] names = EditorSceneUtils.GetSceneNames(scenes);

			EditorGUI.BeginChangeCheck();

			int selected = EditorGUI.Popup(position, label.text, GetSceneIndex(prop, scenes), names, style);
			EditorBuildSettingsScene value = (selected < scenes.Length) ? scenes[selected] : null;

			if (EditorGUI.EndChangeCheck())
				prop.stringValue = (value != null) ? AssetDatabase.AssetPathToGUID(value.path) : string.Empty;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Gets the current scene index for the scene property.
		/// </summary>
		/// <returns>The scene index.</returns>
		/// <param name="prop">Property.</param>
		/// <param name="scenes">Scenes.</param>
		private static int GetSceneIndex(SerializedProperty prop, EditorBuildSettingsScene[] scenes)
		{
			for (int index = 0; index < scenes.Length; index++)
			{
				string path = scenes[index].path;
				string guid = AssetDatabase.AssetPathToGUID(path);

				if (guid == prop.stringValue)
					return index;
			}

			try
			{
				return EditorSceneUtils.GetSceneIndex(EditorApplication.currentScene);
			}
			catch (ArgumentOutOfRangeException)
			{
				return 0;
			}
		}

		/// <summary>
		/// 	Draws an axis input field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void AxisInputField(Rect position, GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			InputUtils.GetAxesInfo(ref s_AxesInfo);
			Array.Resize(ref s_AxesNames, s_AxesInfo.Length);
			int selected = 0;

			for (int index = 0; index < s_AxesInfo.Length; index++)
			{
				string name = s_AxesInfo[index].name;
				if (name == prop.stringValue)
					selected = index;
				s_AxesNames[index] = name;
			}

			EditorGUI.BeginChangeCheck();

			selected = EditorGUI.Popup(position, label.text, selected, s_AxesNames, style);
			string value = s_AxesNames[selected];

			if (EditorGUI.EndChangeCheck())
				prop.stringValue = value;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a texture field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void TextureField(Rect position, GUIContent label, SerializedProperty prop)
		{
			TextureField(position, label, prop, false);
		}

		/// <summary>
		/// 	Draws a texture field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="readable">If set to <c>true</c> readable.</param>
		public static void TextureField(Rect position, GUIContent label, SerializedProperty prop, bool readable)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			Texture texture =
				EditorGUI.ObjectField(position, label, prop.objectReferenceValue, typeof(Texture), false) as Texture;

			if (readable && texture != null && !(texture as Texture2D).IsReadable())
			{
				string warning = string.Format("Texture {0} is not readable.", texture.name);
				Debug.LogWarning(warning);
				texture = null;
			}

			if (texture != prop.objectReferenceValue)
				prop.objectReferenceValue = texture;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws an int field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateIntField(Rect position, GUIContent label, SerializedProperty prop, Func<int, int> validator)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			int value = EditorGUI.IntField(position, label, prop.intValue);
			if (EditorGUI.EndChangeCheck())
				prop.intValue = validator(value);

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a float field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateFloatField(Rect position, GUIContent label, SerializedProperty prop,
											  Func<float, float> validator)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			float value = EditorGUI.FloatField(position, label, prop.floatValue);
			if (EditorGUI.EndChangeCheck())
				prop.floatValue = validator(value);

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a Vector3 field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateVector3Field(Rect position, GUIContent label, SerializedProperty prop,
												Func<Vector3, Vector3> validator)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			bool wide = EditorGUIUtility.wideMode;
			EditorGUIUtility.wideMode = true;

			Vector3 value = EditorGUI.Vector3Field(position, label, prop.vector3Value);
			if (EditorGUI.EndChangeCheck())
				prop.vector3Value = validator(value);

			EditorGUIUtility.wideMode = wide;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws two integer properties, using the validator to process the new values.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="propA">Property a.</param>
		/// <param name="propB">Property b.</param>
		/// <param name="validator">Validator.</param>
		public static void Validate2IntField(Rect position, GUIContent label, SerializedProperty propA,
											 SerializedProperty propB, Func<int, int> validator)
		{
			Rect content = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			Rect halfContent = new Rect(content);
			halfContent.width -= STANDARD_HORIZONTAL_SPACING;
			halfContent.width /= 2.0f;

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = AXIS_LABEL_WIDTH;

			DrawUnindented(() => ValidateIntField(halfContent, s_AxisXLabel, propA, validator));

			halfContent.x += halfContent.width + STANDARD_HORIZONTAL_SPACING;

			DrawUnindented(() => ValidateIntField(halfContent, s_AxisYLabel, propB, validator));

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}

		/// <summary>
		/// 	Behaves like a boolean field but draws a textured button.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="trueTexture">True texture.</param>
		/// <param name="falseTexture">False texture.</param>
		public static void ToggleTexturesField(Rect position, GUIContent label, SerializedProperty prop, Texture2D trueTexture,
											   Texture2D falseTexture)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			contentPosition.width = contentPosition.height;

			GUIStyle style = HydraEditorGUIStyles.TextureButtonStyle(contentPosition);
			Texture2D buttonTexture = prop.boolValue ? trueTexture : falseTexture;

			bool pressed = GUI.Button(contentPosition, buttonTexture, style);
			if (pressed)
			{
				prop.boolValue = !prop.boolValue;
				GUI.changed = true;
			}

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Shorthand toggle left field.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void ToggleLeftField(Rect position, GUIContent label, SerializedProperty prop)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			bool value = EditorGUI.ToggleLeft(position, label, prop.boolValue);
			if (EditorGUI.EndChangeCheck())
				prop.boolValue = value;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a LayerMask field with the given style.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="style">Style.</param>
		public static void LayerMaskField(Rect position, GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			LayerMaskUtils.GetMaskFieldNames(ref s_LayerNames);
			int mappedMask = LayerMaskUtils.MapToMaskField(prop.intValue);
			mappedMask = EditorGUI.MaskField(position, label, mappedMask, s_LayerNames, style);

			if (EditorGUI.EndChangeCheck())
			{
				LayerMask newMask = LayerMaskUtils.MapFromMaskField(mappedMask);
				prop.SetMask(newMask);
			}

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Shorthand enum popup field with a style.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="style">Style.</param>
		/// <typeparam name="T">The enum type.</typeparam>
		public static void EnumPopupField<T>(Rect position, GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			Enum enumValue = (Enum)Enum.ToObject(typeof(T), prop.enumValueIndex);

			enumValue = EditorGUI.EnumPopup(position, label, enumValue, style);
			if (EditorGUI.EndChangeCheck())
				prop.enumValueIndex = Convert.ToInt32(enumValue);

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a single float field and applies that value to all of the axis on the Vector3
		/// 	property.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void UniformVector3Field(Rect position, GUIContent label, SerializedProperty prop)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			float axis = EditorGUI.FloatField(position, label, prop.vector3Value.x);
			if (EditorGUI.EndChangeCheck())
				prop.vector3Value = Vector3.one * axis;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a Vector3 field. For some reason Unity throws an error with vector3 property
		/// 	fields, so this is a workaround.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void Vector3Field(Rect position, GUIContent label, SerializedProperty prop)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();

			bool wide = EditorGUIUtility.wideMode;
			EditorGUIUtility.wideMode = true;

			Vector3 newValue = EditorGUI.Vector3Field(position, label, prop.vector3Value);
			if (EditorGUI.EndChangeCheck())
				prop.vector3Value = newValue;

			EditorGUIUtility.wideMode = wide;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a Vector3 field, tidying to 2 decimal places.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void Vector3Field2Dp(Rect position, GUIContent label, SerializedProperty prop)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.LabelField(position, label);

			Rect content = new Rect(position);
			content.x += EditorGUIUtility.labelWidth;
			content.width -= EditorGUIUtility.labelWidth;
			content.width = content.width - STANDARD_HORIZONTAL_SPACING * 2.0f;
			content.width /= 3.0f;

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = AXIS_LABEL_WIDTH;

			EditorGUI.BeginChangeCheck();

			Vector3 value = prop.vector3Value;
			float x = DrawUnindented(() => FloatField2Dp(content, new GUIContent("X"), value.x));

			content.x += content.width + STANDARD_HORIZONTAL_SPACING;
			float y = DrawUnindented(() => FloatField2Dp(content, new GUIContent("Y"), value.y));

			content.x += content.width + STANDARD_HORIZONTAL_SPACING;
			float z = DrawUnindented(() => FloatField2Dp(content, new GUIContent("Z"), value.z));

			if (EditorGUI.EndChangeCheck())
				prop.vector3Value = new Vector3(x, y, z);

			EditorGUIUtility.labelWidth = oldLabelWidth;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Draws a float field with 2 decimal places.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="position">Position.</param>
		/// <param name="label">Label.</param>
		/// <param name="value">Value.</param>
		public static float FloatField2Dp(Rect position, GUIContent label, float value)
		{
			string floatString = value.ToString("0.00");
			floatString = TrimDecimalPlaces(floatString);

			string input = EditorGUI.TextField(position, label, floatString);

			if (input != floatString)
			{
				float inputFloat;
				bool valid = float.TryParse(input, out inputFloat);
				if (valid)
					value = inputFloat;
			}

			return value;
		}

		/// <summary>
		/// 	Removes trailing "." and "0" characters.
		/// </summary>
		/// <returns>The trimmed string.</returns>
		/// <param name="input">Input.</param>
		private static string TrimDecimalPlaces(string input)
		{
			if (input.Contains("."))
			{
				input = input.TrimEnd('0');
				input = input.TrimEnd('.');
			}

			return input;
		}

		/// <summary>
		/// 	Draws a texture in the inspector.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="texture">Texture.</param>
		public static void PreviewTexture(Rect position, Texture2D texture)
		{
			EditorGUI.DrawPreviewTexture(position, texture);
			GUI.Box(position, GUIContent.none, HydraEditorGUIStyles.previewTextureStyle);
		}

		/// <summary>
		/// 	Clears the indentation level and calls the draw method.
		/// </summary>
		/// <param name="drawMethod">Draw method.</param>
		public static void DrawUnindented(Action drawMethod)
		{
			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			drawMethod();

			EditorGUI.indentLevel = oldIndent;
		}

		/// <summary>
		/// 	Clears the indentation level and calls the draw method.
		/// </summary>
		/// <param name="drawMethod">Draw method.</param>
		/// <typeparam name="T">The return type.</typeparam>
		public static T DrawUnindented<T>(Func<T> drawMethod)
		{
			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			T result = drawMethod();

			EditorGUI.indentLevel = oldIndent;

			return result;
		}

		/// <summary>
		/// 	Draws the foldout.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="position">Position.</param>
		public static bool DrawFoldout(GUIContent title, Rect position)
		{
			bool state = EditorPrefsUtils.GetFoldoutState(title);

			bool newState = EditorGUI.Foldout(position, state, title);

			if (newState != state)
				EditorPrefsUtils.SetFoldoutState(title, newState);

			return newState;
		}

		#endregion
	}
}
