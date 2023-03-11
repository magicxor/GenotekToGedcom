using LINQ2GEDCOM.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace LINQ2GEDCOM
{
	internal class GEDCOMDataWriter
	{
		public GEDCOMDataWriter()
		{
		}

		private static void ValidateFile(string file)
		{
			if (string.IsNullOrWhiteSpace(file))
			{
				throw new ArgumentException("File can not be empty.");
			}
		}

		public static void WriteToFile(GEDCOMContext context, string file)
		{
			GEDCOMDataWriter.ValidateFile(file);
			using (StreamWriter streamWriter = new StreamWriter(file))
			{
				GEDCOMDataWriter.WriteToFile<Header>(context.Headers, streamWriter);
				GEDCOMDataWriter.WriteToFile<Individual>(context.Individuals, streamWriter);
				GEDCOMDataWriter.WriteToFile<Family>(context.Families, streamWriter);
				GEDCOMDataWriter.WriteToFile<Source>(context.Sources, streamWriter);
				GEDCOMDataWriter.WriteToFile<LINQ2GEDCOM.Entities.Object>(context.Objects, streamWriter);
				GEDCOMDataWriter.WriteToFile<Note>(context.Notes, streamWriter);
				GEDCOMDataWriter.WriteToFile<Label>(context.Labels, streamWriter);
				streamWriter.WriteLine("0 TRLR");
			}
		}

		private static void WriteToFile<TEntity>(IList<TEntity> items, StreamWriter writer)
		where TEntity : BaseEntity, new()
		{
			foreach (TEntity item in items)
			{
				writer.Write(item.ToGEDCOMString(0));
			}
		}
	}
}