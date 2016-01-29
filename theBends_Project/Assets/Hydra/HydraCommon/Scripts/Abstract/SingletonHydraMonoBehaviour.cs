// <copyright file=SingletonHydraMonoBehaviour company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	Singleton hydra mono behaviour.
	/// </summary>
	public abstract class SingletonHydraMonoBehaviour<T> : HydraMonoBehaviour
		where T : SingletonHydraMonoBehaviour<T>
	{
		/// <summary>
		///     Single instance of singleton type.
		/// </summary>
		private static T s_Instance;

		/// <summary>
		///     Lock for multithreading.
		/// </summary>
		private static readonly object s_Lock = new object();

		/// <summary>
		///     Indicates whether application is quitting.
		/// </summary>
		private static bool s_ApplicationIsQuitting;

		/// <summary>
		///     Single instance.
		/// </summary>
		public static T instance { get { return Instantiate(); } }

		/// <summary>
		///     When Unity quits, it destroys objects in a random order.
		///     In principle, a Singleton is only destroyed when application quits.
		///     If any script calls Instance after it have been destroyed,
		///     it will create a buggy ghost object that will stay on the Editor scene
		///     even after stopping playing the Application. Really bad!
		///     So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();

			s_ApplicationIsQuitting = true;
		}

		/// <summary>
		/// 	Called when a level finishes loading.
		/// </summary>
		/// <param name="index">Index.</param>
		protected override void OnLevelWasLoaded(int index)
		{
			base.OnLevelWasLoaded(index);

			s_ApplicationIsQuitting = false;
		}

		/// <summary>
		/// 	Instantiate this object.
		/// </summary>
		public static T Instantiate()
		{
			if (s_ApplicationIsQuitting)
				return null;

			lock (s_Lock)
			{
				if (s_Instance != null)
					return s_Instance;

				s_Instance = (T)FindObjectOfType(typeof(T));

				if (FindObjectsOfType(typeof(T)).Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong " + " - there should never be more than 1 singleton!" +
								   " Reopening the scene might fix it.");
					return s_Instance;
				}

				if (s_Instance != null)
					return s_Instance;

				GameObject singleton = new GameObject();
				s_Instance = singleton.AddComponent<T>();
				singleton.name = typeof(T).Name + "(Singleton)";
				DontDestroyOnLoad(singleton);
			}

			return s_Instance;
		}
	}
}
