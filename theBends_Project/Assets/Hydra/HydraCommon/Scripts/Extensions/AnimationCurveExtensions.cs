// <copyright file=AnimationCurveExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	AnimationCurveExtensions provides extension methods for working with AnimationCurves.
	/// </summary>
	public static class AnimationCurveExtensions
	{
		private const float CURVE_DELTA_FUDGE = 0.00001f;

		public const int CURVE_BOUNDS_SAMPLES = 5;
		public const float CURVE_BOUNDS_DELTA = 1.0f / (CURVE_BOUNDS_SAMPLES - 1);

		private static Keyframe[] s_CopyKeys;

		/// <summary>
		/// 	Clears the keys from the AnimationCurve.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static void Clear(this AnimationCurve extends)
		{
			for (int index = 0; index < extends.length; index++)
				extends.RemoveKey(index);
		}

		/// <summary>
		/// 	Gets the gradient at the given time.
		/// </summary>
		/// <returns>The gradient.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="time">Time.</param>
		public static float GetGradient(this AnimationCurve extends, float time)
		{
			float end = extends.GetEndTime();
			bool isEnd = HydraMathUtils.Approximately(end, time);

			float firstTime = isEnd ? time - CURVE_DELTA_FUDGE : time;
			float secondTime = isEnd ? time : time + CURVE_DELTA_FUDGE;

			// Cheat and check the value at time + some small amount
			float first = extends.Evaluate(firstTime);
			float second = extends.Evaluate(secondTime);

			return (second - first) / CURVE_DELTA_FUDGE;
		}

		/// <summary>
		/// 	Sets all of the keys on the AnimationCurve to the given value.
		/// 	If there are no keys, we add one.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="value">Value.</param>
		public static void SetValue(this AnimationCurve extends, float value)
		{
			extends.Clear();

			extends.AddKey(0.0f, value);
			extends.AddKey(1.0f, value);
		}

		/// <summary>
		/// 	Returns an approximation for the greatest value along the curve.
		/// 
		/// 	Unity doesn't provide a way to get the bounding box, so
		/// 	we'll cheat and just check a certain number of samples.
		/// </summary>
		/// <returns>The max value.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetMaxValue(this AnimationCurve extends)
		{
			float max = extends[0].value;

			if (extends.length == 1)
				return max;

			float startTime = extends.GetStartTime();
			float lengthTime = extends.GetTimeLength();
			float delta = CURVE_BOUNDS_DELTA * lengthTime;

			for (int index = 1; index < CURVE_BOUNDS_SAMPLES; index++)
			{
				float time = startTime + delta * index;
				max = HydraMathUtils.Max(max, extends.Evaluate(time));
			}

			return max;
		}

		/// <summary>
		/// 	Gets the length of the curve.
		/// </summary>
		/// <returns>The time length.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetTimeLength(this AnimationCurve extends)
		{
			return extends.GetEndTime() - extends.GetStartTime();
		}

		/// <summary>
		/// 	Gets the start time.
		/// </summary>
		/// <returns>The start time.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetStartTime(this AnimationCurve extends)
		{
			return extends.length > 0 ? extends[0].time : 0.0f;
		}

		/// <summary>
		/// 	Gets the end time.
		/// </summary>
		/// <returns>The end time.</returns>
		/// <param name="extends">Extends.</param>
		public static float GetEndTime(this AnimationCurve extends)
		{
			int length = extends.length;
			return extends.length > 0 ? extends[length - 1].time : 0.0f;
		}

		/// <summary>
		/// 	Sets the wrap mode.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="inMode">In mode.</param>
		/// <param name="outMode">Out mode.</param>
		public static void SetWrapMode(this AnimationCurve extends, WrapMode inMode, WrapMode outMode)
		{
			extends.preWrapMode = inMode;
			extends.postWrapMode = outMode;
		}

		/// <summary>
		/// 	Makes the animation curve linear.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static void SetLinear(this AnimationCurve extends)
		{
			for (int index = 0; index < extends.length; index++)
			{
				Vector2 point1;
				Vector2 point2;
				Vector2 deltapoint;

				Keyframe key = extends[index];

				if (index != 0)
				{
					point1.x = extends[index - 1].time;
					point1.y = extends[index - 1].value;
					point2.x = extends[index].time;
					point2.y = extends[index].value;

					deltapoint = point2 - point1;

					key.inTangent = deltapoint.y / deltapoint.x;
				}

				if (index != extends.length - 1)
				{
					point1.x = extends[index].time;
					point1.y = extends[index].value;
					point2.x = extends[index + 1].time;
					point2.y = extends[index + 1].value;

					deltapoint = point2 - point1;

					key.outTangent = deltapoint.y / deltapoint.x;
				}

				extends.MoveKey(index, key);
			}
		}

		/// <summary>
		/// 	Copies the AnimationCurve values into the target AnimationCurve.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="target">Target.</param>
		public static void DeepCopyInto(this AnimationCurve extends, AnimationCurve target)
		{
			Array.Resize(ref s_CopyKeys, extends.length);

			for (int index = 0; index < extends.length; index++)
				s_CopyKeys[index] = extends[index];

			target.keys = s_CopyKeys;

			target.preWrapMode = extends.preWrapMode;
			target.postWrapMode = extends.postWrapMode;
		}
	}
}
