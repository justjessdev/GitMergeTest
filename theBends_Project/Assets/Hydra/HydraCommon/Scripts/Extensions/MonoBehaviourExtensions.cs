// <copyright file=MonoBehaviourExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	///     Represents extensions for mono behaviour
	/// </summary>
	public static class MonoBehaviourExtensions
	{
		#region Children

		/// <summary>
		/// 	Gets the immediate children, excluding disabled children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="children">Children.</param>
		public static int GetChildren(this MonoBehaviour extends, List<GameObject> children)
		{
			return extends.gameObject.GetChildren(children);
		}

		/// <summary>
		/// 	Gets the immediate children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="children">Children.</param>
		public static int GetChildren(this MonoBehaviour extends, bool includeInactive, List<GameObject> children)
		{
			return extends.gameObject.GetChildren(includeInactive, children);
		}

		/// <summary>
		/// 	Gets all children recursively, excluding disabled children.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="children">Children.</param>
		public static int GetChildrenRecursive(this MonoBehaviour extends, List<GameObject> children)
		{
			return extends.gameObject.GetChildrenRecursive(children);
		}

		/// <summary>
		/// 	Gets all children recursively.
		/// </summary>
		/// <returns>The number of children.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="children">Children.</param>
		public static int GetChildrenRecursive(this MonoBehaviour extends, bool includeInactive, List<GameObject> children)
		{
			return extends.gameObject.GetChildrenRecursive(includeInactive, children);
		}

		/// <summary>
		/// 	Gets the components in children recursively, excluding disabled children/components.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponentsInChildrenRecursive<T>(this MonoBehaviour extends, List<T> components)
			where T : Component
		{
			return extends.gameObject.GetComponentsInChildrenRecursive(components);
		}

		/// <summary>
		/// 	Gets the components in children recursively.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive children/components.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponentsInChildrenRecursive<T>(this MonoBehaviour extends, bool includeInactive,
															  List<T> components) where T : Component
		{
			return extends.gameObject.GetComponentsInChildrenRecursive(includeInactive, components);
		}

		/// <summary>
		/// 	Gets the components.
		/// </summary>
		/// <returns>The number of components.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static int GetComponents<T>(this MonoBehaviour extends, bool includeInactive, List<T> components)
			where T : Component
		{
			return extends.gameObject.GetComponents(includeInactive, components);
		}

		#endregion

		#region Coroutines

		/// <summary>
		///     Convenience method for calling StartCoroutine(string) without using
		///     hardcoded method names.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		public static void StartCoroutine(this MonoBehaviour extends, Func<IEnumerator> method)
		{
			string name = method.Method.Name;
			extends.StartCoroutine(name);
		}

		/// <summary>
		///     Convenience method for calling StartCoroutine(string, object) without using
		///     hardcoded method names.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The param type.</typeparam>
		public static Coroutine StartCoroutine<T>(this MonoBehaviour extends, Func<T, IEnumerator> method, T value)
		{
			string name = method.Method.Name;
			return extends.StartCoroutine(name, value);
		}

		/// <summary>
		///     Convenience method for calling StopCoroutine(string) without using
		///     hardcoded method names.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		public static void StopCoroutine(this MonoBehaviour extends, Func<IEnumerator> method)
		{
			string name = method.Method.Name;
			extends.StopCoroutine(name);
		}

		/// <summary>
		///     Convenience method for calling StopCoroutine(string) without using
		///     hardcoded method names on a method that takes a parameter.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		/// <typeparam name="T">The param type.</typeparam>
		public static void StopCoroutine<T>(this MonoBehaviour extends, Func<T, IEnumerator> method)
		{
			string name = method.Method.Name;
			extends.StopCoroutine(name);
		}

		/// <summary>
		///     Convenience method for stopping and starting the given coroutine method
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		public static void RestartCoroutine(this MonoBehaviour extends, Func<IEnumerator> method)
		{
			extends.StopCoroutine(method);
			extends.StartCoroutine(method);
		}

		/// <summary>
		///     Convenience method for stopping and starting the given coroutine method
		/// </summary>
		/// <returns>The coroutine by name.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="method">Method.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The param type.</typeparam>
		public static Coroutine RestartCoroutine<T>(this MonoBehaviour extends, Func<T, IEnumerator> method, T value)
		{
			extends.StopCoroutine(method);
			return extends.StartCoroutine(method, value);
		}

		#endregion
	}
}
