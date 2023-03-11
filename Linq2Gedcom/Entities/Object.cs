using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Object : BaseEntity
	{
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

		public string File
		{
			get;
			set;
		}

		public byte[] FileData
		{
			get
			{
				if (!System.IO.File.Exists(Path.Combine(base.Context.GEDCOMObjectFolder, this.File)))
				{
					return null;
				}
				return System.IO.File.ReadAllBytes(Path.Combine(base.Context.GEDCOMObjectFolder, this.File));
			}
		}

		public int ID
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

		public IList<EntitySource> Sources
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public Object()
		{
		}

		private static LINQ2GEDCOM.Entities.Object BuildObject(DataHierarchyItem _object, GEDCOMContext context)
		{
			LINQ2GEDCOM.Entities.Object obj = new LINQ2GEDCOM.Entities.Object()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in _object.Items
				where i.Value.StartsWith("TITL")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in _object.Items
				where i.Value.StartsWith("FILE")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in _object.Items
				where i.Value.StartsWith("NOTE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in _object.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in _object.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in _object.Items
				where i.Value.StartsWith("SOUR")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in _object.Items
				where i.Value.StartsWith("_")
				select i;
			obj.ID = _object.Value.GetID("O", 0);
			obj.Title = items.GetValue(5);
			obj.File = dataHierarchyItems.GetValue(5);
			obj.NoteIDs = items1.GetIDs("N");
			obj.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(dataHierarchyItems1, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			obj.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items2, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			obj.Sources = EntitySource.FromDataHierarchy(dataHierarchyItems2, context);
			obj.UserDefinedTags = UserDefinedTag.FromDataHierarchy(items3, context);
			return obj;
		}

		private string FormatChangeString(int hierarchyRoot)
		{
			return this.Change.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatCreateString(int hierarchyRoot)
		{
			return this.Create.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatFileString(int hierarchyRoot)
		{
			return string.Format("{0} FILE {1}", hierarchyRoot, this.File);
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

		private string FormatObjectString(int hierarchyRoot)
		{
			return string.Format("{0} @O{1}@ OBJE", hierarchyRoot, this.ID);
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

		private string FormatTitleString(int hierarchyRoot)
		{
			return string.Format("{0} TITL {1}", hierarchyRoot, this.Title);
		}

		internal static IList<LINQ2GEDCOM.Entities.Object> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select LINQ2GEDCOM.Entities.Object.BuildObject(i, context)).ToList<LINQ2GEDCOM.Entities.Object>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatObjectString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.File))
			{
				stringBuilder.AppendLine(this.FormatFileString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Title))
			{
				stringBuilder.AppendLine(this.FormatTitleString(hierarchyRoot + 1));
			}
			if (this.Change != null)
			{
				stringBuilder.Append(this.FormatChangeString(hierarchyRoot + 1));
			}
			if (this.Create != null)
			{
				stringBuilder.Append(this.FormatCreateString(hierarchyRoot + 1));
			}
			if (this.Sources != null)
			{
				stringBuilder.Append(this.FormatSourceString(hierarchyRoot + 1));
			}
			if (this.NoteIDs != null)
			{
				stringBuilder.Append(this.FormatNoteString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}