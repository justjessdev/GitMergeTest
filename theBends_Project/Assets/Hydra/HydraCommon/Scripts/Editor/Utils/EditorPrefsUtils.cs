// <copyright file=EditorPrefsUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Text;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	/// <summary>
	/// 	EditorPrefsUtils provides utility methods for using editor prefs.
	/// </summary>
	public static class EditorPrefsUtils
	{
		public const string HYDRA_KEY = "HYDRA";
		public const string FOLDOUT_KEY = "FOLDOUT";
		public const string KEY_SPACING = "_";

		private static readonly StringBuilder s_StringBuilder;

		/// <summary>
		/// 	Initializes the EditorPrefsUtils class.
		/// </summary>
		static EditorPrefsUtils()
		{
			s_StringBuilder = new StringBuilder();
		}

		#region Methods

		/// <summary>
		/// 	Gets the bool.
		/// </summary>
		/// <returns>The bool.</returns>
		/// <param name="keys">Keys.</param>
		public static bool GetBool(params string[] keys)
		{
			return GetBool(false, keys);
		}

		/// <summary>
		/// 	Gets the bool.
		/// </summary>
		/// <returns>The bool.</returns>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="keys">Keys.</param>
		public static bool GetBool(bool defaultValue, params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			return EditorPrefs.GetBool(key, defaultValue);
		}

		/// <summary>
		/// 	Sets the bool.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="keys">Keys.</param>
		public static void SetBool(bool value, params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			EditorPrefs.SetBool(key, value);
		}

		/// <summary>
		/// 	Gets the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="keys">Keys.</param>
		public static string GetString(params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			return EditorPrefs.GetString(key);
		}

		/// <summary>
		/// 	Sets the string.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="keys">Keys.</param>
		public static void SetString(string value, params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			EditorPrefs.SetString(key, value);
		}

		/// <summary>
		/// 	Gets the int.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="keys">Keys.</param>
		public static int GetInt(params string[] keys)
		{
			return GetInt(0, keys);
		}

		/// <summary>
		/// 	Gets the int.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="keys">Keys.</param>
		public static int GetInt(int defaultValue, params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			return EditorPrefs.GetInt(key, defaultValue);
		}

		/// <summary>
		/// 	Sets the int.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="keys">Keys.</param>
		public static void SetInt(int value, params string[] keys)
		{
			string key = PrefixHydraKey(keys);
			EditorPrefs.SetInt(key, value);
		}

		/// <summary>
		/// 	Gets the object by uid.
		/// </summary>
		/// <returns>The object by uid.</returns>
		/// <param name="keys">Keys.</param>
		/// <typeparam name="T">The object type.</typeparam>
		public static T GetObjectByUid<T>(params string[] keys) where T : Object
		{
			int uid = GetInt(keys);
			return EditorUtility.InstanceIDToObject(uid) as T;
		}

		/// <summary>
		/// 	Sets the object by uid.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="keys">Keys.</param>
		public static void SetObjectByUid(Object value, params string[] keys)
		{
			int uid = (value != null) ? value.GetInstanceID() : 0;
			SetInt(uid, keys);
		}

		/// <summary>
		/// 	Sets the object by name.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="keys">Keys.</param>
		public static void SetObjectByName(Object value, params string[] keys)
		{
			string name = (value != null) ? value.name : string.Empty;
			SetString(name, keys);
		}

		/// <summary>
		/// 	Gets the state of the foldout.
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="title">Title.</param>
		public static bool GetFoldoutState(GUIContent title)
		{
			return GetFoldoutState(title.text);
		}

		/// <summary>
		/// 	Gets the state of the foldout.
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="title">Title.</param>
		public static bool GetFoldoutState(string title)
		{
			string key = PrefixHydraKey(FOLDOUT_KEY, title);
			return EditorPrefs.GetBool(key, false);
		}

		/// <summary>
		/// 	Sets the state of the foldout.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="state">If set to <c>true</c> state.</param>
		public static void SetFoldoutState(GUIContent title, bool state)
		{
			SetFoldoutState(title.text, state);
		}

		/// <summary>
		/// 	Sets the state of the foldout.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="state">If set to <c>true</c> state.</param>
		public static void SetFoldoutState(string title, bool state)
		{
			string key = PrefixHydraKey(FOLDOUT_KEY, title);
			EditorPrefs.SetBool(key, state);
		}

		/// <summary>
		/// 	For use with EditorPref keys. We prepend some information to try and keep things tidy.
		/// </summary>
		/// <returns>The hydra key.</returns>
		/// <param name="keys">Keys.</param>
		public static string PrefixHydraKey(params string[] keys)
		{
			s_StringBuilder.Length = 0;
			s_StringBuilder.Append(HYDRA_KEY);

			for (int index = 0; index < keys.Length; index++)
			{
				s_StringBuilder.Append(KEY_SPACING);
				s_StringBuilder.Append(keys[index]);
			}

			return s_StringBuilder.ToString();
		}

		#endregion
	}
}
