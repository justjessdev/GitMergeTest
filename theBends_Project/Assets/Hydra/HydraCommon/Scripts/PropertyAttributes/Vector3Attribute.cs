// <copyright file=Vector3Attribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	[Serializable]
	public class Vector3Attribute : AbstractVector3Attribute
	{
		[SerializeField] private Vector3 m_Vector;

		#region Properties

		/// <summary>
		/// 	Gets or sets the vector.
		/// </summary>
		/// <value>The vector.</value>
		public Vector3 vector
		{
			get { return m_Vector; }
			set
			{
				if (locked)
					value = Vector3.one * value.x;
				m_Vector = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Copies the instance values and assigns them to the target.
		/// </summary>
		/// <param name="target">Target.</param>
		public void DeepCopyInto(Vector3Attribute target)
		{
			target.locked = locked;
			target.vector = m_Vector;
		}

		#endregion

		/// <summary>
		/// 	Called when the locked state is changed.
		/// </summary>
		protected override void OnLockedChanged()
		{
			if (locked)
				m_Vector = Vector3.one * m_Vector.x;
		}
	}
}
