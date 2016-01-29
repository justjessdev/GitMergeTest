// <copyright file=ColorUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	ColorUtils provides utility methods for working with colors.
	/// </summary>
	public static class ColorUtils
	{
		public static readonly Color hydraPrimary = RGB(17, 154, 144);
		public static readonly Color hydraSecondary = RGB(34, 199, 187);

		public enum Gamut
		{
			RGB,
			HSL
		}

		public enum BlendMode
		{
			None,
			Replace,
			Normal,

			Darken,
			Multiply,
			ColorBurn,
			LinearBurn,
			DarkerColor,

			Lighten,
			Screen,
			ColorDodge,
			Add,
			LighterColor,

			Overlay,
			SoftLight,
			HardLight,
			PinLight,
			HardMix,

			Difference,
			Exclusion,
			Subtract,
			Divide,

			Hue,
			Saturation,
			Color,
			Luminosity
		}

		/// <summary>
		/// 	Returns a color based on the given rgb 0-255 values.
		/// </summary>
		/// <param name="red">Red.</param>
		/// <param name="green">Green.</param>
		/// <param name="blue">Blue.</param>
		public static Color RGB(int red, int green, int blue)
		{
			return new Color(red / 255.0f, green / 255.0f, blue / 255.0f);
		}

		/// <summary>
		/// 	Returns a linear color at the delta between the two input colors.
		/// </summary>
		/// <param name="colorA">Color a.</param>
		/// <param name="colorB">Color b.</param>
		/// <param name="delta">Delta.</param>
		public static Color Lerp(Color colorA, Color colorB, float delta)
		{
			return new Color(Mathf.Lerp(colorA.r, colorB.r, delta), Mathf.Lerp(colorA.g, colorB.g, delta),
							 Mathf.Lerp(colorA.b, colorB.b, delta), Mathf.Lerp(colorA.a, colorB.a, delta));
		}

		/// <summary>
		/// 	Converts RGB color values to HSL.
		/// </summary>
		/// <returns>The HSL value.</returns>
		/// <param name="red">Red.</param>
		/// <param name="green">Green.</param>
		/// <param name="blue">Blue.</param>
		/// <param name="alpha">Alpha.</param>
		public static Vector4 RgbToHsl(float red, float green, float blue, float alpha)
		{
			float max = HydraMathUtils.Max(red, green, blue);
			float min = HydraMathUtils.Min(red, green, blue);

			float hue = (max + min) / 2.0f;
			float saturation;
			float lightness = hue;

			if (HydraMathUtils.Approximately(max, min))
			{
				// achromatic
				hue = 0.0f;
				saturation = 0.0f;
			}
			else
			{
				float delta = max - min;

				saturation = (lightness > 0.5f) ? delta / (2.0f - max - min) : delta / (max + min);

				if (red >= green && red >= blue)
					hue = (green - blue) / delta + (green < blue ? 6.0f : 0.0f);
				else if (green >= red && green >= blue)
					hue = (blue - red) / delta + 2.0f;
				else
					hue = (red - green) / delta + 4.0f;

				hue /= 6.0f;
			}

			return new Vector4(hue, saturation, lightness, alpha);
		}

		/// <summary>
		/// 	Converts RGB color values to HSL.
		/// </summary>
		/// <returns>The HSL value.</returns>
		/// <param name="red">Red.</param>
		/// <param name="green">Green.</param>
		/// <param name="blue">Blue.</param>
		public static Vector4 RgbToHsl(float red, float green, float blue)
		{
			return RgbToHsl(red, green, blue, 1.0f);
		}

		/// <summary>
		/// 	Converts RGB color values to HSL.
		/// </summary>
		/// <returns>The HSL value.</returns>
		/// <param name="color">Color.</param>
		public static Vector4 RgbToHsl(Color color)
		{
			return RgbToHsl(color.r, color.g, color.b, color.a);
		}

		/// <summary>
		/// 	Converts RGB color values to a Vector3. Color(0.5, 0.5, 0.5) is Vector3(0, 0, 0).
		/// </summary>
		/// <returns>The vector.</returns>
		/// <param name="color">Color.</param>
		public static Vector3 RgbToVector(Color color)
		{
			return new Vector3((color.r - 0.5f) * 2.0f, (color.g - 0.5f) * 2.0f, (color.b - 0.5f) * 2.0f);
		}

		/// <summary>
		/// 	Converts HSL color values to RGB.
		/// </summary>
		/// <returns>The RGB value.</returns>
		/// <param name="hue">Hue.</param>
		/// <param name="saturation">Saturation.</param>
		/// <param name="lightness">Lightness.</param>
		/// <param name="alpha">Alpha.</param>
		public static Color HslToRgb(float hue, float saturation, float lightness, float alpha)
		{
			// Achromatic
			if (HydraMathUtils.Approximately(saturation, 0.0f))
				return new Color(lightness, lightness, lightness, alpha);

			float q = (lightness < 0.5f) ? lightness * (1.0f + saturation) : lightness + saturation - lightness * saturation;
			float p = 2.0f * lightness - q;

			float r = HueToChannel(p, q, hue + 1.0f / 3.0f);
			float g = HueToChannel(p, q, hue);
			float b = HueToChannel(p, q, hue - 1.0f / 3.0f);

			return new Color(r, g, b, alpha);
		}

		/// <summary>
		/// 	Converts HSL color values to RGB.
		/// </summary>
		/// <returns>The RGB value.</returns>
		/// <param name="hue">Hue.</param>
		/// <param name="saturation">Saturation.</param>
		/// <param name="lightness">Lightness.</param>
		public static Color HslToRgb(float hue, float saturation, float lightness)
		{
			return HslToRgb(hue, saturation, lightness, 1.0f);
		}

		/// <summary>
		/// 	Converts HSL color values to RGB.
		/// </summary>
		/// <returns>The RGB value.</returns>
		/// <param name="hsl">Hsl.</param>
		public static Color HslToRgb(Vector4 hsl)
		{
			return HslToRgb(hsl.x, hsl.y, hsl.z, hsl.w);
		}

		/// <summary>
		/// 	Converts a hue to a single RGB channel.
		/// </summary>
		/// <returns>The RGB channel value.</returns>
		/// <param name="p">P.</param>
		/// <param name="q">Q.</param>
		/// <param name="t">T.</param>
		public static float HueToChannel(float p, float q, float t)
		{
			if (t < 0)
				t += 1;
			if (t > 1)
				t -= 1;

			if (t < 1.0f / 6.0f)
				return p + (q - p) * 6 * t;
			if (t < 1.0f / 2.0f)
				return q;
			if (t < 2.0f / 3.0f)
				return p + (q - p) * (2.0f / 3.0f - t) * 6.0f;

			return p;
		}

		/// <summary>
		/// 	Invert the specified input.
		/// </summary>
		/// <param name="input">Input.</param>
		public static Color Invert(Color input)
		{
			return new Color(1.0f - input.r, 1.0f - input.g, 1.0f - input.b, 1.0f - input.a);
		}

		/// <summary>
		/// 	Blend the specified Colors using the given BlendMode.
		/// </summary>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		/// <param name="mode">Mode.</param>
		public static Color Blend(Color baseLayer, Color topLayer, BlendMode mode)
		{
			switch (mode)
			{
				case BlendMode.None:
					return baseLayer;
				case BlendMode.Replace:
					return topLayer;
				case BlendMode.Normal:
					return BlendNormal(baseLayer, topLayer);

				case BlendMode.Darken:
					return BlendDarken(baseLayer, topLayer);
				case BlendMode.Multiply:
					return BlendMultiply(baseLayer, topLayer);
				case BlendMode.ColorBurn:
					return BlendColorBurn(baseLayer, topLayer);
				case BlendMode.LinearBurn:
					return BlendLinearBurn(baseLayer, topLayer);
				case BlendMode.DarkerColor:
					return BlendDarkerColor(baseLayer, topLayer);

				case BlendMode.Lighten:
					return BlendLighten(baseLayer, topLayer);
				case BlendMode.Screen:
					return BlendScreen(baseLayer, topLayer);
				case BlendMode.ColorDodge:
					return BlendColorDodge(baseLayer, topLayer);
				case BlendMode.Add:
					return BlendAdd(baseLayer, topLayer);
				case BlendMode.LighterColor:
					return BlendLighterColor(baseLayer, topLayer);

				case BlendMode.Overlay:
					return BlendOverlay(baseLayer, topLayer);
				case BlendMode.SoftLight:
					return BlendSoftLight(baseLayer, topLayer);
				case BlendMode.HardLight:
					return BlendHardLight(baseLayer, topLayer);
				case BlendMode.PinLight:
					return BlendPinLight(baseLayer, topLayer);
				case BlendMode.HardMix:
					return BlendHardMix(baseLayer, topLayer);

				case BlendMode.Difference:
					return BlendDifference(baseLayer, topLayer);
				case BlendMode.Exclusion:
					return BlendExclusion(baseLayer, topLayer);
				case BlendMode.Subtract:
					return BlendSubtract(baseLayer, topLayer);
				case BlendMode.Divide:
					return BlendDivide(baseLayer, topLayer);

				case BlendMode.Hue:
					return BlendHue(baseLayer, topLayer);
				case BlendMode.Saturation:
					return BlendSaturation(baseLayer, topLayer);
				case BlendMode.Color:
					return BlendColor(baseLayer, topLayer);
				case BlendMode.Luminosity:
					return BlendLuminosity(baseLayer, topLayer);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Blends the two colors together.
		/// </summary>
		/// <returns>The blended color.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendNormal(Color baseLayer, Color topLayer)
		{
			if (HydraMathUtils.Approximately(topLayer.a, 0.0f))
				return baseLayer;

			if (HydraMathUtils.Approximately(baseLayer.a, 0.0f))
				return topLayer;

			float alpha = 1.0f - (1.0f - topLayer.a) * (1.0f - baseLayer.a);

			return new Color(topLayer.r * topLayer.a / alpha + baseLayer.r * baseLayer.a * (1.0f - topLayer.a) / alpha,
							 topLayer.g * topLayer.a / alpha + baseLayer.g * baseLayer.a * (1.0f - topLayer.a) / alpha,
							 topLayer.b * topLayer.a / alpha + baseLayer.b * baseLayer.a * (1.0f - topLayer.a) / alpha, alpha);
		}

		/// <summary>
		/// 	Returns the smallest value in each channel.
		/// </summary>
		/// <returns>The smallest channels.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendDarken(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Min(baseLayer.r, topLayer.r), HydraMathUtils.Min(baseLayer.g, topLayer.g),
							 HydraMathUtils.Min(baseLayer.b, topLayer.b), HydraMathUtils.Min(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	Multiplies the two colors.
		/// </summary>
		/// <returns>The multiplied color.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendMultiply(Color baseLayer, Color topLayer)
		{
			return baseLayer * topLayer;
		}

		/// <summary>
		/// 	Divides the inverted bottom layer by the top layer, and then inverts the result.
		/// </summary>
		/// <returns>The color burn.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendColorBurn(Color baseLayer, Color topLayer)
		{
			Color divide = BlendDivide(Invert(baseLayer), topLayer);
			return Invert(divide);
		}

		/// <summary>
		/// 	Sums the value in the two layers and subtracts 1.
		/// </summary>
		/// <returns>The linear burn.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendLinearBurn(Color baseLayer, Color topLayer)
		{
			return new Color((baseLayer.r + topLayer.r) - 1.0f, (baseLayer.g + topLayer.g) - 1.0f,
							 (baseLayer.b + topLayer.b) - 1.0f, (baseLayer.a + topLayer.a) - 1.0f);
		}

		/// <summary>
		/// 	Returns the darker color.
		/// </summary>
		/// <returns>The darker color.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendDarkerColor(Color baseLayer, Color topLayer)
		{
			return (RgbToHsl(baseLayer).z < RgbToHsl(topLayer).z) ? baseLayer : topLayer;
		}

		/// <summary>
		/// 	Returns the largest value in each channel.
		/// </summary>
		/// <returns>The largest channels.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendLighten(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Max(baseLayer.r, topLayer.r), HydraMathUtils.Max(baseLayer.g, topLayer.g),
							 HydraMathUtils.Max(baseLayer.b, topLayer.b), HydraMathUtils.Max(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	Inverts both layers, multiplies them, and then inverts the result.
		/// </summary>
		/// <returns>The screen blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendScreen(Color baseLayer, Color topLayer)
		{
			Color multiple = BlendMultiply(Invert(baseLayer), Invert(topLayer));
			return Invert(multiple);
		}

		/// <summary>
		/// 	Divides the bottom layer by the inverted top layer.
		/// </summary>
		/// <returns>The color dodge blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendColorDodge(Color baseLayer, Color topLayer)
		{
			return BlendDivide(baseLayer, Invert(topLayer));
		}

		/// <summary>
		/// 	Returns the sums of the color channels.
		/// </summary>
		/// <returns>The added layers.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendAdd(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Min(baseLayer.r + topLayer.r, 1.0f),
							 HydraMathUtils.Min(baseLayer.g + topLayer.g, 1.0f), HydraMathUtils.Min(baseLayer.b + topLayer.b, 1.0f),
							 HydraMathUtils.Min(baseLayer.a + topLayer.a, 1.0f));
		}

		/// <summary>
		/// 	Returns the lighter color.
		/// </summary>
		/// <returns>The lighter color.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendLighterColor(Color baseLayer, Color topLayer)
		{
			return (RgbToHsl(baseLayer).z > RgbToHsl(topLayer).z) ? baseLayer : topLayer;
		}

		/// <summary>
		/// 	Overlay combines Multiply and Screen blend modes.
		/// </summary>
		/// <returns>The overlay.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendOverlay(Color baseLayer, Color topLayer)
		{
			return new Color(OverlayChannel(baseLayer.r, topLayer.r), OverlayChannel(baseLayer.g, topLayer.g),
							 OverlayChannel(baseLayer.b, topLayer.b), OverlayChannel(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	The parts of the top layer where base layer is light become lighter,
		/// 	the parts where the base layer is dark become darker.
		/// </summary>
		/// <returns>The channel.</returns>
		/// <param name="baseChannel">Base channel.</param>
		/// <param name="topChannel">Top channel.</param>
		public static float OverlayChannel(float baseChannel, float topChannel)
		{
			float output = 2.0f * baseChannel * topChannel;

			if (baseChannel >= 0.5f)
				output = 1.0f - 2.0f * (1.0f - baseChannel) * (1.0f - topChannel);

			return HydraMathUtils.Clamp(output, 0.0f, 1.0f);
		}

		/// <summary>
		/// 	A softer version of Hard Light. Applying pure black or white does not result in pure black or white.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendSoftLight(Color baseLayer, Color topLayer)
		{
			return new Color(SoftLightChannel(baseLayer.r, topLayer.r), SoftLightChannel(baseLayer.g, topLayer.g),
							 SoftLightChannel(baseLayer.b, topLayer.b), SoftLightChannel(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	Applies soft light blending to a single channel.
		/// </summary>
		/// <returns>The light channel.</returns>
		/// <param name="baseChannel">Base channel.</param>
		/// <param name="topChannel">Top channel.</param>
		public static float SoftLightChannel(float baseChannel, float topChannel)
		{
			return (1.0f - 2.0f * topChannel) * baseChannel * baseChannel + 2.0f * topChannel * baseChannel;
		}

		/// <summary>
		/// 	Combines Multiply and Screen blend modes. Equivalent to Overlay,
		/// 	but with the bottom and top images swapped.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendHardLight(Color baseLayer, Color topLayer)
		{
			return BlendOverlay(topLayer, baseLayer);
		}

		/// <summary>
		/// 	Replaces colors depending on the lightness of the topLayer. If the topLayer is
		/// 	more than 50% lightness and the baseLayer is darker than the topLayer, then the
		/// 	baseLayer is returned. If the topLayer is less than 50% lightness
		/// 	and the baseLayer is lighter than the topLayer, then the baseLayer is returned.
		/// </summary>
		/// <returns>The pin light.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendPinLight(Color baseLayer, Color topLayer)
		{
			Vector4 baseHsl = RgbToHsl(baseLayer);
			Vector4 topHsl = HslToRgb(topLayer);

			if (topHsl.z >= 0.5f && baseHsl.z <= topHsl.z)
				return baseLayer;

			if (topHsl.z <= 0.5f && baseHsl.z >= topHsl.z)
				return baseLayer;

			return topLayer;
		}

		/// <summary>
		/// 	Returns a hard mix blend of the colors.
		/// </summary>
		/// <returns>The hard mix.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendHardMix(Color baseLayer, Color topLayer)
		{
			return new Color(HardMixChannel(baseLayer.r, topLayer.r), HardMixChannel(baseLayer.g, topLayer.g),
							 HardMixChannel(baseLayer.b, topLayer.b), HardMixChannel(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	Applies hard mix blending to a single channel.
		/// </summary>
		/// <returns>The mix channel.</returns>
		/// <param name="baseChannel">Base channel.</param>
		/// <param name="topChannel">Top channel.</param>
		public static float HardMixChannel(float baseChannel, float topChannel)
		{
			if (baseChannel + topChannel < 1.0f)
				return 0.0f;

			if (baseChannel + topChannel > 1.0f)
				return 1.0f;

			return (baseChannel >= topChannel) ? 1.0f : 0.0f;
		}

		/// <summary>
		/// 	Takes the magnitude of each channel once subtracted.
		/// </summary>
		/// <returns>The difference.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendDifference(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Abs(baseLayer.r - topLayer.r), HydraMathUtils.Abs(baseLayer.g - topLayer.g),
							 HydraMathUtils.Abs(baseLayer.b - topLayer.b), HydraMathUtils.Abs(baseLayer.a - topLayer.a));
		}

		/// <summary>
		/// 	Exclusion blending mode inverts base layer according to the brightness values in the top layer.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendExclusion(Color baseLayer, Color topLayer)
		{
			return new Color(ExclusionChannel(baseLayer.r, topLayer.r), ExclusionChannel(baseLayer.g, topLayer.g),
							 ExclusionChannel(baseLayer.b, topLayer.b), ExclusionChannel(baseLayer.a, topLayer.a));
		}

		/// <summary>
		/// 	Applies exclusion blending to a single channel.
		/// </summary>
		/// <returns>The channel.</returns>
		/// <param name="baseChannel">Base channel.</param>
		/// <param name="topChannel">Top channel.</param>
		public static float ExclusionChannel(float baseChannel, float topChannel)
		{
			return baseChannel * (1.0f - topChannel) + topChannel * (1.0f - baseChannel);
		}

		/// <summary>
		/// 	Returns the top layer subtracted from the base layer.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendSubtract(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Max(baseLayer.r - topLayer.r, 0.0f),
							 HydraMathUtils.Max(baseLayer.g - topLayer.g, 0.0f), HydraMathUtils.Max(baseLayer.b - topLayer.b, 0.0f),
							 HydraMathUtils.Max(baseLayer.a - topLayer.a, 0.0f));
		}

		/// <summary>
		/// 	Divides the channels.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendDivide(Color baseLayer, Color topLayer)
		{
			return new Color(HydraMathUtils.Min(baseLayer.r / topLayer.r, 1.0f),
							 HydraMathUtils.Min(baseLayer.g / topLayer.g, 1.0f), HydraMathUtils.Min(baseLayer.b / topLayer.b, 1.0f),
							 HydraMathUtils.Min(baseLayer.a / topLayer.a, 1.0f));
		}

		/// <summary>
		/// 	Preserves the saturation and lightness of the bottom layer, while adopting the hue of the top layer.
		/// </summary>
		/// <returns>The hue.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendHue(Color baseLayer, Color topLayer)
		{
			Vector4 baseHsl = RgbToHsl(baseLayer);
			Vector4 topHsl = RgbToHsl(topLayer);

			return HslToRgb(new Vector4(topHsl.x, baseHsl.y, baseHsl.z, baseHsl.w));
		}

		/// <summary>
		/// 	Preserves the hue and lightness of the bottom layer, while adopting the saturation of the top layer.
		/// </summary>
		/// <returns>The saturation.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendSaturation(Color baseLayer, Color topLayer)
		{
			Vector4 baseHsl = RgbToHsl(baseLayer);
			Vector4 topHsl = RgbToHsl(topLayer);

			return HslToRgb(new Vector4(baseHsl.x, topHsl.y, baseHsl.z, baseHsl.w));
		}

		/// <summary>
		/// 	Preserves the lightness of the bottom layer, while adopting the hue and saturation of the top layer.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendColor(Color baseLayer, Color topLayer)
		{
			Vector4 baseHsl = RgbToHsl(baseLayer);
			Vector4 topHsl = RgbToHsl(topLayer);

			return HslToRgb(new Vector4(topHsl.x, topHsl.y, baseHsl.z, baseHsl.w));
		}

		/// <summary>
		/// 	Preserves the hue and saturation of the bottom layer, while adopting the lightness of the top layer.
		/// </summary>
		/// <returns>The luminosity.</returns>
		/// <param name="baseLayer">Base layer.</param>
		/// <param name="topLayer">Top layer.</param>
		public static Color BlendLuminosity(Color baseLayer, Color topLayer)
		{
			Vector4 baseHsl = RgbToHsl(baseLayer);
			Vector4 topHsl = RgbToHsl(topLayer);

			return HslToRgb(new Vector4(baseHsl.x, baseHsl.y, topHsl.z, baseHsl.w));
		}
	}
}
