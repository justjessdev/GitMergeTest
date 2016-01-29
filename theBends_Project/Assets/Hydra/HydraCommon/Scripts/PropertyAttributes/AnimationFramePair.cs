// <copyright file=AnimationFramePair company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	AnimationFramePair simply holds inclusive start and end frames for splitting
	/// 	animations.
	/// </summary>
	[Serializable]
	public class AnimationFramePair : HydraPropertyAttribute
	{
		[SerializeField] private string m_Name;

		[SerializeField] private int m_StartFrame;
		[SerializeField] private int m_EndFrame;

		#region Properties

		/// <summary>
		/// 	Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string name { get { return m_Name; } set { m_Name = value; } }

		/// <summary>
		/// 	Gets or sets the start frame.
		/// </summary>
		/// <value>The start frame.</value>
		public int startFrame { get { return m_StartFrame; } set { m_StartFrame = value; } }

		/// <summary>
		/// 	Gets or sets the end frame.
		/// </summary>
		/// <value>The end frame.</value>
		public int endFrame { get { return m_EndFrame; } set { m_EndFrame = value; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="AnimationFramePair"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="startFrame">Start frame.</param>
		/// <param name="endFrame">End frame.</param>
		public AnimationFramePair(string name, int startFrame, int endFrame)
		{
			m_Name = name;

			m_StartFrame = startFrame;
			m_EndFrame = endFrame;
		}
	}
}
