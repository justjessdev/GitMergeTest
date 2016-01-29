// <copyright file=ButtonInputDrawer company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.PropertyAttributes.InputAttributes;
using UnityEditor;

namespace Hydra.HydraCommon.Editor.Drawers.InputDrawers
{
	/// <summary>
	/// 	ButtonInputDrawer draws the inpspector elements for the ButtonInputAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(ButtonInputAttribute))]
	public class ButtonInputDrawer : AbstractAxesInputDrawer {}
}
