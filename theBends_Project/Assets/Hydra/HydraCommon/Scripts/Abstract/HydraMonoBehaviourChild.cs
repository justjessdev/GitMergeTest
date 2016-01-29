// <copyright file=HydraMonoBehaviourChild company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	HydraMonoBehaviourChild provides methods similar to ScriptableObject.
	/// </summary>
	public class HydraMonoBehaviourChild
	{
		private List<HydraMonoBehaviourChild> m_MonoBehaviourChildren;

		public enum Message
		{
			OnUpdate,
			OnEnable,
			OnDisable,
			OnDestroy,
			OnReset
		}

		#region Methods

		/// <summary>
		/// 	Calls OnUpdate().
		/// </summary>
		/// <param name="parent">Parent.</param>
		public void Update(HydraMonoBehaviour parent)
		{
			OnUpdate(parent);
		}

		/// <summary>
		/// 	Calls OnReset().
		/// </summary>
		/// <param name="parent">Parent.</param>
		public void Reset(HydraMonoBehaviour parent)
		{
			OnReset(parent);
		}

		/// <summary>
		/// 	Calls OnEnable().
		/// </summary>
		/// <param name="parent">Parent.</param>
		public void Enable(HydraMonoBehaviour parent)
		{
			OnEnable(parent);
		}

		/// <summary>
		/// 	Calls OnDisable().
		/// </summary>
		/// <param name="parent">Parent.</param>
		public void Disable(HydraMonoBehaviour parent)
		{
			OnDisable(parent);
		}

		/// <summary>
		/// 	Calls OnDestroy().
		/// </summary>
		/// <param name="parent">Parent.</param>
		public void Destroy(HydraMonoBehaviour parent)
		{
			OnDestroy(parent);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// 	Called when the parent updates.
		/// </summary>
		protected virtual void OnUpdate(HydraMonoBehaviour parent)
		{
			ProcessMessage(parent, Message.OnUpdate);
		}

		/// <summary>
		/// 	Called to reset the instance to default values.
		/// </summary>
		protected virtual void OnReset(HydraMonoBehaviour parent)
		{
			ProcessMessage(parent, Message.OnReset);
		}

		/// <summary>
		/// 	Called when the parent is about to be destroyed.
		/// </summary>
		protected virtual void OnDestroy(HydraMonoBehaviour parent)
		{
			ProcessMessage(parent, Message.OnDestroy);
		}

		/// <summary>
		/// 	Called when the parent is enabled.
		/// </summary>
		protected virtual void OnEnable(HydraMonoBehaviour parent)
		{
			ProcessMessage(parent, Message.OnEnable);
		}

		/// <summary>
		/// 	Called when the parent is disabled.
		/// </summary>
		protected virtual void OnDisable(HydraMonoBehaviour parent)
		{
			ProcessMessage(parent, Message.OnDisable);
		}

		/// <summary>
		/// 	Override this method to return any HydraMonoBehaviourChild instances.
		/// </summary>
		/// <returns>The mono behaviour children.</returns>
		protected virtual List<HydraMonoBehaviourChild> GetMonoBehaviourChildren()
		{
			if (m_MonoBehaviourChildren == null)
				m_MonoBehaviourChildren = new List<HydraMonoBehaviourChild>();

			m_MonoBehaviourChildren.Clear();

			return m_MonoBehaviourChildren;
		}

		/// <summary>
		/// 	Processes the message.
		/// </summary>
		/// <param name="message">Message.</param>
		private void ProcessMessage(HydraMonoBehaviour parent, Message message)
		{
			ProcessMessage(GetMonoBehaviourChildren(), parent, message);
		}

		#endregion

		/// <summary>
		/// 	Creates the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="parent">Parent.</param>
		/// <typeparam name="T">The instance type.</typeparam>
		public static T CreateInstance<T>(HydraMonoBehaviour parent) where T : HydraMonoBehaviourChild, new()
		{
			T output = new T();
			output.Enable(parent);

			return output;
		}

		/// <summary>
		/// 	Processes the message.
		/// </summary>
		/// <param name="children">Children.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="message">Message.</param>
		public static void ProcessMessage(List<HydraMonoBehaviourChild> children, HydraMonoBehaviour parent, Message message)
		{
			for (int index = 0; index < children.Count; index++)
			{
				HydraMonoBehaviourChild child = children[index];

				if (child == null)
					continue;

				switch (message)
				{
					case Message.OnUpdate:
						child.Update(parent);
						break;
					case Message.OnDestroy:
						child.Destroy(parent);
						break;
					case Message.OnDisable:
						child.Disable(parent);
						break;
					case Message.OnEnable:
						child.Enable(parent);
						break;
					case Message.OnReset:
						child.Reset(parent);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
