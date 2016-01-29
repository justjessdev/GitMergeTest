// <copyright file=AbstractAxesInputAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes.InputAttributes
{
	/// <summary>
	/// 	AbstractAxesInputAttribute is the base class for inputs with an axes name.
	/// </summary>
	public abstract class AbstractAxesInputAttribute : AbstractInputAttribute
	{
		[SerializeField] private string m_Input;

		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="Hydra.HydraCommon.PropertyAttributes.InputAttributes.AbstractAxesInputAttribute"/> class.
		/// </summary>
		/// <param name="input">Input.</param>
		protected AbstractAxesInputAttribute(string input)
		{
			m_Input = input;
		}

		#region Properties

		/// <summary>
		/// 	Gets or sets the input.
		/// </summary>
		/// <value>The input.</value>
		public string input { get { return m_Input; } set { m_Input = value; } }

		#endregion
	}
}
