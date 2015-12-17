using Artportalen.Response.Web;
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

    public class SiteService
    {
        private readonly ParseSiteStorage _siteStorage;
        private static readonly ILog Log = LogManager.GetLogger<SiteService>();

        private PropertyInfo[] siteProperties;

        public SiteService(ParseSiteStorage siteStorage)
        {
            _siteStorage = siteStorage;
        }

        private PropertyInfo[] SiteProperties
        {
            get
            {
                if (this.siteProperties == null)
                {
                    this.siteProperties =
                        typeof(SiteResponse).GetProperties().Where(p => p.PropertyType == typeof(string)).ToArray();
                }

                return this.siteProperties;
            }
        }

        public void StoreSites(IEnumerable<SiteResponse> sites)
        {
            var updatedSiteIds = new HashSet<long>();
            var siteDtos = new Dictionary<long, SiteDto>();

            foreach (var site in sites)
            {
                if (!siteDtos.ContainsKey(site.SiteId))
                {
                    var siteDto = new SiteDto
                    {
                        SiteId = site.SiteId,
                        SiteName = site.SiteName,
                        Kommun = site.Kommun,
                        SiteYCoord = site.SiteYCoord,
                        SiteXCoord = site.SiteXCoord,
                        Accuracy = site.Accuracy,
                        IsPublic = site.IsPublic,
                        ParentId = site.ParentId != 0 ? site.ParentId : (int?)null
                    };

                    siteDtos[site.SiteId] = siteDto;
                }
            }

            using (var session = NHibernateConfiguration.GetSession())
            {
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
                        if (!string.IsNullOrEmpty(siteDto.ParseId))
                        {
                            updatedSiteIds.Add(siteDto.SiteId);
                        }

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

                        site.ParentId = siteDto.ParentId;
                        site.Accuracy = siteDto.Accuracy;
                        site.IsPublic = siteDto.IsPublic;

                        session.Update(site);

                        siteDtos[key] = site;
                    }
                }
                session.Flush();
            }

            // Uppdatera endast lokaler som redan har ett ParseId för att undvika dubbletter
            using (var session = NHibernateConfiguration.GetSession())
            {
                foreach (var key in updatedSiteIds.ToArray())
                {
                    var siteDto = siteDtos[key];
                    try
                    {
                        var id = _siteStorage.Save(siteDto).Result;
                    }
                    catch (Exception e)
                    {
                        Log.Error("Unable to save site " + siteDto.SiteId + " to parse", e);
                    }
                }
                session.Flush();
            }
        }
    }
}
