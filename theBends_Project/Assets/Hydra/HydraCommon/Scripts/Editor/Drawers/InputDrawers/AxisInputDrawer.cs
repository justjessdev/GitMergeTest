// <copyright file=AxisInputDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.PropertyAttributes.InputAttributes;
using UnityEditor;

namespace Hydra.HydraCommon.Editor.Drawers.InputDrawers
{
	/// <summary>
	/// 	AxisInputDrawer draws the inpspector elements for the AxisInputAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(AxisInputAttribute))]
	public class AxisInputDrawer : AbstractAxesInputDrawer {}
}
