// <copyright file=SingletonHydraScriptableObject company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Reflection;
using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	Singleton hydra scriptable object.
	/// 
	/// 	Useful for configuration assets that are modified via inspector.
	/// </summary>
	public abstract class SingletonHydraScriptableObject<T> : HydraScriptableObject
		where T : SingletonHydraScriptableObject<T>
	{
		private const string MODULE_PROPERTY_NAME = "moduleName";
		private const string SUB_DIR_PROPERTY_NAME = "subDirectoryName";

		private static T s_CachedInstance;

		#region Properties

		/// <summary>
		/// 	Gets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		private static string fileName { get { return typeof(T).Name; } }

		/// <summary>
		/// 	Gets the module name.
		/// </summary>
		/// <value>The module.</value>
		private static string module
		{
			get
			{
				PropertyInfo moduleProperty = ReflectionUtils.GetPropertyByName(typeof(T), MODULE_PROPERTY_NAME);
				if (moduleProperty == null)
				{
					throw new Exception(string.Format("{0} needs to implement a static string getter for {1}", typeof(T).Name,
													  MODULE_PROPERTY_NAME));
				}

				return moduleProperty.GetValue(null, null) as string;
			}
		}

		/// <summary>
		/// 	Gets the sub directory name.
		/// </summary>
		/// <value>The sub dir.</value>
		private static string subDir
		{
			get
			{
				PropertyInfo subDirProperty = ReflectionUtils.GetPropertyByName(typeof(T), SUB_DIR_PROPERTY_NAME);
				if (subDirProperty == null)
				{
					throw new Exception(string.Format("{0} needs to implement a static string getter for {1}", typeof(T).Name,
													  SUB_DIR_PROPERTY_NAME));
				}

				return subDirProperty.GetValue(null, null) as string;
			}
		}

		/// <summary>
		/// 	Gets the resource path.
		/// </summary>
		/// <value>The resource path.</value>
		private static string resourcePath { get { return string.Format("{0}/{1}", subDir, fileName); } }

		/// <summary>
		/// 	Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T instance
		{
			get
			{
				if (s_CachedInstance == null)
					s_CachedInstance = Resources.Load<T>(resourcePath);

				if (s_CachedInstance == null)
					throw new Exception(string.Format("No asset made for {0}", typeof(T).Name));

				return s_CachedInstance;
			}
		}

		#endregion
	}
}
