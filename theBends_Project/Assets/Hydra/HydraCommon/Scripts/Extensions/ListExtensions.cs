// <copyright file=ListExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;

namespace Hydra.HydraCommon.Extensions
{
	public static class ListExtensions
	{
		/// <summary>
		/// 	Clears the list and sets the capacity.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="capacity">Capacity.</param>
		/// <typeparam name="T">The contents type.</typeparam>
		public static void Clear<T>(this List<T> extends, int capacity)
		{
			extends.Clear();
			extends.Capacity = capacity;
		}
	}
}
