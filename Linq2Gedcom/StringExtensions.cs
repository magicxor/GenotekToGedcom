using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LINQ2GEDCOM
{
	internal static class StringExtensions
	{
		internal static int GetID(this string value, string characterToReplace, int index = 1)
		{
			char[] chrArray = new char[] { ' ' };
			return int.Parse(value.Split(chrArray)[index].Replace("@", string.Empty).Replace(characterToReplace, string.Empty));
		}

		internal static string GetSubstring(this string value, int startIndex)
		{
			if (value.Length < startIndex)
			{
				return string.Empty;
			}
			return value.Substring(startIndex);
		}

		internal static IEnumerable<string> SplitIntoChunks(this string text, int chunkSize)
		{
			int num = 0;
			for (int i = 0; i < text.Length; i += num)
			{
				num = Math.Min(chunkSize, text.Length - i);
				yield return text.Substring(i, num);
			}
		}
	}
}