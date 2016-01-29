// <copyright file=ReflectionUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Reflection utility methods.
	/// </summary>
	public static class ReflectionUtils
	{
		/// <summary>
		/// 	Gets the subclasses of the given type.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="type">Type.</param>
		public static void GetSubclasses(List<Type> output, Type type)
		{
			Type[] assemblyTypes = Assembly.GetAssembly(type).GetTypes();

			for (int index = 0; index < assemblyTypes.Length; index++)
			{
				Type subType = assemblyTypes[index];
				if (subType != type && type.IsAssignableFrom(subType))
					output.Add(subType);
			}
		}

		/// <summary>
		/// 	Gets all items flags.
		/// </summary>
		/// <returns>The all items flags.</returns>
		public static BindingFlags GetAllItemsFlags()
		{
			return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance |
				   BindingFlags.DeclaredOnly;
		}

		/// <summary>
		/// 	Gets all fields in a given type.
		/// </summary>
		/// <returns>The fields.</returns>
		/// <param name="type">Object type.</param>
		public static FieldInfo[] GetAllFields(Type type)
		{
			if (type == null)
				return new FieldInfo[0];

			BindingFlags flags = GetAllItemsFlags();

			FieldInfo[] fields = type.GetFields(flags);
			FieldInfo[] baseFields = GetAllFields(type.BaseType);

			ArrayUtils.AddRange(ref fields, baseFields);

			return fields;
		}

		/// <summary>
		/// 	Gets all properties in a given type.
		/// </summary>
		/// <returns>The properties.</returns>
		/// <param name="type">Object type.</param>
		public static PropertyInfo[] GetAllProperties(Type type)
		{
			if (type == null)
				return new PropertyInfo[0];

			BindingFlags flags = GetAllItemsFlags();

			PropertyInfo[] properties = type.GetProperties(flags);
			PropertyInfo[] baseProperties = GetAllProperties(type.BaseType);

			ArrayUtils.AddRange(ref properties, baseProperties);

			return properties;
		}

		/// <summary>
		/// 	Gets all methods in a given type.
		/// </summary>
		/// <returns>The methods.</returns>
		/// <param name="type">Type.</param>
		public static MethodInfo[] GetAllMethods(Type type)
		{
			if (type == null)
				return new MethodInfo[0];

			BindingFlags flags = GetAllItemsFlags();

			MethodInfo[] methods = type.GetMethods(flags);
			MethodInfo[] baseMethods = GetAllMethods(type.BaseType);

			ArrayUtils.AddRange(ref methods, baseMethods);

			return methods;
		}

		/// <summary>
		/// 	Gets all constructors.
		/// </summary>
		/// <returns>The constructors.</returns>
		/// <param name="type">Type.</param>
		public static ConstructorInfo[] GetAllConstructors(Type type)
		{
			if (type == null)
				return new ConstructorInfo[0];

			BindingFlags flags = GetAllItemsFlags();

			return type.GetConstructors(flags);
		}

		/// <summary>
		/// 	Gets the type with the given name.
		/// </summary>
		/// <returns>The type.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="name">Name.</param>
		public static Type GetTypeByName(Assembly assembly, string name)
		{
			Type[] types = assembly.GetTypes();

			for (int index = 0; index < types.Length; index++)
			{
				Type type = types[index];
				if (type.Name == name)
					return type;
			}

			return null;
		}

		/// <summary>
		/// 	Returns the field with the matching name.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="objectType">Object type.</param>
		/// <param name="fieldName">Field name.</param>
		public static FieldInfo GetFieldByName(Type objectType, string fieldName)
		{
			FieldInfo[] fields = GetAllFields(objectType);

			for (int index = 0; index < fields.Length; index++)
			{
				FieldInfo field = fields[index];
				if (field.Name == fieldName)
					return field;
			}

			return null;
		}

		/// <summary>
		/// 	Returns the property with the matching name.
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="objectType">Object type.</param>
		/// <param name="propertyName">Property name.</param>
		public static PropertyInfo GetPropertyByName(Type objectType, string propertyName)
		{
			PropertyInfo[] properties = GetAllProperties(objectType);

			for (int index = 0; index < properties.Length; index++)
			{
				PropertyInfo property = properties[index];
				if (property.Name == propertyName)
					return property;
			}

			return null;
		}

		/// <summary>
		/// 	Returns the method with the matching name.
		/// </summary>
		/// <returns>The method.</returns>
		/// <param name="objectType">Object type.</param>
		/// <param name="methodName">Method name.</param>
		public static MethodInfo GetMethodByName(Type objectType, string methodName)
		{
			MethodInfo[] methods = GetAllMethods(objectType);

			for (int index = 0; index < methods.Length; index++)
			{
				MethodInfo method = methods[index];
				if (method.Name == methodName)
					return method;
			}

			return null;
		}

		/// <summary>
		/// 	Gets the source field value.
		/// </summary>
		/// <returns>The source field value.</returns>
		/// <param name="source">Source.</param>
		/// <param name="fieldName">Field name.</param>
		public static object GetSourceFieldValue(object source, string fieldName)
		{
			if (source == null)
				return null;

			Type type = source.GetType();
			FieldInfo f = GetFieldByName(type, fieldName);

			if (f == null)
				return null;

			return f.GetValue(source);
		}

		/// <summary>
		/// 	Gets the source field value.
		/// </summary>
		/// <returns>The source field value.</returns>
		/// <param name="source">Source.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="index">Index.</param>
		public static object GetSourceFieldValue(object source, string fieldName, int index)
		{
			object sourceFieldValue = GetSourceFieldValue(source, fieldName);

			if (sourceFieldValue is Array)
			{
				object[] array = sourceFieldValue as object[];
				return array[index];
			}

			throw new FieldAccessException();
		}

		/// <summary>
		/// 	Returns true if the toCheck type is a subclass of the generic type.
		/// </summary>
		/// <returns>Returns true if the toCheck type is a subclass of the generic type.</returns>
		/// <param name="generic">Generic.</param>
		/// <param name="toCheck">To check.</param>
		public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				Type current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == current)
					return true;

				toCheck = toCheck.BaseType;
			}

			return false;
		}
	}
}
