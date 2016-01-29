// <copyright file=TransformExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	TransformExtensions provides extension methods for working with Transforms.
	/// </summary>
	public static class TransformExtensions
	{
		/// <summary>
		/// 	Searches the transform recursively for an object with the given name.
		/// </summary>
		/// <returns>The transform.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="name">Name.</param>
		public static Transform FindRecursive(this Transform extends, string name)
		{
			if (extends.name == name)
				return extends;

			for (int index = 0; index < extends.childCount; index++)
			{
				Transform child = extends.GetChild(index).FindRecursive(name);
				if (child != null)
					return child;
			}

			return null;
		}

		/// <summary>
		/// 	Searches the transform recursively for components that match the given type.
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="extends">Extends.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void FindAllRecursive<T>(this Transform extends, List<T> output) where T : Component
		{
			T component = extends.GetComponent<T>();
			if (component != null)
				output.Add(component);

			for (int index = 0; index < extends.childCount; index++)
				extends.GetChild(index).FindAllRecursive<T>(output);
		}

		/// <summary>
		/// 	Copies the position, rotation and local scale of the given object.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static void Copy(this Transform extends, Transform other)
		{
			int index = other.GetSiblingIndex();

			extends.parent = other.parent;
			extends.SetSiblingIndex(index);

			extends.position = other.position;
			extends.rotation = other.rotation;
			extends.localScale = other.localScale;
		}

		/// <summary>
		/// 	Copies the position, rotation and local scale of the given object.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static void Copy(this Transform extends, GameObject other)
		{
			extends.Copy(other.transform);
		}

		/// <summary>
		/// 	Copies the position, rotation and local scale of the given object.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static void Copy(this Transform extends, Component other)
		{
			extends.Copy(other.transform);
		}
	}
}
