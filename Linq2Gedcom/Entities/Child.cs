using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Child : BaseEntity
	{
		public int ID
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.Individual Individual
		{
			get
			{
				return (
					from i in base.Context.Individuals
					where i.ID == this.ID
					select i).Single<LINQ2GEDCOM.Entities.Individual>();
			}
		}

		public string Pedigree
		{
			get;
			set;
		}

		public Child()
		{
		}

		private static Child BuildChild(DataHierarchyItem child, GEDCOMContext context)
		{
			Child d = new Child()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in child.Items
				where i.Value.StartsWith("PEDI")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in child.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = child.Value.GetID("I", 1);
			d.Pedigree = items.GetValue(5);
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems, context);
			return d;
		}

		private string FormatChildString(int hierarchyRoot)
		{
			return string.Format("{0} CHIL @I{1}@", hierarchyRoot, this.ID);
		}

		private string FormatPedigreeString(int hierarchyRoot)
		{
			return string.Format("{0} PEDI {1}", hierarchyRoot, this.Pedigree);
		}

		internal static IList<Child> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Child.BuildChild(i, context)).ToList<Child>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatChildString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Pedigree))
			{
				stringBuilder.AppendLine(this.FormatPedigreeString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}