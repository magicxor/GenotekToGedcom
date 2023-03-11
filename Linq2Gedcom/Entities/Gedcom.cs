using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Gedcom : BaseEntity
	{
		public string Format
		{
			get;
			set;
		}

		public string Version
		{
			get;
			set;
		}

		public Gedcom()
		{
		}

		private static Gedcom BuildGedcom(DataHierarchyItem gedcom, GEDCOMContext context)
		{
			Gedcom value = new Gedcom()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in gedcom.Items
				where i.Value.StartsWith("VERS")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in gedcom.Items
				where i.Value.StartsWith("FORM")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in gedcom.Items
				where i.Value.StartsWith("_")
				select i;
			value.Version = items.GetValue(5);
			value.Format = dataHierarchyItems.GetValue(5);
			value.UserDefinedTags = UserDefinedTag.FromDataHierarchy(items1, context);
			return value;
		}

		private string FormatFormatString(int hierarchyRoot)
		{
			return string.Format("{0} FORM {1}", hierarchyRoot, this.Format);
		}

		private string FormatGedcomString(int hierarchyRoot)
		{
			return string.Format("{0} GEDC", hierarchyRoot);
		}

		private string FormatVersionString(int hierarchyRoot)
		{
			return string.Format("{0} VERS {1}", hierarchyRoot, this.Version);
		}

		internal static IList<Gedcom> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Gedcom.BuildGedcom(i, context)).ToList<Gedcom>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatGedcomString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Version))
			{
				stringBuilder.AppendLine(this.FormatVersionString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Format))
			{
				stringBuilder.AppendLine(this.FormatFormatString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}