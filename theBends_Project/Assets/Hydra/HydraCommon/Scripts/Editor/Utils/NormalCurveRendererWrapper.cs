// <copyright file=NormalCurveRendererWrapper company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using System;
using System.Reflection;
using Hydra.HydraCommon.Utils;
using UnityEditor;
using UnityEngine;

namespace Hydra.HydraCommon.Editor.Utils
{
	public static class NormalCurveRendererWrapper
	{
		public const string NORMAL_CURVE_RENDERER_TYPE_NAME = "NormalCurveRenderer";
		public const string NORMAL_CURVE_RENDERER_GET_BOUNDS_METHOD_NAME = "GetBounds";
		public const string NORMAL_CURVE_RENDERER_CURVE_FIELD_NAME = "m_Curve";

		public const string BAD_MEMBER_NAMES_ERROR =
			"Can't use NormalCurveRenderer in this version of Unity. " +
			"Dear Unity Technologies, please provide a method for calculating curve bounds.";

		private static readonly ConstructorInfo s_Constructor;
		private static readonly FieldInfo s_CurveField;
		private static readonly MethodInfo s_GetBoundsMethod;

		private static object s_CachedRenderer;

		private static readonly object[] s_EmptyParams;

		/// <summary>
		/// 	Initializes the <see cref="NormalCurveRendererWrapper"/> class.
		/// </summary>
		static NormalCurveRendererWrapper()
		{
			s_EmptyParams = new object[0];

			Type normalCurveRendererType = ReflectionUtils.GetTypeByName(Assembly.GetAssembly(typeof(EditorWindow)),
																		 NORMAL_CURVE_RENDERER_TYPE_NAME);
			if (normalCurveRendererType == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			ConstructorInfo[] constructors = ReflectionUtils.GetAllConstructors(normalCurveRendererType);
			if (constructors.Length == 0)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);
			s_Constructor = constructors[0];

			s_CurveField = ReflectionUtils.GetFieldByName(normalCurveRendererType, NORMAL_CURVE_RENDERER_CURVE_FIELD_NAME);
			if (s_CurveField == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);

			s_GetBoundsMethod = ReflectionUtils.GetMethodByName(normalCurveRendererType,
																NORMAL_CURVE_RENDERER_GET_BOUNDS_METHOD_NAME);
			if (s_GetBoundsMethod == null)
				throw new Exception(BAD_MEMBER_NAMES_ERROR);
		}

		#region Properties

		/// <summary>
		/// 	Gets the normal curve renderer.
		/// </summary>
		/// <value>The normal curve renderer.</value>
		public static object normalCurveRenderer
		{
			get { return s_CachedRenderer ?? (s_CachedRenderer = s_Constructor.Invoke(new object[] {null})); }
		}

		/// <summary>
		/// 	Gets or sets the curve.
		/// </summary>
		/// <value>The curve.</value>
		public static AnimationCurve curve
		{
			get { return (AnimationCurve)s_CurveField.GetValue(normalCurveRenderer); }
			set { s_CurveField.SetValue(normalCurveRenderer, value); }
		}

		/// <summary>
		/// 	Gets the bounds.
		/// </summary>
		/// <value>The bounds.</value>
		public static Rect bounds
		{
			get
			{
				Bounds bounds = (Bounds)s_GetBoundsMethod.Invoke(normalCurveRenderer, s_EmptyParams);
				return Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Gets the bounds.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="animationCurve">Animation curve.</param>
		public static Rect GetBounds(AnimationCurve animationCurve)
		{
			curve = animationCurve;
			return bounds;
		}

		#endregion
	}
}
