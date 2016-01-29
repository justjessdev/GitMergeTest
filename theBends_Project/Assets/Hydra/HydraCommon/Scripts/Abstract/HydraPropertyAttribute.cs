// <copyright file=HydraPropertyAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.API;
using UnityEngine;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	HydraPropertyattribute is the base class for all Hydra PropertyAttributes.
	/// </summary>
	public abstract class HydraPropertyAttribute : PropertyAttribute, IRecyclable
	{
		#region Methods

		/// <summary>
		/// 	Resets the instances values back to defaults.
		/// </summary>
		public virtual void Reset() {}

		/// <summary>
		/// 	Call this method after instantiation to initialize child resources.
		/// </summary>
		public virtual void Enable() {}

		/// <summary>
		/// 	Call this method before the object goes out of scope to ensure
		/// 	any Object resources are destroyed.
		/// </summary>
		public virtual object Destroy()
		{
			return null;
		}

		#endregion

		/// <summary>
		/// 	Creates an instance of the attribute and calls Enable() on the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <typeparam name="T">The attribute type.</typeparam>
		public static T CreateInstance<T>() where T : HydraPropertyAttribute, new()
		{
			T target = new T();
			target.Enable();
			return target;
		}

		/// <summary>
		/// 	Creates an instance of the attribute if the target is null. Calls Enable() on the instance regardless.
		/// </summary>
		/// <returns><c>true</c>, if instance was created, <c>false</c> otherwise.</returns>
		/// <param name="target">Target.</param>
		/// <typeparam name="T">The attribute type.</typeparam>
		public static bool CreateInstance<T>(ref T target) where T : HydraPropertyAttribute, new()
		{
			bool create = target == null;

			if (create)
				target = new T();
			target.Enable();

			return create;
		}

		/// <summary>
		/// 	Destroy the specified attribute.
		/// </summary>
		/// <param name="attribute">Attribute.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Destroy<T>(T attribute) where T : HydraPropertyAttribute
		{
			return (attribute == null) ? null : attribute.Destroy() as T;
		}
	}
}
