using LINQ2GEDCOM.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LINQ2GEDCOM
{
	internal class GEDCOMDataReader
	{
		private static IList<DataHierarchyItem> GEDCOMDataItems
		{
			get;
			set;
		}

		private GEDCOMDataReader()
		{
		}

		private static void AddItemToHierarchy(int hierarchyIndentationNumber, DataHierarchyItem item)
		{
			switch (hierarchyIndentationNumber)
			{
				case 0:
				{
					GEDCOMDataReader.GEDCOMDataItems.Add(item);
					return;
				}
				case 1:
				{
					GEDCOMDataReader.GEDCOMDataItems.Last<DataHierarchyItem>().Items.Add(item);
					return;
				}
				case 2:
				{
					GEDCOMDataReader.GEDCOMDataItems.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Add(item);
					return;
				}
				case 3:
				{
					GEDCOMDataReader.GEDCOMDataItems.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Add(item);
					return;
				}
				case 4:
				{
					GEDCOMDataReader.GEDCOMDataItems.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Last<DataHierarchyItem>().Items.Add(item);
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void DestroyDataHierarchy()
		{
			GEDCOMDataReader.GEDCOMDataItems = null;
		}

		private static int GetHierarchyNumber(string line)
		{
			int num = -1;
			int.TryParse(line.Substring(0, 1), out num);
			if (num == -1)
			{
				throw new Exception(string.Format("Unable to parse GEDCOM file line: {0}", line));
			}
			return num;
		}

		private static IList<DataHierarchyItem> GetHierarchyPosition(int hierarchyIndentationNumber, IList<DataHierarchyItem> hierarchy)
		{
			if (hierarchyIndentationNumber == 0)
			{
				return hierarchy;
			}
			return GEDCOMDataReader.GetHierarchyPosition(hierarchyIndentationNumber - 1, hierarchy.Last<DataHierarchyItem>().Items);
		}

		private static void InitializeDataHierarchy(string file)
		{
			GEDCOMDataReader.ValidateFile(file);
			GEDCOMDataReader.GEDCOMDataItems = new List<DataHierarchyItem>();
			foreach (string str in File.ReadLines(file))
			{
				int hierarchyNumber = GEDCOMDataReader.GetHierarchyNumber(str);
				DataHierarchyItem dataHierarchyItem = new DataHierarchyItem()
				{
					Value = str.Substring(2)
				};
				GEDCOMDataReader.AddItemToHierarchy(hierarchyNumber, dataHierarchyItem);
			}
		}

		internal static void LoadDataIntoContext(GEDCOMContext context)
		{
			GEDCOMDataReader.InitializeDataHierarchy(context.GEDCOMFile);
			context.Headers = Header.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith("HEAD")
				select i, context);
			context.Individuals = Individual.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" INDI")
				select i, context);
			context.Families = Family.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" FAM")
				select i, context);
			context.Sources = Source.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" SOUR")
				select i, context);
			context.Objects = LINQ2GEDCOM.Entities.Object.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" OBJE")
				select i, context);
			context.Notes = Note.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" NOTE")
				select i, context);
			context.Labels = Label.FromDataHierarchy(
				from i in GEDCOMDataReader.GEDCOMDataItems
				where i.Value.EndsWith(" LABL")
				select i, context);
			GEDCOMDataReader.DestroyDataHierarchy();
		}

		private static void ValidateFile(string file)
		{
			if (string.IsNullOrWhiteSpace(file))
			{
				throw new ArgumentException("File can not be empty.");
			}
			if (!File.Exists(file))
			{
				throw new ArgumentException("File not found.");
			}
		}
	}
}