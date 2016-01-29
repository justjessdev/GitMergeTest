// <copyright file=Vector3CurvesDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Extensions;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Editor.Windows;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.PropertyAttributes;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	/// <summary>
	/// 	Vector3CurvesDrawer presents the Vector3CurvesAttribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(Vector3CurvesAttribute))]
	public class Vector3CurvesDrawer : AbstractVector3Drawer
	{
		public static readonly Color s_ColorX = Color.red;
		public static readonly Color s_ColorY = Color.green;
		public static readonly Color s_ColorZ = Color.blue;
		public static readonly Color s_LockedColor = Color.yellow;
		public static readonly Color s_CurveBackgroundColor = new Color();

		private SerializedProperty m_CurveXProperty;
		private SerializedProperty m_CurveYProperty;
		private SerializedProperty m_CurveZProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected override void FindProperties(SerializedProperty prop)
		{
			base.FindProperties(prop);

			m_CurveXProperty = prop.FindPropertyRelative("m_CurveX");
			m_CurveYProperty = prop.FindPropertyRelative("m_CurveY");
			m_CurveZProperty = prop.FindPropertyRelative("m_CurveZ");
		}

		#region Methods

		/// <summary>
		/// 	Draws the content.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="locked">Locked.</param>
		protected override void DrawContent(Rect position, bool locked)
		{
			HydraEditorUtils.DrawUnindented(
										    () => GUI.Box(position, GUIContent.none, HydraEditorGUIStyles.vector3CurvesBackgroundStyle));

			// Draw the curves
			Rect curvesBounds = GetCurvesBoundingRect();

			if (locked)
			{
				DrawCurveSwatch(position, m_CurveXProperty, s_LockedColor, curvesBounds);

				m_CurveYProperty.animationCurveValue = m_CurveXProperty.animationCurveValue;
				m_CurveZProperty.animationCurveValue = m_CurveXProperty.animationCurveValue;
			}
			else
			{
				DrawCurveSwatch(position, m_CurveXProperty, s_ColorX, curvesBounds);
				DrawCurveSwatch(position, m_CurveYProperty, s_ColorY, curvesBounds);
				DrawCurveSwatch(position, m_CurveZProperty, s_ColorZ, curvesBounds);
			}

			// Draw the foreground button
			DrawForegroundButton(position, locked);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Draws the curve swatch.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="property">Property.</param>
		/// <param name="color">Color.</param>
		/// <param name="curveRanges">Curve ranges.</param>
		private void DrawCurveSwatch(Rect position, SerializedProperty property, Color color, Rect curveRanges)
		{
			Rect curvePosition = new Rect(position);

			// Special case - The curve is a flat line.
			if (HydraMathUtils.Approximately(curveRanges.height, 0.0f))
			{
				curveRanges.y = -0.5f + curveRanges.y;
				curveRanges.height = 1.0f;
			}

			EditorGUIUtility.DrawCurveSwatch(curvePosition, property.animationCurveValue, property, color, s_CurveBackgroundColor,
											 curveRanges);
		}

		/// <summary>
		/// 	Draws the foreground button.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="locked">Locked.</param>
		private void DrawForegroundButton(Rect position, bool locked)
		{
			string name = m_CurveXProperty.GetHashCode().ToString();

			GUI.SetNextControlName(name);

			bool pressed =
				HydraEditorUtils.DrawUnindented(
											    () => GUI.Button(position, GUIContent.none, HydraEditorGUIStyles.vector3CurvesForegroundStyle));

			if (!pressed)
				return;

			GUI.FocusControl(name);

			Vector3CurvesAttribute parent = m_CurveXProperty.GetParent() as Vector3CurvesAttribute;

			if (locked)
				EditCurve(parent.curveX, "Edit XYZ", s_LockedColor);
			else
			{
				GenericMenu menu = new GenericMenu();

				menu.AddItem(new GUIContent("Edit X"), false, () => EditCurve(parent.curveX, "Edit X", s_ColorX));
				menu.AddItem(new GUIContent("Edit Y"), false, () => EditCurve(parent.curveY, "Edit Y", s_ColorY));
				menu.AddItem(new GUIContent("Edit Z"), false, () => EditCurve(parent.curveZ, "Edit Z", s_ColorZ));

				menu.ShowAsContext();
			}
		}

		/// <summary>
		/// 	Shows the curve editor window for the given curve.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="title">Title.</param>
		/// <param name="color">Color.</param>
		private void EditCurve(AnimationCurve curve, string title, Color color)
		{
			CurveEditorWindowWrapper.ShowCurveEditorWindow(title);
			CurveEditorWindowWrapper.curve = curve;
			CurveEditorWindowWrapper.color = color;

			Rect bounds = NormalCurveRendererWrapper.GetBounds(curve);

			if (HydraMathUtils.Approximately(bounds.height, 0.0f))
			{
				bounds.height = bounds.width;
				bounds.y -= bounds.height / 2.0f;
			}

			CurveEditorWindowWrapper.Frame(bounds);
		}

		/// <summary>
		/// 	Gets the curves bounding rect.
		/// </summary>
		/// <returns>The curves bounding rect.</returns>
		private Rect GetCurvesBoundingRect()
		{
			Rect xBounds = NormalCurveRendererWrapper.GetBounds(m_CurveXProperty.animationCurveValue);
			Rect yBounds = NormalCurveRendererWrapper.GetBounds(m_CurveYProperty.animationCurveValue);
			Rect zBounds = NormalCurveRendererWrapper.GetBounds(m_CurveZProperty.animationCurveValue);

			Rect output = new Rect(xBounds);
			output = output.Encapsulate(yBounds);
			return output.Encapsulate(zBounds);
		}

		#endregion
	}
}
