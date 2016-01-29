// <copyright file=IBezierPoint company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.API
{
	public enum TangentMode
	{
		Smooth,
		Corner,
		Symmetric,
		Auto
	}

	public interface IBezierPoint
	{
		/// <summary>
		/// 	Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		Vector3 position { get; set; }

		/// <summary>
		/// 	Gets or sets the tangent mode.
		/// </summary>
		/// <value>The tangent mode.</value>
		TangentMode tangentMode { get; set; }

		/// <summary>
		/// 	Gets or sets the in tangent.
		/// </summary>
		/// <value>The in tangent.</value>
		Vector3 inTangent { get; set; }

		/// <summary>
		/// 	Gets or sets the out tangent.
		/// </summary>
		/// <value>The out tangent.</value>
		Vector3 outTangent { get; set; }
	}
}
