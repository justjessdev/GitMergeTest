// <copyright file=HydraEditorWindow company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEditor;

namespace Hydra.HydraCommon.Editor.Windows
{
	/// <summary>
	/// 	HydraEditorWindow is the base class for all Hydra EditorWindows.
	/// </summary>
	public abstract class HydraEditorWindow : EditorWindow
	{
		public const string MENU = "Window/Hydra/";

		/// <summary>
		/// 	Called to draw the window contents.
		/// </summary>
		protected virtual void OnGUI() {}

		/// <summary>
		/// 	Called when the inspector updates.
		/// </summary>
		protected virtual void OnInspectorUpdate()
		{
			Repaint();
		}

		/// <summary>
		/// 	Called whenever the selection changes.
		/// </summary>
		protected virtual void OnSelectionChange() {}
	}
}
