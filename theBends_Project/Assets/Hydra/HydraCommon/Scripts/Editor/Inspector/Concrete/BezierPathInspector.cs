// <copyright file=BezierPathInspector company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.API;
using Hydra.HydraCommon.Concrete;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Inspector.Concrete
{
	public static class BezierPathInspector
	{
		private static readonly GUIContent s_PointsLabel = new GUIContent("Points", "The bezier points that make up the path.");
		private static readonly GUIContent s_PointElementLabel = new GUIContent("Point");

		private static readonly GUIContent s_ClosedLabel = new GUIContent("Closed",
																		  "When enabled the bezier path forms a complete loop.");

		private static SerializedProperty s_PointsProp;
		private static SerializedProperty s_ClosedProp;

		// Cache
		private static Vector3[] s_BezierPoints;
		private static BezierPointSerializedProperty s_PointA;
		private static BezierPointSerializedProperty s_PointB;
		private static BezierPointSerializedProperty s_PointC;
		private static BezierPointSerializedProperty s_PointD;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="serializedProperty">Serialized property.</param>
		public static void FindProperties(SerializedProperty serializedProperty)
		{
			s_PointsProp = serializedProperty.FindPropertyRelative("m_Points");
			s_ClosedProp = serializedProperty.FindPropertyRelative("m_Closed");
		}

		/// <summary>
		/// 	Draws the inspector.
		/// </summary>
		/// <param name="serializedProperty">Serialized property.</param>
		public static void DrawInspector(SerializedProperty serializedProperty)
		{
			FindProperties(serializedProperty);

			HydraEditorLayoutUtils.ArrayField(s_PointsProp, s_PointsLabel, s_PointElementLabel);
			EditorGUILayout.PropertyField(s_ClosedProp, s_ClosedLabel);
		}

		#region Handles

		/// <summary>
		/// 	Draws the handles.
		/// </summary>
		public static void DrawHandles(SerializedProperty prop)
		{
			FindProperties(prop);

			for (int index = 0; index < s_PointsProp.arraySize; index++)
			{
				// Get all of the points to draw the patch
				IBezierPoint pointA = GetPoint(ref s_PointA, index - 1);
				IBezierPoint pointB = GetPoint(ref s_PointB, index);
				IBezierPoint pointC = GetPoint(ref s_PointC, index + 1);
				IBezierPoint pointD = GetPoint(ref s_PointD, index + 2);

				BezierPointHandle(pointB);

				if (pointC != null)
					BezierSegmentHandle(pointA, pointB, pointC, pointD);

				switch (pointB.tangentMode)
				{
					case TangentMode.Corner:
						BezierPointTangentsHandle(pointA, pointB, pointC);
						break;
					case TangentMode.Smooth:
						BezierPointTangentsHandle(pointA, pointB, pointC);
						break;
					case TangentMode.Auto:
						break;
					case TangentMode.Symmetric:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// 	Draws the handle for a bezier point.
		/// </summary>
		/// <param name="point">Point.</param>
		public static void BezierPointHandle(IBezierPoint point)
		{
			float pointSize = HandleUtils.GetDotSize(point.position);
			point.position = Handles.FreeMoveHandle(point.position, Quaternion.identity, pointSize, Vector3.zero, Handles.DotCap);
		}

		/// <summary>
		/// 	Draws the handle for a bezier points tangents.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		public static void BezierPointTangentsHandle(IBezierPoint pointA, IBezierPoint pointB, IBezierPoint pointC)
		{
			if (pointA != null)
			{
				Vector3 inTangent = BezierPath.GetInTangent(pointA, pointB, pointC);
				pointB.inTangent = TangentHandle(pointB.position, inTangent);
			}

			if (pointC != null)
			{
				Vector3 outTangent = BezierPath.GetOutTangent(pointA, pointB, pointC);
				pointB.outTangent = TangentHandle(pointB.position, outTangent);
			}
		}

		/// <summary>
		/// 	Draws a tangent handle.
		/// </summary>
		/// <returns>The new tangent.</returns>
		/// <param name="pointPosition">Point position.</param>
		/// <param name="tangent">Tangent.</param>
		public static Vector3 TangentHandle(Vector3 pointPosition, Vector3 tangent)
		{
			Vector3 tangentPosition = pointPosition + tangent;
			float pointSize = HandleUtils.GetDotSize(tangentPosition);

			Vector3 newTangentPosition = Handles.FreeMoveHandle(tangentPosition, Quaternion.identity, pointSize, Vector3.zero,
																Handles.DotCap);

			Color oldColor = Handles.color;
			Handles.color = oldColor * new Color(1.0f, 1.0f, 1.0f, 0.5f);
			Handles.DrawLine(pointPosition, newTangentPosition);
			Handles.color = oldColor;

			return newTangentPosition - pointPosition;
		}

		/// <summary>
		/// 	Draws the bezier segment between pointB and pointC.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		/// <param name="pointD">Point d.</param>
		public static void BezierSegmentHandle(IBezierPoint pointA, IBezierPoint pointB, IBezierPoint pointC,
											   IBezierPoint pointD)
		{
			if (Event.current.type != EventType.Repaint)
				return;

			BezierPath.GetSegmentPoints(ref s_BezierPoints, pointA, pointB, pointC, pointD);

			HandleUtils.BeginGL(GL.LINES, HandleUtils.handleWireMaterial);

			for (int index = 0; index < s_BezierPoints.Length - 1; index++)
			{
				GL.Vertex(s_BezierPoints[index]);
				GL.Vertex(s_BezierPoints[index + 1]);
			}

			HandleUtils.EndGL();
		}

		#endregion

		/// <summary>
		/// 	Gets the point.
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="point">Point.</param>
		/// <param name="index">Index.</param>
		private static IBezierPoint GetPoint(ref BezierPointSerializedProperty point, int index)
		{
			if (point == null)
				point = new BezierPointSerializedProperty();

			int arraySize = s_PointsProp.arraySize;
			bool repeat = s_ClosedProp.boolValue;

			if (!repeat && (index < 0 || index >= arraySize))
				return null;

			index = HydraMathUtils.Repeat(index, arraySize);
			point.serializedProperty = s_PointsProp.GetArrayElementAtIndex(index);

			return point;
		}
	}
}
