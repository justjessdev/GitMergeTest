// <copyright file=GLUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Extensions;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	GLUtils provides methods for drawing with GL.
	/// </summary>
	public static class GLUtils
	{
		public const int DISC_POINTS = 60;

		// Cache
		private static Vector3[] s_DiscPoints;

		#region Methods

		/// <summary>
		/// 	Draws the quad.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public static void DrawQuad(Rect rect)
		{
			Rect uvs = RectUtils.MinMaxRect(Vector2.zero, Vector2.one);
			DrawQuad(rect, uvs);
		}

		/// <summary>
		/// 	Draws the quad.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="uvs">Uvs.</param>
		public static void DrawQuad(Rect rect, Rect uvs)
		{
			GL.TexCoord(uvs.GetBottomLeft());
			GL.Vertex(rect.GetBottomLeft());

			GL.TexCoord(uvs.GetTopLeft());
			GL.Vertex(rect.GetTopLeft());

			GL.TexCoord(uvs.GetTopRight());
			GL.Vertex(rect.GetTopRight());

			GL.TexCoord(uvs.GetBottomRight());
			GL.Vertex(rect.GetBottomRight());
		}

		/// <summary>
		/// 	Draws a circle with the specified center, normal and radius.
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="normal">Normal.</param>
		/// <param name="radius">Radius.</param>
		public static void DrawCircle(Vector3 center, Vector3 normal, float radius)
		{
			Vector3 from = Vector3.Cross(normal, Vector3.up);

			if (from.sqrMagnitude < 0.001f)
				from = Vector3.Cross(normal, Vector3.right);

			DrawWireArc(center, normal, from, 360.0f, radius);
		}

		/// <summary>
		/// 	Draws the poly line.
		/// </summary>
		/// <param name="points">Points.</param>
		public static void DrawPolyLine(Vector3[] points)
		{
			for (int index = 1; index < points.Length; index++)
			{
				GL.Vertex(points[index]);
				GL.Vertex(points[index - 1]);
			}
		}

		#endregion

		/// <summary>
		/// 	Draws the wire arc.
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="normal">Normal.</param>
		/// <param name="from">From.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="radius">Radius.</param>
		private static void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
		{
			SetDiscSectionPoints(ref s_DiscPoints, center, normal, from, angle, radius);
			DrawPolyLine(s_DiscPoints);
		}

		private static void SetDiscSectionPoints(ref Vector3[] dest, Vector3 center, Vector3 normal, Vector3 from, float angle,
												 float radius)
		{
			Array.Resize(ref dest, DISC_POINTS);

			from = from.normalized;
			Quaternion quaternion = Quaternion.AngleAxis(angle / (DISC_POINTS - 1), normal);

			Vector3 vector = from * radius;
			for (int i = 0; i < DISC_POINTS; i++)
			{
				dest[i] = center + vector;
				vector = quaternion * vector;
			}
		}
	}
}
