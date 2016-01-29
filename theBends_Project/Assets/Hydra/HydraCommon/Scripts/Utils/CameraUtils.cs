// <copyright file=CameraUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Provides utility methods for working with cameras.
	/// </summary>
	public static class CameraUtils
	{
		private static Vector3[] s_BoundsCorners;
		private static Vector3[] s_ScreenCorners;
		private static Camera[] s_Cameras;

		/// <summary>
		/// 	Gets the depth of the worldspace point.
		/// </summary>
		/// <returns>The depth.</returns>
		/// <param name="camera">Camera.</param>
		/// <param name="point">Point.</param>
		public static float GetDepth(Camera camera, Vector3 point)
		{
			Vector3 direction = point - camera.transform.position;
			return Vector3.Dot(direction, camera.transform.forward);
		}

		/// <summary>
		/// 	Finds camera by name.
		/// </summary>
		/// <returns>The camera.</returns>
		/// <param name="name">Name.</param>
		public static Camera FindByName(string name)
		{
			GetAllCameras(ref s_Cameras);

			for (int index = 0; index < s_Cameras.Length; index++)
			{
				Camera camera = s_Cameras[index];
				if (camera.name == name)
					return camera;
			}

			return null;
		}

		/// <summary>
		/// 	Returns the screenspace bounds for the given bounding box.
		/// </summary>
		/// <param name="camera">Camera.</param>
		/// <param name="bounds">Bounds.</param>
		public static Bounds ScreenBounds(Camera camera, Bounds bounds)
		{
			BoundsUtils.GetCorners(bounds, ref s_BoundsCorners);
			Array.Resize(ref s_ScreenCorners, s_BoundsCorners.Length);

			for (int index = 0; index < s_BoundsCorners.Length; index++)
				s_ScreenCorners[index] = camera.WorldToScreenPoint(s_BoundsCorners[index]);

			return BoundsUtils.GetBounds(s_ScreenCorners);
		}

		/// <summary>
		/// 	Gets the corners.
		/// </summary>
		/// <param name="camera">Camera.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="corners">Corners.</param>
		public static void GetCorners(Camera camera, float distance, ref Vector3[] corners)
		{
			Array.Resize(ref corners, 4);

			// Top left
			corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 1, distance));

			// Top right
			corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

			// Bottom left
			corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));

			// Bottom right
			corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 0, distance));
		}

		/// <summary>
		/// 	Gets all cameras.
		/// </summary>
		/// <param name="cameras">Cameras.</param>
		/// <param name="sceneViewCameras">If set to <c>true</c>, also returns scene view cameras.</param>
		public static void GetAllCameras(ref Camera[] cameras, bool sceneViewCameras = true)
		{
			Array.Resize(ref cameras, GetCameraCount(sceneViewCameras));

			Camera.GetAllCameras(cameras);

#if UNITY_EDITOR
			if (sceneViewCameras)
			{
				Camera[] sceneCameras = UnityEditor.SceneView.GetAllSceneCameras();
				Array.Copy(sceneCameras, 0, cameras, Camera.allCamerasCount, sceneCameras.Length);
			}
#endif
		}

		/// <summary>
		/// 	Returns the number of cameras in the scene.
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="sceneViewCameras">If set to <c>true</c>, includes scene view cameras.</param>
		public static int GetCameraCount(bool sceneViewCameras = true)
		{
			int output = Camera.allCamerasCount;
#if UNITY_EDITOR
			if (sceneViewCameras)
				output += UnityEditor.SceneView.GetAllSceneCameras().Length;
#endif
			return output;
		}
	}
}
