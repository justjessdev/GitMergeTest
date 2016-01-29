// <copyright file=ButtonInputAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.InputAttributes
{
	/// <summary>
	/// 	Button Input attribute.
	/// </summary>
	[Serializable]
	public class ButtonInputAttribute : AbstractAxesInputAttribute
	{
		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="Hydra.HydraCommon.PropertyAttributes.InputAttributes.ButtonInputAttribute"/> class.
		/// </summary>
		/// <param name="input">Input.</param>
		public ButtonInputAttribute(string input) : base(input) {}

		#region Methods

		/// <summary>
		/// 	Returns whether the given button is held down.
		/// </summary>
		/// <returns><c>true</c>, if button is held down, <c>false</c> otherwise.</returns>
		public bool GetButton()
		{
			return Input.GetButton(input);
		}

		/// <summary>
		/// 	Returns true during the frame the user pressed the given button.
		/// </summary>
		/// <returns><c>true</c>, if button is pressed, <c>false</c> otherwise.</returns>
		public bool GetButtonDown()
		{
			return Input.GetButtonDown(input);
		}

		/// <summary>
		/// 	Returns true during the frame the user releases the given button.
		/// </summary>
		/// <returns><c>true</c>, if button is released, <c>false</c> otherwise.</returns>
		public bool GetButtonUp()
		{
			return Input.GetButtonUp(input);
		}

		#endregion
	}
}
