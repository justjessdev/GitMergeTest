using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemGroup : EditorWindow	
{
	Vector2 scrollPosition;
	string EmptySelectionString = "No objects selected";

	private static GameObject Parent;
	private static List<GameObject> SelectionArray;
	private static bool groupEnabled;
	private static bool myBool = true;
	private static float myFloat = 1.23f;

	static ItemGroup()
	{
		SelectionArray = new List<GameObject>();
	}

	[MenuItem ("Window/RojUtil/Add-Remove Children")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<ItemGroup> (false, "Item Grouping", true);
	}

	/// <summary>
	/// 	Updates the object container with the editor's current selection.
	/// </summary>
	private static void UpdateSelection()
	{
		SelectionArray.Clear ();
		GameObject[] selection = Selection.gameObjects;
		foreach (GameObject obj in selection) 
		{
			if(IgnoreGameObject(obj))
				continue;

			SelectionArray.Add(obj);
		}

		SelectionArray.Sort(delegate(GameObject x, GameObject y) { return x.name.CompareTo(y.name); });
	}

	/// <summary>
	/// 	<para>Unity Pre-defined Function:</para>
	/// 	<para>Runs each frame the Editor Selection changes.</para>
	/// </summary>	
	void OnSelectionChange()
	{
		UpdateSelection ();
		Repaint ();
	}

	/// <summary>
	/// 	<para>Unity Pre-defined Function:</para>
	/// 	<para>Runs 100 times per second while window has focus.</para>
	/// </summary>
	void OnGUI () {
		Parent = EditorGUILayout.ObjectField ("Parent", Parent, typeof(GameObject), true, null) as GameObject;
		EditorGUILayout.BeginVertical (GUI.skin.box);
		{
			GUI.backgroundColor = new Color (GUI.backgroundColor.r, GUI.backgroundColor.g, GUI.backgroundColor.b, 1.0f);
			scrollPosition = GUILayout.BeginScrollView (scrollPosition);
			{
				if (SelectionArray.Count == 0)
				{
					GUIStyle style = ArrayElementStyle (0);
					GUILayout.BeginHorizontal (style);
					GUILayout.Label (EmptySelectionString);
					GUILayout.EndHorizontal ();
				}
				else
				{
					for (int index = 0; index < SelectionArray.Count; index++) {
						GameObject original = SelectionArray [index];
						
						GUIStyle style = ArrayElementStyle (index);
						GUILayout.BeginHorizontal (style);
						if (original != null)
							GUILayout.Label (original.name);
						GUILayout.EndHorizontal ();
					}
				}
			}
		}
			GUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginHorizontal ();
		// Applies the 
		if (GUILayout.Button ("Add Children")) 
			AttachChildren ();
		if (GUILayout.Button ("Remove Children")) 
			AttachChildren ();
		EditorGUILayout.EndHorizontal ();
	}

	/// <summary>
	/// 	Gets the array element style.
	/// </summary>
	/// <value>The array element style.</value>
	GUIStyle ArrayElementStyle(int index)
	{
		bool even = index % 2 == 0;
		return even ? new GUIStyle("AnimationRowEven") : new GUIStyle("AnimationRowOdd");
	}

	/// <summary>
	/// 	Returns true if the target GameObject should be ignored for replacement.
	/// </summary>
	/// <returns><c>true</c>, if game object is ignored, <c>false</c> otherwise.</returns>
	/// <param name="gameObject">Target GameObject to be tested for replacement.</param>
	private static bool IgnoreGameObject(GameObject gameObject)
	{
		if (gameObject == null)
			return true;
		
		// Don't allow replacing prefabs with prefabs.
		if (PrefabUtility.GetPrefabType (gameObject) == PrefabType.Prefab)
			return true;

		return false;
	}

	// <summary>
	/// 	Returns true if the target GameObject should be ignored for replacement.
	/// </summary>
	/// <returns><c>true</c>, if game object is ignored, <c>false</c> otherwise.</returns>
	/// <param name="gameObject">Target GameObject to be tested for replacement.</param>
	/// <param name="parent">Target parent to test if childed.</param>
	private static bool IgnoreGameObject(GameObject gameObject, GameObject parent)
	{
		if (gameObject == null)
			return true;
		
		// Don't allow replacing prefabs with prefabs.
		if (PrefabUtility.GetPrefabType (gameObject) == PrefabType.Prefab)
			return true;

		// Don't allow ignoring children who are not childed to parent.
		if (gameObject.transform.parent != parent.transform)
			return true;

		return false;
	}

	/// <summary>
	/// 	Childs all children in SelectionArray to the selected Parent.
	/// </summary>
	private static void AttachChildren()
	{
		AttachChildren(SelectionArray, Parent);
	}

	/// <summary>
	/// 	Childs all gameObjects in given List to a parent.
	/// </summary>
	/// <param name="children">List of GameObjects to be childed to parent.</param>
	/// <param name="parent">GameObject that the children will be parented to.</param> 
	public static void AttachChildren (List<GameObject> children, GameObject parent)
	{
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName(typeof(ItemGroup).Name);
		int undoIndex = Undo.GetCurrentGroup();

		foreach (GameObject child in children) 
		{
			AttachChild(child, parent);
		}

		Undo.CollapseUndoOperations(undoIndex);
	}

	/// <summary>
	/// 	Childs gameObject.
	/// </summary>
	/// <param name="child">Child.</param>
	/// <param name="parent">Parent.</param> 
	private static void AttachChild (GameObject child, GameObject parent)
	{
		if (IgnoreGameObject (child))
			return;
		Undo.SetTransformParent (child.transform, parent.transform, "Add object as child to parent");
		//child.transform.parent = parent.transform;
	}

	/// <summary>
	/// 	Removes all children in SelectionArray from the selected Parent.
	/// </summary>
	private static void RemoveChildren()
	{
		RemoveChildren(SelectionArray, Parent);
	}
	
	/// <summary>
	/// 	Removes all GameObjects in given List from a parent.
	/// </summary>
	/// <param name="children">List of GameObjects to be childed to parent.</param>
	/// <param name="parent">GameObject that the children will be parented to.</param> 
	public static void RemoveChildren (List<GameObject> children, GameObject parent)
	{
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName(typeof(ItemGroup).Name);
		int undoIndex = Undo.GetCurrentGroup();
		
		foreach (GameObject child in children) 
		{
			RemoveChild(child, parent);
		}
		
		Undo.CollapseUndoOperations(undoIndex);
	}
	
	/// <summary>
	/// 	Childs gameObject.
	/// </summary>
	/// <param name="child">Child.</param>
	/// <param name="parent">Parent.</param> 
	private static void RemoveChild (GameObject child, GameObject parent)
	{
		if (IgnoreGameObject (child, parent))
			return;
		Undo.SetTransformParent (child.transform, null, "Add object as child to parent");
		//child.transform.parent = parent.transform;
	}
}