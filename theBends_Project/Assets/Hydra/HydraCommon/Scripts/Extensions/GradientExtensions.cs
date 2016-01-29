// <copyright file=GradientExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	GradientExtensions provides extension methods for working with Gradients.
	/// </summary>
	public static class GradientExtensions
	{
		public enum GradientWrapMode
		{
			Clamp,
			Loop,
			PingPong
		}

		/// <summary>
		/// 	Sets all of the keys on the gradient to the given color.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="color">Color.</param>
		public static void SetValue(this Gradient extends, Color color)
		{
			for (int index = 0; index < extends.colorKeys.Length; index++)
				extends.colorKeys[index].color = color;

			for (int index = 0; index < extends.alphaKeys.Length; index++)
				extends.alphaKeys[index].alpha = color.a;
		}

		/// <summary>
		/// 	Returns the color at the specified time, using the given wrap mode.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="time">Time.</param>
		/// <param name="wrapMode">Wrap mode.</param>
		public static Color Evaluate(this Gradient extends, float time, GradientWrapMode wrapMode)
		{
			switch (wrapMode)
			{
				case GradientWrapMode.Clamp:
					return extends.Evaluate(time);

				case GradientWrapMode.Loop:
					return extends.Evaluate(Mathf.Repeat(time, 1.0f));

				case GradientWrapMode.PingPong:
					return extends.Evaluate(HydraMathUtils.PingPong(time, 1.0f));

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
