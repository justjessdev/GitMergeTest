// <copyright file=InputUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	AxisInfo provides details about an InputManager axis.
	/// </summary>
	public struct AxisInfo
	{
		public enum InputType
		{
			KeyOrMouseButton,
			MouseMovement,
			JoystickAxis
		}

		private string m_Name;
		private int m_Axis;
		private InputType m_InputType;

		public string name { get { return m_Name; } }
		public int axis { get { return m_Axis; } }
		public InputType inputType { get { return m_InputType; } }

		public AxisInfo(string name, int axis, InputType inputType)
		{
			m_Name = name;
			m_Axis = axis;
			m_InputType = inputType;
		}
	}

	/// <summary>
	/// 	InputUtils provides utility methods for working with the InputManager.
	/// </summary>
	public static class InputUtils
	{
		private const string INPUT_MANAGER_PATH = "ProjectSettings/InputManager.asset";

		private const string AXES_ARRAY_NAME = "m_Axes";

		private const string AXIS_NAME = "m_Name";
		private const string AXIS_AXIS = "axis";
		private const string AXIS_TYPE = "type";

		/// <summary>
		/// 	Gets the axes info.
		/// </summary>
		/// <param name="output">Output.</param>
		public static void GetAxesInfo(ref AxisInfo[] output)
		{
			Object inputManager = AssetDatabase.LoadAllAssetsAtPath(INPUT_MANAGER_PATH)[0];

			SerializedObject serialized = new SerializedObject(inputManager);
			SerializedProperty axisArray = serialized.FindProperty(AXES_ARRAY_NAME);

			Array.Resize(ref output, axisArray.arraySize);

			for (int index = 0; index < axisArray.arraySize; index++)
			{
				SerializedProperty axis = axisArray.GetArrayElementAtIndex(index);

				string name = axis.FindPropertyRelative(AXIS_NAME).stringValue;
				int axisVal = axis.FindPropertyRelative(AXIS_AXIS).intValue;
				AxisInfo.InputType inputType = (AxisInfo.InputType)axis.FindPropertyRelative(AXIS_TYPE).intValue;

				output[index] = new AxisInfo(name, axisVal, inputType);
			}
		}
	}
}
