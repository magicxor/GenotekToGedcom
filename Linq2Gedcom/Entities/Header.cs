using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Header : BaseEntity
	{
		public string Character
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.Gedcom Gedcom
		{
			get;
			set;
		}

		public HeaderSource Source
		{
			get;
			set;
		}

		public Header()
		{
		}

		private static Header BuildHeader(DataHierarchyItem header, GEDCOMContext context)
		{
			Header value = new Header()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in header.Items
				where i.Value.StartsWith("SOUR")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in header.Items
				where i.Value.StartsWith("CHAR")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in header.Items
				where i.Value.StartsWith("GEDC")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in header.Items
				where i.Value.StartsWith("_")
				select i;
			value.Source = HeaderSource.FromDataHierarchy(items, context).LastOrDefault<HeaderSource>();
			value.Character = dataHierarchyItems.GetValue(5);
			value.Gedcom = LINQ2GEDCOM.Entities.Gedcom.FromDataHierarchy(items1, context).LastOrDefault<LINQ2GEDCOM.Entities.Gedcom>();
			value.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems1, context);
			return value;
		}

		private string FormatCharacterString(int hierarchyRoot)
		{
			return string.Format("{0} CHAR {1}", hierarchyRoot, this.Character);
		}

		private string FormatGedcomString(int hierarchyRoot)
		{
			return this.Gedcom.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatHeaderString(int hierarchyRoot)
		{
			return string.Format("{0} HEAD", hierarchyRoot);
		}

		private string FormatSourceString(int hierarchyRoot)
		{
			return this.Source.ToGEDCOMString(hierarchyRoot);
		}

		internal static IList<Header> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Header.BuildHeader(i, context)).ToList<Header>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatHeaderString(hierarchyRoot));
			if (this.Source != null)
			{
				stringBuilder.AppendLine(this.FormatSourceString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Character))
			{
				stringBuilder.AppendLine(this.FormatCharacterString(hierarchyRoot + 1));
			}
			if (this.Gedcom != null)
			{
				stringBuilder.Append(this.FormatGedcomString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}