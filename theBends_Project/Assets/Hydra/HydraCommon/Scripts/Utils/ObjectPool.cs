// <copyright file=ObjectPool company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;
using Hydra.HydraCommon.API;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	ObjectPool provides a simple mechanism for recycling objects.
	/// </summary>
	public class ObjectPool<TRecyclable, TFactory>
		where TRecyclable : IRecyclable
		where TFactory : IFactory<TRecyclable>
	{
		private readonly TFactory m_Factory;

		private readonly Stack<TRecyclable> m_ObjectStack;

		private readonly Action<TRecyclable> m_ResetAction;
		private readonly Action<TRecyclable> m_OnetimeInitAction;

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the ObjectPool class.
		/// </summary>
		/// <param name="factory">Factory.</param>
		public ObjectPool(TFactory factory)
		{
			m_Factory = factory;
			m_ObjectStack = new Stack<TRecyclable>();
		}

		/// <summary>
		/// 	Initializes a new instance of the ObjectPool class.
		/// </summary>
		/// <param name="factory">Factory.</param>
		/// <param name="initialBufferSize">Initial buffer size.</param>
		public ObjectPool(TFactory factory, int initialBufferSize)
		{
			m_Factory = factory;
			m_ObjectStack = new Stack<TRecyclable>(initialBufferSize);
		}

		/// <summary>
		/// 	Initializes a new instance of the ObjectPool class.
		/// </summary>
		/// <param name="factory">Factory.</param>
		/// <param name="initialBufferSize">Initial buffer size.</param>
		/// <param name="resetAction">Reset action.</param>
		/// <param name="onetimeInitAction">Onetime init action.</param>
		public ObjectPool(TFactory factory, int initialBufferSize, Action<TRecyclable> resetAction,
						  Action<TRecyclable> onetimeInitAction)
		{
			m_Factory = factory;
			m_ObjectStack = new Stack<TRecyclable>(initialBufferSize);
			m_ResetAction = resetAction;
			m_OnetimeInitAction = onetimeInitAction;
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Either returns a stored item or creates a new item if nothing is stored.
		/// </summary>
		public TRecyclable New()
		{
			TRecyclable item = (m_ObjectStack.Count > 0) ? m_ObjectStack.Pop() : NewInstance();

			// Reset the item
			item.Reset();
			if (m_ResetAction != null)
				m_ResetAction(item);

			return item;
		}

		/// <summary>
		/// 	Stores an item for recycling.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Store(TRecyclable item)
		{
			m_ObjectStack.Push(item);
		}

		#endregion

		/// <summary>
		/// 	Creates a new instance of TRecyclable
		/// </summary>
		/// <returns>The instance.</returns>
		private TRecyclable NewInstance()
		{
			TRecyclable item = m_Factory.Create();

			if (m_OnetimeInitAction != null)
				m_OnetimeInitAction(item);

			return item;
		}
	}
}
