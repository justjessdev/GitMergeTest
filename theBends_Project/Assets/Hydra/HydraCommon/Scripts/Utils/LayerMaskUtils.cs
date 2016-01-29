// <copyright file=LayerMaskUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	public static class LayerMaskUtils
	{
		public const int EMPTY = 0;
		public const int TOTAL_LAYER_COUNT = 32;

		#region Methods

		/// <summary>
		/// 	Determines if a is layer defined (i.e. has a name).
		/// </summary>
		/// <returns><c>true</c> if layer is defined; otherwise, <c>false</c>.</returns>
		/// <param name="index">Index.</param>
		public static bool IsLayerDefined(int index)
		{
			string name = LayerMask.LayerToName(index);
			return (!string.IsNullOrEmpty(name));
		}

		/// <summary>
		/// 	Gets the layer names.
		/// </summary>
		/// <param name="output">Output.</param>
		public static void GetLayerNames(ref string[] output)
		{
			Array.Resize(ref output, TOTAL_LAYER_COUNT);

			for (int index = 0; index < TOTAL_LAYER_COUNT; index++)
				output[index] = LayerMask.LayerToName(index);
		}

		/// <summary>
		/// 	Gets the layer count.
		/// </summary>
		/// <returns>The layer count.</returns>
		public static int GetLayerCount()
		{
			int output = 0;

			for (int index = 0; index < TOTAL_LAYER_COUNT; index++)
			{
				if (IsLayerDefined(index))
					output++;
			}

			return output;
		}

		/// <summary>
		/// 	Gets the non empty layer names.
		/// </summary>
		/// <param name="output">Output.</param>
		public static void GetMaskFieldNames(ref string[] output)
		{
			Array.Resize(ref output, GetLayerCount());

			int index = 0;

			for (int layerIndex = 0; layerIndex < TOTAL_LAYER_COUNT; layerIndex++)
			{
				if (!IsLayerDefined(layerIndex))
					continue;

				string name = LayerMask.LayerToName(layerIndex);
				output[index] = name;
				index++;
			}
		}

		/// <summary>
		/// 	Maps the 32 bit layer mask to match the number of non-empty layers
		/// </summary>
		/// <returns>The mapped mask.</returns>
		/// <param name="mask">Mask.</param>
		public static int MapToMaskField(LayerMask mask)
		{
			if (mask <= 0)
				return mask;

			int output = 0;
			int index = 0;

			for (int layerIndex = 0; layerIndex < TOTAL_LAYER_COUNT; layerIndex++)
			{
				if (!IsLayerDefined(layerIndex))
					continue;

				bool bit = (mask & (1 << layerIndex)) != 0;
				if (bit)
					output += 1 << index;

				index++;
			}

			return output;
		}

		/// <summary>
		/// 	Maps the non-empty layer mask to a 32 bit layer mask.
		/// </summary>
		/// <returns>The unmapped mask.</returns>
		/// <param name="mask">Mask.</param>
		public static LayerMask MapFromMaskField(int mask)
		{
			if (mask <= 0)
				return mask;

			int output = 0;
			int index = 0;

			for (int layerIndex = 0; layerIndex < TOTAL_LAYER_COUNT; layerIndex++)
			{
				if (!IsLayerDefined(layerIndex))
					continue;

				bool bit = (mask & (1 << index)) != 0;
				if (bit)
					output += 1 << layerIndex;

				index++;
			}

			return output;
		}

		#endregion
	}
}
