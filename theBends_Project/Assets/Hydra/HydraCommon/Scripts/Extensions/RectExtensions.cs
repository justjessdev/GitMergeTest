// <copyright file=RectExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	RectExtensions provides extension methods for working with rects.
	/// </summary>
	public static class RectExtensions
	{
		/// <summary>
		/// 	Flips the rect such that width and height are both positive.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static Rect Abs(this Rect extends)
		{
			if (extends.width < 0.0f)
			{
				extends.x += extends.width;
				extends.width *= -1.0f;
			}

			if (extends.height < 0.0f)
			{
				extends.y += extends.height;
				extends.height *= -1.0f;
			}

			return extends;
		}

		/// <summary>
		/// 	Clamps the other Rect to this one.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static Rect Clamp(this Rect extends, Rect other)
		{
			Vector2 min = extends.Clamp(other.min);
			Vector2 max = extends.Clamp(other.max);

			return RectUtils.MinMaxRect(min, max);
		}

		/// <summary>
		/// 	Clamp the point to the rect.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="point">Point.</param>
		public static Vector2 Clamp(this Rect extends, Vector2 point)
		{
			extends = extends.Abs();

			float x = HydraMathUtils.Clamp(point.x, extends.xMin, extends.xMax);
			float y = HydraMathUtils.Clamp(point.y, extends.yMin, extends.yMax);

			return new Vector2(x, y);
		}

		/// <summary>
		/// 	Extend the size of the rect to overlap the other rect.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="other">Other.</param>
		public static Rect Encapsulate(this Rect extends, Rect other)
		{
			return extends.Encapsulate(other.min, other.max);
		}

		/// <summary>
		/// 	Extend the size of the rect to overlap the given points.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		public static Rect Encapsulate(this Rect extends, Vector2 pointA, Vector2 pointB)
		{
			Rect output = extends.Encapsulate(pointA);
			return output.Encapsulate(pointB);
		}

		/// <summary>
		/// 	Extend the size of the rect to overlap the given point.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="point">Point.</param>
		public static Rect Encapsulate(this Rect extends, Vector2 point)
		{
			extends.xMin = (point.x < extends.xMin) ? point.x : extends.xMin;
			extends.xMax = (point.x > extends.xMax) ? point.x : extends.xMax;
			extends.yMin = (point.y < extends.yMin) ? point.y : extends.yMin;
			extends.yMax = (point.y > extends.yMax) ? point.y : extends.yMax;

			return extends;
		}

		/// <summary>
		/// 	Gets the top left.
		/// </summary>
		/// <returns>The top left.</returns>
		/// <param name="extends">Extends.</param>
		public static Vector2 GetTopLeft(this Rect extends)
		{
			return new Vector2(extends.xMin, extends.yMin);
		}

		/// <summary>
		/// 	Gets the top right.
		/// </summary>
		/// <returns>The top right.</returns>
		/// <param name="extends">Extends.</param>
		public static Vector2 GetTopRight(this Rect extends)
		{
			return new Vector2(extends.xMax, extends.yMin);
		}

		/// <summary>
		/// 	Gets the bottom left.
		/// </summary>
		/// <returns>The bottom left.</returns>
		/// <param name="extends">Extends.</param>
		public static Vector2 GetBottomLeft(this Rect extends)
		{
			return new Vector2(extends.xMin, extends.yMax);
		}

		/// <summary>
		/// 	Gets the bottom right.
		/// </summary>
		/// <returns>The bottom right.</returns>
		/// <param name="extends">Extends.</param>
		public static Vector2 GetBottomRight(this Rect extends)
		{
			return new Vector2(extends.xMax, extends.yMax);
		}
	}
}
