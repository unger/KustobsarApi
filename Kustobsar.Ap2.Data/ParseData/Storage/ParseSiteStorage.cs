using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kustobsar.Ap2.Data.Model;
using Kustobsar.Ap2.Data.ParseData.Model;
using Parse;
using SafeMapper;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace Kustobsar.Ap2.Data.ParseData.Storage
{
    public class ParseSiteStorage
    {
        public string Save(SiteDto site)
        {
            var webMerc = new WebMercatorPosition(site.SiteYCoord, site.SiteXCoord);
            var location = PositionConverter.ToWgs84(webMerc);

            var parseSite = SafeMap.Convert<SiteDto, ParseSite>(site);
            parseSite.ObjectId = site.ParseId;
            parseSite.Location = new ParseGeoPoint(location.Latitude, location.Longitude);

            parseSite.SaveAsync().Wait();

            return parseSite.ObjectId;
        }
    }
}
