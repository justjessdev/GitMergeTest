// <copyright file=AxisStateAttribute company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.PropertyAttributes
{
	/// <summary>
	/// 	AxisStateAttribute simply provides a boolean property for each axis.
	/// </summary>
	[Serializable]
	public class AxisStateAttribute : HydraPropertyAttribute
	{
		[SerializeField] private bool m_XState;
		[SerializeField] private bool m_YState;
		[SerializeField] private bool m_ZState;

		#region Properties

		/// <summary>
		/// 	Gets or sets the X axis state.
		/// </summary>
		/// <value>The X axis state.</value>
		public bool xState { get { return m_XState; } set { m_XState = value; } }

		/// <summary>
		/// 	Gets or sets the Y axis state.
		/// </summary>
		/// <value>The Y axis state.</value>
		public bool yState { get { return m_YState; } set { m_YState = value; } }

		/// <summary>
		/// 	Gets or sets the Z axis state.
		/// </summary>
		/// <value>The Z axis state.</value>
		public bool zState { get { return m_ZState; } set { m_ZState = value; } }

		#endregion

		#region Methods

		/// <summary>
		/// 	Sets the states.
		/// </summary>
		/// <param name="states">States.</param>
		public void SetStates(bool states)
		{
			SetStates(states, states, states);
		}

		/// <summary>
		/// 	Sets the states.
		/// </summary>
		/// <param name="x">X state.</param>
		/// <param name="y">Y state.</param>
		/// <param name="z">Z state.</param>
		public void SetStates(bool x, bool y, bool z)
		{
			xState = x;
			yState = y;
			zState = z;
		}

		/// <summary>
		/// 	Locks the vector based on the current axis states.
		/// 	If an axis is set to false, the Vector3 axis will be set to 0.
		/// </summary>
		/// <returns>The locked vector.</returns>
		/// <param name="input">Input.</param>
		public Vector3 LockVector(Vector3 input)
		{
			return LockVector(input, false);
		}

		/// <summary>
		/// 	Locks the vector based on the current axis states.
		/// 	If an axis is set to false, the Vector3 axis will be set to 0. If invertStates
		/// 	is true, the opposite occurs.
		/// </summary>
		/// <returns>The locked vector.</returns>
		/// <param name="input">Input.</param>
		/// <param name="invertStates">If set to <c>true</c> invert axis states.</param>
		public Vector3 LockVector(Vector3 input, bool invertStates)
		{
			return LockVector(input, Vector3.zero, invertStates);
		}

		/// <summary>
		/// 	Locks the vector based on the current axis states.
		/// 	If an axis is set to false, the Vector3 axis will be set to lockPosition.
		/// </summary>
		/// <returns>The vector.</returns>
		/// <param name="input">Input.</param>
		/// <param name="lockPosition">Lock position.</param>
		public Vector3 LockVector(Vector3 input, Vector3 lockPosition)
		{
			return LockVector(input, lockPosition, false);
		}

		/// <summary>
		/// 	Locks the vector based on the current axis states.
		/// 	If an axis is set to false, the Vector3 axis will be set to lockPosition. If invertStates
		/// 	is true, the opposite occurs.
		/// </summary>
		/// <returns>The vector.</returns>
		/// <param name="input">Input.</param>
		/// <param name="lockPosition">Lock position.</param>
		/// <param name="invertStates">If set to <c>true</c> invert states.</param>
		public Vector3 LockVector(Vector3 input, Vector3 lockPosition, bool invertStates)
		{
			bool xState = invertStates ? !m_XState : m_XState;
			bool yState = invertStates ? !m_YState : m_YState;
			bool zState = invertStates ? !m_ZState : m_ZState;

			return new Vector3(xState ? input.x : lockPosition.x, yState ? input.y : lockPosition.y,
							   zState ? input.z : lockPosition.z);
		}

		/// <summary>
		/// 	Locks the quaternion based on the current axis states.
		/// 	If an axis is set to false, the quaternion axis will be set to 0.
		/// </summary>
		/// <returns>The locked quaternion.</returns>
		/// <param name="input">Input.</param>
		public Quaternion LockQuaternion(Quaternion input)
		{
			return LockQuaternion(input, false);
		}

		/// <summary>
		/// 	Locks the quaternion based on the current axis states.
		/// 	If an axis is set to false, the quaternion axis will be set to 0. If invertStates
		/// 	is true, the opposite occurs.
		/// </summary>
		/// <returns>The locked quaternion.</returns>
		/// <param name="input">Input.</param>
		/// <param name="invertStates">If set to <c>true</c> invert axis states.</param>
		public Quaternion LockQuaternion(Quaternion input, bool invertStates)
		{
			bool xState = invertStates ? !m_XState : m_XState;
			bool yState = invertStates ? !m_YState : m_YState;
			bool zState = invertStates ? !m_ZState : m_ZState;

			if (!xState || !yState || !zState)
			{
				Vector3 eulerAngles = input.eulerAngles;
				input = Quaternion.Euler(xState ? eulerAngles.x : 0.0f, yState ? eulerAngles.y : 0.0f, zState ? eulerAngles.z : 0.0f);
			}

			return input;
		}

		/// <summary>
		/// 	Locks the quaternion based on the current axis states.
		/// 	If an axis is set to false, the quaternion axis will be set to the lock rotation.
		/// </summary>
		/// <returns>The locked quaternion.</returns>
		/// <param name="input">Input.</param>
		/// <param name="lockRotation">Lock rotation.</param>
		public Quaternion LockQuaternion(Quaternion input, Quaternion lockRotation)
		{
			if (!m_XState || !m_YState || !m_ZState)
			{
				Vector3 eulerAngles = input.eulerAngles;
				Vector3 lockEulerAngles = lockRotation.eulerAngles;
				input = Quaternion.Euler(m_XState ? eulerAngles.x : lockEulerAngles.x, m_YState ? eulerAngles.y : lockEulerAngles.y,
										 m_ZState ? eulerAngles.z : lockEulerAngles.z);
			}

			return input;
		}

		#endregion
	}
}
