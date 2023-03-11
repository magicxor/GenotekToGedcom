using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Individual : BaseEntity
	{
		public Event Adoption
		{
			get;
			set;
		}

		public Event Birth
		{
			get;
			set;
		}

		public Event Burial
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.DateTime Change
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.DateTime Create
		{
			get;
			set;
		}

		public Event Death
		{
			get;
			set;
		}

		public IList<Event> Emigrations
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public IList<Event> Immigrations
		{
			get;
			set;
		}

		public IList<Event> MilitaryServices
		{
			get;
			set;
		}

		public IList<Name> Names
		{
			get;
			set;
		}

		public IList<Event> Naturalizations
		{
			get;
			set;
		}

		public IList<int> NoteIDs
		{
			get;
			set;
		}

		public IEnumerable<Note> Notes
		{
			get
			{
				return 
					from n in base.Context.Notes
					where this.NoteIDs.Contains(n.ID)
					select n;
			}
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

		public IList<Event> Ordinances
		{
			get;
			set;
		}

		public IList<Event> Ordinations
		{
			get;
			set;
		}

		public Family ParentFamily
		{
			get
			{
				if (!this.ParentFamilyID.HasValue)
				{
					return null;
				}
				return base.Context.Families.Where<Family>((Family f) => {
					int d = f.ID;
					int? parentFamilyID = this.ParentFamilyID;
					if (d != parentFamilyID.GetValueOrDefault())
					{
						return false;
					}
					return parentFamilyID.HasValue;
				}).Single<Family>();
			}
		}

		public int? ParentFamilyID
		{
			get;
			set;
		}

		public string Sex
		{
			get;
			set;
		}

		public IList<EntitySource> Sources
		{
			get;
			set;
		}

		public IEnumerable<Family> SpousalFamilies
		{
			get
			{
				return 
					from f in base.Context.Families
					where this.SpousalFamilyIDs.Contains(f.ID)
					select f;
			}
		}

		public IList<int> SpousalFamilyIDs
		{
			get;
			set;
		}

		public Individual()
		{
		}

		private static Individual BuildIndividual(DataHierarchyItem individual, GEDCOMContext context)
		{
			Individual d = new Individual()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in individual.Items
				where i.Value.StartsWith("NAME")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in individual.Items
				where i.Value.StartsWith("SEX")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in individual.Items
				where i.Value.StartsWith("BURI")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in individual.Items
				where i.Value.StartsWith("DEAT")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in individual.Items
				where i.Value.StartsWith("BIRT")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in individual.Items
				where i.Value.StartsWith("ADOP")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in individual.Items
				where i.Value.StartsWith("IMMI")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems3 = 
				from i in individual.Items
				where i.Value.StartsWith("NATU")
				select i;
			IEnumerable<DataHierarchyItem> items4 = 
				from i in individual.Items
				where i.Value.StartsWith("EMIG")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems4 = 
				from i in individual.Items
				where i.Value.StartsWith("MISE")
				select i;
			IEnumerable<DataHierarchyItem> items5 = 
				from i in individual.Items
				where i.Value.StartsWith("ORDI")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems5 = 
				from i in individual.Items
				where i.Value.StartsWith("ORDN")
				select i;
			IEnumerable<DataHierarchyItem> items6 = 
				from i in individual.Items
				where i.Value.StartsWith("SOUR")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems6 = 
				from i in individual.Items
				where i.Value.StartsWith("NOTE")
				select i;
			IEnumerable<DataHierarchyItem> items7 = 
				from i in individual.Items
				where i.Value.StartsWith("OBJE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems7 = 
				from i in individual.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> items8 = 
				from i in individual.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems8 = 
				from i in individual.Items
				where i.Value.StartsWith("FAMS")
				select i;
			IEnumerable<DataHierarchyItem> items9 = 
				from i in individual.Items
				where i.Value.StartsWith("FAMC")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems9 = 
				from i in individual.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = individual.Value.GetID("I", 0);
			d.Names = Name.FromDataHierarchy(items, context);
			d.Sex = dataHierarchyItems.GetValue(4);
			d.Burial = Event.FromDataHierarchy(items1, context, Event.EventType.Burial).LastOrDefault<Event>();
			d.Death = Event.FromDataHierarchy(dataHierarchyItems1, context, Event.EventType.Death).LastOrDefault<Event>();
			d.Birth = Event.FromDataHierarchy(items2, context, Event.EventType.Birth).LastOrDefault<Event>();
			d.Adoption = Event.FromDataHierarchy(dataHierarchyItems2, context, Event.EventType.Adoption).LastOrDefault<Event>();
			d.Immigrations = Event.FromDataHierarchy(items3, context, Event.EventType.Immigration);
			d.Naturalizations = Event.FromDataHierarchy(dataHierarchyItems3, context, Event.EventType.Naturalization);
			d.Emigrations = Event.FromDataHierarchy(items4, context, Event.EventType.Emigration);
			d.MilitaryServices = Event.FromDataHierarchy(dataHierarchyItems4, context, Event.EventType.MilitaryService);
			d.Ordinances = Event.FromDataHierarchy(items5, context, Event.EventType.Ordinance);
			d.Ordinations = Event.FromDataHierarchy(dataHierarchyItems5, context, Event.EventType.Ordination);
			d.Sources = EntitySource.FromDataHierarchy(items6, context);
			d.NoteIDs = dataHierarchyItems6.GetIDs("N");
			d.ObjectIDs = items7.GetIDs("O");
			d.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(dataHierarchyItems7, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items8, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.SpousalFamilyIDs = dataHierarchyItems8.GetIDs("F");
			d.ParentFamilyID = items9.GetID("F");
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems9, context);
			return d;
		}

		private string FormatAdditionalNamesString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Name name in this.Names.Skip<Name>(1))
			{
				stringBuilder.Append(name.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatAdoptionString(int hierarchyRoot)
		{
			return this.Adoption.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatBirthString(int hierarchyRoot)
		{
			return this.Birth.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatBurialString(int hierarchyRoot)
		{
			return this.Burial.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatChangeString(int hierarchyRoot)
		{
			return this.Change.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatCreateString(int hierarchyRoot)
		{
			return this.Create.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatDeathString(int hierarchyRoot)
		{
			return this.Death.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatEmigrationString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event emigration in this.Emigrations)
			{
				stringBuilder.Append(emigration.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatFamilyChildString(int hierarchyRoot)
		{
			return string.Format("{0} FAMC @F{1}@", hierarchyRoot, this.ParentFamilyID);
		}

		private string FormatFamilySpouseString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int spousalFamilyID in this.SpousalFamilyIDs)
			{
				stringBuilder.AppendLine(string.Format("{0} FAMS @F{1}@", hierarchyRoot, spousalFamilyID));
			}
			return stringBuilder.ToString();
		}

		private string FormatImmigrationString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event immigration in this.Immigrations)
			{
				stringBuilder.Append(immigration.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatIndividualString(int hierarchyRoot)
		{
			return string.Format("{0} @I{1}@ INDI", hierarchyRoot, this.ID);
		}

		private string FormatMilitaryServiceString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event militaryService in this.MilitaryServices)
			{
				stringBuilder.Append(militaryService.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatNaturalizationString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event naturalization in this.Naturalizations)
			{
				stringBuilder.Append(naturalization.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatNoteString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int noteID in this.NoteIDs)
			{
				stringBuilder.AppendLine(string.Format("{0} NOTE @N{1}@", hierarchyRoot, noteID));
			}
			return stringBuilder.ToString();
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

		private string FormatOrdinanceString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event ordinance in this.Ordinances)
			{
				stringBuilder.Append(ordinance.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatOrdinationString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Event ordination in this.Ordinations)
			{
				stringBuilder.Append(ordination.ToGEDCOMString(hierarchyRoot));
			}
			return stringBuilder.ToString();
		}

		private string FormatPrimaryNameString(int hierarchyRoot)
		{
			return string.Format(this.Names.First<Name>().ToGEDCOMString(hierarchyRoot), new object[0]);
		}

		private string FormatSexString(int hierarchyRoot)
		{
			return string.Format("{0} SEX {1}", hierarchyRoot, this.Sex);
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

		internal static IList<Individual> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Individual.BuildIndividual(i, context)).ToList<Individual>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatIndividualString(hierarchyRoot));
			if (this.Names != null && this.Names.Count > 0)
			{
				stringBuilder.Append(this.FormatPrimaryNameString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Sex))
			{
				stringBuilder.AppendLine(this.FormatSexString(hierarchyRoot + 1));
			}
			if (this.Names != null && this.Names.Count > 1)
			{
				stringBuilder.Append(this.FormatAdditionalNamesString(hierarchyRoot + 1));
			}
			if (this.Birth != null)
			{
				stringBuilder.Append(this.FormatBirthString(hierarchyRoot + 1));
			}
			if (this.Adoption != null)
			{
				stringBuilder.Append(this.FormatAdoptionString(hierarchyRoot + 1));
			}
			if (this.Immigrations != null)
			{
				stringBuilder.Append(this.FormatImmigrationString(hierarchyRoot + 1));
			}
			if (this.Naturalizations != null)
			{
				stringBuilder.Append(this.FormatNaturalizationString(hierarchyRoot + 1));
			}
			if (this.Emigrations != null)
			{
				stringBuilder.Append(this.FormatEmigrationString(hierarchyRoot + 1));
			}
			if (this.MilitaryServices != null)
			{
				stringBuilder.Append(this.FormatMilitaryServiceString(hierarchyRoot + 1));
			}
			if (this.Ordinances != null)
			{
				stringBuilder.Append(this.FormatOrdinanceString(hierarchyRoot + 1));
			}
			if (this.Ordinations != null)
			{
				stringBuilder.Append(this.FormatOrdinationString(hierarchyRoot + 1));
			}
			if (this.Death != null)
			{
				stringBuilder.Append(this.FormatDeathString(hierarchyRoot + 1));
			}
			if (this.Burial != null)
			{
				stringBuilder.Append(this.FormatBurialString(hierarchyRoot + 1));
			}
			if (this.Sources != null)
			{
				stringBuilder.Append(this.FormatSourceString(hierarchyRoot + 1));
			}
			if (this.NoteIDs != null)
			{
				stringBuilder.Append(this.FormatNoteString(hierarchyRoot + 1));
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
			if (this.SpousalFamilyIDs != null)
			{
				stringBuilder.Append(this.FormatFamilySpouseString(hierarchyRoot + 1));
			}
			if (this.ParentFamilyID.HasValue)
			{
				stringBuilder.AppendLine(this.FormatFamilyChildString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}