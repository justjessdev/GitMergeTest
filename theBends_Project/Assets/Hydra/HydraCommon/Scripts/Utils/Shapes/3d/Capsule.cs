// <copyright file=Capsule company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using UnityEngine;

namespace Hydra.HydraCommon.Utils.Shapes._3d
{
	/// <summary>
	/// 	Capsule.
	/// </summary>
	public struct Capsule
	{
		private const float SPHERE_VOLUME_CONST = (4.0f / 3.0f) * Mathf.PI;

		private readonly Vector3 m_Point1;
		private readonly Vector3 m_Point2;
		private readonly float m_Radius;

		#region Properties

		/// <summary>
		/// 	Gets the point1.
		/// </summary>
		/// <value>The point1.</value>
		public Vector3 point1 { get { return m_Point1; } }

		/// <summary>
		/// 	Gets the point2.
		/// </summary>
		/// <value>The point2.</value>
		public Vector3 point2 { get { return m_Point2; } }

		/// <summary>
		/// 	Gets the radius.
		/// </summary>
		/// <value>The radius.</value>
		public float radius { get { return m_Radius; } }

		#endregion

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Hydra.HydraCommon.Utils.Shapes._3d.Capsule"/> struct.
		/// </summary>
		/// <param name="point1">Point1.</param>
		/// <param name="point2">Point2.</param>
		/// <param name="radius">Radius.</param>
		public Capsule(Vector3 point1, Vector3 point2, float radius)
		{
			m_Point1 = point1;
			m_Point2 = point2;
			m_Radius = radius;
		}

		#region Methods

		/// <summary>
		/// 	Gets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		public float GetVolume()
		{
			// Sphere
			float sphereVolume = SPHERE_VOLUME_CONST * radius * radius * radius;

			// Cylinder
			float height = (point1 - point2).magnitude;
			float cylinderVolume = Mathf.PI * radius * radius * height;

			return sphereVolume + cylinderVolume;
		}

		#endregion
	}
}
