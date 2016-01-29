// <copyright file=RepaintScheduler company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

using Hydra.HydraCommon.Abstract;
using UnityEngine;

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	The RepaintScheduler is a simple attempt at limiting repaint calls to once
	/// 	per update.
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class RepaintScheduler : SingletonHydraMonoBehaviour<RepaintScheduler>
	{
		private static bool s_RepaintAll;

		#region Properties

		/// <summary>
		///     Single instance.
		/// </summary>
		public new static RepaintScheduler instance { get { return Instantiate(); } }

		#endregion

		#region Messages

		/// <summary>
		/// 	Called when the editor updates.
		/// </summary>
		protected override void OnEditorUpdate()
		{
			base.OnEditorUpdate();

			if (s_RepaintAll)
			{
#if UNITY_EDITOR
				if (!Application.isPlaying)
					UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
				s_RepaintAll = false;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Initializes a new instance of the
		/// 	<see cref="RepaintScheduler"/> class.
		/// 
		/// 	We protect the constructor to prevent usage.
		/// </summary>
		protected RepaintScheduler() {}

		/// <summary>
		/// 	Schedules to repaint all views.
		/// </summary>
		public static void RepaintAllViews()
		{
			Instantiate();

			s_RepaintAll = true;
		}

		/// <summary>
		/// 	Instantiate this object.
		/// </summary>
		public new static RepaintScheduler Instantiate()
		{
			RepaintScheduler instance = SingletonHydraMonoBehaviour<RepaintScheduler>.Instantiate();

			// Hide the instance
			instance.gameObject.hideFlags = HideFlags.HideAndDontSave;

			return instance;
		}

		#endregion
	}
}
