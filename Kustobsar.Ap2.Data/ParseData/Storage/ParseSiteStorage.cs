﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kustobsar.Ap2.Data.Model;
using Kustobsar.Ap2.Data.ParseData.Model;
using Parse;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace Kustobsar.Ap2.Data.ParseData.Storage
{
    public class ParseSiteStorage
    {
        public async Task<string> Save(SiteDto site)
        {
            var webMerc = new WebMercatorPosition(site.SiteYCoord, site.SiteXCoord);
            var location = PositionConverter.ToWgs84(webMerc);

            var parseSite = new ParseSite
            {
                ObjectId = site.ParseId,
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                Forsamling = site.Forsamling,
                Kommun = site.Kommun,
                Lan = site.Lan,
                Landskap = site.Landskap,
                Socken = site.Socken,
                UseCount = site.UseCount ?? 0,
                SiteXCoord = site.SiteXCoord,
                SiteYCoord = site.SiteYCoord,
                ParentId = site.ParentId,
                IsPublic = site.IsPublic,
                Accuracy = site.Accuracy,
                Location = new ParseGeoPoint(location.Latitude, location.Longitude)
            };

            await parseSite.SaveAsync();

            return parseSite.ObjectId;
        }
    }
}
