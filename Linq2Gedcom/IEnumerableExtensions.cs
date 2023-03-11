using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LINQ2GEDCOM
{
	internal static class IEnumerableExtensions
	{
		internal static int? GetID(this IEnumerable<DataHierarchyItem> items, string characterToReplace)
		{
			if (items.Count<DataHierarchyItem>() < 1)
			{
				return null;
			}
			string value = items.Last<DataHierarchyItem>().Value;
			char[] chrArray = new char[] { ' ' };
			return new int?(int.Parse(value.Split(chrArray)[1].Replace("@", string.Empty).Replace(characterToReplace, string.Empty)));
		}

		internal static IList<int> GetIDs(this IEnumerable<DataHierarchyItem> items, string characterToReplace)
		{
			return (
				from i in items
				select int.Parse(i.Value.Split(new char[] { ' ' })[1].Replace("@", string.Empty).Replace(characterToReplace, string.Empty))).ToList<int>();
		}

		internal static string GetValue(this IEnumerable<DataHierarchyItem> items, int substringLength = 5)
		{
			if (items.Count<DataHierarchyItem>() < 1)
			{
				return string.Empty;
			}
			return items.Last<DataHierarchyItem>().Value.GetSubstring(substringLength);
		}

		internal static IList<string> GetValues(this IEnumerable<DataHierarchyItem> items)
		{
			return (
				from i in items
				select i.Value.GetSubstring(5)).ToList<string>();
		}
	}
}