// <copyright file=Texture2DExtensions company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Extensions
{
	/// <summary>
	/// 	Texture unreadable exception.
	/// </summary>
	public class TextureUnreadableException : Exception {}

	/// <summary>
	/// 	Texture2DExtensions provides extension methods for working with Texture2Ds.
	/// </summary>
	public static class Texture2DExtensions
	{
		/// <summary>
		/// 	Wraps GetPixelBilinear(x, y).
		/// </summary>
		/// <returns>The bilinear sampled pixel.</returns>
		/// <param name="extends">Extends.</param>
		/// <param name="uV">UV.</param>
		public static Color GetPixelBilinear(this Texture2D extends, Vector2 uV)
		{
			return extends.GetPixelBilinear(uV.x, uV.y);
		}

		/// <summary>
		/// 	Copy the specified texture.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static Texture2D Copy(this Texture2D extends)
		{
			if (!extends.IsReadable())
				throw new TextureUnreadableException();

			Texture2D output = new Texture2D(extends.width, extends.height, extends.format, extends.HasMipMaps());
			output.wrapMode = extends.wrapMode;

			output.SetPixels32(extends.GetPixels32());
			output.Apply();

			return output;
		}

		/// <summary>
		/// 	Creates a copy of the texture, set to clamped.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static Texture2D Clamp(this Texture2D extends)
		{
			Texture2D output = extends.Copy();
			output.wrapMode = TextureWrapMode.Clamp;

			return output;
		}

		/// <summary>
		/// 	Creates a copy of the texture, set to repeat.
		/// </summary>
		/// <param name="extends">Extends.</param>
		public static Texture2D Repeat(this Texture2D extends)
		{
			Texture2D output = extends.Copy();
			output.wrapMode = TextureWrapMode.Repeat;

			return output;
		}

		/// <summary>
		/// 	Creates a new texture 4x bigger with flipped quadrants.
		/// </summary>
		/// <returns>The resulting texture.</returns>
		/// <param name="extends">Extends.</param>
		public static Texture2D PingPong(this Texture2D extends)
		{
			if (!extends.IsReadable())
				throw new TextureUnreadableException();

			int width = extends.width * 2;
			int height = extends.height * 2;

			Texture2D output = new Texture2D(width, height, extends.format, extends.HasMipMaps());
			output.wrapMode = TextureWrapMode.Repeat;

			Color32[] pixels = extends.GetPixels32();
			Color32[] outputPixels = output.GetPixels32();

			for (int y = 0; y < height; y++)
			{
				bool flipVertical = y >= extends.height;
				int targetY = (flipVertical) ? height - (y + 1) : y;

				for (int x = 0; x < width; x++)
				{
					bool flipHorizontal = x >= extends.width;
					int targetX = (flipHorizontal) ? width - (x + 1) : x;

					int index = (y * width) + x;
					int targetIndex = (targetY * extends.width) + targetX;

					Color32 pixel = pixels[targetIndex];
					outputPixels[index] = pixel;
				}
			}

			output.SetPixels32(outputPixels);
			output.Apply();

			return output;
		}

		/// <summary>
		/// 	Determines if the texture has mip maps.
		/// </summary>
		/// <returns><c>true</c> if has mip maps; otherwise, <c>false</c>.</returns>
		/// <param name="extends">Extends.</param>
		public static bool HasMipMaps(this Texture2D extends)
		{
			return extends.mipmapCount > 1;
		}

		/// <summary>
		/// 	Returns true if the texture is readable.
		/// </summary>
		/// <returns><c>true</c>, if is readable, <c>false</c> otherwise.</returns>
		/// <param name="extends">Extends.</param>
		public static bool IsReadable(this Texture2D extends)
		{
			try
			{
				extends.GetPixel(0, 0);
			}
			catch (UnityException)
			{
				return false;
			}

			return true;
		}
	}
}
