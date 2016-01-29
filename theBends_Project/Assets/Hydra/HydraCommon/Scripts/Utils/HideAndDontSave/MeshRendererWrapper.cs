// <copyright file=MeshRendererWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.HideAndDontSave
{
	/// <summary>
	/// 	MeshRendererWrapper provides a temporary MeshRenderer.
	/// </summary>
	public class MeshRendererWrapper : AbstractHideAndDontSaveWrapper
	{
		private Mesh m_Mesh;
		private MeshFilter m_MeshFilter;
		private MeshRenderer m_MeshRenderer;

		#region Properties

		/// <summary>
		/// 	Gets the mesh.
		/// </summary>
		/// <value>The mesh.</value>
		public Mesh mesh { get { return m_Mesh; } }

		/// <summary>
		/// 	Gets the mesh filter.
		/// </summary>
		/// <value>The mesh filter.</value>
		public MeshFilter meshFilter { get { return m_MeshFilter; } }

		/// <summary>
		/// 	Gets the mesh renderer.
		/// </summary>
		/// <value>The mesh renderer.</value>
		public MeshRenderer meshRenderer { get { return m_MeshRenderer; } }

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
		}

		/// <summary>
		/// 	Destroys the child.
		/// </summary>
		/// <returns>The child.</returns>
		protected override void DestroyChild()
		{
			base.DestroyChild();

			m_Mesh = ObjectUtils.SafeDestroy(m_Mesh);
			m_MeshFilter = null;
			m_MeshRenderer = null;
		}
	}
}
