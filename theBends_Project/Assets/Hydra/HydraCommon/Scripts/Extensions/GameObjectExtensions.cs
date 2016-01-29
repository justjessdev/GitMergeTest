// <copyright file=GameObjectExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	GameObjectExtensions provides extension methods for working with GameObjects.
	/// </summary>
	public static class GameObjectExtensions
	{
		private static readonly List<GameObject> s_TempChildrenRecursive;

		/// <summary>
		/// 	Initializes the GameObjectExtensions class.
		/// </summary>
		static GameObjectExtensions()
		{
			s_TempChildrenRecursive = new List<GameObject>();
		}

		/// <summary>
		/// 	Gets the immediate children, excluding disabled children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="children">Children.</param>
		public static int GetChildren(this GameObject extends, List<GameObject> children)
		{
			return extends.GetChildren(false, children);
		}

		/// <summary>
		/// 	Gets the immediate children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="children">Children.</param>
		public static int GetChildren(this GameObject extends, bool includeInactive, List<GameObject> children)
		{
			Transform transform = extends.transform;
			int output = 0;

			for (int index = 0; index < transform.childCount; index++)
			{
				GameObject child = transform.GetChild(index).gameObject;

				if (!includeInactive && !child.activeInHierarchy)
					continue;

				children.Add(child);
				output++;
			}

			return output;
		}

		/// <summary>
		/// 	Gets all children recursively, excluding disabled children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="children">Children.</param>
		public static int GetChildrenRecursive(this GameObject extends, List<GameObject> children)
		{
			return extends.GetChildrenRecursive(false, children);
		}

		/// <summary>
		/// 	Gets all inactive children recursively, excluding active children.
		/// </summary>
		/// <returns>The inactive children recursive.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="children">Children.</param>
		public static int GetInactiveChildrenRecursive(this GameObject extends, List<GameObject> children)
		{
			int output = 0;

			if (!extends.activeInHierarchy)
			{
				children.Add(extends);
				output++;
			}

			List<GameObject> immediateChildren = new List<GameObject>();
			extends.GetChildren(true, immediateChildren);

			for (int index = 0; index < immediateChildren.Count; index++)
				output += immediateChildren[index].GetInactiveChildrenRecursive(children);

			return output;
		}

		/// <summary>
		/// 	Gets all children recursively.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="children">Children.</param>
		public static int GetChildrenRecursive(this GameObject extends, bool includeInactive, List<GameObject> children)
		{
			int output = 0;

			int index = children.Count;
			int immediate = extends.GetChildren(includeInactive, children);

			for (int immediateIndex = 0; immediateIndex < immediate; immediateIndex++)
			{
				int childIndex = index + immediateIndex;
				GameObject child = children[childIndex];

				output += child.GetChildrenRecursive(includeInactive, children);
				output++;
			}

			return output;
		}

		/// <summary>
		/// 	Gets the components in children recursively, excluding disabled children/components.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponentsInChildrenRecursive<T>(this GameObject extends, List<T> components) where T : Component
		{
			return extends.GetComponentsInChildrenRecursive(false, components);
		}

		/// <summary>
		/// 	Gets the components in children recursively.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive children/components.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponentsInChildrenRecursive<T>(this GameObject extends, bool includeInactive,
															  List<T> components) where T : Component
		{
			int output = 0;

			s_TempChildrenRecursive.Clear();
			int children = extends.GetChildrenRecursive(includeInactive, s_TempChildrenRecursive);

			for (int index = 0; index < children; index++)
			{
				GameObject child = s_TempChildrenRecursive[index];
				output += child.GetComponents(includeInactive, components);
			}

			return output;
		}

		/// <summary>
		/// 	Gets the components.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponents<T>(this GameObject extends, bool includeInactive, List<T> components)
			where T : Component
		{
			int output = 0;

			T[] childComponents = extends.GetComponents<T>();

			for (int index = 0; index < childComponents.Length; index++)
			{
				T component = childComponents[index];

				// Enabled test
				if (!includeInactive && component is Behaviour)
				{
					if (!(component as Behaviour).enabled)
						continue;
				}

				components.Add(component);
				output++;
			}

			return output;
		}

		/// <summary>
		/// 	Recursively sets the layer of this GameObject and all children.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="layer">Layer.</param>
		public static void SetLayerRecursive(this GameObject extends, int layer)
		{
			extends.layer = layer;

			Transform transform = extends.transform;

			for (int index = 0; index < transform.childCount; index++)
				transform.GetChild(index).gameObject.SetLayerRecursive(layer);
		}
	}
}
