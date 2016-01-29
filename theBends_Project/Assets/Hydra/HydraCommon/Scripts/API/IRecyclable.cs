// <copyright file=IRecyclable company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

namespace Hydra.HydraCommon.API
{
	/// <summary>
	/// 	IRecyclable is an interface for objects that can have their values
	/// 	reset back to defaults.
	/// </summary>
	public interface IRecyclable
	{
		/// <summary>
		/// 	Resets the instances values back to defaults.
		/// </summary>
		void Reset();
	}
}
