// <copyright file=ObjectUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	ObjectUtils provides utility methods for working with Unity Objects.
	/// </summary>
	public static class ObjectUtils
	{
		/// <summary>
		/// 	Instantiates the prefab if it doesn't already exist. This method will
		/// 	also search the scene for an instance before instantiating a new one.
		/// </summary>
		/// <returns><c>true</c>, if instantiated or found, <c>false</c> otherwise.</returns>
		/// <param name="prefab">Prefab.</param>
		/// <param name="instance">Instance.</param>
		/// <typeparam name="T">The prefab type.</typeparam>
		public static bool LazyInstantiateOrFind<T>(T prefab, ref T instance) where T : Object
		{
			if (instance != null)
				return false;

			instance = Object.FindObjectOfType<T>();
			if (instance != null)
				return true;

			return LazyInstantiate(prefab, ref instance);
		}

		/// <summary>
		/// 	Instantiates the prefab if it doesn't already exist.
		/// </summary>
		/// <returns><c>true</c>, if instantiated, <c>false</c> otherwise.</returns>
		/// <param name="prefab">Prefab.</param>
		/// <param name="instance">Instance.</param>
		/// <typeparam name="T">The prefab type.</typeparam>
		public static bool LazyInstantiate<T>(T prefab, ref T instance) where T : Object
		{
			if (instance != null)
				return false;

			instance = Instantiate(prefab);

			return true;
		}

		/// <summary>
		/// 	Instantiate the specified prefab.
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		/// <typeparam name="T">The prefab type.</typeparam>
		public static T Instantiate<T>(T prefab) where T : Object
		{
#if UNITY_EDITOR
			return PrefabUtility.InstantiatePrefab(prefab) as T;
#else
			return GameObject.Instantiate(prefab) as T;
			#endif
		}

		/// <summary>
		/// 	Uses DestroyImmediate if in editor, otherwise Destroy.
		/// </summary>
		/// <returns>Null.</returns>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The object type.</typeparam>
		public static T SafeDestroy<T>(T obj) where T : Object
		{
			return SafeDestroy(obj, false);
		}

		/// <summary>
		/// 	Uses DestroyImmediate if in editor and not waitForEndOfFrame, otherwise Destroy.
		/// </summary>
		/// <returns>Null.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="waitForEndOfFrame">If set to <c>true</c> wait for end of frame.</param>
		/// <typeparam name="T">The object type.</typeparam>
		public static T SafeDestroy<T>(T obj, bool waitForEndOfFrame) where T : Object
		{
			if (Application.isEditor && !waitForEndOfFrame)
				Object.DestroyImmediate(obj);
			else
				Object.Destroy(obj);

			return null;
		}

		/// <summary>
		/// 	Uses SafeDestroy to destroy the GameObject that the Component
		/// 	is attached to.
		/// </summary>
		/// <returns>Null.</returns>
		/// <param name="component">Component.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static T SafeDestroyGameObject<T>(T component) where T : Component
		{
			return SafeDestroyGameObject(component, false);
		}

		/// <summary>
		/// 	Uses SafeDestroy to destroy the GameObject that the Component
		/// 	is attached to.
		/// </summary>
		/// <returns>Null.</returns>
		/// <param name="component">Component.</param>
		/// <param name="waitForEndOfFrame">If set to <c>true</c> wait for end of frame.</param>
		/// <typeparam name="T">The component type.</typeparam>
		public static T SafeDestroyGameObject<T>(T component, bool waitForEndOfFrame) where T : Component
		{
			if (component != null)
				SafeDestroy(component.gameObject, waitForEndOfFrame);

			return null;
		}
	}
}
