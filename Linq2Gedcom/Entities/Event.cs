using LINQ2GEDCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LINQ2GEDCOM.Entities
{
	public class Event : BaseEntity
	{
		private Event.EventType _eventType;

		public string Address
		{
			get;
			set;
		}

		public string Agency
		{
			get;
			set;
		}

		public string Cause
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

		public System.DateTime? DateTimeValue
		{
			get
			{
				System.DateTime minValue = System.DateTime.MinValue;
				if (System.DateTime.TryParse(this.Date, out minValue))
				{
					return new System.DateTime?(minValue);
				}
				return null;
			}
		}

		private string EventString
		{
			get
			{
				switch (this._eventType)
				{
					case Event.EventType.Burial:
					{
						return "BURI";
					}
					case Event.EventType.Death:
					{
						return "DEAT";
					}
					case Event.EventType.Birth:
					{
						return "BIRT";
					}
					case Event.EventType.Adoption:
					{
						return "ADOP";
					}
					case Event.EventType.Immigration:
					{
						return "IMMI";
					}
					case Event.EventType.MilitaryService:
					{
						return "MISE";
					}
					case Event.EventType.Ordinance:
					{
						return "ORDI";
					}
					case Event.EventType.Ordination:
					{
						return "ORDN";
					}
					case Event.EventType.Emigration:
					{
						return "EMIG";
					}
					case Event.EventType.Marriage:
					{
						return "MARR";
					}
					case Event.EventType.Divorce:
					{
						return "DIV";
					}
					case Event.EventType.Naturalization:
					{
						return "NATU";
					}
				}
				throw new Exception("Invalid Event Type");
			}
		}

		public string EventText
		{
			get;
			set;
		}

		public string Latitude
		{
			get;
			set;
		}

		public string Longitude
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

		public string Place
		{
			get;
			set;
		}

		public IList<EntitySource> Sources
		{
			get;
			set;
		}

		private Event()
		{
		}

		public Event(Event.EventType eventType) : this()
		{
			this._eventType = eventType;
		}

		private static Event BuildEvent(DataHierarchyItem _event, GEDCOMContext context, Event.EventType eventType)
		{
			Event @event = new Event(eventType)
			{
				Context = context
			};
			IEnumerable<DataHierarchyItem> items = 
				from i in _event.Items
				where i.Value.StartsWith("DATE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems = 
				from i in _event.Items
				where i.Value.StartsWith("AGNC")
				select i;
			IEnumerable<DataHierarchyItem> items1 = 
				from i in _event.Items
				where i.Value.StartsWith("ADDR")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems1 = 
				from i in _event.Items
				where i.Value.StartsWith("PLAC")
				select i;
			IEnumerable<DataHierarchyItem> items2 = 
				from i in _event.Items
				where i.Value.StartsWith("LONG")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems2 = 
				from i in _event.Items
				where i.Value.StartsWith("LATI")
				select i;
			IEnumerable<DataHierarchyItem> items3 = 
				from i in _event.Items
				where i.Value.StartsWith("CAUS")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems3 = 
				from i in _event.Items
				where i.Value.StartsWith("SOUR")
				select i;
			IEnumerable<DataHierarchyItem> items4 = 
				from i in _event.Items
				where i.Value.StartsWith("NOTE")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems4 = 
				from i in _event.Items
				where i.Value.StartsWith("CHAN")
				select i;
			IEnumerable<DataHierarchyItem> items5 = 
				from i in _event.Items
				where i.Value.StartsWith("CREA")
				select i;
			IEnumerable<DataHierarchyItem> dataHierarchyItems5 = 
				from i in _event.Items
				where i.Value.StartsWith("_")
				select i;
			@event.EventText = _event.Value.GetSubstring(5);
			@event.Date = items.GetValue(5);
			@event.Agency = dataHierarchyItems.GetValue(5);
			@event.Address = items1.GetValue(5);
			@event.Place = dataHierarchyItems1.GetValue(5);
			@event.Latitude = dataHierarchyItems2.GetValue(5);
			@event.Longitude = items2.GetValue(5);
			@event.Cause = items3.GetValue(5);
			@event.Sources = EntitySource.FromDataHierarchy(dataHierarchyItems3, context);
			@event.NoteIDs = items4.GetIDs("N");
			@event.Change = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(dataHierarchyItems4, context, LINQ2GEDCOM.Entities.DateTime.DateType.Change).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			@event.Create = LINQ2GEDCOM.Entities.DateTime.FromDataHierarchy(items5, context, LINQ2GEDCOM.Entities.DateTime.DateType.Create).LastOrDefault<LINQ2GEDCOM.Entities.DateTime>();
			@event.UserDefinedTags = UserDefinedTag.FromDataHierarchy(dataHierarchyItems5, context);
			return @event;
		}

		private string FormatAddressString(int hierarchyRoot)
		{
			return string.Format("{0} ADDR {1}", hierarchyRoot, this.Address);
		}

		private string FormatAgencyString(int hierarchyRoot)
		{
			return string.Format("{0} AGNC {1}", hierarchyRoot, this.Agency);
		}

		private string FormatCauseString(int hierarchyRoot)
		{
			return string.Format("{0} CAUS {1}", hierarchyRoot, this.Cause);
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

		private string FormatEventString(int hierarchyRoot)
		{
			if (string.IsNullOrWhiteSpace(this.EventText) || !(this.EventString != this.EventText))
			{
				return string.Format("{0} {1}", hierarchyRoot, this.EventString);
			}
			return string.Format("{0} {1} {2}", hierarchyRoot, this.EventString, this.EventText);
		}

		private string FormatLatitudeString(int hierarchyRoot)
		{
			return string.Format("{0} LATI {1}", hierarchyRoot, this.Latitude);
		}

		private string FormatLongitudeString(int hierarchyRoot)
		{
			return string.Format("{0} LONG {1}", hierarchyRoot, this.Longitude);
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

		private string FormatPlaceString(int hierarchyRoot)
		{
			return string.Format("{0} PLAC {1}", hierarchyRoot, this.Place);
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

		internal static IList<Event> FromDataHierarchy(IEnumerable<DataHierarchyItem> items, GEDCOMContext context, Event.EventType eventType)
		{
			return (
				from i in items
				select Event.BuildEvent(i, context, eventType)).ToList<Event>();
		}

		internal override string ToGEDCOMString(int hierarchyRoot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.FormatEventString(hierarchyRoot));
			if (!string.IsNullOrWhiteSpace(this.Date))
			{
				stringBuilder.AppendLine(this.FormatDateString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Agency))
			{
				stringBuilder.AppendLine(this.FormatAgencyString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Cause))
			{
				stringBuilder.AppendLine(this.FormatCauseString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Address))
			{
				stringBuilder.AppendLine(this.FormatAddressString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Place))
			{
				stringBuilder.AppendLine(this.FormatPlaceString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Longitude))
			{
				stringBuilder.AppendLine(this.FormatLongitudeString(hierarchyRoot + 1));
			}
			if (!string.IsNullOrWhiteSpace(this.Latitude))
			{
				stringBuilder.AppendLine(this.FormatLatitudeString(hierarchyRoot + 1));
			}
			foreach (UserDefinedTag userDefinedTag in base.UserDefinedTags)
			{
				stringBuilder.Append(userDefinedTag.ToGEDCOMString(hierarchyRoot + 1));
			}
			if (this.Sources != null)
			{
				stringBuilder.Append(this.FormatSourceString(hierarchyRoot + 1));
			}
			if (this.NoteIDs != null)
			{
				stringBuilder.Append(this.FormatNoteString(hierarchyRoot + 1));
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

		public enum EventType
		{
			Burial,
			Death,
			Birth,
			Adoption,
			Immigration,
			MilitaryService,
			Ordinance,
			Ordination,
			Emigration,
			Marriage,
			Divorce,
			Naturalization
		}
	}
}