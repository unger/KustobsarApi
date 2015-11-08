namespace Kustobsar.Ap2.Data.Mappings
{
    using FluentNHibernate.Mapping;

    using Kustobsar.Ap2.Data.Model;

    public class SiteDtoMap : ClassMap<SiteDto>
    {
        public SiteDtoMap()
        {
            this.Id(x => x.SiteId).GeneratedBy.Assigned();
            this.Map(x => x.SiteName);
            this.Map(x => x.Accuracy);
            this.Map(x => x.Lan);
            this.Map(x => x.Forsamling);
            this.Map(x => x.Kommun);
            this.Map(x => x.Socken);
            this.Map(x => x.Landskap);
            this.Map(x => x.SiteYCoord);
            this.Map(x => x.SiteXCoord);
            this.Map(x => x.UseCount);
            this.Map(x => x.ParseId).Length(10);

            this.Version(x => x.Updated).CustomType("Timestamp").Nullable();
        }
    }
}
