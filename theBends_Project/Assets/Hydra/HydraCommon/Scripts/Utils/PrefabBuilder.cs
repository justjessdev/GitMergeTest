// <copyright file=PrefabBuilder company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	PrefabBuilder simply handles the instantiation and destruction of a prefab.
	/// </summary>
	[Serializable]
	public class PrefabBuilder<T>
		where T : Component
	{
		[SerializeField] private T m_Prefab;
		private T m_Instance;

		#region Properties

		/// <summary>
		/// 	Gets or sets the prefab.
		/// </summary>
		/// <value>The prefab.</value>
		public T prefab { get { return m_Prefab; } set { m_Prefab = value; } }

		/// <summary>
		/// 	Gets the instantiated prefab.
		/// </summary>
		/// <value>The instantiated prefab.</value>
		public T instance { get { return m_Instance; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Instantiates the prefab.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <returns>The instantiated prefab.</returns>
		public T Instantiate(Transform parent)
		{
			return Instantiate(parent, parent.position, parent.rotation);
		}

		/// <summary>
		/// 	Instantiate the specified position, rotation and scale.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		public T Instantiate(Transform parent, Vector3 position, Quaternion rotation)
		{
			DestroyInstance();

			m_Instance = Object.Instantiate(m_Prefab, position, rotation) as T;
			m_Instance.transform.parent = parent;

			return m_Instance;
		}

		/// <summary>
		/// 	Destroys the instantiated prefab.
		/// </summary>
		/// <returns>Null.</returns>
		public T DestroyInstance()
		{
			if (Application.isEditor)
				Object.DestroyImmediate(m_Instance);
			else
				Object.Destroy(m_Instance);

			m_Instance = null;
			return m_Instance;
		}

		#endregion
	}
}
