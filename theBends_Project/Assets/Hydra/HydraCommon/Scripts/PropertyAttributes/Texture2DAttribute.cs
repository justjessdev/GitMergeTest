// <copyright file=Texture2DAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	Texture2DAttribute provides a wrapper around a Texture2D for easier repeating.
	/// </summary>
	[Serializable]
	public class Texture2DAttribute : HydraPropertyAttribute
	{
		public enum Wrap
		{
			Clamp,
			Repeat,
			PingPong
		}

		[SerializeField] private Texture2D m_Texture;
		[SerializeField] private Wrap m_Wrap;
		[SerializeField] private Texture2D m_WrappedTexture;

		#region Properties

		/// <summary>
		/// 	Gets or sets the texture.
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D texture
		{
			get { return m_Texture; }
			set
			{
				if (value == m_Texture)
					return;

				ClearCache();

				if (!value.IsReadable())
				{
					string warning = string.Format("Texture {0} is not readable.", texture.name);
					Debug.LogWarning(warning);
					return;
				}

				m_Texture = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the wrap.
		/// </summary>
		/// <value>The wrap.</value>
		public Wrap wrap
		{
			get { return m_Wrap; }
			set
			{
				if (value == m_Wrap)
					return;

				ClearCache();

				m_Wrap = value;
			}
		}

		/// <summary>
		/// 	Gets the wrapped texture.
		/// </summary>
		/// <value>The wrapped texture.</value>
		public Texture2D wrappedTexture { get { return GetWrappedTexture(); } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Clears the wrapped texture. This is important if the
		/// 	original texture is being manipulated and the changes
		/// 	need to be seen in the wrapped texture.
		/// </summary>
		/// <returns>The cleared texture (null).</returns>
		public Texture2D ClearCache()
		{
			m_WrappedTexture = ObjectUtils.SafeDestroy(m_WrappedTexture);
			return m_WrappedTexture;
		}

		/// <summary>
		/// 	Samples the texture at the given UV coords, using bilinear sample.
		/// </summary>
		/// <returns>The sample.</returns>
		/// <param name="uV">UV.</param>
		public Color SampleBilinear(Vector2 uV)
		{
			if (m_Texture == null)
				return default(Color);

			uV = GetWrappedUv(uV);

			return m_Texture.GetPixelBilinear(uV);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Gets the wrapped uv.
		/// </summary>
		/// <returns>The wrapped uv.</returns>
		/// <param name="uV">U v.</param>
		private Vector2 GetWrappedUv(Vector2 uV)
		{
			float u = uV.x;
			float v = uV.y;

			switch (m_Wrap)
			{
				case Wrap.Clamp:
					u = HydraMathUtils.Clamp(u, 0.0f, 1.0f);
					v = HydraMathUtils.Clamp(v, 0.0f, 1.0f);
					break;

				case Wrap.Repeat:
					u = Mathf.Repeat(u, 1.0f);
					v = Mathf.Repeat(v, 1.0f);
					break;

				case Wrap.PingPong:
					u = HydraMathUtils.PingPong(u, 1.0f);
					v = HydraMathUtils.PingPong(v, 1.0f);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			return new Vector2(u, v);
		}

		/// <summary>
		/// 	Gets the wrapped texture.
		/// </summary>
		/// <returns>The wrapped texture.</returns>
		private Texture2D GetWrappedTexture()
		{
			if (m_Texture == null)
				return ClearCache();

			if (m_WrappedTexture == null)
			{
				switch (m_Wrap)
				{
					case Wrap.Clamp:
						m_WrappedTexture = m_Texture.Clamp();
						break;

					case Wrap.Repeat:
						m_WrappedTexture = m_Texture.Repeat();
						break;

					case Wrap.PingPong:
						m_WrappedTexture = m_Texture.PingPong();
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

				m_WrappedTexture.hideFlags = HideFlags.HideAndDontSave;
			}

			return m_WrappedTexture;
		}

		#endregion
	}
}
