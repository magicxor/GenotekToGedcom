using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class EntitySource : BaseEntity
	{
		public int ID
		{
			get;
			set;
		}

		public string QualityOfData
		{
			get;
			set;
		}

		public LINQ2GEDCOM.Entities.Source Source
		{
			get
			{
				return (
					from s in base.Context.Sources
					where s.ID == this.ID
					select s).Single<LINQ2GEDCOM.Entities.Source>();
			}
		}

		public EntitySource()
		{
		}

		private static EntitySource BuildEntitySource(DataHierarchyItem source, GEDCOMContext context)
		{
			EntitySource entitySource = new EntitySource()
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in source.Items
				where i.Value.StartsWith("QUAY")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in source.Items
				where i.Value.StartsWith("_")
				select i;
			entitySource.ID = source.Value.GetID("S", 1);
			entitySource.QualityOfData = items.GetValue(5);
			entitySource.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems, context);
			return entitySource;
		}

		private string FormatQualityOfDataString(int hierarchyRoot)
		{
			return string.Format("{0} QUAY {1}", hierarchyRoot, this.QualityOfData);
		}

		private string FormatSourceString(int hierarchyRoot)
		{
			return string.Format("{0} SOUR @S{1}@", hierarchyRoot, this.ID);
		}

		internal static IList<EntitySource> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context)
		{
			return (
				from i in items
				select EntitySource.BuildEntitySource(i, context)).ToList<EntitySource>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatSourceString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.QualityOfData))
			{
				stringBuilder.AppendLine(this.FormatQualityOfDataString(hierarchyRoot + 1));
			}
			return stringBuilder.ToString();
		}
	}
}