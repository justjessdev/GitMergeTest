// <copyright file=HydraScriptableObject company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Abstract
{
	/// <summary>
	/// 	HydraScriptableObject is the base class for all Hydra ScriptableObjects.
	/// </summary>
	public abstract class HydraScriptableObject : ScriptableObject
	{
		#region Messages

		/// <summary>
		/// 	Called when the object is about to be destroyed.
		/// </summary>
		protected virtual void OnDestroy() {}

		/// <summary>
		/// 	Called when the object is loaded.
		/// </summary>
		protected virtual void OnEnable() {}

		/// <summary>
		/// 	Called when the object goes out of scope.
		/// </summary>
		protected virtual void OnDisable() {}

		#endregion
	}
}
