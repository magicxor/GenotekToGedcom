using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Name : BaseEntity
	{
		public string GivenName
		{
			get;
			set;
		}

		public string NamePrefix
		{
			get;
			set;
		}

		public string NameSuffix
		{
			get;
			set;
		}

		public string SecondGivenName
		{
			get;
			set;
		}

		public string Surname
		{
			get;
			set;
		}

		public string SurnamePrefix
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public Name()
		{
		}

		private static Name BuildName(DataHierarchyItem name, GEDCOMContext context)
		{
			Name value = new Name()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in name.Items
				where i.Value.StartsWith("TYPE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in name.Items
				where i.Value.StartsWith("GIVN")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in name.Items
				where i.Value.StartsWith("SECG")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in name.Items
				where i.Value.StartsWith("SURN")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in name.Items
				where i.Value.StartsWith("SPFX")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in name.Items
				where i.Value.StartsWith("NPFX")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in name.Items
				where i.Value.StartsWith("NSFX")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems3 = 
				from i in name.Items
				where i.Value.StartsWith("_")
				select i;
			value.Type = items.GetValue(5);
			value.GivenName = dataHierarchyItems.GetValue(5);
			value.SecondGivenName = items1.GetValue(5);
			value.Surname = dataHierarchyItems1.GetValue(5);
			value.SurnamePrefix = items2.GetValue(5);
			value.NamePrefix = dataHierarchyItems2.GetValue(5);
			value.NameSuffix = items3.GetValue(5);
			value.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems3, context);
			return value;
		}

		private string FormatGivenNameString(int hierarchyRoot)
		{
			return string.Format("{0} GIVN {1}", hierarchyRoot, this.GivenName);
		}

		private string FormatNamePrefixString(int hierarchyRoot)
		{
			return string.Format("{0} NPFX {1}", hierarchyRoot, this.NamePrefix);
		}

		private string FormatNameString(int hierarchyRoot)
		{
			string empty = string.Empty;
			if (!string.IsNullOrWhiteSpace(this.GivenName))
			{
				empty = this.GivenName;
			}
			if (!string.IsNullOrWhiteSpace(this.SecondGivenName))
			{
				empty = string.Format("{0} {1}", empty, this.SecondGivenName);
			}
			if (!string.IsNullOrWhiteSpace(this.SurnamePrefix))
			{
				empty = string.Format("{0} {1}", empty, this.SurnamePrefix);
			}
			empty = string.Format("{0}/{1}/", empty, this.Surname);
			return string.Format("{0} NAME {1}", hierarchyRoot, empty);
		}

		private string FormatNameSuffixString(int hierarchyRoot)
		{
			return string.Format("{0} NSFX {1}", hierarchyRoot, this.NameSuffix);
		}

		private string FormatSecondGivenNameString(int hierarchyRoot)
		{
			return string.Format("{0} SECG {1}", hierarchyRoot, this.SecondGivenName);
		}

		private string FormatSurnamePrefixString(int hierarchyRoot)
		{
			return string.Format("{0} SPFX {1}", hierarchyRoot, this.SurnamePrefix);
		}

		private string FormatSurnameString(int hierarchyRoot)
		{
			return string.Format("{0} SURN {1}", hierarchyRoot, this.Surname);
		}

		private string FormatTypeString(int hierarchyRoot)
		{
			return string.Format("{0} TYPE {1}", hierarchyRoot, this.Type);
		}

		internal static IList<Name> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Name.BuildName(i, context)).ToList<Name>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatNameString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Type))
			{
				stringBuilder.AppendLine(this.FormatTypeString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.GivenName))
			{
				stringBuilder.AppendLine(this.FormatGivenNameString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Surname))
			{
				stringBuilder.AppendLine(this.FormatSurnameString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.SecondGivenName))
			{
				stringBuilder.AppendLine(this.FormatSecondGivenNameString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.SurnamePrefix))
			{
				stringBuilder.AppendLine(this.FormatSurnamePrefixString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.NamePrefix))
			{
				stringBuilder.AppendLine(this.FormatNamePrefixString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.NameSuffix))
			{
				stringBuilder.AppendLine(this.FormatNameSuffixString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}