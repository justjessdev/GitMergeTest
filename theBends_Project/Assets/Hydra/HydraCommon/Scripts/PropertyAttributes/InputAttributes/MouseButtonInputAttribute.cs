// <copyright file=MouseButtonInputAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.InputAttributes
{
	/// <summary>
	/// 	Mouse button input attribute.
	/// </summary>
	[Serializable]
	public class MouseButtonInputAttribute : AbstractInputAttribute
	{
		public enum Buttons
		{
			Left = 0,
			Right = 1,
			Middle = 2
		}

		[SerializeField] private Buttons m_Input;

		#region Properties

		/// <summary>
		/// 	Gets or sets the button.
		/// </summary>
		/// <value>The button.</value>
		public Buttons input { get { return m_Input; } set { m_Input = value; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Returns whether the given mouse button is held down.
		/// </summary>
		/// <returns><c>true</c>, if button is held down, <c>false</c> otherwise.</returns>
		public bool GetButton()
		{
			return Input.GetMouseButton((int)m_Input);
		}

		/// <summary>
		/// 	Returns true during the frame the user pressed the given mouse button.
		/// </summary>
		/// <returns><c>true</c>, if button is pressed, <c>false</c> otherwise.</returns>
		public bool GetButtonDown()
		{
			return Input.GetMouseButtonDown((int)m_Input);
		}

		/// <summary>
		/// 	Returns true during the frame the user releases the given mouse button.
		/// </summary>
		/// <returns><c>true</c>, if button is released, <c>false</c> otherwise.</returns>
		public bool GetButtonUp()
		{
			return Input.GetMouseButtonUp((int)m_Input);
		}

		#endregion
	}
}
