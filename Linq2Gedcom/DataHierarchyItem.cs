using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LINQ2GEDCOM
{
	internal class DataHierarchyItem
	{
		internal IList<DataHierarchyItem> Items
		{
			get;
			set;
		}

		internal string Value
		{
			get;
			set;
		}

		internal DataHierarchyItem()
		{
			this.Value = string.Empty;
			this.Items = new List<DataHierarchyItem>();
		}
	}
}