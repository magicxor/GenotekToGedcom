using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Label : BaseEntity
	{
		public string ColorText
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public Label()
		{
		}

		private static Label BuildLabel(DataHierarchyItem label, GEDCOMContext context)
		{
			Label d = new Label()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in label.Items
				where i.Value.StartsWith("TITL")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in label.Items
				where i.Value.StartsWith("COLR")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in label.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = label.Value.GetID("L", 0);
			d.Title = items.GetValue(5);
			d.ColorText = dataHierarchyItems.GetValue(5);
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(items1, context);
			return d;
		}

		private string FormatColorString(int hierarchyRoot)
		{
			return string.Format("{0} COLR {1}", hierarchyRoot, this.ColorText);
		}

		private string FormatLabelString(int hierarchyRoot)
		{
			object obj = hierarchyRoot;
			int d = this.ID;
			return string.Format("{0} @L{1}@ LABL", obj, d.ToString());
		}

		private string FormatTitleString(int hierarchyRoot)
		{
			return string.Format("{0} TITL {1}", hierarchyRoot, this.Title);
		}

		internal static IList<Label> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Label.BuildLabel(i, context)).ToList<Label>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatLabelString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Title))
			{
				stringBuilder.AppendLine(this.FormatTitleString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.ColorText))
			{
				stringBuilder.AppendLine(this.FormatColorString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}