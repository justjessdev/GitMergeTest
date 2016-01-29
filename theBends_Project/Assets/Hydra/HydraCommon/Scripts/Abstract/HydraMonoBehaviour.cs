// <copyright file=HydraMonoBehaviour company=Hydra>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Collections.Generic;
using Hydra.HydraCommon.API;
using Hydra.HydraCommon.EventArguments;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	HydraMonoBehaviour simply provides a common base class for all Hydra MonoBehaviours.
	/// </summary>
	public abstract class HydraMonoBehaviour : MonoBehaviour, IRecyclable
	{
		public event EventHandler onUpdateCallback;
		public event EventHandler onFixedUpdateCallback;
		public event EventHandler onWillRenderObjectCallback;
		public event EventHandler<CollisionArguments> onCollisionEnterCallback;

#if UNITY_EDITOR
		/// <summary>
		/// 	When the editor goes out of focus it may be an extended amount of
		/// 	time before it updates again. To avoid processing long update
		/// 	intervals (and potentially locking up) we will ignore updates that
		/// 	take longer than this amount of time.
		/// </summary>
		private const float MAX_EDITOR_UPDATE_TIME = 3.0f;

		private float? m_LastEditorUpdateTime;
#endif

		private List<HydraMonoBehaviourChild> m_MonoBehaviourChildren;

		// Cache
		private Camera m_CachedCamera;
		private Collider m_CachedCollider;
		private Transform m_CachedTransform;
		private Rigidbody m_CachedRigidbody;
		private Renderer m_CachedRenderer;

		#region Optimized Properties

		/// <summary>
		/// 	Gets the camera.
		/// </summary>
		/// <value>The camera.</value>
		public new Camera camera
		{
			get
			{
				if (m_CachedCamera == null)
					m_CachedCamera = base.GetComponent<Camera>();
				return m_CachedCamera;
			}
		}

		/// <summary>
		/// 	Gets the collider.
		/// </summary>
		/// <value>The collider.</value>
		public new Collider collider
		{
			get
			{
				if (m_CachedCollider == null)
					m_CachedCollider = base.GetComponent<Collider>();
				return m_CachedCollider;
			}
		}

		/// <summary>
		/// 	Gets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public new Transform transform
		{
			get
			{
				if (m_CachedTransform == null)
					m_CachedTransform = base.transform;
				return m_CachedTransform;
			}
		}

		/// <summary>
		/// 	Gets the rigidbody.
		/// </summary>
		/// <value>The rigidbody.</value>
		public new Rigidbody rigidbody
		{
			get
			{
				if (m_CachedRigidbody == null)
					m_CachedRigidbody = base.GetComponent<Rigidbody>();
				return m_CachedRigidbody;
			}
		}

		/// <summary>
		/// 	Gets the renderer.
		/// </summary>
		/// <value>The renderer.</value>
		public new Renderer renderer
		{
			get
			{
				if (m_CachedRenderer == null)
					m_CachedRenderer = base.GetComponent<Renderer>();
				return m_CachedRenderer;
			}
		}

		#endregion

		#region Messages

		/// <summary>
		/// 	Resets the instances values back to defaults.
		/// </summary>
		public virtual void Reset()
		{
			ProcessMessage(HydraMonoBehaviourChild.Message.OnReset);
		}

		/// <summary>
		/// 	Called before the first Update.
		/// </summary>
		protected virtual void Start() {}

		/// <summary>
		/// 	Called when the object is instantiated.
		/// </summary>
		protected virtual void Awake() {}

		/// <summary>
		/// 	Called once every frame.
		/// </summary>
		protected virtual void Update()
		{
			EventHandler handler = onUpdateCallback;
			if (handler != null)
				handler(this, EventArgs.Empty);

			ProcessMessage(HydraMonoBehaviourChild.Message.OnUpdate);
		}

		/// <summary>
		/// 	Called every physics timestep.
		/// </summary>
		protected virtual void FixedUpdate()
		{
			EventHandler handler = onFixedUpdateCallback;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 	Called after all Updates have finished.
		/// </summary>
		protected virtual void LateUpdate() {}

		/// <summary>
		/// 	Called when the component is enabled.
		/// </summary>
		protected virtual void OnEnable()
		{
			ProcessMessage(HydraMonoBehaviourChild.Message.OnEnable);

			Subscribe();
		}

		/// <summary>
		/// 	Called when the component is disabled.
		/// </summary>
		protected virtual void OnDisable()
		{
			ProcessMessage(HydraMonoBehaviourChild.Message.OnDisable);

			Unsubscribe();
		}

		/// <summary>
		/// 	Called when culling is about to take place.
		/// </summary>
		protected virtual void OnPreCull() {}

		/// <summary>
		/// 	Called when the editor updates.
		/// </summary>
		protected virtual void OnEditorUpdate()
		{
#if UNITY_EDITOR
			float realTime = Time.realtimeSinceStartup;

			if (m_LastEditorUpdateTime != null)
			{
				float updateTime = realTime - (float)m_LastEditorUpdateTime;

				// Don't process every little update interval
				if (updateTime < Time.fixedDeltaTime)
					return;

				if (updateTime <= MAX_EDITOR_UPDATE_TIME)
					OnEditorUpdate(updateTime);
			}

			m_LastEditorUpdateTime = realTime;
#endif
		}

		/// <summary>
		/// 	Called when the editor updates.
		/// </summary>
		/// <param name="updateTime">Update time.</param>
		protected virtual void OnEditorUpdate(float updateTime) {}

		/// <summary>
		/// 	Called when the attached Camera component is done rendering.
		/// </summary>
		protected virtual void OnPostRender() {}

		/// <summary>
		/// 	Called once for each camera if the object is visible.
		/// </summary>
		protected virtual void OnWillRenderObject()
		{
			EventHandler handler = onWillRenderObjectCallback;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 	OnControllerColliderHit is called when the CharacterController hits a collider while performing a Move.
		/// </summary>
		/// <param name="hit">Hit.</param>
		protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {}

		/// <summary>
		/// 	OnCollisionEnter is called when this collider/rigidbody has begun
		/// 	touching another rigidbody/collider.
		/// </summary>
		/// <param name="collision">Collision.</param>
		protected virtual void OnCollisionEnter(Collision collision)
		{
			EventHandler<CollisionArguments> handler = onCollisionEnterCallback;
			if (handler != null)
				handler(this, new CollisionArguments(collision));
		}

		/// <summary>
		/// 	OnTriggerStay is called every FixedUpdate for every Collider that is
		/// 	touching the trigger.
		/// </summary>
		/// <param name="other">Other.</param>
		protected virtual void OnTriggerStay(Collider other) {}

		/// <summary>
		/// 	OnTriggerEnter is called when the Collider other enters the trigger.
		/// </summary>
		/// <param name="other">Other.</param>
		protected virtual void OnTriggerEnter(Collider other) {}

		/// <summary>
		/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
		/// </summary>
		/// <param name="other">Other.</param>
		protected virtual void OnTriggerExit(Collider other) {}

		/// <summary>
		/// 	Called to draw gizmos in the scene view.
		/// </summary>
		protected virtual void OnDrawGizmos() {}

		/// <summary>
		/// 	Called to draw gizmos in the scene view while selected.
		/// </summary>
		protected virtual void OnDrawGizmosSelected() {}

		/// <summary>
		/// 	Called when the object is destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			ProcessMessage(HydraMonoBehaviourChild.Message.OnDestroy);
		}

		/// <summary>
		/// 	Called to draw to the GUI.
		/// </summary>
		protected virtual void OnGUI() {}

		/// <summary>
		/// 	OnMouseDown is called when the user has pressed the mouse button while over the
		/// 	GUIElement or Collider.
		/// </summary>
		protected virtual void OnMouseDown() {}

		/// <summary>
		/// 	Called when a level finishes loading.
		/// </summary>
		/// <param name="index">Index.</param>
		protected virtual void OnLevelWasLoaded(int index) {}

		#endregion

		/// <summary>
		/// 	Override this method to return any HydraMonoBehaviourChild instances.
		/// </summary>
		/// <returns>The mono behaviour children.</returns>
		protected virtual List<HydraMonoBehaviourChild> GetMonoBehaviourChildren()
		{
			if (m_MonoBehaviourChildren == null)
				m_MonoBehaviourChildren = new List<HydraMonoBehaviourChild>();

			m_MonoBehaviourChildren.Clear();

			return m_MonoBehaviourChildren;
		}

		/// <summary>
		/// 	Processes the message.
		/// </summary>
		/// <param name="message">Message.</param>
		private void ProcessMessage(HydraMonoBehaviourChild.Message message)
		{
			HydraMonoBehaviourChild.ProcessMessage(GetMonoBehaviourChildren(), this, message);
		}

		/// <summary>
		/// 	Subscribe to Unity events.
		/// </summary>
		private void Subscribe()
		{
#if UNITY_EDITOR
			m_LastEditorUpdateTime = null;
			EditorApplication.update += OnEditorUpdate;
#endif
		}

		/// <summary>
		/// 	Unsubscribe from Unity events.
		/// </summary>
		private void Unsubscribe()
		{
#if UNITY_EDITOR
			m_LastEditorUpdateTime = null;
			EditorApplication.update -= OnEditorUpdate;
#endif
		}
	}
}
