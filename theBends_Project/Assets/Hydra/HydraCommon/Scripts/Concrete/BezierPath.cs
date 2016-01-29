// <copyright file=BezierPath company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using Hydra.HydraCommon.Abstract;
using Hydra.HydraCommon.API;
using Hydra.HydraCommon.Extensions;
using Hydra.HydraCommon.PropertyAttributes;
using Hydra.HydraCommon.Utils;
using UnityEngine;

namespace Hydra.HydraCommon.Concrete
{
	/// <summary>
	/// 	Bezier path.
	/// </summary>
	[Serializable]
	public class BezierPath : HydraMonoBehaviourChild
	{
		// This value determines the accuracy of mapping linear deltas onto
		// a bezier patch. Higher numbers = greater accuracy.
		public const int LENGTH_LINE_SEGMENTS = 50;
		public const float DELTA_LINE_SEGMENTS = 1.0f / LENGTH_LINE_SEGMENTS;

		// When calculating the normal we cheat and use a slightly incremented delta
		// to find a new tangent.
		public const float NORMAL_INTERVAL = 0.001f;

		public static readonly float s_Root2 = Mathf.Sqrt(2.0f);

		[SerializeField] private BezierPointAttribute[] m_Points;
		[SerializeField] private bool m_Closed;

		// Cache
		private static float[] s_SegmentPointDeltas;

		#region Properties

		/// <summary>
		/// 	Gets or sets the points.
		/// </summary>
		/// <value>The points.</value>
		public BezierPointAttribute[] points { get { return m_Points; } set { m_Points = value; } }

		/// <summary>
		/// 	Gets or sets a value indicating whether this BezierPath is closed.
		/// </summary>
		/// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
		public bool closed { get { return m_Closed; } set { m_Closed = value; } }

		#endregion

		#region Messages

