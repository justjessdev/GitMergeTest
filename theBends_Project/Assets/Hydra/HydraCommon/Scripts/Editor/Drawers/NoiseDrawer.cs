// <copyright file=NoiseDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes;
using Hydra.HydraCommon.Utils.RNG;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	/// <summary>
	/// 	NoiseDrawer draws the inpspector elements for the NoiseAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(NoiseAttribute))]
	public class NoiseDrawer : PropertyDrawer
	{
		public const int PREVIEW_TEXTURE_SIZE = 64;

		private static readonly GUIContent s_NoiseFrequencyLabel = new GUIContent("Frequency");
		private static readonly GUIContent s_NoisePhaseLabel = new GUIContent("Phase");

		private static Vector3 s_FrequencyCache;
		private static Vector3 s_PhaseCache;
		private static Texture2D s_TextureCache;
		private static Color[] s_Pixels;

		private SerializedProperty m_FrequencyProp;
		private SerializedProperty m_PhaseProp;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected virtual void FindProperties(SerializedProperty prop)
		{
			m_FrequencyProp = prop.FindPropertyRelative("m_Frequency");
			m_PhaseProp = prop.FindPropertyRelative("m_Phase");
		}

		#region Override Methods

		/// <summary>
		/// 	Override this method to make your own GUI for the property
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="prop">Property</param>
		/// <param name="label">Label</param>
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			FindProperties(prop);

			// Draw the preview texture
			Rect textureRect = new Rect(position);
			textureRect.x += textureRect.width - PREVIEW_TEXTURE_SIZE;
			textureRect.width = PREVIEW_TEXTURE_SIZE;
			textureRect.height = PREVIEW_TEXTURE_SIZE;

			DrawPreviewTexture(textureRect);

			// Draw the fields
			Rect contentRect = new Rect(position);
			contentRect.width -= textureRect.width + HydraEditorUtils.STANDARD_HORIZONTAL_SPACING * 2.0f;
			HydraEditorUtils.Vector3Field(contentRect, s_NoiseFrequencyLabel, m_FrequencyProp);

			contentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			HydraEditorUtils.Vector3Field(contentRect, s_NoisePhaseLabel, m_PhaseProp);

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 	Gets the height of the property.
		/// </summary>
		/// <returns>The property height.</returns>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return PREVIEW_TEXTURE_SIZE + EditorGUIUtility.standardVerticalSpacing;
		}

		#endregion

		/// <summary>
		/// 	Draws the preview texture.
		/// </summary>
		private void DrawPreviewTexture(Rect position)
		{
			Texture2D texture = GetTexture(m_FrequencyProp, m_PhaseProp);
			HydraEditorUtils.PreviewTexture(position, texture);
		}

		/// <summary>
		/// 	Gets the texture.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="frequencyProp">Frequency property.</param>
		/// <param name="phaseProp">Phase property.</param>
		private Texture2D GetTexture(SerializedProperty frequencyProp, SerializedProperty phaseProp)
		{
			bool dirty = false;

			if (s_TextureCache == null)
			{
				s_TextureCache = new Texture2D(PREVIEW_TEXTURE_SIZE, PREVIEW_TEXTURE_SIZE);
				s_TextureCache.hideFlags = HideFlags.HideAndDontSave;
				dirty = true;
			}

			Vector3 frequency = frequencyProp.vector3Value;
			if (frequency != s_FrequencyCache)
			{
				dirty = true;
				s_FrequencyCache = frequency;
			}

			Vector3 phase = phaseProp.vector3Value;
			if (phase != s_PhaseCache)
			{
				dirty = true;
				s_PhaseCache = phase;
			}

			if (dirty)
				GenerateNoise(s_TextureCache, frequency, phase);

			return s_TextureCache;
		}

		/// <summary>
		/// 	Generates the noise texture.
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="frequency">Frequency.</param>
		/// <param name="phase">Phase.</param>
		private void GenerateNoise(Texture2D texture, Vector3 frequency, Vector3 phase)
		{
			Array.Resize(ref s_Pixels, texture.width * texture.height);

			int index = 0;
			for (int y = 0; y < texture.height; y++)
			{
				for (int x = 0; x < texture.width; x++)
				{
					Vector3 scaled = Vector3.Scale(new Vector3(x, y, 1), frequency);
					Vector3 offset = scaled + phase;

					float noise = (float)SimplexNoise.Noise(offset.x, offset.y, offset.z);
					noise = (noise + 1.0f) / 2.0f;

					s_Pixels[index] = new Color(noise, noise, noise);
					index++;
				}
			}

			texture.SetPixels(s_Pixels);
			texture.Apply();
		}
	}
}
