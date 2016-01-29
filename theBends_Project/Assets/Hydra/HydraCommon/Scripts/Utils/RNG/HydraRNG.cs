// <copyright file=HydraRNG company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.RNG
{
	/// <summary>
	/// 	HydraRNG is an alternative to UnityEngine.Random to
	/// 	avoid sharing seeds across the application.
	/// </summary>
	[Serializable]
	public class HydraRNG
	{
		[SerializeField] private SimpleRNG m_RNG;
		[SerializeField] private readonly uint m_PrimarySeed;

		/// <summary>
		/// 	Lazy loads the random number generator.
		/// </summary>
		/// <value>The random number generator.</value>
		private SimpleRNG lazyRng { get { return m_RNG ?? (m_RNG = new SimpleRNG()); } }

		#region Properties

		/// <summary>
		/// 	Gets a random value between 0.0f and 1.0f.
		/// </summary>
		/// <value>The value.</value>
		public float value { get { return (float)lazyRng.GetUniform(); } }

		/// <summary>
		/// 	Returns a position on the circumference of a unit circle.
		/// </summary>
		/// <value>The position on unit circle.</value>
		public Vector2 onUnitCircle
		{
			get
			{
				float angle = value * 2.0f * Mathf.PI;
				return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			}
		}

		/// <summary>
		/// 	Returns a position inside of a unit circle.
		/// </summary>
		/// <value>The position in unit circle.</value>
		public Vector2 insideUnitCircle { get { return onUnitCircle * value; } }

		/// <summary>
		/// 	Returns a random position on a unit sphere.
		/// </summary>
		/// <value>The position on unit sphere.</value>
		public Vector3 onUnitSphere
		{
			get
			{
				float u = value * 2.0f * Mathf.PI;
				float v = Mathf.Acos(2.0f * value - 1.0f);

				float sinV = Mathf.Sin(v);

				float x = Mathf.Cos(u) * sinV;
				float y = Mathf.Sin(u) * sinV;
				float z = Mathf.Cos(v);

				return new Vector3(x, y, z);
			}
		}

		/// <summary>
		/// 	Gets a random position inside a unit sphere.
		/// </summary>
		/// <value>The position inside unit sphere.</value>
		public Vector3 insideUnitSphere { get { return onUnitSphere * value; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Hydra.HydraCommon.Utils.RNG.HydraRNG"/> class.
		/// </summary>
		public HydraRNG()
		{
			m_PrimarySeed = CycleToUint(GetHashCode());
		}

		#region Methods

		/// <summary>
		/// 	Sets the seed.
		/// </summary>
		/// <param name="seed">Seed.</param>
		public void SetSeed(uint seed)
		{
			lazyRng.SetSeed(m_PrimarySeed, seed);
		}

		/// <summary>
		/// 	Generates a random seed.
		/// </summary>
		/// <returns>The random seed.</returns>
		public uint GetRandomSeed()
		{
			int random = Range(1, int.MaxValue);
			return CycleToUint(random);
		}

		/// <summary>
		/// 	Returns an int in the min-max range, min inclusive.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public int Range(int min, int max)
		{
			float range = Range((float)min, (float)max);

			return HydraMathUtils.FloorToInt(range);
		}

		/// <summary>
		/// 	Returns a float in the range min-max.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public float Range(float min, float max)
		{
			return HydraMathUtils.MapRange(0.0f, 1.0f, min, max, value);
		}

		/// <summary>
		/// 	Returns a Vector3 in the range min-max.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public Vector3 Range(Vector3 min, Vector3 max)
		{
			return Range(min, max, true);
		}

		/// <summary>
		/// 	Returns a Vector3 in the range min-max.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		/// <param name="linear">If set to <c>true</c> linear.</param>
		public Vector3 Range(Vector3 min, Vector3 max, bool linear)
		{
			if (linear)
				return Vector3.Lerp(min, max, value);

			return new Vector3(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
		}

		/// <summary>
		/// 	Returns a random item from from the collection.
		/// </summary>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The contents type.</typeparam>
		public T Range<T>(T[] collection)
		{
			return collection[Range(0, collection.Length)];
		}

		/// <summary>
		/// 	Returns a random color in the specified range.
		/// </summary>
		/// <returns>The random color in range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public Color Range(Color min, Color max)
		{
			return Range(min, max, true);
		}

		/// <summary>
		/// 	Returns a random color in the specified range.
		/// </summary>
		/// <returns>The random color in range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		/// <param name="linear">If set to <c>true</c> linear.</param>
		public Color Range(Color min, Color max, bool linear)
		{
			if (linear)
				return Color.Lerp(min, max, value);

			return new Color(Range(min.r, max.r), Range(min.g, max.g), Range(min.b, max.b), Range(min.a, max.a));
		}

		/// <summary>
		/// 	Returns a random color in the specified range, using HSL for blending.
		/// </summary>
		/// <returns>The random color in hsl range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public Color RangeHsl(Color min, Color max)
		{
			return RangeHsl(min, max, true);
		}

		/// <summary>
		/// 	Returns a random color in the specified range, using HSL for blending.
		/// </summary>
		/// <returns>The random color in hsl range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		/// <param name="linear">If set to <c>true</c> linear.</param>
		public Color RangeHsl(Color min, Color max, bool linear)
		{
			Vector4 hslA = ColorUtils.RgbToHsl(min);
			Vector4 hslB = ColorUtils.RgbToHsl(max);

			if (linear)
			{
				Vector4 lerp = Vector4.Lerp(hslA, hslB, value);
				return ColorUtils.HslToRgb(lerp);
			}

			Vector4 random = new Vector4(Range(hslA.x, hslB.x), Range(hslA.y, hslB.y), Range(hslA.z, hslB.z),
										 Range(hslA.w, hslB.w));

			return ColorUtils.HslToRgb(random);
		}

		/// <summary>
		/// 	Returns a random point in the given triangle.
		/// </summary>
		/// <returns>The random point.</returns>
		/// <param name="point1">Point1.</param>
		/// <param name="point2">Point2.</param>
		/// <param name="point3">Point3.</param>
		public Vector3 PointInTriangle(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			return PointInTriangle(new Triangle3d(point1, point2, point3));
		}

		/// <summary>
		/// 	Returns a random point in the given triangle.
		/// </summary>
		/// <returns>The random point.</returns>
		/// <param name="triangle">Triangle.</param>
		public Vector3 PointInTriangle(Triangle3d triangle)
		{
			Vector3 vector1 = triangle.pointB - triangle.pointA;
			Vector3 vector2 = triangle.pointC - triangle.pointA;

			float variant1 = value;
			float variant2 = value;

			Vector3 randomInQuad = variant1 * vector1 + variant2 * vector2;
			randomInQuad += triangle.pointA;

			if (!triangle.Contains(randomInQuad))
			{
				randomInQuad = variant1 * (-1.0f * vector1) + variant2 * (-1.0f * vector2);
				randomInQuad += triangle.pointA + vector1 + vector2;
			}

			return randomInQuad;
		}

		#endregion

		/// <summary>
		/// 	Cycles the input integer to an unsigned integer.
		/// 	
		/// 	For example, -1 would return uint.MaxValue - 1.
		/// </summary>
		/// <returns>The to uint.</returns>
		/// <param name="value">Value.</param>
		public static uint CycleToUint(int value)
		{
			if (value > 0)
				return (uint)value;

			uint negative = (uint)HydraMathUtils.Abs(value);
			return uint.MaxValue - negative;
		}
	}
}
