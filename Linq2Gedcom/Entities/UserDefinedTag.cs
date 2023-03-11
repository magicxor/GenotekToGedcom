using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class UserDefinedTag : BaseEntity
	{
		public string Tag
		{
			get;
			set;
		}

		public new IList<UserDefinedTag> UserDefinedTags
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public UserDefinedTag()
		{
		}

		private static UserDefinedTag BuildUserDefinedTag(DataHierarchyItem userDefinedTag, GEDCOMContext context)
		{
			UserDefinedTag empty = new UserDefinedTag()
			{
				Context = context
			};
			if (!userDefinedTag.Value.Contains<char>(' '))
			{
				string value = userDefinedTag.Value;
				char[] chrArray = new char[] { ' ' };
				empty.Tag = value.Split(chrArray)[0];
				empty.Value = string.Empty;
			}
			else
			{
				string str = userDefinedTag.Value;
				char[] chrArray1 = new char[] { ' ' };
				empty.Tag = str.Split(chrArray1)[0];
				string value1 = userDefinedTag.Value;
				char[] chrArray2 = new char[] { ' ' };
				empty.Value = value1.Split(chrArray2)[1];
			}
			empty.UserDefinedTags = (
				from i in userDefinedTag.Items
				select UserDefinedTag.BuildUserDefinedTag(i, context)).ToList<UserDefinedTag>();
			return empty;
		}

		private string FormatUserDefinedTagString(int hierarchyRoot)
		{
			if (string.IsNullOrWhiteSpace(this.Value))
			{
				return string.Format("{0} {1}", hierarchyRoot, this.Tag);
			}
			return string.Format("{0} {1} {2}", hierarchyRoot, this.Tag, this.Value);
		}

		internal static IList<UserDefinedTag> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select UserDefinedTag.BuildUserDefinedTag(i, context)).ToList<UserDefinedTag>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatUserDefinedTagString(hierarchyRoot));
			foreach (UserDefinedTag userDefinedTag in this.UserDefinedTags)
			{
				stringBuilder.Append(userDefinedTag.ToGEDCOMString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}