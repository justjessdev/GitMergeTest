// <copyright file=MeshUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils.Shapes._3d;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	public static class MeshUtils
	{
		public const int MAX_VERTS = ushort.MaxValue;

		private static Vector3[] s_Tan1;
		private static Vector3[] s_Tan2;
		private static Vector4[] s_Tangents;
		private static Triangle3d[] s_VolumeTriangles;

		/// <summary>
		/// 	Generates a billboard mesh (A 1x1 quad that faces Vector3.forward).
		/// </summary>
		public static Mesh Billboard()
		{
			Mesh output = new Mesh();
			output.name = "Billboard";

			Billboard(output);

			return output;
		}

		/// <summary>
		/// 	Generates billboard geometry (A 1x1 quad that faces Vector3.forward).
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public static void Billboard(Mesh mesh)
		{
			mesh.vertices = new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f),
				new Vector3(-0.5f, 0.5f, 0.0f)
			};

			mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};
			mesh.triangles = new int[] {0, 2, 1, 0, 3, 2};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			RecalculateTangents(mesh);
		}

		/// <summary>
		/// 	Creates mesh geometry for a box with the given size.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="size">Size.</param>
		public static void Box(Mesh mesh, Vector3 size)
		{
			Box(mesh, Vector3.zero, size);
		}

		/// <summary>
		/// 	Creates box mesh geometry with the given center and size.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="center">Center.</param>
		/// <param name="size">Size.</param>
		public static void Box(Mesh mesh, Vector3 center, Vector3 size)
		{
			float xMin = size.x * -0.5f + center.x;
			float xMax = size.x * 0.5f + center.x;
			float yMin = size.y * -0.5f + center.y;
			float yMax = size.y * 0.5f + center.y;
			float zMin = size.z * -0.5f + center.z;
			float zMax = size.z * 0.5f + center.z;

			Vector3 vert0 = new Vector3(xMin, yMin, zMin);
			Vector3 vert1 = new Vector3(xMin, yMin, zMax);
			Vector3 vert2 = new Vector3(xMax, yMin, zMax);
			Vector3 vert3 = new Vector3(xMax, yMin, zMin);
			Vector3 vert4 = new Vector3(xMin, yMax, zMin);
			Vector3 vert5 = new Vector3(xMin, yMax, zMax);
			Vector3 vert6 = new Vector3(xMax, yMax, zMax);
			Vector3 vert7 = new Vector3(xMax, yMax, zMin);

			mesh.vertices = new Vector3[]
			{
				// Bottom square
				vert0, vert1, vert2, vert3,
				
				// Top square
				vert4, vert5, vert6, vert7,

				// Back square
				vert0, vert4, vert7, vert3,

				// Forward square
				vert1, vert5, vert6, vert2,

				// Left square
				vert1, vert5, vert4, vert0,

				// Right square
				vert2, vert6, vert7, vert3
			};

			Vector2 uv0 = new Vector2(0, 0);
			Vector2 uv1 = new Vector2(0, 1);
			Vector2 uv2 = new Vector2(1, 1);
			Vector2 uv3 = new Vector2(1, 0);

			mesh.uv = new Vector2[]
			{
				// Bottom square
				uv3, uv2, uv1, uv0,
				
				// Top square
				uv2, uv3, uv0, uv1,
				
				// Back square
				uv2, uv3, uv0, uv1,
				
				// Forward square
				uv3, uv2, uv1, uv0,
				
				// Left square
				uv0, uv1, uv2, uv3,
				
				// Right square
				uv3, uv2, uv1, uv0
			};

			mesh.triangles = new int[]
			{
				// Bottom square
				0, 2, 1, 0, 3, 2,
				
				// Top square
				4, 5, 6, 7, 4, 6,
				
				// Back square
				8, 9, 10, 10, 11, 8,
				
				// Forward square
				12, 14, 13, 14, 12, 15,
				
				// Left square
				16, 17, 18, 18, 19, 16,
				
				// Right square
				20, 22, 21, 22, 20, 23
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			RecalculateTangents(mesh);
		}

		/// <summary>
		/// 	Copy the specified mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public static Mesh Copy(Mesh mesh)
		{
			return Mesh.Instantiate(mesh);
		}

		/// <summary>
		/// 	Generates the tangents.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public static void RecalculateTangents(Mesh mesh)
		{
			int[] triangles = mesh.triangles;
			Vector3[] vertices = mesh.vertices;
			Vector2[] uv = mesh.uv;
			Vector3[] normals = mesh.normals;

			int triangleCount = triangles.Length;
			int vertexCount = vertices.Length;

			Array.Resize(ref s_Tan1, vertexCount);
			Array.Resize(ref s_Tan2, vertexCount);
			Array.Resize(ref s_Tangents, vertexCount);

			for (long index = 0; index < triangleCount; index += 3)
			{
				long i1 = triangles[index + 0];
				long i2 = triangles[index + 1];
				long i3 = triangles[index + 2];

				Vector3 v1 = vertices[i1];
				Vector3 v2 = vertices[i2];
				Vector3 v3 = vertices[i3];

				Vector2 w1 = uv[i1];
				Vector2 w2 = uv[i2];
				Vector2 w3 = uv[i3];

				float x1 = v2.x - v1.x;
				float x2 = v3.x - v1.x;
				float y1 = v2.y - v1.y;
				float y2 = v3.y - v1.y;
				float z1 = v2.z - v1.z;
				float z2 = v3.z - v1.z;

				float s1 = w2.x - w1.x;
				float s2 = w3.x - w1.x;
				float t1 = w2.y - w1.y;
				float t2 = w3.y - w1.y;

				float div = s1 * t2 - s2 * t1;
				float r = HydraMathUtils.Approximately(div, 0.0f) ? 0.0f : 1.0f / div;

				Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
				Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

				s_Tan1[i1] += sdir;
				s_Tan1[i2] += sdir;
				s_Tan1[i3] += sdir;

				s_Tan2[i1] += tdir;
				s_Tan2[i2] += tdir;
				s_Tan2[i3] += tdir;
			}

			for (long index = 0; index < vertexCount; index++)
			{
				Vector3 normal = normals[index];
				Vector3 tangent = s_Tan1[index];

				Vector3.OrthoNormalize(ref normal, ref tangent);

				s_Tangents[index].x = tangent.x;
				s_Tangents[index].y = tangent.y;
				s_Tangents[index].z = tangent.z;

				s_Tangents[index].w = (Vector3.Dot(Vector3.Cross(normal, tangent), s_Tan2[index]) < 0.0f) ? -1.0f : 1.0f;
			}

			mesh.tangents = s_Tangents;
		}

		/// <summary>
		/// 	Splits the quad into two triangles. Note, corners ABCD are assumed to be
		/// 	in order.
		/// </summary>
		/// <param name="a">Corner A.</param>
		/// <param name="b">Corner B.</param>
		/// <param name="c">Corner C.</param>
		/// <param name="d">Corner D.</param>
		/// <param name="t1">Triangle 1.</param>
		/// <param name="t2">Triangle 1.</param>
		public static void SplitQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Triangle3d t1, out Triangle3d t2)
		{
			float diagonal1 = (a - c).sqrMagnitude;
			float diagonal2 = (b - d).sqrMagnitude;

			if (diagonal1 < diagonal2)
			{
				t1 = new Triangle3d(a, b, c);
				t2 = new Triangle3d(a, c, d);
			}
			else
			{
				t1 = new Triangle3d(a, b, d);
				t2 = new Triangle3d(b, c, d);
			}
		}

		/// <summary>
		/// 	Fills the output array with triangles from the mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="output">Output.</param>
		public static void ToTriangles(Mesh mesh, ref Triangle3d[] output)
		{
			ToTriangles(mesh, Matrix4x4.identity, ref output);
		}

		/// <summary>
		/// 	Fills the output array with triangles from the mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="transform">Transform.</param>
		/// <param name="output">Output.</param>
		public static void ToTriangles(Mesh mesh, Matrix4x4 transform, ref Triangle3d[] output)
		{
			Vector3[] verts = mesh.vertices;
			int[] triangles = mesh.triangles;
			int triangleCount = triangles.Length / 3;

			Array.Resize(ref output, triangleCount);

			for (int index = 0; index < triangleCount; index++)
			{
				int vert1 = triangles[index * 3];
				int vert2 = triangles[index * 3 + 1];
				int vert3 = triangles[index * 3 + 2];

				Vector3 position1 = transform * verts[vert1];
				Vector3 position2 = transform * verts[vert2];
				Vector3 position3 = transform * verts[vert3];

				output[index] = new Triangle3d(position1, position2, position3);
			}
		}

		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="mesh">Mesh.</param>
		public static float GetVolume(Mesh mesh)
		{
			float volume = 0.0f;

			ToTriangles(mesh, ref s_VolumeTriangles);

			for (int index = 0; index < s_VolumeTriangles.Length; index ++)
				volume += s_VolumeTriangles[index].SignedVolume();

			return HydraMathUtils.Abs(volume);
		}
	}
}
