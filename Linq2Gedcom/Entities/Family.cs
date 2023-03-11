using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Family : BaseEntity
	{
		public LINQ2GEDCOM.Entities.DateTime Change
		{
			get;
			set;
		}

		public IList<Child> Children
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.DateTime Create
		{
			get;
			set;
		}

		public Event Divorce
		{
			get;
			set;
		}

		public Individual Husband
		{
			get
			{
				if (!this.HusbandID.HasValue)
				{
					return null;
				}
				return base.Context.Individuals.Where<Individual>((Individual i) => {
					int d = i.ID;
					int? husbandID = this.HusbandID;
					if (d != husbandID.GetValueOrDefault())
					{
						return false;
					}
					return husbandID.HasValue;
				}).Single<Individual>();
			}
		}

		public int? HusbandID
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public Event Marriage
		{
			get;
			set;
		}

		public IList<int> ObjectIDs
		{
			get;
			set;
		}

		public IEnumerable<LINQ2GEDCOM.Entities.Object> Objects
		{
			get
			{
				return 
					from o in base.Context.Objects
					where this.ObjectIDs.Contains(o.ID)
					select o;
			}
		}

		public IList<EntitySource> Sources
		{
			get;
			set;
		}

		public Individual Wife
		{
			get
			{
				if (!this.WifeID.HasValue)
				{
					return null;
				}
				return base.Context.Individuals.Where<Individual>((Individual i) => {
					int d = i.ID;
					int? wifeID = this.WifeID;
					if (d != wifeID.GetValueOrDefault())
					{
						return false;
					}
					return wifeID.HasValue;
				}).Single<Individual>();
			}
		}

		public int? WifeID
		{
			get;
			set;
		}

		public Family()
		{
		}

		private static Family BuildFamily(DataHierarchyItem family, GEDCOMContext context)
		{
			Family d = new Family()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in family.Items
				where i.Value.StartsWith("HUSB")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in family.Items
				where i.Value.StartsWith("WIFE")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in family.Items
				where i.Value.StartsWith("SOUR")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in family.Items
				where i.Value.StartsWith("OBJE")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in family.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in family.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in family.Items
				where i.Value.StartsWith("CHIL")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems3 = 
				from i in family.Items
				where i.Value.StartsWith("MARR")
				select i;
			IEnumerable<DataHierarchyItem> items4 = 
				from i in family.Items
				where i.Value.StartsWith("DIV")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems4 = 
				from i in family.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = family.Value.GetID("F", 0);
			d.HusbandID = items.GetID("I");
			d.WifeID = dataHierarchyItems.GetID("I");
			d.ObjectIDs = dataHierarchyItems1.GetIDs("O");
			d.Sources = EntitySource.FromDataHierarchy(items1, context);
			d.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items2, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(dataHierarchyItems2, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.Children = Child.FromDataHierarchy(items3, context);
			d.Marriage = Event.FromDataHierarchy(dataHierarchyItems3, context, Event.EventType.Marriage).LastOrDefault<Event>();
			d.Divorce = Event.FromDataHierarchy(items4, context, Event.EventType.Divorce).LastOrDefault<Event>();
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems4, context);
			return d;
		}

		private string FormatChangeString(int hierarchyRoot)
		{
			return this.Change.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatChildrenString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Child child in this.Children)
			{
				stringBuilder.Append(child.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatCreateString(int hierarchyRoot)
		{
			return this.Create.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatDivorceString(int hierarchyRoot)
		{
			return this.Divorce.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatFamilyString(int hierarchyRoot)
		{
			return string.Format("{0} @F{1}@ FAM", hierarchyRoot, this.ID);
		}

		private string FormatHusbandString(int hierarchyRoot)
		{
			return string.Format("{0} HUSB @I{1}@", hierarchyRoot, this.HusbandID);
		}

		private string FormatMarriageString(int hierarchyRoot)
		{
			return this.Marriage.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatObjectsString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int objectID in this.ObjectIDs)
			{
				stringBuilder.AppendLine(string.Format("{0} OBJE @O{1}@", hierarchyRoot, objectID));
			}
			return stringBuilder.ToString();
		}

		private string FormatSourceString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (EntitySource source in this.Sources)
			{
				stringBuilder.Append(source.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatWifeString(int hierarchyRoot)
		{
			return string.Format("{0} WIFE @I{1}@", hierarchyRoot, this.WifeID);
		}

		internal static IList<Family> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Family.BuildFamily(i, context)).ToList<Family>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatFamilyString(hierarchyRoot));
			if (this.Marriage != null)
			{
				stringBuilder.Append(this.FormatMarriageString(hierarchyRoot + 1));
			}
			if (this.Divorce != null)
			{
				stringBuilder.Append(this.FormatDivorceString(hierarchyRoot + 1));
			}
			if (this.HusbandID.HasValue)
			{
				stringBuilder.AppendLine(this.FormatHusbandString(hierarchyRoot + 1));
			}
			if (this.WifeID.HasValue)
			{
				stringBuilder.AppendLine(this.FormatWifeString(hierarchyRoot + 1));
			}
			if (this.Children != null)
			{
				stringBuilder.Append(this.FormatChildrenString(hierarchyRoot + 1));
			}
			if (this.Sources != null)
			{
				stringBuilder.Append(this.FormatSourceString(hierarchyRoot + 1));
			}
			if (this.ObjectIDs != null)
			{
				stringBuilder.Append(this.FormatObjectsString(hierarchyRoot + 1));
			}
			if (this.Change != null)
			{
				stringBuilder.Append(this.FormatChangeString(hierarchyRoot + 1));
			}
			if (this.Create != null)
			{
				stringBuilder.Append(this.FormatCreateString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}