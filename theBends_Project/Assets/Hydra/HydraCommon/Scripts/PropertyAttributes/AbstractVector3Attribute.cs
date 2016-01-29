// <copyright file=AbstractVector3Attribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	public abstract class AbstractVector3Attribute : HydraPropertyAttribute
	{
		[SerializeField] private bool m_Locked;

		#region Properties

		/// <summary>
		/// 	Gets or sets a value indicating whether all curves move together.
		/// </summary>
		/// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
		public bool locked
		{
			get { return m_Locked; }
			set
			{
				if (value == m_Locked)
					return;

				OnLockedChanged();

				m_Locked = value;
			}
		}

		#endregion

		/// <summary>
		/// 	Called when the locked state is changed.
		/// </summary>
		protected abstract void OnLockedChanged();
	}
}
