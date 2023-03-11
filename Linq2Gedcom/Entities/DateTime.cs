using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class DateTime : BaseEntity
	{
		private LINQ2GEDCOM.Entities.DateTime.DateType _dateType;

		public string Date
		{
			get;
			set;
		}

		private string DateString
		{
			get
			{
				switch (this._dateType)
				{
					case LINQ2GEDCOM.Entities.DateTime.DateType.Change:
					{
						return "CHAN";
					}
					case LINQ2GEDCOM.Entities.DateTime.DateType.Create:
					{
						return "CREA";
					}
				}
				throw new Exception("Invalid Date Type");
			}
		}

		public System.DateTime? DateTimeValue
		{
			get
			{
				System.DateTime minValue = System.DateTime.MinValue;
				if (System.DateTime.TryParse(string.Format("{0} {1}", this.Date, this.Time), out minValue))
				{
					return new System.DateTime?(minValue);
				}
				return null;
			}
		}

		public string Time
		{
			get;
			set;
		}

		private DateTime()
		{
		}

		public DateTime(LINQ2GEDCOM.Entities.DateTime.DateType dateType) : this()
		{
			this._dateType = dateType;
		}

		private static LINQ2GEDCOM.Entities.DateTime BuildDateTime(DataHierarchyItem dateTime, GEDCOMContext context, LINQ2GEDCOM.Entities.DateTime.DateType dateType)
		{
			LINQ2GEDCOM.Entities.DateTime substring = new LINQ2GEDCOM.Entities.DateTime(dateType)
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in dateTime.Items
				where i.Value.StartsWith("DATE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in dateTime.Items
				where i.Value.StartsWith("_")
				select i;
			foreach (DataHierarchyItem item in items)
			{
				substring.Date = item.Value.GetSubstring(5);
				substring.Time = (
					from i in item.Items
					where i.Value.StartsWith("TIME")
					select i).GetValue(5);
				substring.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems, context);
			}
			return substring;
		}

		private string FormatDateString(int hierarchyRoot)
		{
			return string.Format("{0} DATE {1}", hierarchyRoot, this.Date);
		}

		private string FormatDateTimeString(int hierarchyRoot)
		{
			return string.Format("{0} {1}", hierarchyRoot, this.DateString);
		}

		private string FormatTimeString(int hierarchyRoot)
		{
			return string.Format("{0} TIME {1}", hierarchyRoot, this.Time);
		}

		internal static IList<LINQ2GEDCOM.Entities.DateTime> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context, LINQ2GEDCOM.Entities.DateTime.DateType dateType)
		{
			return (
				from i in items
				select LINQ2GEDCOM.Entities.DateTime.BuildDateTime(i, context, dateType)).ToList<LINQ2GEDCOM.Entities.DateTime>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatDateTimeString(hierarchyRoot));
			stringBuilder.AppendLine(this.FormatDateString(hierarchyRoot + 1).Trim());
			if (!string.IsNullOrWhiteSpace(this.Time))
			{
				stringBuilder.AppendLine(this.FormatTimeString(hierarchyRoot + 2));
			}
			return stringBuilder.ToString();
		}

		public enum DateType
		{
			Change,
			Create
		}
	}
}