// <copyright file=HydraCommonMenuItems company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Menus
{
	/// <summary>
	/// 	Hydra common menu items.
	/// </summary>
	public static class HydraCommonMenuItems
	{
		public const int WEBSITE_MENU_ITEM_PRIORITY = int.MaxValue - 10;
		public const int DOCUMENTATION_ITEM_PRIORITY = WEBSITE_MENU_ITEM_PRIORITY + 1;

		public const string HYDRA_SUBMENU = "Hydra/";

		public const string WINDOW_MENU = "Window/";
		public const string HYDRA_WINDOW_MENU = WINDOW_MENU + HYDRA_SUBMENU;

		public const string GAMEOBJECT_MENU = "GameObject/";
		public const string HYDRA_GAMEOBJECT_MENU = GAMEOBJECT_MENU + HYDRA_SUBMENU;

		public const string WEBSITE_MENU_ITEM = HYDRA_WINDOW_MENU + "Visit Website";
		public const string HYDRA_ADDRESS = "http://www.thisishydra.com/";

		public const string DOCUMENTATION_MENU_ITEM = HYDRA_WINDOW_MENU + "Documentation";
		public const string DOCUMENTATION_ADDRESS = HYDRA_ADDRESS + "index.php/documentation";

		#region Methods

		/// <summary>
		/// 	Opens the Hydra website.
		/// </summary>
		[MenuItem(WEBSITE_MENU_ITEM, false, WEBSITE_MENU_ITEM_PRIORITY)]
		public static void OpenHydraWebsite()
		{
			Application.OpenURL(HYDRA_ADDRESS);
		}

		/// <summary>
		/// 	Opens the documentation page.
		/// </summary>
		[MenuItem(DOCUMENTATION_MENU_ITEM, false, DOCUMENTATION_ITEM_PRIORITY)]
		public static void OpenDocumentationPage()
		{
			Application.OpenURL(DOCUMENTATION_ADDRESS);
		}

		#endregion
	}
}
