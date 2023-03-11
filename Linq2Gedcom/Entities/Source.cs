using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Source : BaseEntity
	{
		public string Author
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

		public string Date
		{
			get;
			set;
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

		public string Text
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public Source()
		{
		}

		private static Source BuildSource(DataHierarchyItem source, GEDCOMContext context)
		{
			Source d = new Source()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in source.Items
				where i.Value.StartsWith("TITL")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in source.Items
				where i.Value.StartsWith("AUTH")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in source.Items
				where i.Value.StartsWith("DATE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in source.Items
				where i.Value.StartsWith("NOTE")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in source.Items
				where i.Value.StartsWith("TEXT")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in source.Items
				where i.Value.StartsWith("OBJE")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in source.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems3 = 
				from i in source.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> items4 = 
				from i in source.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = source.Value.GetID("S", 0);
			d.Title = items.GetValue(5);
			d.Author = dataHierarchyItems.GetValue(5);
			d.Date = items1.GetValue(5);
			d.NoteIDs = dataHierarchyItems1.GetIDs("N");
			d.ObjectIDs = dataHierarchyItems2.GetIDs("O");
			foreach (DataHierarchyItem item in items2)
			{
				IEnumerable<DataHierarchyItem> dataHierarchyItems4 = item.Items.Where<DataHierarchyItem>((DataHierarchyItem i) => {
					if (i.Value.StartsWith("CONT"))
					{
						return true;
					}
					return i.Value.StartsWith("CONC");
				});
				foreach (DataHierarchyItem dataHierarchyItem in dataHierarchyItems4)
				{
					if (!dataHierarchyItem.Value.StartsWith("CONT"))
					{
						if (!dataHierarchyItem.Value.StartsWith("CONC"))
						{
							continue;
						}
						Source source1 = d;
						source1.Text = string.Concat(source1.Text, dataHierarchyItem.Value.GetSubstring(5));
					}
					else
					{
						Source source2 = d;
						source2.Text = string.Concat(source2.Text, Environment.NewLine, dataHierarchyItem.Value.GetSubstring(5));
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(d.Text))
			{
				while (d.Text.StartsWith(Environment.NewLine))
				{
					d.Text = d.Text.Substring(Environment.NewLine.Length);
				}
			}
			d.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items3, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(dataHierarchyItems3, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(items4, context);
			return d;
		}

		private string FormatAuthorString(int hierarchyRoot)
		{
			return string.Format("{0} AUTH {1}", hierarchyRoot, this.Author);
		}

		private string FormatChangeString(int hierarchyRoot)
		{
			return this.Change.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatCreateString(int hierarchyRoot)
		{
			return this.Create.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatDateString(int hierarchyRoot)
		{
			return string.Format("{0} DATE {1}", hierarchyRoot, this.Date);
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

		private string FormatSourceString(int hierarchyRoot)
		{
			object obj = hierarchyRoot;
			int d = this.ID;
			return string.Format("{0} @S{1}@ SOUR", obj, d.ToString());
		}

		private string FormatTextString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("{0} TEXT", hierarchyRoot));
			hierarchyRoot++;
			string text = this.Text;
			string[] newLine = new string[] { Environment.NewLine };
			string[] strArrays = text.Split(newLine, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				if (str.Length > 255)
				{
					IEnumerable<string> strs = str.SplitIntoChunks(255);
					if (strs.Count<string>() > 0)
					{
						stringBuilder.AppendLine(string.Format("{0} CONT {1}", hierarchyRoot, strs.First<string>()));
						foreach (string str1 in strs.Skip<string>(1))
						{
							stringBuilder.AppendLine(string.Format("{0} CONC {1}", hierarchyRoot, str1));
						}
					}
				}
				else if (!string.IsNullOrWhiteSpace(str))
				{
					stringBuilder.AppendLine(string.Format("{0} CONT {1}", hierarchyRoot, str));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("{0} CONT", hierarchyRoot));
				}
			}
			return stringBuilder.ToString();
		}

		private string FormatTitleString(int hierarchyRoot)
		{
			return string.Format("{0} TITL {1}", hierarchyRoot, this.Title);
		}

		internal static IList<Source> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Source.BuildSource(i, context)).ToList<Source>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatSourceString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Title))
			{
				stringBuilder.AppendLine(this.FormatTitleString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Author))
			{
				stringBuilder.AppendLine(this.FormatAuthorString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Date))
			{
				stringBuilder.AppendLine(this.FormatDateString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				stringBuilder.Append(this.FormatTextString(hierarchyRoot + 1));
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
			return stringBuilder.ToString();
		}
	}
}