// <copyright file=AxisInputAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.InputAttributes
{
	/// <summary>
	/// 	Axis Input attribute.
	/// </summary>
	[Serializable]
	public class AxisInputAttribute : AbstractAxesInputAttribute
	{
		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="Hydra.HydraCommon.PropertyAttributes.InputAttributes.AxisInputAttribute"/> class.
		/// </summary>
		/// <param name="input">Input.</param>
		public AxisInputAttribute(string input) : base(input) {}

		/// <summary>
		/// 	The value will be in the range -1 - 1 for keyboard and joystick input.
		/// 	If the axis is setup to be delta mouse movement, the mouse delta is multiplied
		/// 	by the axis sensitivity and the range is not -1 - 1.
		/// </summary>
		/// <returns>The axis.</returns>
		public float GetAxis()
		{
			return Input.GetAxis(input);
		}

		/// <summary>
		/// 	The same as GetAxis, but without any smoothing.
		/// </summary>
		/// <returns>The raw axis.</returns>
		public float GetAxisRaw()
		{
			return Input.GetAxisRaw(input);
		}
	}
}
