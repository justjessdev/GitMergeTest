// <copyright file=DebugUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.Utils.Shapes._2d;
using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	DebugUtils provides util methods for debugging information.
	/// </summary>
	public static class DebugUtils
	{
		/// <summary>
		/// 	Gets the object string.
		/// </summary>
		/// <returns>The object string.</returns>
		/// <param name="item">Item.</param>
		public static string GetObjectString(object item)
		{
			if (item == null)
				return "null";

			return item.ToString();
		}

		/// <summary>
		/// 	Log the specified objects.
		/// </summary>
		/// <param name="objects">Objects.</param>
		public static void Log(params object[] objects)
		{
			string log = string.Empty;

			if (objects.Length > 0)
			{
				log = GetObjectString(objects[0]);

				for (int index = 1; index < objects.Length; index++)
					log = string.Format("{0}, {1}", log, GetObjectString(objects[index]));
			}

			Debug.Log(log);
		}

		/// <summary>
		/// 	Draws a set of lines for visualizing a raycast collision.
		/// </summary>
		/// <param name="hit">Hit.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="reflection">If set to <c>true</c> draws the reflection.</param>
		/// <param name="duration">The amount of time before the line is removed.</param>
		public static void RaycastHitLines(RaycastHit hit, Vector3 direction, bool reflection, float duration)
		{
			Debug.DrawRay(hit.point, hit.normal, Color.red, duration);
			Debug.DrawRay(hit.point, direction * -1.0f, Color.green, duration);

			if (reflection)
				Debug.DrawRay(hit.point, Vector3.Reflect(direction, hit.normal), Color.blue, duration);
		}

		/// <summary>
		/// 	Draws the line.
		/// </summary>
		/// <param name="line">Line.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawLine(Line2d line, Color color, float duration)
		{
			Debug.DrawLine(line.pointA, line.pointB, color, duration);
		}

		/// <summary>
		/// 	Draws the line.
		/// </summary>
		/// <param name="line">Line.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawLine(Line3d line, Color color, float duration)
		{
			Debug.DrawLine(line.pointA, line.pointB, color, duration);
		}

		/// <summary>
		/// 	Draws a cross.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawCross(Vector3 position, Quaternion rotation, Vector3 scale, Color color, float duration)
		{
			scale /= 2.0f;

			// X axis
			Vector3 pointA = rotation * Vector3.Scale(Vector3.left, scale) + position;
			Vector3 pointB = rotation * Vector3.Scale(Vector3.right, scale) + position;

			// Y axis
			Vector3 pointC = rotation * Vector3.Scale(Vector3.up, scale) + position;
			Vector3 pointD = rotation * Vector3.Scale(Vector3.down, scale) + position;

			// Z axis
			Vector3 pointE = rotation * Vector3.Scale(Vector3.forward, scale) + position;
			Vector3 pointF = rotation * Vector3.Scale(Vector3.back, scale) + position;

			Debug.DrawLine(pointA, pointB, color, duration);
			Debug.DrawLine(pointC, pointD, color, duration);
			Debug.DrawLine(pointE, pointF, color, duration);
		}

		/// <summary>
		/// 	Draws the triangle.
		/// </summary>
		/// <param name="triangle">Triangle.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawTriangle(Triangle3d triangle, Color color, float duration)
		{
			DrawLine(triangle.lineA, color, duration);
			DrawLine(triangle.lineB, color, duration);
			DrawLine(triangle.lineC, color, duration);
		}

		/// <summary>
		/// 	Draws the triangle.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawTriangle(Vector3 pointA, Vector3 pointB, Vector3 pointC, Color color, float duration)
		{
			DrawTriangle(new Triangle3d(pointA, pointB, pointC), color, duration);
		}

		/// <summary>
		/// 	Draws the rect.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawRect(Rect rect, Color color, float duration)
		{
			Vector2 topLeft = rect.GetTopLeft();
			Vector2 topRight = rect.GetTopRight();
			Vector2 bottomLeft = rect.GetBottomLeft();
			Vector2 bottomRight = rect.GetBottomRight();

			Debug.DrawLine(topLeft, topRight, color, duration);
			Debug.DrawLine(bottomLeft, bottomRight, color, duration);
			Debug.DrawLine(topLeft, bottomLeft, color, duration);
			Debug.DrawLine(topRight, bottomRight, color, duration);
		}

		/// <summary>
		/// 	Draws the bounds.
		/// </summary>
		/// <param name="bounds">Bounds.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawBounds(Bounds bounds, Color color, float duration)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			// Bottom square
			Debug.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(min.x, min.y, max.z), color, duration);
			Debug.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, min.y, max.z), color, duration);
			Debug.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z), color, duration);
			Debug.DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z), color, duration);

			// Top square
			Debug.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, max.y, max.z), color, duration);
			Debug.DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(max.x, max.y, max.z), color, duration);
			Debug.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z), color, duration);
			Debug.DrawLine(new Vector3(min.x, max.y, max.z), new Vector3(max.x, max.y, max.z), color, duration);

			// Struts
			Debug.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(min.x, max.y, min.z), color, duration);
			Debug.DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z), color, duration);
			Debug.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z), color, duration);
			Debug.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, max.y, max.z), color, duration);
		}

		/// <summary>
		/// 	Draws the normals.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="color">Color.</param>
		/// <param name="duration">Duration.</param>
		public static void DrawNormals(Mesh mesh, Color color, float duration)
		{
			Vector3[] verts = mesh.vertices;
			Vector3[] normals = mesh.normals;

			for (int index = 0; index < mesh.vertexCount; index++)
				Debug.DrawRay(verts[index], normals[index], color, duration);
		}
	}
}
