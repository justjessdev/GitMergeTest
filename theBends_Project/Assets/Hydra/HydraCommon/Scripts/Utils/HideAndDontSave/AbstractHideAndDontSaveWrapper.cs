// <copyright file=AbstractHideAndDontSaveWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.HideAndDontSave
{
	/// <summary>
	/// 	AbstractHideAndDontSaveWrapper handles the instantiation and destruction of a "child"
	/// 	GameObject without maintaining a strict asset hierarchy. HideAndDontSave GameObjects will
	/// 	cause errors on serialization if they have parents.
	/// </summary>
	public abstract class AbstractHideAndDontSaveWrapper : HydraMonoBehaviourChild
	{
		private GameObject m_Child;

		#region Messages

		/// <summary>
		/// 	Called when the parent updates.
		/// </summary>
		/// <param name="parent">Parent.</param>
		protected override void OnUpdate(HydraMonoBehaviour parent)
		{
			base.OnUpdate(parent);

			SetLayer(parent.gameObject.layer);
		}

		/// <summary>
		/// 	Called when the object is loaded.
		/// </summary>
		protected override void OnEnable(HydraMonoBehaviour parent)
		{
			base.OnEnable(parent);

			InstantiateChild();
		}

		/// <summary>
		/// 	Called when the object goes out of scope.
		/// </summary>
		protected override void OnDisable(HydraMonoBehaviour parent)
		{
			base.OnDisable(parent);

			DestroyChild();
		}

		/// <summary>
		/// 	Called when the object is about to be destroyed.
		/// </summary>
		protected override void OnDestroy(HydraMonoBehaviour parent)
		{
			base.OnDestroy(parent);

			DestroyChild();
		}

		#endregion

		/// <summary>
		/// 	Instantiates the child.
		/// </summary>
		private void InstantiateChild()
		{
			DestroyChild();

			m_Child = new GameObject();
			m_Child.name = string.Format("{0}-Child", GetType().Name);

			m_Child.hideFlags = HideFlags.HideAndDontSave;

#if UNITY_EDITOR && false
			m_Child.hideFlags = HideFlags.DontSave;
#endif

			ConfigureChild(m_Child);
		}

		/// <summary>
		/// 	Sets the layer.
		/// </summary>
		/// <param name="layer">Layer.</param>
		private void SetLayer(int layer)
		{
			if (m_Child == null)
				return;

			m_Child.layer = layer;
		}

		/// <summary>
		/// 	Override this method to configure the child upon instantiation, e.g. to add components.
		/// </summary>
		/// <param name="child">Child.</param>
		protected virtual void ConfigureChild(GameObject child) {}

		/// <summary>
		/// 	Destroys the child.
		/// </summary>
		/// <returns>The child.</returns>
		protected virtual void DestroyChild()
		{
			m_Child = ObjectUtils.SafeDestroy(m_Child);
		}
	}
}
