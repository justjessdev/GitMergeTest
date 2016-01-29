// <copyright file=SceneDrawer company=Hydra>
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
	/// 	SceneDrawer draws the inpspector elements for the SceneAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class SceneDrawer : PropertyDrawer
	{
		private SerializedProperty m_GuidProp;
		private SerializedProperty m_IndexProp;
		private SerializedProperty m_NameProp;

		/// <summary>
		/// 	Finds the properties.
		/// </summary>
		/// <param name="prop">Property.</param>
		protected virtual void FindProperties(SerializedProperty prop)
		{
			m_GuidProp = prop.FindPropertyRelative("m_Guid");
			m_IndexProp = prop.FindPropertyRelative("m_Index");
			m_NameProp = prop.FindPropertyRelative("m_Name");
		}

		#region Override Methods

		/// <summary>
		/// 	Override this method to make your own GUI for the property
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="prop">Property</param>
		/// <param name="label">Label</param>
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, prop);

			FindProperties(prop);

			HydraEditorUtils.SceneField(position, label, m_GuidProp, HydraEditorGUIStyles.enumStyle);

			string path = AssetDatabase.GUIDToAssetPath(m_GuidProp.stringValue);

			m_NameProp.stringValue = string.IsNullOrEmpty(path) ? string.Empty : EditorSceneUtils.GetSceneName(path);
			m_IndexProp.intValue = string.IsNullOrEmpty(path) ? 0 : EditorSceneUtils.GetSceneIndex(path);

			EditorGUI.EndProperty();
		}

		#endregion
	}
}
