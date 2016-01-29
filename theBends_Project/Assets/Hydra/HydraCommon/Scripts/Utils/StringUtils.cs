// <copyright file=StringUtils company=Hydra>
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Christopher Cameron</author>

namespace Hydra.HydraCommon.Utils
{
	/// <summary>
	/// 	String utils.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// 	Formats the input path for unix.
		/// </summary>
		/// <returns>The unix path.</returns>
		/// <param name="path">Path.</param>
		public static string ToUnixPath(string path)
		{
			return path.Replace("\\", "/");
		}

		/// <summary>
		/// 	Removes all instances of the given substring.
		/// </summary>
		/// <returns>The cleaned string.</returns>
		/// <param name="source">Source.</param>
		/// <param name="remove">Remove.</param>
		public static string RemoveSubstring(string source, string remove)
		{
			int index = source.IndexOf(remove);
			string clean = (index < 0) ? source : source.Remove(index, remove.Length);

			if (clean.Length != source.Length)
				return RemoveSubstring(clean, remove);

			return clean;
		}
	}
}
