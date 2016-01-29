// <copyright file=HydraEditor company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Inspector
{
	/// <summary>
	/// 	Hydra editor.
	/// </summary>
	public abstract class HydraEditor<T> : UnityEditor.Editor
		where T : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// 	Gets the target cast to a useful type.
		/// </summary>
		/// <value>The cast target.</value>
		public T castTarget { get { return target as T; } }

		/// <summary>
		/// 	Gets a value indicating whether the target is in the scene.
		/// </summary>
		/// <value><c>true</c> if target is in scene; otherwise, <c>false</c>.</value>
		public bool isInScene
		{
			get
			{
				PrefabType prefabType = PrefabUtility.GetPrefabType(castTarget);

				switch (prefabType)
				{
					case PrefabType.None:
						return true;
					case PrefabType.Prefab:
						return false;
					case PrefabType.ModelPrefab:
						return false;
					case PrefabType.PrefabInstance:
						return true;
					case PrefabType.ModelPrefabInstance:
						return true;
					case PrefabType.MissingPrefabInstance:
						return true;
					case PrefabType.DisconnectedPrefabInstance:
						return true;
					case PrefabType.DisconnectedModelPrefabInstance:
						return true;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// 	Gets a value indicating whether this HydraEditor should draw
		/// 	the default inspector.
		/// </summary>
		/// <value><c>true</c> if draw default inspector; otherwise, <c>false</c>.</value>
		public virtual bool drawDefaultInspector { get { return true; } }

		#endregion

		#region Messages

		/// <summary>
		/// 	Called to draw the inspector GUI.
		/// </summary>
		public override void OnInspectorGUI()
		{
			if (drawDefaultInspector)
				base.OnInspectorGUI();

			if (Event.current.type == EventType.ValidateCommand)
			{
				switch (Event.current.commandName)
				{
					case "UndoRedoPerformed":
						RepaintScheduler.RepaintAllViews();
						break;
				}
			}
		}

		/// <summary>
		/// 	Called when the inspector becomes enabled.
		/// </summary>
		protected virtual void OnEnable() {}

		/// <summary>
		/// 	Called to draw the GUI in scene view.
		/// </summary>
		protected virtual void OnSceneGUI()
		{
			Handles.BeginGUI();
			OnSceneViewGUI(SceneView.currentDrawingSceneView);
			Handles.EndGUI();
		}

		/// <summary>
		/// 	Called to draw the GUI in scene view.
		/// </summary>
		/// <param name="sceneView">Scene View.</param>
		protected virtual void OnSceneViewGUI(SceneView sceneView) {}

		#endregion
	}
}
