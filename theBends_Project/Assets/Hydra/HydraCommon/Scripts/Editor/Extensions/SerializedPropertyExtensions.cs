// <copyright file=SerializedPropertyExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Editor.Extensions
{
	/// <summary>
	/// 	Provides extension methods for SerializedProperties.
	/// </summary>
	public static class SerializedPropertyExtensions
	{
		/// <summary>
		/// 	Gets the parent.
		/// </summary>
		/// <returns>The parent.</returns>
		/// <param name="extends">Extends.</param>
		public static object GetParent(this SerializedProperty extends)
		{
			string path = extends.propertyPath.Replace(".Array.data[", "[");
			object obj = extends.serializedObject.targetObject;
			string[] elements = path.Split('.');

			for (int eIndex = 0; eIndex < elements.Length - 1; eIndex++)
			{
				string element = elements[eIndex];

				if (element.Contains("["))
				{
					string elementName = element.Substring(0, element.IndexOf("["));
					int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = ReflectionUtils.GetSourceFieldValue(obj, elementName, index);
				}
				else
					obj = ReflectionUtils.GetSourceFieldValue(obj, element);
			}

			return obj;
		}

		/// <summary>
		/// 	When increasing the array size of a serialized property, Unity will copy the
		/// 	last element for the new elements.
		/// 	
		/// 	This is useful for things like values, but we do NOT want it to copy object
		/// 	reference values.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="size">Size.</param>
		public static void SetArraySizeSafe(this SerializedProperty extends, int size)
		{
			int startSize = extends.arraySize;

			if (size == startSize)
				return;

			extends.arraySize = size;

			for (int index = startSize; index < size; index++)
			{
				SerializedProperty element = extends.GetArrayElementAtIndex(index);

				if (element.propertyType != SerializedPropertyType.ObjectReference)
					continue;

				if (element.objectReferenceValue is Object)
					element.objectReferenceValue = null;
			}
		}

		/// <summary>
		/// 	Gets the enum value.
		/// </summary>
		/// <returns>The enum value.</returns>
		/// <param name="extends">Extends.</param>
		/// <typeparam name="T">The enum type.</typeparam>
		public static T GetEnumValue<T>(this SerializedProperty extends)
		{
			return (T)Enum.ToObject(typeof(T), extends.enumValueIndex);
		}

		/// <summary>
		/// 	Sets the enum value.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The enum type.</typeparam>
		public static void SetEnumValue<T>(this SerializedProperty extends, T value)
		{
			extends.enumValueIndex = (int)Enum.ToObject(typeof(T), value);
		}

		/// <summary>
		/// 	In Unity5 you can't assign negative numbers to LayerMask properties.
		/// 	This is a problem since -1 is shorthand for "Everything".
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="value">Value.</param>
		public static void SetMask(this SerializedProperty extends, int value)
		{
			int oldValue = extends.intValue;
			value = (value == -1) ? int.MaxValue : value;

			extends.intValue = value;

			if (extends.intValue != value)
			{
				extends.intValue = oldValue;
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
