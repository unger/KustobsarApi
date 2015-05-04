namespace Kustobsar.Ap2.Data.Mappings
{
    using FluentNHibernate.Mapping;

    using Kustobsar.Ap2.Data.Model;

    public class TaxonInfoMap : ClassMap<TaxonInfo>
    {
        public TaxonInfoMap()
        {
            this.Id(x => x.TaxonId).GeneratedBy.Assigned();
            this.Map(x => x.CommonName);
            this.Map(x => x.EnglishName);
            this.Map(x => x.ScientificName);
            this.Map(x => x.Auctor);
            this.Map(x => x.SortOrder);
        }
    }
}
