namespace LINQ2GEDCOM.Entities
{
    public abstract class BaseEntity
    {
        public GEDCOMContext Context { get; internal set; }

        public IList<UserDefinedTag> UserDefinedTags { get; set; } = new List<UserDefinedTag>();

        protected BaseEntity()
        {
        }

        internal abstract string ToGEDCOMString(int hierarchyRoot);
    }
}