// <copyright file=GameObjectUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System.Collections.Generic;
using Hydra.HydraCommon.Extensions;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	Util methods for working with GameObjects.
	/// </summary>
	public static class GameObjectUtils
	{
		/// <summary>
		/// 	Finds inactive GameObjects. Will NOT find orphaned GameObjects.
		/// </summary>
		/// <param name="output">Output.</param>
		public static void FindInactive(List<GameObject> output)
		{
			GameObject[] active = GameObject.FindObjectsOfType<GameObject>();

			for (int index = 0; index < active.Length; index++)
			{
				GameObject gameObject = active[index];
				if (gameObject.transform.parent == null)
					gameObject.GetInactiveChildrenRecursive(output);
			}
		}
	}
}
