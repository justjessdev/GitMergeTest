// <copyright file=IDictionaryExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	IDictionaryExtensions provides extension methods for working with IDictionaries.
	/// </summary>
	public static class IDictionaryExtensions
	{
		/// <summary>
		/// 	Returns the value for the given key if the key exists, otherwise returns the type default.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="key">Key.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> extends, TKey key)
		{
			return extends.Get(key, default(TValue));
		}

		/// <summary>
		/// 	Returns the value for the given key if the key exists, otherwise returns the specified default value.
		/// </summary>
		/// <param name="extends">Extends.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> extends, TKey key, TValue defaultValue)
		{
			return (extends.ContainsKey(key)) ? extends[key] : defaultValue;
		}

		/// <summary>
		/// 	Gets the key for value.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		public static TKey GetKeyForValue<TKey, TValue>(this IDictionary<TKey, TValue> extends, TValue value)
		{
			foreach (KeyValuePair<TKey, TValue> pair in extends)
			{
				if (pair.Value.Equals(value))
					return pair.Key;
			}

			throw new KeyNotFoundException();
		}
	}
}
