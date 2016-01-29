// <copyright file=HydraEditorLayoutUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Text;
using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	public static class HydraEditorLayoutUtils
	{
		private static readonly GUIContent s_ArrayIncrementLabel = new GUIContent("+", "Increase the size of the collection.");
		private static readonly GUIContent s_ArrayDecrementLabel = new GUIContent("-", "Reduce the size of the collection.");

		private static readonly StringBuilder s_StringBuilder;

		private static Camera[] s_Cameras;
		private static GUIContent[] s_CameraNames;

		/// <summary>
		/// 	Initializes the HydraEditorLayoutUtils class.
		/// </summary>
		static HydraEditorLayoutUtils()
		{
			s_StringBuilder = new StringBuilder();
		}

		#region Methods

		/// <summary>
		/// 	Draws a camera selection field.
		/// </summary>
		/// <returns>The selection field.</returns>
		public static Camera CameraSelectionField(GUIContent label, Camera value, bool sceneViewCameras = false)
		{
			FindCameras(sceneViewCameras);

			int index = 0;

			for (int searchIndex = 0; searchIndex < s_Cameras.Length; searchIndex++)
			{
				if (s_Cameras[searchIndex] == value)
				{
					index = searchIndex;
					break;
				}
			}

			index = EditorGUILayout.Popup(label, index, s_CameraNames, HydraEditorGUIStyles.enumStyle);

			return s_Cameras[index];
		}

		/// <summary>
		/// 	Finds the cameras.
		/// </summary>
		private static void FindCameras(bool sceneViewCameras)
		{
			CameraUtils.GetAllCameras(ref s_Cameras, sceneViewCameras);
			Array.Resize(ref s_CameraNames, s_Cameras.Length);

			for (int index = 0; index < s_Cameras.Length; index++)
				s_CameraNames[index] = new GUIContent(s_Cameras[index].name);
		}

		public static string SaveFileField(GUIContent label, string path, string extention)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			return HydraEditorUtils.SaveFileField(position, label, path, extention);
		}

		/// <summary>
		/// 	Draws an int field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateIntField(GUIContent label, SerializedProperty prop, Func<int, int> validator)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.ValidateIntField(position, label, prop, validator);
		}

		/// <summary>
		/// 	Draws a float field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateFloatField(GUIContent label, SerializedProperty prop, Func<float, float> validator)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.ValidateFloatField(position, label, prop, validator);
		}

		/// <summary>
		/// 	Draws a Vector3 field for a property, using the validator to process the new value.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="validator">Validator.</param>
		public static void ValidateVector3Field(GUIContent label, SerializedProperty prop, Func<Vector3, Vector3> validator)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.ValidateVector3Field(position, label, prop, validator);
		}

		/// <summary>
		/// 	Draws two integer properties, using the validator to process the new values.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="propA">Property a.</param>
		/// <param name="propB">Property b.</param>
		/// <param name="validator">Validator.</param>
		public static void Validate2IntField(GUIContent label, SerializedProperty propA, SerializedProperty propB,
											 Func<int, int> validator)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.Validate2IntField(position, label, propA, propB, validator);
		}

		/// <summary>
		/// 	Draws a Vector3 field. For some reason Unity throws an error with vector3 property
		/// 	fields, so this is a workaround.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		public static void Vector3Field(GUIContent label, SerializedProperty prop)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.Vector3Field(position, label, prop);
		}

		/// <summary>
		/// 	Shorthand enum popup field with a style.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="style">Style.</param>
		/// <typeparam name="T">The enum type.</typeparam>
		public static void EnumPopupField<T>(GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.EnumPopupField<T>(position, label, prop, style);
		}

		/// <summary>
		/// 	Draws a float field with 2 decimal places.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="label">Label.</param>
		/// <param name="value">Value.</param>
		public static float FloatField2Dp(string label, float value)
		{
			return FloatField2Dp(new GUIContent(label), value);
		}

		/// <summary>
		/// 	Draws a float field with 2 decimal places.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="label">Label.</param>
		/// <param name="value">Value.</param>
		public static float FloatField2Dp(GUIContent label, float value)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			return HydraEditorUtils.FloatField2Dp(position, label, value);
		}

		/// <summary>
		/// 	Draws a layer mask field.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="prop">Property.</param>
		/// <param name="style">Style.</param>
		public static void LayerMaskField(GUIContent label, SerializedProperty prop, GUIStyle style)
		{
			Rect position = EditorGUILayout.GetControlRect(true);
			HydraEditorUtils.LayerMaskField(position, label, prop, style);
		}

		/// <summary>
		/// 	Begins the box.
		/// </summary>
		public static void BeginBox(bool indent = true)
		{
			Color oldColor = GUI.backgroundColor;

			if (!EditorGUIUtility.isProSkin)
				GUI.backgroundColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);

			GUILayout.BeginVertical(HydraEditorGUIStyles.boxStyle);

			GUI.backgroundColor = oldColor;

			if (indent)
				EditorGUI.indentLevel++;
		}

		/// <summary>
		/// 	Ends the box.
		/// </summary>
		public static void EndBox(bool indent = true)
		{
			GUILayout.EndVertical();

			if (indent)
				EditorGUI.indentLevel--;
		}

		/// <summary>
		/// 	Draws an array field.
		/// </summary>
		/// <param name="prop">Property.</param>
		/// <param name="label">Label.</param>
		/// <param name="elementLabel">Element label.</param>
		/// <param name="elementMethod">Element method.</param>
		public static void ArrayField(SerializedProperty prop, GUIContent label, GUIContent elementLabel,
									  Action<SerializedProperty, GUIContent> elementMethod = null)
		{
			DrawFoldout(label, () => ArrayFieldContents(prop, elementLabel, elementMethod));
		}

		/// <summary>
		/// 	Draws an array field contents.
		/// </summary>
		/// <param name="prop">Property.</param>
		/// <param name="elementLabel">Element label.</param>
		/// <param name="elementMethod">Element method.</param>
		public static void ArrayFieldContents(SerializedProperty prop, GUIContent elementLabel,
											  Action<SerializedProperty, GUIContent> elementMethod = null)
		{
			for (int index = 0; index < prop.arraySize; index++)
			{
				Color oldColor = GUI.backgroundColor;
				if (EditorGUIUtility.isProSkin)
					GUI.backgroundColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);

				EditorGUILayout.BeginVertical(HydraEditorGUIStyles.ArrayElementStyle(index));

				GUI.backgroundColor = oldColor;

				s_StringBuilder.Length = 0;
				s_StringBuilder.Append(elementLabel.text);
				s_StringBuilder.Append(" ");
				s_StringBuilder.Append(index);

				SerializedProperty element = prop.GetArrayElementAtIndex(index);
				GUIContent label = new GUIContent(s_StringBuilder.ToString(), elementLabel.image, elementLabel.tooltip);

				if (elementMethod != null)
					elementMethod(element, label);
				else
					EditorGUILayout.PropertyField(element, label);

				EditorGUILayout.EndVertical();
			}

			ArrayFieldFooter(prop);
		}

		/// <summary>
		/// 	Draws an array field footer.
		/// </summary>
		/// <param name="prop">Property.</param>
		public static void ArrayFieldFooter(SerializedProperty prop)
		{
			if (prop.arraySize > 0)
				EditorGUILayout.Separator();

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(s_ArrayIncrementLabel, HydraEditorGUIStyles.buttonStyle, GUILayout.Width(24),
									 GUILayout.Height(EditorGUIUtility.singleLineHeight)))
					prop.SetArraySizeSafe(prop.arraySize + 1);

				if (GUILayout.Button(s_ArrayDecrementLabel, HydraEditorGUIStyles.buttonStyle, GUILayout.Width(24),
									 GUILayout.Height(EditorGUIUtility.singleLineHeight)))
					prop.SetArraySizeSafe(prop.arraySize - 1);
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// 	Draws a foldout and uses the drawContentsMethod to fill it.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="drawContentsMethod">Draw contents method.</param>
		public static bool DrawFoldout(string title, Action drawContentsMethod)
		{
			return DrawFoldout(new GUIContent(title), drawContentsMethod);
		}

		/// <summary>
		/// 	Draws a foldout and uses the drawContentsMethod to fill it.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="drawContentsMethod">Draw contents method.</param>
		public static bool DrawFoldout(GUIContent title, Action drawContentsMethod)
		{
			bool state = EditorPrefsUtils.GetFoldoutState(title);

			bool newState = EditorGUILayout.Foldout(state, title);
			if (newState)
			{
				int indentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel++;

				drawContentsMethod();

				EditorGUI.indentLevel = indentLevel;
			}

			if (newState != state)
				EditorPrefsUtils.SetFoldoutState(title, newState);

			return newState;
		}

		#endregion
	}
}
