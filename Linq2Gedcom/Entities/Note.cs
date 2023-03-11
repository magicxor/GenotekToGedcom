using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Note : BaseEntity
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

		public int ID
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public Note()
		{
		}

		private static Note BuildNote(DataHierarchyItem note, GEDCOMContext context)
		{
			Note d = new Note()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> dataHierarchyItems = note.Items.Where<DataHierarchyItem>((DataHierarchyItem i) => {
				if (i.Value.StartsWith("CONT"))
				{
					return true;
				}
				return i.Value.StartsWith("CONC");
			});
			IEnumerable<DataHierarchyItem> items = 
				from i in note.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in note.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in note.Items
				where i.Value.StartsWith("_")
				select i;
			d.ID = note.Value.GetID("N", 0);
			foreach (DataHierarchyItem dataHierarchyItem in dataHierarchyItems)
			{
				if (!dataHierarchyItem.Value.StartsWith("CONT"))
				{
					if (!dataHierarchyItem.Value.StartsWith("CONC"))
					{
						continue;
					}
					Note note1 = d;
					note1.Text = string.Concat(note1.Text, dataHierarchyItem.Value.GetSubstring(5));
				}
				else
				{
					Note note2 = d;
					note2.Text = string.Concat(note2.Text, Environment.NewLine, dataHierarchyItem.Value.GetSubstring(5));
				}
			}
			while (d.Text.StartsWith(Environment.NewLine))
			{
				d.Text = d.Text.Substring(Environment.NewLine.Length);
			}
			d.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items1, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			d.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems1, context);
			return d;
		}

		private string FormatChangeString(int hierarchyRoot)
		{
			return this.Change.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatCreateString(int hierarchyRoot)
		{
			return this.Create.ToGEDCOMString(hierarchyRoot);
		}

		private string FormatNoteString(int hierarchyRoot)
		{
			object obj = hierarchyRoot;
			int d = this.ID;
			return string.Format("{0} @N{1}@ NOTE", obj, d.ToString());
		}

		private string FormatTextString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
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

		internal static IList<Note> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select Note.BuildNote(i, context)).ToList<Note>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatNoteString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				stringBuilder.Append(this.FormatTextString(hierarchyRoot + 1));
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