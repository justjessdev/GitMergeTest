// <copyright file=BaseTimerInspector company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Inspector.Abstract
{
	/// <summary>
	/// 	Base timer inspector.
	/// </summary>
	public abstract class BaseTimerInspector<T> : HydraEditor<T>
		where T : AbstractTimer
	{
		public const int WIDTH = 192;
		public const int HEIGHT = 64;

		public const int SCREEN_PADDING_X = 16;
		public const int SCREEN_PADDING_Y = 8;

		public const int INNER_MARGIN = 3;
		public const int INNER_WIDTH = WIDTH - 2 * INNER_MARGIN;

		public const int LINE_HEIGHT = 20;

		public const int LABEL_WIDTH = 64;

		public const string PLAY_NAME = "Play.tif";
		public const string PAUSE_NAME = "Pause.tif";
		public const string STOP_NAME = "Stop.tif";
		public const string RESTART_NAME = "Restart.tif";

		private static readonly int s_WindowId = "TimerWindow".GetHashCode();
		private static readonly List<T> s_Children;

		/// <summary>
		/// 	Initializes the BaseTimerInspector class.
		/// </summary>
		static BaseTimerInspector()
		{
			s_Children = new List<T>();
		}

		#region Messages

		/// <summary>
		/// 	Called to draw the GUI in scene view.
		/// </summary>
		/// <param name="sceneView">Scene View.</param>
		protected override void OnSceneViewGUI(SceneView sceneView)
		{
			base.OnSceneViewGUI(sceneView);

			Rect cameraRect = sceneView.camera.pixelRect;

			Rect guiRect = new Rect(cameraRect.width - (WIDTH + SCREEN_PADDING_X),
									cameraRect.height - (HEIGHT + SCREEN_PADDING_Y), WIDTH, HEIGHT);

			Color backgroundColor = GUI.backgroundColor;
			if (!EditorGUIUtility.isProSkin)
				GUI.backgroundColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.8f);

			Rect windowRect = GUILayout.Window(s_WindowId, guiRect, DrawWindowContents, GetWindowTitle(),
											   HydraEditorGUIStyles.windowStyle);

			GUI.backgroundColor = backgroundColor;

			// Correct for the header
			windowRect.y -= 16;
			HydraEditorUtils.EatMouseInput(windowRect, s_WindowId);
		}

		/// <summary>
		/// 	Draws the window contents.
		/// </summary>
		/// <param name="id">Identifier.</param>
		private void DrawWindowContents(int id)
		{
			ButtonsRow(castTarget);
			TimeField(castTarget);
			TimeScaleField(castTarget);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Gets the window title.
		/// </summary>
		/// <returns>The window title.</returns>
		private string GetWindowTitle()
		{
			string title = castTarget.name;

			s_Children.Clear();
			int children = castTarget.GetComponentsInChildrenRecursive(s_Children);

			if (children > 0)
				title = string.Format("{0} (+{1})", title, children);

			return title;
		}

		#endregion

		/// <summary>
		/// 	Returns true if the timer (or any of it's children) are currently playing.
		/// </summary>
		/// <returns><c>true</c>, if playing, <c>false</c> otherwise.</returns>
		/// <param name="timer">Timer.</param>
		public static bool GetPlayState(T timer)
		{
			List<T> timerAndChildren = GetTimerAndChildren(timer);
			return GetPlayState(timerAndChildren);
		}

		/// <summary>
		/// 	Returns true if any of the timers are currently playing.
		/// </summary>
		/// <returns><c>true</c>, if playing, <c>false</c> otherwise.</returns>
		/// <param name="timers">Timers.</param>
		public static bool GetPlayState(List<T> timers)
		{
			for (int index = 0; index < timers.Count; index++)
			{
				if (timers[index].isPlaying)
					return true;
			}

			return false;
		}

		/// <summary>
		/// 	Draws the buttons row.
		/// </summary>
		/// <param name="timer">Timer.</param>
		public static void ButtonsRow(T timer)
		{
			GUILayout.BeginHorizontal();
			{
				PlayButton(timer);
				StopButton(timer);
				RestartButton(timer);
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// 	Draws the time field.
		/// </summary>
		/// <param name="timer">Timer.</param>
		public static void TimeField(T timer)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = LABEL_WIDTH;

			HydraEditorLayoutUtils.FloatField2Dp("Time", timer.time);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}

		/// <summary>
		/// 	Draws the time scale field.
		/// </summary>
		/// <param name="timer">Timer.</param>
		public static void TimeScaleField(T timer)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = LABEL_WIDTH;

			EditorGUILayout.FloatField("Scale", timer.timeScale);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}

		/// <summary>
		/// 	Draws a play button for the given selection.
		/// </summary>
		public static void PlayButton(T timer)
		{
			string textureName = timer.isPlaying ? PAUSE_NAME : PLAY_NAME;
			Texture2D buttonTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures", textureName);

			if (!GUILayout.Button(buttonTexture, ButtonStyle()))
				return;

			List<T> timerAndChildren = GetTimerAndChildren(timer);
			bool playState = GetPlayState(timerAndChildren);

			for (int index = 0; index < timerAndChildren.Count; index++)
			{
				if (playState)
					timerAndChildren[index].Pause();
				else
					timerAndChildren[index].Play();
			}
		}

		/// <summary>
		/// 	Draws a stop button for the given selection.
		/// </summary>
		public static void StopButton(T timer)
		{
			Texture2D buttonTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures", STOP_NAME);

			if (!GUILayout.Button(buttonTexture, ButtonStyle()))
				return;

			List<T> timerAndChildren = GetTimerAndChildren(timer);

			for (int index = 0; index < timerAndChildren.Count; index++)
				timerAndChildren[index].Stop();
		}

		/// <summary>
		/// 	Draws a restart button for the given selection.
		/// </summary>
		public static void RestartButton(T timer)
		{
			Texture2D buttonTexture = EditorResourceCache.GetResource<Texture2D>("HydraCommon", "Textures", RESTART_NAME);

			if (!GUILayout.Button(buttonTexture, ButtonStyle()))
				return;

			List<T> timerAndChildren = GetTimerAndChildren(timer);

			for (int index = 0; index < timerAndChildren.Count; index++)
				timerAndChildren[index].Restart();
		}

		/// <summary>
		/// 	Gets the timer and children.
		/// </summary>
		/// <returns>The timer and children.</returns>
		/// <param name="timer">Timer.</param>
		private static List<T> GetTimerAndChildren(T timer)
		{
			s_Children.Clear();
			s_Children.Add(timer);
			timer.GetComponentsInChildrenRecursive(s_Children);

			return s_Children;
		}

		/// <summary>
		/// 	Returns the button style.
		/// </summary>
		/// <returns>The style.</returns>
		public static GUIStyle ButtonStyle()
		{
			Rect titleRect = new Rect(INNER_MARGIN, INNER_MARGIN, INNER_WIDTH, LINE_HEIGHT);
			Rect buttonRect = new Rect(titleRect);
			buttonRect.width -= 2 * INNER_MARGIN;
			buttonRect.width /= 3.0f;

			GUIStyle buttonStyle = HydraEditorGUIStyles.PictureButtonStyle(buttonRect);

			return buttonStyle;
		}
	}
}
