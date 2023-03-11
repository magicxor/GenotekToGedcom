using LINQ2GEDCOM.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LINQ2GEDCOM
{
	public class GEDCOMContext
	{
		private string _file;

		private string _folder;

		public IList<Family> Families
		{
			get;
			internal set;
		}

		public string GEDCOMFile
		{
			get
			{
				return this._file;
			}
		}

		public string GEDCOMObjectFolder
		{
			get
			{
				return this._folder;
			}
		}

		public IList<Header> Headers
		{
			get;
			internal set;
		}

		public IList<Individual> Individuals
		{
			get;
			internal set;
		}

		public IList<Label> Labels
		{
			get;
			internal set;
		}

		public IList<Note> Notes
		{
			get;
			internal set;
		}

		public IList<LINQ2GEDCOM.Entities.Object> Objects
		{
			get;
			internal set;
		}

		public IList<Source> Sources
		{
			get;
			internal set;
		}

		private GEDCOMContext()
		{
		}

		public GEDCOMContext(string file, string objectFolder = null)
		{
			this._file = file;
			this._folder = objectFolder;
			this.Headers = new List<Header>();
			this.Individuals = new List<Individual>();
			this.Families = new List<Family>();
			this.Sources = new List<Source>();
			this.Objects = new List<LINQ2GEDCOM.Entities.Object>();
			this.Notes = new List<Note>();
			this.Labels = new List<Label>();
			GEDCOMDataReader.LoadDataIntoContext(this);
		}

		public void SubmitChanges(string file = null)
		{
			GEDCOMDataWriter.WriteToFile(this, file ?? this._file);
		}
	}
}