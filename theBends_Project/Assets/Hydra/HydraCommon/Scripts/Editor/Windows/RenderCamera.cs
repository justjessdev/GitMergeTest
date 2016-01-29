// <copyright file=RenderCamera company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.IO;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Windows
{
	/// <summary>
	/// 	Provides tools for rendering a camera to an image.
	/// </summary>
	public class RenderCamera : HydraEditorWindow
	{
		public const string TITLE = "Render Camera";

		public const string PREFS_KEY = "RENDERCAMERA";
		public const string CAMERA_NAME_KEY = "CAMERANAME";
		public const string CAMERA_UID_KEY = "CAMERAUID";
		public const string FILE_PATH_KEY = "PATH";
		public const string RES_X_KEY = "RESX";
		public const string RES_Y_KEY = "RESY";
		public const string OPEN_RENDER_KEY = "OPENRENDER";

		private const string EXTENSION = "png";

		private static readonly GUIContent s_CameraFieldLabel = new GUIContent("Camera");
		private static readonly GUIContent s_PathFieldLabel = new GUIContent("Path");

		private static readonly GUIContent s_OpenRenderLabel = new GUIContent("Open Render",
																			  "Automatically open the resulting image file.");

		/// <summary>
		/// 	Shows the window.
		/// </summary>
		[MenuItem(MENU + TITLE)]
		public static void Init()
		{
			GetWindow<RenderCamera>(false, TITLE, true);
		}

		#region Properties

		/// <summary>
		/// 	Gets or sets the camera.
		/// </summary>
		/// <value>The camera.</value>
		public Camera camera
		{
			get
			{
				Camera camera = EditorPrefsUtils.GetObjectByUid<Camera>(PREFS_KEY, CAMERA_UID_KEY);
				if (camera != null)
					return camera;

				string name = EditorPrefsUtils.GetString(PREFS_KEY, CAMERA_NAME_KEY);
				return CameraUtils.FindByName(name);
			}
			set
			{
				EditorPrefsUtils.SetObjectByUid(value, PREFS_KEY, CAMERA_UID_KEY);
				EditorPrefsUtils.SetObjectByName(value, PREFS_KEY, CAMERA_NAME_KEY);
			}
		}

		/// <summary>
		/// 	Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
		public string filePath
		{
			get { return EditorPrefsUtils.GetString(PREFS_KEY, FILE_PATH_KEY); }
			set { EditorPrefsUtils.SetString(value, PREFS_KEY, FILE_PATH_KEY); }
		}

		/// <summary>
		/// 	Gets or sets the resolution x.
		/// </summary>
		/// <value>The resolution x.</value>
		public int resolutionX
		{
			get
			{
				int res = EditorPrefsUtils.GetInt(1920, PREFS_KEY, RES_X_KEY);
				return HydraMathUtils.Max(res, 1);
			}
			set { EditorPrefsUtils.SetInt(value, PREFS_KEY, RES_X_KEY); }
		}

		/// <summary>
		/// 	Gets or sets the resolution y.
		/// </summary>
		/// <value>The resolution y.</value>
		public int resolutionY
		{
			get
			{
				int res = EditorPrefsUtils.GetInt(1080, PREFS_KEY, RES_Y_KEY);
				return HydraMathUtils.Max(res, 1);
			}
			set { EditorPrefsUtils.SetInt(value, PREFS_KEY, RES_Y_KEY); }
		}

		/// <summary>
		/// 	Sets a value indicating whether the render is opened.
		/// </summary>
		/// <value><c>true</c> if opening render; otherwise, <c>false</c>.</value>
		public bool openRender
		{
			get { return EditorPrefsUtils.GetBool(PREFS_KEY, OPEN_RENDER_KEY); }
			set { EditorPrefsUtils.SetBool(value, PREFS_KEY, OPEN_RENDER_KEY); }
		}

		#endregion

		#region Messages

		/// <summary>
		/// 	Called to draw the window contents.
		/// </summary>
		protected override void OnGUI()
		{
			base.OnGUI();

			HydraEditorLayoutUtils.BeginBox();

			camera = HydraEditorLayoutUtils.CameraSelectionField(s_CameraFieldLabel, camera, true);
			filePath = HydraEditorLayoutUtils.SaveFileField(s_PathFieldLabel, filePath, EXTENSION);

			resolutionX = EditorGUILayout.IntField(HydraEditorUtils.s_AxisXLabel, resolutionX);
			resolutionY = EditorGUILayout.IntField(HydraEditorUtils.s_AxisYLabel, resolutionY);

			HydraEditorLayoutUtils.EndBox();
			HydraEditorLayoutUtils.BeginBox();

			openRender = EditorGUILayout.Toggle(s_OpenRenderLabel, openRender);

			HydraEditorLayoutUtils.EndBox();

			RenderButton();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Draws the render button.
		/// </summary>
		private void RenderButton()
		{
			bool pressed;

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				pressed = GUILayout.Button("Render", HydraEditorGUIStyles.buttonStyle,
										   GUILayout.MinHeight(EditorGUIUtility.singleLineHeight), GUILayout.MinWidth(128.0f));
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();

			if (pressed)
				Render();
		}

		/// <summary>
		/// 	Render the selected camera.
		/// </summary>
		private void Render()
		{
			if (camera == null)
				throw new Exception("No camera selected.");

			if (string.IsNullOrEmpty(filePath))
				throw new Exception("No path specified.");

			RenderTexture renderTexture = RenderTexture.GetTemporary(resolutionX, resolutionY, 24, RenderTextureFormat.ARGB32,
																	 RenderTextureReadWrite.Default, 8);
			RenderTexture oldTexture = camera.targetTexture;

			camera.targetTexture = renderTexture;

			camera.Render();

			RenderTexture oldActiveTexture = RenderTexture.active;
			RenderTexture.active = renderTexture;

			Texture2D image = new Texture2D(resolutionX, resolutionY, TextureFormat.RGB24, false);
			image.hideFlags = HideFlags.HideAndDontSave;
			image.ReadPixels(new Rect(0, 0, resolutionX, resolutionY), 0, 0);
			image.Apply();

			byte[] bytes = image.EncodeToPNG();
			File.WriteAllBytes(filePath, bytes);

			RenderTexture.active = oldActiveTexture;
			camera.targetTexture = oldTexture;

			ObjectUtils.SafeDestroy(image);
			RenderTexture.ReleaseTemporary(renderTexture);

			if (openRender)
				Application.OpenURL(filePath);
		}

		#endregion
	}
}
