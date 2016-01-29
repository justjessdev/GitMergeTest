// <copyright file=ArrayUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Text;

namespace Hydra.HydraCommon.Utils
{
	public static class ArrayUtils
	{
		/// <summary>
		/// 	Adds the range to the output array.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="range">Range.</param>
		/// <typeparam name="T">The array contents type.</typeparam>
		public static void AddRange<T>(ref T[] output, T[] range)
		{
			int outputLength = output.Length;
			int rangeLength = range.Length;

			Array.Resize(ref output, outputLength + rangeLength);
			Array.Copy(range, 0, output, outputLength, rangeLength);
		}

		/// <summary>
		/// 	Returns the array as a delimited string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="array">Array.</param>
		/// <typeparam name="T">The array contents type.</typeparam>
		public static string ToDelimitedString<T>(this T[] array)
		{
			if (array == null)
				return null;

			StringBuilder builder = new StringBuilder("[");

			for (int index = 0; index < array.Length; index++)
			{
				builder.Append(array[index]);
				builder.Append(", ");
			}

			builder.Append("]");

			return builder.ToString();
		}
	}
}
