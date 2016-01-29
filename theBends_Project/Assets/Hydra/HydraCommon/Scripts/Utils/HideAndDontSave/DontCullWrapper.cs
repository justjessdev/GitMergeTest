// <copyright file=DontCullWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.Concrete;
using UnityEngine;

namespace Hydra.HydraCommon.Utils.HideAndDontSave
{
	/// <summary>
	/// 	DontCullWrapper provides a callback that is called every time any camera
	/// 	is about to render a frame.
	/// </summary>
	public class DontCullWrapper : AbstractHideAndDontSaveWrapper
	{
		public event EventHandler onWillRenderObjectCallback;

		private Mesh m_Mesh;
		private MeshFilter m_MeshFilter;
		private MeshRenderer m_MeshRenderer;
		private HydraEventHelper m_EventHelper;

		// Cache
		private static Camera[] s_Cameras;
		private static Bounds[] s_CameraBounds;

		#region Messages

		/// <summary>
		/// 	Called when the object is loaded.
		/// </summary>
		protected override void OnEnable(HydraMonoBehaviour parent)
		{
			Subscribe(m_EventHelper);

			base.OnEnable(parent);
		}

		/// <summary>
		/// 	Called when the object goes out of scope.
		/// </summary>
		protected override void OnDisable(HydraMonoBehaviour parent)
		{
			Unsubscribe(m_EventHelper);

			base.OnDisable(parent);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Updates the bounds.
		/// </summary>
		public void UpdateBounds()
		{
			m_Mesh.bounds = CameraCullingBounds();
		}

		#endregion

		/// <summary>
		/// 	Override this method to configure the child upon instantiation, e.g. to add components.
		/// </summary>
		/// <param name="child">Child.</param>
		protected override void ConfigureChild(GameObject child)
		{
			base.ConfigureChild(child);

			m_Mesh = new Mesh();
			m_Mesh.hideFlags = HideFlags.HideAndDontSave;

			m_MeshRenderer = child.AddComponent<MeshRenderer>();

			m_MeshFilter = child.AddComponent<MeshFilter>();
			m_MeshFilter.mesh = m_Mesh;

			m_EventHelper = child.AddComponent<HydraEventHelper>();
			Subscribe(m_EventHelper);
		}

		/// <summary>
		/// 	Destroys the child.
		/// </summary>
		/// <returns>The child.</returns>
		protected override void DestroyChild()
		{
			base.DestroyChild();

			m_Mesh = ObjectUtils.SafeDestroy(m_Mesh);
			m_MeshFilter = ObjectUtils.SafeDestroy(m_MeshFilter);
			m_MeshRenderer = ObjectUtils.SafeDestroy(m_MeshRenderer);
			m_EventHelper = ObjectUtils.SafeDestroy(m_EventHelper);
		}

		/// <summary>
		/// 	Subscribe to the specified eventHelper.
		/// </summary>
		/// <param name="eventHelper">Event helper.</param>
		private void Subscribe(HydraMonoBehaviour eventHelper)
		{
			if (eventHelper == null)
				return;

			eventHelper.onWillRenderObjectCallback += OnChildWillRenderObject;
		}

		/// <summary>
		/// 	Unsubscribe from the specified eventHelper.
		/// </summary>
		/// <param name="eventHelper">Event helper.</param>
		private void Unsubscribe(HydraMonoBehaviour eventHelper)
		{
			if (eventHelper == null)
				return;

			eventHelper.onWillRenderObjectCallback -= OnChildWillRenderObject;
		}

		/// <summary>
		/// 	Raises the will render object event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void OnChildWillRenderObject(object sender, EventArgs e)
		{
			EventHandler handler = onWillRenderObjectCallback;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 	Creates a bounding rect that is visible to all cameras.
		/// </summary>
		/// <returns>The culling bounds.</returns>
		public static Bounds CameraCullingBounds()
		{
			CameraUtils.GetAllCameras(ref s_Cameras);
			Array.Resize(ref s_CameraBounds, s_Cameras.Length);

			for (int index = 0; index < s_Cameras.Length; index++)
			{
				Camera camera = s_Cameras[index];

				// boundsTarget is the center of the camera's frustum, in world coordinates:
				Vector3 camPosition = camera.transform.position;
				Vector3 normCamForward = Vector3.Normalize(camera.transform.forward);
				float boundsDistance = (camera.farClipPlane - camera.nearClipPlane) / 2.0f + camera.nearClipPlane;
				Vector3 boundsTarget = camPosition + (normCamForward * boundsDistance);

				s_CameraBounds[index] = new Bounds(boundsTarget, Vector3.zero);
			}

			return BoundsUtils.CombineBounds(s_CameraBounds);
		}
	}
}
