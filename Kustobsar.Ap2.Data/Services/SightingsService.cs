using Kustobsar.Ap2.Data.ParseData.Storage;
using SafeMapper;

namespace Kustobsar.Ap2.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Artportalen.Response;

    using Common.Logging;

    using Kustobsar.Ap2.Data;
    using Kustobsar.Ap2.Data.Model;

    using NHibernate.Linq;

    public class SightingsService
    {
        private readonly ParseSiteStorage _siteStorage;
        private readonly ParseSightingsStorage _sightingsStorage;
        private readonly TaxonStorage _taxonStorage;
        private static readonly ILog Log = LogManager.GetLogger<SightingsService>();

        private PropertyInfo[] sightingProperties;

        public SightingsService(ParseSiteStorage siteStorage, ParseSightingsStorage sightingsStorage, TaxonStorage taxonStorage)
        {
            _siteStorage = siteStorage;
            _sightingsStorage = sightingsStorage;
            _taxonStorage = taxonStorage;
        }

        private PropertyInfo[] SightingProperties
        {
            get
            {
                if (this.sightingProperties == null)
                {
                    this.sightingProperties =
                        typeof(Sighting).GetProperties().Where(p => p.PropertyType == typeof(string)).ToArray();
                }

                return this.sightingProperties;
            }
        }

        public IEnumerable<SightingDto> GetSightings(DateTime date)
        {
            var nextDate = date.AddDays(1);
            using (var session = NHibernateConfiguration.GetSession())
            {
                return session.Query<SightingDto>().Where(x => x.StartDate >= date && x.EndDate < nextDate).ToArray();
            }
        }

        public long? GetLastSightingId()
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                var sighting = session.Query<SightingDto>().OrderByDescending(x => x.SightingId).FirstOrDefault();
                if (sighting != null)
                {
                    return sighting.SightingId;
                }

                return null;
            }
        }

        public void StoreSightings(IEnumerable<Sighting> sightings)
        {
            var siteDtos = new Dictionary<long, SiteDto>();
            var taxonDtos = new Dictionary<int, TaxonDto>();
            var sightingDtos = new List<SightingDto>();

            foreach (var sighting in sightings)
            {
                if (sighting.SightingObservers != null)
                {
                    sighting.SightingObservers = sighting.SightingObservers.Trim();
                }

                this.VerifyStringLengths(sighting);

                var taxonDto = SafeMap.Convert<Sighting, TaxonDto>(sighting);
                var siteDto = SafeMap.Convert<Sighting, SiteDto>(sighting);
                var sightingDto = SafeMap.Convert<Sighting, SightingDto>(sighting);

                if (taxonDto.TaxonId == 0)
                {
                    taxonDto.TaxonId = this.FindTaxonIdByName(
                        taxonDto.ScientificName,
                        taxonDto.CommonName,
                        taxonDto.EnglishName);
                }

                // Taxons
                if (!taxonDtos.ContainsKey(taxonDto.TaxonId))
                {
                    taxonDtos.Add(taxonDto.TaxonId, taxonDto);
                }
                else
                {
                    taxonDto = taxonDtos[taxonDto.TaxonId];
                }

                sightingDto.Taxon = taxonDto;

                // Sites
                if (!siteDtos.ContainsKey(siteDto.SiteId))
                {
                    siteDtos.Add(siteDto.SiteId, siteDto);
                }
                else
                {
                    siteDto = siteDtos[siteDto.SiteId];
                }

                sightingDto.Site = siteDto;

                sightingDtos.Add(sightingDto);
            }

            using (var session = NHibernateConfiguration.GetSession())
            {
                // Taxons
                foreach (var key in taxonDtos.Keys.ToArray())
                {
                    var taxonDto = taxonDtos[key];
                    var taxon = session.Get<TaxonDto>(key);
                    if (taxon == null)
                    {
                        session.Save(taxonDto, taxonDto.TaxonId);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(taxonDto.EnglishName) && taxon.EnglishName != taxonDto.EnglishName)
                        {
                            taxon.EnglishName = taxonDto.EnglishName;
                        }

                        session.Update(taxon);
                        taxonDtos[key] = taxon;
                    }
                }

                // Sites
                foreach (var key in siteDtos.Keys.ToArray())
                {
                    var siteDto = siteDtos[key];
                    var site = session.Get<SiteDto>(key);
                    if (site == null)
                    {
                        session.Save(siteDto, siteDto.SiteId);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(siteDto.SiteName) && site.SiteName != siteDto.SiteName)
                        {
                            site.SiteName = siteDto.SiteName;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Kommun) && site.Kommun != siteDto.Kommun)
                        {
                            site.Kommun = siteDto.Kommun;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Landskap) && site.Landskap != siteDto.Landskap)
                        {
                            site.Landskap = siteDto.Landskap;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Forsamling) && site.Forsamling != siteDto.Forsamling)
                        {
                            site.Forsamling = siteDto.Forsamling;
                        }

                        site.SiteXCoord = siteDto.SiteXCoord;
                        site.SiteYCoord = siteDto.SiteYCoord;

                        session.Update(site);

                        siteDtos[key] = site;
                    }
                }

                var updatedUseCountSites = new Dictionary<long, SiteDto>();

                foreach (var sighting in sightingDtos)
                {
                    sighting.Taxon = taxonDtos[sighting.Taxon.TaxonId];
                    sighting.Site = siteDtos[sighting.Site.SiteId];

                    var sightingFromDb = session.Get<SightingDto>(sighting.SightingId);
                    if (sightingFromDb == null)
                    {
                        var site = siteDtos[sighting.Site.SiteId];
                        site.UseCount = (site.UseCount ?? 0) + 1;
                        if (!updatedUseCountSites.ContainsKey(site.SiteId))
                        {
                            updatedUseCountSites.Add(site.SiteId, site);
                        }

                        session.Save(sighting, sighting.SightingId);
                    }
                    else
                    {
                        sighting.ParseId = sightingFromDb.ParseId;
                        session.Delete(sightingFromDb);
                        session.Save(sighting, sighting.SightingId);
                    }
                }

                foreach (var key in updatedUseCountSites.Keys)
                {
                    session.Update(updatedUseCountSites[key]);
                }

                session.Flush();
            }

            using (var session = NHibernateConfiguration.GetSession())
            {
                foreach (var key in siteDtos.Keys.ToArray())
                {
                    var siteDto = siteDtos[key];
                    try
                    {
                        var id = _siteStorage.Save(siteDto);
                        if (siteDto.ParseId == null)
                        {
                            siteDto.ParseId = id;
                            session.Update(siteDto);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Unable to save site " + siteDto.SiteId + " to parse", e);
                    }
                }

                foreach (var sightingDto in sightingDtos)
                {
                    try
                    {
                        var id = _sightingsStorage.Save(sightingDto);
                        if (sightingDto.ParseId == null)
                        {
                            sightingDto.ParseId = id;
                            session.Update(sightingDto);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Unable to save sighting " + sightingDto.SightingId + " to parse", e);
                    }
                }

                // Taxons
                foreach (var key in taxonDtos.Keys.ToArray())
                {
                    var taxonDto = taxonDtos[key];
                    try
                    {
                        // Only save taxons not yet created to parse
                        if (taxonDto.ParseId == null)
                        {
                            var id = _taxonStorage.Save(taxonDto);
                            taxonDto.ParseId = id;
                            session.Update(taxonDto);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Unable to save taxon " + taxonDto.TaxonId + " to parse", e);
                    }
                }


                session.Flush();
            }
        }

        public void RemoveOldSightings(int days)
        {
            if (days > 0)
            {
                using (var session = NHibernateConfiguration.GetSession())
                {
                    session.CreateQuery("delete from SightingDto d where d.StartDate < :startDate")
                        .SetDateTime("startDate", DateTime.Today.AddDays(-days))
                        .ExecuteUpdate();
                }
            }
        }

        private int FindTaxonIdByName(string scientificName, string commonName, string englishName)
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                var result =
                    session.Query<TaxonInfo>()
                        .FirstOrDefault(
                            x =>
                            x.ScientificName == scientificName || x.CommonName == commonName
                            || x.EnglishName == englishName);

                if (result != null)
                {
                    return result.TaxonId;
                }

                return 0;
            }
        }

        private void VerifyStringLengths(Sighting sighting)
        {
            foreach (var prop in this.SightingProperties)
            {
                var value = prop.GetValue(sighting) as string;
                if (value != null)
                {
                    if (value.Length > 255 && prop.Name != "PublicComment")
                    {
                        Log.ErrorFormat(
                            "SightingId: {0} String value will be truncated property: {1} value: '{2}'",
                            sighting.SightingId,
                            prop.Name,
                            value.Length);
                        prop.SetValue(sighting, value.Substring(0, 254));
                    }
                }
            }
        }
    }
}
