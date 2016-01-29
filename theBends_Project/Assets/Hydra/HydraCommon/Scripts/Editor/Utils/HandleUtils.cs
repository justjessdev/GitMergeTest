// <copyright file=HandleUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Reflection;
using Hydra.HydraCommon.Utils;
using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	HandleUtils provides utility methods for drawing handles.
	/// </summary>
	public static class HandleUtils
	{
		public const float DOT_SIZE = 0.04f;

		public const string HANDLE_WIRE_MATERIAL_PROPERTY_NAME = "handleWireMaterial";

		private static Triangle3d[] s_WireTriangles;
		private static readonly PropertyInfo s_HandleWireMaterialProperty;

		/// <summary>
		/// 	Initializes the HandleUtils class.
		/// </summary>
		static HandleUtils()
		{
			s_HandleWireMaterialProperty = ReflectionUtils.GetPropertyByName(typeof(HandleUtility),
																			 HANDLE_WIRE_MATERIAL_PROPERTY_NAME);
			if (s_HandleWireMaterialProperty == null)
				throw new Exception("The handles wire material property has been moved. Unity, please make this property public.");
		}

		#region Properties

		/// <summary>
		/// 	Gets the handle wire material.
		/// </summary>
		/// <value>The handle wire material.</value>
		public static Material handleWireMaterial
		{
			get { return s_HandleWireMaterialProperty.GetValue(null, new object[0]) as Material; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Begins GL drawing with the current handles settings.
		/// 	(this is taken from the Handles.DrawLine method)
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="material">Material.</param>
		public static void BeginGL(int mode, Material material)
		{
			material.SetPass(0);

			GL.PushMatrix();
			GL.MultMatrix(Handles.matrix);

			GL.Begin(mode);

			Color color = Handles.color * new Color(1.0f, 1.0f, 1.0f, 0.75f);
			GL.Color(color);
		}

		/// <summary>
		/// 	Ends GL drawing for handles.
		/// </summary>
		public static void EndGL()
		{
			GL.End();
			GL.PopMatrix();
		}

		/// <summary>
		/// 	Returns a normalized direction towards the camera from the given position,
		/// 	considering the current handles matrix.
		/// </summary>
		/// <returns>The direction to the camera.</returns>
		/// <param name="position">Position.</param>
		public static Vector3 ToCamera(Vector3 position)
		{
			Matrix4x4 matrix = Handles.inverseMatrix;
			Vector3 cameraPos = Camera.current.transform.position;

			return matrix.MultiplyPoint(cameraPos) - position;
		}

		/// <summary>
		/// 	Gets the dot size.
		/// </summary>
		/// <returns>The dot size.</returns>
		/// <param name="position">Position.</param>
		public static float GetDotSize(Vector3 position)
		{
			return HandleUtility.GetHandleSize(position) * DOT_SIZE;
		}

		/// <summary>
		/// 	Draws a dot at the given position.
		/// </summary>
		/// <param name="position">Position.</param>
		public static void DrawDot(Vector3 position)
		{
			Handles.DotCap(0, position, Quaternion.identity, GetDotSize(position));
		}

		/// <summary>
		/// 	Draws the wireframe.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public static void DrawWireframe(Mesh mesh)
		{
			MeshUtils.ToTriangles(mesh, ref s_WireTriangles);

			BeginGL(GL.LINES, handleWireMaterial);

			for (int index = 0; index < s_WireTriangles.Length; index++)
			{
				Triangle3d triangle = s_WireTriangles[index];

				if (Vector3.Dot(triangle.normal, ToCamera(triangle.centroid)) < 0.0f)
					continue;

				GL.Vertex(triangle.pointA);
				GL.Vertex(triangle.pointB);

				GL.Vertex(triangle.pointB);
				GL.Vertex(triangle.pointC);

				GL.Vertex(triangle.pointC);
				GL.Vertex(triangle.pointA);
			}

			EndGL();
		}

		/// <summary>
		/// 	Draws the verts.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public static void DrawVerts(Mesh mesh)
		{
			Vector3[] verts = mesh.vertices;
			Vector3[] normals = mesh.normals;

			for (int index = 0; index < verts.Length; index++)
			{
				Vector3 position = verts[index];

				if (Vector3.Dot(normals[index], ToCamera(position)) > 0.0f)
					DrawDot(position);
			}
		}

		/// <summary>
		/// 	Draws a box size handle.
		/// </summary>
		/// <returns>The new box size.</returns>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		public static Vector3 BoxSizeHandle(Quaternion rotation, Vector3 position, Vector3 size)
		{
			float xMin = size.x * -0.5f + position.x;
			float xMax = size.x * 0.5f + position.x;

			float yMin = size.y * -0.5f + position.y;
			float yMax = size.y * 0.5f + position.y;

			float zMin = size.z * -0.5f + position.z;
			float zMax = size.z * 0.5f + position.z;

			// Bottom square
			Handles.DrawLine(rotation * new Vector3(xMin, yMin, zMin), rotation * new Vector3(xMin, yMin, zMax));
			Handles.DrawLine(rotation * new Vector3(xMax, yMin, zMin), rotation * new Vector3(xMax, yMin, zMax));
			Handles.DrawLine(rotation * new Vector3(xMin, yMin, zMin), rotation * new Vector3(xMax, yMin, zMin));
			Handles.DrawLine(rotation * new Vector3(xMin, yMin, zMax), rotation * new Vector3(xMax, yMin, zMax));

			// Top square
			Handles.DrawLine(rotation * new Vector3(xMin, yMax, zMin), rotation * new Vector3(xMin, yMax, zMax));
			Handles.DrawLine(rotation * new Vector3(xMax, yMax, zMin), rotation * new Vector3(xMax, yMax, zMax));
			Handles.DrawLine(rotation * new Vector3(xMin, yMax, zMin), rotation * new Vector3(xMax, yMax, zMin));
			Handles.DrawLine(rotation * new Vector3(xMin, yMax, zMax), rotation * new Vector3(xMax, yMax, zMax));

			// Struts
			Handles.DrawLine(rotation * new Vector3(xMin, yMin, zMin), rotation * new Vector3(xMin, yMax, zMin));
			Handles.DrawLine(rotation * new Vector3(xMin, yMin, zMax), rotation * new Vector3(xMin, yMax, zMax));
			Handles.DrawLine(rotation * new Vector3(xMax, yMin, zMin), rotation * new Vector3(xMax, yMax, zMin));
			Handles.DrawLine(rotation * new Vector3(xMax, yMin, zMax), rotation * new Vector3(xMax, yMax, zMax));

			// Dots
			Vector3 dotPosition = rotation * new Vector3(position.x, yMax, position.z);
			size += Vector3.up * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.up) * 2.0f;

			dotPosition = rotation * new Vector3(position.x, yMin, position.z);
			size += Vector3.up * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.down) * 2.0f;

			dotPosition = rotation * new Vector3(position.x, position.y, zMax);
			size += Vector3.forward * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.forward) * 2.0f;

			dotPosition = rotation * new Vector3(position.x, position.y, zMin);
			size += Vector3.forward * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.back) * 2.0f;

			dotPosition = rotation * new Vector3(xMax, position.y, position.z);
			size += Vector3.right * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.right) * 2.0f;

			dotPosition = rotation * new Vector3(xMin, position.y, position.z);
			size += Vector3.right * NormalMoveHandle(dotPosition, rotation, rotation * Vector3.left) * 2.0f;

			return size;
		}

		/// <summary>
		/// 	Draws a dot cap handle that returns the distance it was dragged along its normal.
		/// </summary>
		/// <returns>The distance the cap was dragged.</returns>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="normal">Normal.</param>
		public static float NormalMoveHandle(Vector3 position, Quaternion rotation, Vector3 normal)
		{
			Color oldColor = Handles.color;

			// Check if the face is facing the other direction
			if (Vector3.Dot(normal, ToCamera(position)) < 0.0f)
				Handles.color *= new Color(1.0f, 1.0f, 1.0f, 0.5f);

			Vector3 newPosition = Handles.FreeMoveHandle(position, rotation, GetDotSize(position), Vector3.zero, Handles.DotCap);

			Handles.color = oldColor;

			if (newPosition == position)
				return 0.0f;

			Vector3 newVector = newPosition - position;
			if (HydraMathUtils.Approximately(Vector3.Dot(newVector, normal), 0.0f))
				return 0.0f;

			// Calculate how far the point was dragged along the normal
			float angle = Vector3.Angle(normal, newVector);

			bool positive = (angle < 90.0f);
			if (angle > 90.0f)
				angle = 180.0f - angle;

			float adjacent = Mathf.Cos(Mathf.Deg2Rad * angle) * newVector.magnitude;

			return (positive) ? adjacent * 1.0f : adjacent * -1.0f;
		}

		/// <summary>
		/// 	As of writing, the Unity radius handle is visually broken when setting the handles matrix.
		/// </summary>
		/// <returns>The radius.</returns>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
		public static float RadiusHandle(Quaternion rotation, Vector3 position, float radius)
		{
			radius = CircleRadiusHandle(rotation, position, radius);
			radius = CircleRadiusHandle(rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f), position, radius);
			return CircleRadiusHandle(rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), position, radius);
		}

		/// <summary>
		/// 	Draws a HemiSphere radius handle.
		/// </summary>
		/// <returns>The new HemiSphere radius.</returns>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
		public static float HemiSphereRadiusHandle(Quaternion rotation, Vector3 position, float radius)
		{
			// Draw the circle
			radius = CircleRadiusHandle(rotation, position, radius);

			// Draw the arcs
			Quaternion arcRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
			Handles.DrawWireArc(position, rotation * arcRotation * Vector3.forward, rotation * arcRotation * Vector3.up, 180.0f,
								radius);

			arcRotation = Quaternion.Euler(90.0f, 90.0f, 0.0f);
			Handles.DrawWireArc(position, rotation * arcRotation * Vector3.forward, rotation * arcRotation * Vector3.up, 180.0f,
								radius);

			// Draw the arcs dot
			Vector3 dotPosition = position + rotation * Vector3.forward * radius;
			radius += NormalMoveHandle(dotPosition, rotation, rotation * Vector3.forward);

			return radius;
		}

		/// <summary>
		/// 	Draws a circle radius handle.
		/// </summary>
		/// <returns>The new radius.</returns>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
		public static float CircleRadiusHandle(Quaternion rotation, Vector3 position, float radius)
		{
			Handles.DrawWireDisc(position, rotation * Vector3.forward, radius);

			Vector3 dotPosition = position + rotation * Vector3.up * radius;
			radius += NormalMoveHandle(dotPosition, rotation, rotation * Vector3.up);

			dotPosition = position + rotation * Vector3.down * radius;
			radius += NormalMoveHandle(dotPosition, rotation, rotation * Vector3.down);

			dotPosition = position + rotation * Vector3.left * radius;
			radius += NormalMoveHandle(dotPosition, rotation, rotation * Vector3.left);

			dotPosition = position + rotation * Vector3.right * radius;
			radius += NormalMoveHandle(dotPosition, rotation, rotation * Vector3.right);

			return radius;
		}

		/// <summary>
		/// 	Draws a cone size handle.
		/// </summary>
		/// <returns>The radius, angle and length in a Vector3 tuple.</returns>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="length">Length.</param>
		public static Vector3 ConeSizeHandle(Quaternion rotation, Vector3 position, float radius, float angle, float length)
		{
			// Base circle
			radius = CircleRadiusHandle(rotation, position, radius);

			// Now we need to do some maths to get the radius of the secondary circle
			float secondaryRadius = radius + Mathf.Tan(Mathf.Deg2Rad * angle) * length;

			// Draw the struts
			Vector3 secondaryCirclePosition = position + rotation * Vector3.forward * length;

			Handles.DrawLine(position + rotation * Vector3.up * radius,
							 secondaryCirclePosition + rotation * Vector3.up * secondaryRadius);
			Handles.DrawLine(position + rotation * Vector3.down * radius,
							 secondaryCirclePosition + rotation * Vector3.down * secondaryRadius);
			Handles.DrawLine(position + rotation * Vector3.left * radius,
							 secondaryCirclePosition + rotation * Vector3.left * secondaryRadius);
			Handles.DrawLine(position + rotation * Vector3.right * radius,
							 secondaryCirclePosition + rotation * Vector3.right * secondaryRadius);

			// Draw the secondary circle
			float newSecondaryRadius = CircleRadiusHandle(rotation, secondaryCirclePosition, secondaryRadius);
			newSecondaryRadius = HydraMathUtils.Max(newSecondaryRadius, radius);

			if (!HydraMathUtils.Approximately(newSecondaryRadius, secondaryRadius))
			{
				float delta = newSecondaryRadius - radius;
				angle = Mathf.Rad2Deg * Mathf.Atan(delta / length);
			}

			// Draw the dot in the middle of the secondary circle
			length += NormalMoveHandle(secondaryCirclePosition, rotation, rotation * Vector3.forward);

			return new Vector3(radius, angle, length);
		}

		#endregion
	}
}
