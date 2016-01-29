// <copyright file=IFactory company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

namespace Hydra.HydraCommon.API
{
	/// <summary>
	/// 	IFactory classes are responsible for instantiating objects.
	/// </summary>
	public interface IFactory<out T>
	{
		/// <summary>
		/// 	Creates a new instance of T.
		/// </summary>
		T Create();
	}
}
