using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LINQ2GEDCOM.Entities
{
	public abstract class BaseEntity
	{
		public GEDCOMContext Context
		{
			get;
			internal set;
		}

		public IList<UserDefinedTag> UserDefinedTags
		{
			get;
			set;
		}

		protected BaseEntity()
		{
		}

		internal abstract string ToGEDCOMString(int hierarchyRoot);
	}
}