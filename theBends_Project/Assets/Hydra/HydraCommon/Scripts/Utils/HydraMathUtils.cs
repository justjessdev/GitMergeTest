// <copyright file=HydraMathUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Hydra math utils.
	/// </summary>
	public static class HydraMathUtils
	{
		/// <summary>
		/// 	Finds a tangent for the given vector.
		/// </summary>
		/// <param name="vector">Vector.</param>
		public static Vector3 Tangent(Vector3 vector)
		{
			Vector3 tangent = Vector3.Cross(vector, Vector3.forward);

			if (Approximately(tangent.magnitude, 0.0f))
				tangent = Vector3.Cross(vector, Vector3.up);

			return tangent;
		}

		/// <summary>
		/// 	Returns a radius given a size vector.
		/// 	
		/// 	This has the same behaviour as the SphereCollider: the greatest axis is
		/// 	considered.
		/// </summary>
		/// <param name="size">Size.</param>
		public static float Radius(Vector3 size)
		{
			return Max(Abs(size.x), Abs(size.y), Abs(size.z));
		}

		/// <summary>
		/// 	Returns the given value after the input range has been mapped to a new range
		/// </summary>
		/// <returns>The newly mapped value</returns>
		/// <param name="inputStart">Input start.</param>
		/// <param name="inputEnd">Input end.</param>
		/// <param name="outputStart">Output start.</param>
		/// <param name="outputEnd">Output end.</param>
		/// <param name="value">Value.</param>
		public static float MapRange(float inputStart, float inputEnd, float outputStart, float outputEnd, float value)
		{
			float slope = (outputEnd - outputStart) / (inputEnd - inputStart);
			return outputStart + slope * (value - inputStart);
		}

		/// <summary>
		/// 	Faster than Mathf.Clamp.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static float Clamp(float value, float min, float max)
		{
			if (value > max)
				value = max;
			else if (value < min)
				value = min;
			return value;
		}

		/// <summary>
		/// 	Faster than Mathf.Clamp.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static int Clamp(int value, int min, int max)
		{
			if (value > max)
				value = max;
			else if (value < min)
				value = min;
			return value;
		}

		/// <summary>
		/// 	Same as Mathf.Max, but doesn't create a params array.
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		public static float Max(float a, float b)
		{
			return (a > b) ? a : b;
		}

		/// <summary>
		/// 	Same as Mathf.Max, but doesn't create a params array.
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		public static int Max(int a, int b)
		{
			return (a > b) ? a : b;
		}

		/// <summary>
		/// 	Same as Mathf.Min, but doesn't create a params array.
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		public static float Min(float a, float b)
		{
			return (a < b) ? a : b;
		}

		/// <summary>
		/// 	Same as Mathf.Max, but doesn't create a params array.
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		/// <param name="c">The third component.</param>
		public static float Max(float a, float b, float c)
		{
			return Max(a, Max(b, c));
		}

		/// <summary>
		/// 	Same as Mathf.Min, but doesn't create a params array.
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		/// <param name="c">The third component.</param>
		public static float Min(float a, float b, float c)
		{
			return Min(a, Min(b, c));
		}

		/// <summary>
		/// 	Faster than Mathf.Abs
		/// </summary>
		/// <param name="value">Value.</param>
		public static float Abs(float value)
		{
			return (value < 0.0f) ? -value : value;
		}

		/// <summary>
		/// 	Faster than Mathf.Abs
		/// </summary>
		/// <param name="value">Value.</param>
		public static int Abs(int value)
		{
			return (value < 0) ? -value : value;
		}

		/// <summary>
		/// 	Faster than Mathf.Approximately
		/// </summary>
		/// <param name="a">The first component.</param>
		/// <param name="b">The second component.</param>
		public static bool Approximately(float a, float b)
		{
			return a + 0.0000000596f >= b && a - 0.0000000596f <= b;
		}

		/// <summary>
		/// 	Returns true if the value is in the given range.
		/// </summary>
		/// <returns><c>true</c>, if value is in range, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static bool InRange(float value, float min, float max)
		{
			return value >= min && value <= max;
		}

		/// <summary>
		/// 	Repeats the integer t by the given length.
		/// </summary>
		/// <param name="t">The value to repeat.</param>
		/// <param name="length">The length of the sequence.</param>
		public static int Repeat(int t, int length)
		{
			return (int)Mathf.Repeat((float)t, (float)length);
		}

		/// <summary>
		/// 	Faster than Mathf.FloorToInt
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="value">The first component.</param>
		public static int FloorToInt(double value)
		{
			return value >= 0 ? (int)value : (int)value - 1;
		}

		/// <summary>
		/// 	Faster than Mathf.PingPong
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="t">T.</param>
		/// <param name="length">Length.</param>
		public static float PingPong(float t, float length)
		{
			t = Mathf.Repeat(t, length * 2.0f);
			return (length - Abs(t - length));
		}

		/// <summary>
		/// 	PingPongs an integer.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="t">T.</param>
		/// <param name="length">Length.</param>
		public static int PingPong(int t, int length)
		{
			t = Repeat(t, length * 2);
			return (length - Abs(t - length));
		}
	}
}
