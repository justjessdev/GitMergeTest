// <copyright file=RectUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Provides util methods for working with Rects
	/// </summary>
	public static class RectUtils
	{
		/// <summary>
		/// 	Builds a Rect from two points.
		/// </summary>
		/// <returns>The rect.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static Rect MinMaxRect(Vector2 min, Vector2 max)
		{
			return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		}
	}
}
