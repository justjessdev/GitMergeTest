// <copyright file=Vector3Drawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Editor.Utils;
using Hydra.HydraCommon.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Drawers
{
	/// <summary>
	/// 	Vector3Drawer presents the Vector3Attribute in the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(Vector3Attribute))]
	public class Vector3Drawer : AbstractVector3Drawer
	{
		private SerializedProperty m_VectorProperty;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected override void FindProperties(SerializedProperty prop)
		{
			base.FindProperties(prop);

			m_VectorProperty = prop.FindPropertyRelative("m_Vector");
		}

		#region Methods

		/// <summary>
		/// 	Draws the content.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="locked">Locked.</param>
		protected override void DrawContent(Rect position, bool locked)
		{
			if (locked)
			{
				HydraEditorUtils.DrawUnindented(
											    () => HydraEditorUtils.UniformVector3Field(position, GUIContent.none, m_VectorProperty));
				return;
			}

			HydraEditorUtils.DrawUnindented(() => HydraEditorUtils.Vector3Field(position, GUIContent.none, m_VectorProperty));
		}

		#endregion
	}
}