		/// <summary>
		/// 	Called when the parent is enabled.
		/// </summary>
		protected override void OnEnable(HydraMonoBehaviour parent)
		{
			if (m_Points == null)
				m_Points = new BezierPointAttribute[0];

			base.OnEnable(parent);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Gets the first point.
		/// </summary>
		/// <returns>The first point.</returns>
		public BezierPointAttribute GetFirstPoint()
		{
			return (m_Points.Length == 0) ? null : m_Points[0];
		}

		/// <summary>
		/// 	Gets the last point.
		/// </summary>
		/// <returns>The last point.</returns>
		public BezierPointAttribute GetLastPoint()
		{
			return (m_Points.Length == 0) ? null : m_Points[m_Points.Length - 1];
		}

		/// <summary>
		/// 	Gets the previous point in the path. If this is the first point and
		/// 	and the path is closed, this method will return the last point.
		/// </summary>
		/// <returns>The previous point.</returns>
		/// <param name="point">Point.</param>
		public BezierPointAttribute GetPreviousPoint(BezierPointAttribute point)
		{
			int index = m_Points.IndexOf(point);
			return GetPreviousPoint(index);
		}

		/// <summary>
		/// 	Gets the previous point in the path. If this is the first point and
		/// 	and the path is closed, this method will return the last point.
		/// </summary>
		/// <returns>The previous point.</returns>
		/// <param name="index">Index.</param>
		public BezierPointAttribute GetPreviousPoint(int index)
		{
			if (index == 0)
				return m_Closed ? GetLastPoint() : null;
			return m_Points[index - 1];
		}

		/// <summary>
		/// 	Gets the next point in the path. If this is the last point and
		/// 	and the path is closed, this method will return the first point.
		/// </summary>
		/// <returns>The next point.</returns>
		/// <param name="point">Point.</param>
		public BezierPointAttribute GetNextPoint(BezierPointAttribute point)
		{
			int index = m_Points.IndexOf(point);
			return GetNextPoint(index);
		}

		/// <summary>
		/// 	Gets the next point in the path. If this is the last point and
		/// 	and the path is closed, this method will return the first point.
		/// </summary>
		/// <returns>The next point.</returns>
		/// <param name="index">Index.</param>
		public BezierPointAttribute GetNextPoint(int index)
		{
			if (index == m_Points.Length - 1)
				return m_Closed ? GetFirstPoint() : null;
			return m_Points[index + 1];
		}

		/// <summary>
		/// 	Returns the position at the given delta.
		/// </summary>
		/// <param name="delta">Delta.</param>
		public Vector3 Interpolate(float delta)
		{
			if (m_Points.Length == 0)
				throw new Exception("Path has no points.");

			if (m_Points.Length == 1 && !m_Closed)
				return m_Points[0].position;

			BezierPointAttribute pointA;
			BezierPointAttribute pointB;
			float patchDelta;

			PatchForDelta(out pointA, out pointB, out patchDelta, delta);

			return Interpolate(pointA, pointB, patchDelta);
		}

		/// <summary>
		/// 	Returns the tangent at the given delta.
		/// </summary>
		/// <param name="delta">Delta.</param>
		public Vector3 Tangent(float delta)
		{
			if (m_Points.Length == 0)
				throw new Exception("Path has no points.");

			if (m_Points.Length == 1 && !m_Closed)
				return Vector3.right;

			BezierPointAttribute pointA;
			BezierPointAttribute pointB;
			float patchDelta;

			PatchForDelta(out pointA, out pointB, out patchDelta, delta);

			return Tangent(pointA, pointB, patchDelta);
		}

		/// <summary>
		/// 	Returns the normal at the given delta.
		/// </summary>
		/// <param name="delta">Delta.</param>
		public Vector3 Normal(float delta)
		{
			if (m_Points.Length == 0)
				throw new Exception("Path has no points.");

			if (m_Points.Length == 1 && !m_Closed)
				return Vector3.up;

			BezierPointAttribute pointA;
			BezierPointAttribute pointB;
			float patchDelta;

			PatchForDelta(out pointA, out pointB, out patchDelta, delta);

			return Normal(pointA, pointB, patchDelta);
		}

		/// <summary>
		/// 	Interpolate returns an interpolated value on the curve between pointA and pointB.
		/// 	Be aware that this value is NOT linear.
		/// </summary>
		/// <returns>The interpolated value.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="delta">Delta.</param>
		public Vector3 Interpolate(BezierPointAttribute pointA, BezierPointAttribute pointB, float delta)
		{
			BezierPointAttribute previousPoint = GetPreviousPoint(pointA);
			BezierPointAttribute nextPoint = GetNextPoint(pointB);

			Vector3 outTangent = GetOutTangent(previousPoint, pointA, pointB);
			Vector3 inTangent = GetInTangent(pointA, pointB, nextPoint);

			float x = ComputeBezier(pointA.position.x, outTangent.x, inTangent.x, pointB.position.x, delta);
			float y = ComputeBezier(pointA.position.y, outTangent.y, inTangent.y, pointB.position.y, delta);
			float z = ComputeBezier(pointA.position.z, outTangent.z, inTangent.z, pointB.position.z, delta);

			return new Vector3(x, y, z);
		}

		/// <summary>
		/// 	Returns the positions on the curve between the two points at the given deltas.
		/// 	
		/// 	This method is faster than calling Interpolate multiple times for the same patch.
		/// </summary>
		/// <returns>The interpolated positions.</returns>
		/// <param name="output">Output.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="deltas">Deltas.</param>
		public void Interpolate(ref Vector3[] output, BezierPointAttribute pointA, BezierPointAttribute pointB, float[] deltas)
		{
			BezierPointAttribute previousPoint = GetPreviousPoint(pointA);
			BezierPointAttribute nextPoint = GetNextPoint(pointB);

			Interpolate(ref output, previousPoint, pointA, pointB, nextPoint, deltas);
		}

		/// <summary>
		/// 	Returns the tangent at the given non-linear delta.
		/// </summary>
		/// <returns>The tangent vector.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="delta">Delta.</param>
		public Vector3 Tangent(BezierPointAttribute pointA, BezierPointAttribute pointB, float delta)
		{
			BezierPointAttribute previousPoint = GetPreviousPoint(pointA);
			BezierPointAttribute nextPoint = GetNextPoint(pointB);

			Vector3 outTangent = GetOutTangent(previousPoint, pointA, pointB);
			Vector3 inTangent = GetInTangent(pointA, pointB, nextPoint);

			float x = ComputeBezierDerivative(pointA.position.x, outTangent.x, inTangent.x, pointB.position.x, delta);
			float y = ComputeBezierDerivative(pointA.position.y, outTangent.y, inTangent.y, pointB.position.y, delta);
			float z = ComputeBezierDerivative(pointA.position.z, outTangent.z, inTangent.z, pointB.position.z, delta);

			return new Vector3(x, y, z);
		}

		/// <summary>
		/// 	Returns the normal at the given non-linear delta.
		/// </summary>
		/// <returns>The normal vector.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="delta">Delta.</param>
		public Vector3 Normal(BezierPointAttribute pointA, BezierPointAttribute pointB, float delta)
		{
			Vector3 position = Interpolate(pointA, pointB, delta);
			Vector3 tangent = Tangent(pointA, pointB, delta).normalized;

			// Cheat to get the next point along the curve
			float nextDelta = delta + NORMAL_INTERVAL;

			Vector3 position2 = Interpolate(pointA, pointB, nextDelta);
			Vector3 tangent2 = Tangent(pointA, pointB, nextDelta);
			tangent2 -= (position2 - position);
			tangent2 = tangent2.normalized;

			Vector3 c =
				new Vector3(tangent2.y * tangent.z - tangent2.z * tangent.y, tangent2.z * tangent.x - tangent2.x * tangent.z,
							tangent2.x * tangent.y - tangent2.y * tangent.x).normalized;

			Matrix4x4 r = Matrix4x4.identity;
			r.SetRow(0, new Vector4(c.x * c.x, c.x * c.y - c.z, c.x * c.z + c.y));
			r.SetRow(1, new Vector4(c.x * c.y + c.z, c.y * c.y, c.y * c.z - c.x));
			r.SetRow(2, new Vector4(c.x * c.z - c.y, c.y * c.z + c.x, c.z * c.z));

			return r.MultiplyVector(tangent);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Provides the patch and patch delta for the given path delta.
		/// </summary>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="patchDelta">Patch delta.</param>
		/// <param name="delta">Delta.</param>
		private void PatchForDelta(out BezierPointAttribute pointA, out BezierPointAttribute pointB, out float patchDelta,
								   float delta)
		{
			int patches = m_Points.Length - 1;
			if (m_Closed)
				patches++;

			if (patches < 1)
				throw new Exception("No patches in the path!");

			delta = Mathf.Repeat(delta, 1.0f);

			int patchIndex = HydraMathUtils.FloorToInt(delta * patches);

			pointA = m_Points[patchIndex];
			pointB = GetNextPoint(patchIndex);

			float singlePatchDelta = 1.0f / patches;

			patchDelta = delta - (patchIndex * singlePatchDelta);
			patchDelta *= patches;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// 	Returns an interpolated value between two points on a single axis. This is NOT linear.
		/// </summary>
		/// <returns>The interpolated value.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="outNormal">Out normal.</param>
		/// <param name="inNormal">In normal.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="delta">Delta.</param>
		public static float ComputeBezier(float pointA, float outNormal, float inNormal, float pointB, float delta)
		{
			float outNormalPosition = pointA + outNormal;
			float inNormalPosition = pointB + inNormal;

			float inverseDelta = 1.0f - delta;

			float firstTerm = (inverseDelta * inverseDelta * inverseDelta) * pointA;
			float secondTerm = 3.0f * (inverseDelta * inverseDelta) * delta * outNormalPosition;
			float thirdTerm = 3.0f * inverseDelta * (delta * delta) * inNormalPosition;
			float fourthTerm = (delta * delta * delta) * pointB;

			return firstTerm + secondTerm + thirdTerm + fourthTerm;
		}

		/// <summary>
		/// 	Returns the tangent at a given delta between two points on a single axis. The delta is NOT linear.
		/// </summary>
		/// <returns>The tangent.</returns>
		/// <param name="pointA">Point a.</param>
		/// <param name="outNormal">Out normal.</param>
		/// <param name="inNormal">In normal.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="delta">Delta.</param>
		public static float ComputeBezierDerivative(float pointA, float outNormal, float inNormal, float pointB, float delta)
		{
			float outNormalPosition = pointA + outNormal;
			float inNormalPosition = pointB + inNormal;

			float deltaSquare = delta * delta;
			float inverseDelta = 1.0f - delta;
			float inverseDeltaSquare = inverseDelta * inverseDelta;

			float firstTerm = -3.0f * pointA * inverseDeltaSquare;
			float secondTerm = outNormalPosition * (3.0f * inverseDeltaSquare - 6.0f * (1.0f - delta) * delta);
			float thirdTerm = inNormalPosition * (6.0f * (1.0f - delta) * delta - 3.0f * deltaSquare);
			float fourthTerm = 3.0f * pointB * deltaSquare;

			return firstTerm + secondTerm + thirdTerm + fourthTerm;
		}

		/// <summary>
		/// 	Creates a bezier circle path.
		/// </summary>
		/// <param name="radius">Radius.</param>
		public static BezierPath Circle(float radius)
		{
			BezierPath path = CreateInstance<BezierPath>(null);
			path.closed = true;

			float controlDistance = radius * 4.0f * (s_Root2 - 1.0f) / 3.0f;

			Quaternion offset = Quaternion.identity;
			Quaternion increment = Quaternion.Euler(0.0f, 0.0f, 90.0f);

			BezierPointAttribute[] points = new BezierPointAttribute[4];

			for (int index = 0; index < 4; index++)
			{
				Vector3 position = Vector3.up * radius;
				Vector3 inTangent = Vector3.right * controlDistance;
				Vector3 outTangent = Vector3.left * controlDistance;

				BezierPointAttribute point = BezierPointAttribute.CreateInstance<BezierPointAttribute>();

				point.tangentMode = TangentMode.Smooth;
				point.position = offset * position;
				point.inTangent = offset * inTangent;
				point.outTangent = offset * outTangent;

				points[index] = point;

				offset *= increment;
			}

			path.points = points;

			return path;
		}

		/// <summary>
		/// 	Gets the in tangent.
		/// </summary>
		/// <returns>The in tangent.</returns>
		/// <param name="previousPoint">Previous point.</param>
		/// <param name="point">Point.</param>
		/// <param name="nextPoint">Next point.</param>
		public static Vector3 GetInTangent(IBezierPoint previousPoint, IBezierPoint point, IBezierPoint nextPoint)
		{
			Vector3 inTangent;
			Vector3 outTangent;

			GetTangents(out inTangent, out outTangent, previousPoint, point, nextPoint);

			return inTangent;
		}

		/// <summary>
		/// 	Gets the out tangent.
		/// </summary>
		/// <returns>The out tangent.</returns>
		/// <param name="previousPoint">Previous point.</param>
		/// <param name="point">Point.</param>
		/// <param name="nextPoint">Next point.</param>
		public static Vector3 GetOutTangent(IBezierPoint previousPoint, IBezierPoint point, IBezierPoint nextPoint)
		{
			Vector3 inTangent;
			Vector3 outTangent;

			GetTangents(out inTangent, out outTangent, previousPoint, point, nextPoint);

			return outTangent;
		}

		/// <summary>
		/// 	Gets the tangents.
		/// </summary>
		/// <param name="inTangent">In tangent.</param>
		/// <param name="outTangent">Out tangent.</param>
		/// <param name="previousPoint">Previous point.</param>
		/// <param name="point">Point.</param>
		/// <param name="nextPoint">Next point.</param>
		public static void GetTangents(out Vector3 inTangent, out Vector3 outTangent, IBezierPoint previousPoint,
									   IBezierPoint point, IBezierPoint nextPoint)
		{
			switch (point.tangentMode)
			{
				case TangentMode.Smooth:
					inTangent = point.inTangent;
					outTangent = point.inTangent * -1.0f;
					break;

				case TangentMode.Corner:
					inTangent = point.inTangent;
					outTangent = point.outTangent;
					break;

				case TangentMode.Symmetric:
					GetSymmetricTangents(out inTangent, out outTangent, previousPoint, point, nextPoint);
					break;

				case TangentMode.Auto:
					GetAutoTangents(out inTangent, out outTangent, previousPoint, point, nextPoint);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 	Gets the symmetric tangents.
		/// </summary>
		/// <param name="inTangent">In tangent.</param>
		/// <param name="outTangent">Out tangent.</param>
		/// <param name="previousPoint">Previous point.</param>
		/// <param name="point">Point.</param>
		/// <param name="nextPoint">Next point.</param>
		private static void GetSymmetricTangents(out Vector3 inTangent, out Vector3 outTangent, IBezierPoint previousPoint,
												 IBezierPoint point, IBezierPoint nextPoint)
		{
			Vector3 previousToNext;
			Vector3 thisToPrevious = Vector3.zero;
			Vector3 thisToNext = Vector3.zero;

			if (previousPoint != null)
				thisToPrevious = previousPoint.position - point.position;

			if (nextPoint != null)
				thisToNext = nextPoint.position - point.position;

			if (previousPoint == null)
				previousToNext = thisToNext;
			else if (nextPoint == null)
				previousToNext = thisToPrevious;
			else
				previousToNext = nextPoint.position - previousPoint.position;

			float inTangentMagnitude = thisToPrevious.magnitude / 3.0f;
			float outTangentMagnitude = thisToNext.magnitude / 3.0f;
			float averageMagnitude = (inTangentMagnitude + outTangentMagnitude) / 2.0f;

			inTangent = -1.0f * averageMagnitude * previousToNext.normalized;
			outTangent = averageMagnitude * previousToNext.normalized;
		}

		/// <summary>
		/// 	Gets the auto tangents.
		/// </summary>
		/// <param name="inTangent">In tangent.</param>
		/// <param name="outTangent">Out tangent.</param>
		/// <param name="previousPoint">Previous point.</param>
		/// <param name="point">Point.</param>
		/// <param name="nextPoint">Next point.</param>
		private static void GetAutoTangents(out Vector3 inTangent, out Vector3 outTangent, IBezierPoint previousPoint,
											IBezierPoint point, IBezierPoint nextPoint)
		{
			Vector3 previousToNext;
			Vector3 thisToPrevious = Vector3.zero;
			Vector3 thisToNext = Vector3.zero;

			if (previousPoint != null)
				thisToPrevious = previousPoint.position - point.position;

			if (nextPoint != null)
				thisToNext = nextPoint.position - point.position;

			if (previousPoint == null)
				previousToNext = thisToNext;
			else if (nextPoint == null)
				previousToNext = thisToPrevious;
			else
				previousToNext = nextPoint.position - previousPoint.position;

			float inTangentMagnitude = thisToPrevious.magnitude / 3.0f;
			float outTangentMagnitude = thisToNext.magnitude / 3.0f;

			inTangent = -1.0f * inTangentMagnitude * previousToNext.normalized;
			outTangent = outTangentMagnitude * previousToNext.normalized;
		}

		/// <summary>
		/// 	Using the curve between pointA and pointB we take a discrete number of line segments
		/// 	and return all of the points that make those lines.
		/// </summary>
		/// <returns>The segment points.</returns>
		/// <param name="output">Output.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		/// <param name="pointD">Point d.</param>
		public static void GetSegmentPoints(ref Vector3[] output, IBezierPoint pointA, IBezierPoint pointB,
											IBezierPoint pointC, IBezierPoint pointD)
		{
			Array.Resize(ref s_SegmentPointDeltas, LENGTH_LINE_SEGMENTS + 1);

			for (int index = 0; index < LENGTH_LINE_SEGMENTS + 1; index++)
				s_SegmentPointDeltas[index] = DELTA_LINE_SEGMENTS * index;

			Interpolate(ref output, pointA, pointB, pointC, pointD, s_SegmentPointDeltas);
		}

		/// <summary>
		/// 	Returns the positions on the curve between the two points at the given deltas.
		/// 	
		/// 	This method is faster than calling Interpolate multiple times for the same patch.
		/// </summary>
		/// <returns>The interpolated positions.</returns>
		/// <param name="output">Output.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="pointC">Point c.</param>
		/// <param name="pointD">Point d.</param>
		/// <param name="deltas">Deltas.</param>
		public static void Interpolate(ref Vector3[] output, IBezierPoint pointA, IBezierPoint pointB, IBezierPoint pointC,
									   IBezierPoint pointD, float[] deltas)
		{
			Array.Resize(ref output, deltas.Length);

			Vector3 outTangent = GetOutTangent(pointA, pointB, pointC);
			Vector3 inTangent = GetInTangent(pointB, pointC, pointD);

			for (int index = 0; index < deltas.Length; index++)
			{
				float delta = deltas[index];

				float x = ComputeBezier(pointB.position.x, outTangent.x, inTangent.x, pointC.position.x, delta);
				float y = ComputeBezier(pointB.position.y, outTangent.y, inTangent.y, pointC.position.y, delta);
				float z = ComputeBezier(pointB.position.z, outTangent.z, inTangent.z, pointC.position.z, delta);

				output[index] = new Vector3(x, y, z);
			}
		}

		#endregion
	}
}
