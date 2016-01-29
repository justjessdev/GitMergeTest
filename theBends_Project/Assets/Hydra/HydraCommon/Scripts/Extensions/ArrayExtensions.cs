// <copyright file=ArrayExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;

namespace Hydra.HydraCommon.Extensions
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// 	Returns the index of the item in the array, otherwise -1.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The contents type.</typeparam>
		public static int IndexOf<T>(this T[] extends, T item)
		{
			for (int index = 0; index < extends.Length; index++)
			{
				if (EqualityComparer<T>.Default.Equals(extends[index], item))
					return index;
			}
			return -1;
		}
	}
}
