using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class HeaderSource : BaseEntity
	{
		public string Corporate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Version
		{
			get;
			set;
		}

		public HeaderSource()
		{
		}

		private static HeaderSource BuildHeaderSource(DataHierarchyItem source, GEDCOMContext context)
		{
			HeaderSource headerSource = new HeaderSource()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in source.Items
				where i.Value.StartsWith("NAME")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in source.Items
				where i.Value.StartsWith("VERS")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in source.Items
				where i.Value.StartsWith("CORP")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in source.Items
				where i.Value.StartsWith("_")
				select i;
			headerSource.Name = items.GetValue(5);
			headerSource.Version = dataHierarchyItems.GetValue(5);
			headerSource.Corporate = items1.GetValue(5);
			headerSource.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems1, context);
			return headerSource;
		}

		private string FormatCorporateString(int hierarchyRoot)
		{
			return string.Format("{0} CORP {1}", hierarchyRoot, this.Corporate);
		}

		private string FormatNameString(int hierarchyRoot)
		{
			return string.Format("{0} NAME {1}", hierarchyRoot, this.Name);
		}

		private string FormatSourceString(int hierarchyRoot)
		{
			return string.Format("{0} SOUR", hierarchyRoot);
		}

		private string FormatVersionString(int hierarchyRoot)
		{
			return string.Format("{0} VERS {1}", hierarchyRoot, this.Version);
		}

		internal static IList<HeaderSource> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select HeaderSource.BuildHeaderSource(i, context)).ToList<HeaderSource>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatSourceString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Name))
			{
				stringBuilder.AppendLine(this.FormatNameString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Version))
			{
				stringBuilder.AppendLine(this.FormatVersionString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Corporate))
			{
				stringBuilder.Append(this.FormatCorporateString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}