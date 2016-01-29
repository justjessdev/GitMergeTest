// <copyright file=SceneAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	SceneAttribute stores the reference to a scene.
	/// </summary>
	[Serializable]
	public class SceneAttribute : HydraPropertyAttribute
	{
		[SerializeField] private string m_Guid;
		[SerializeField] private int m_Index;
		[SerializeField] private string m_Name;

		#region Properties

		/// <summary>
		/// 	Gets the index.
		/// </summary>
		/// <value>The index.</value>
		public int index { get { return m_Index; } }

		/// <summary>
		/// 	Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string name { get { return m_Name; } }

		#endregion
	}
}
