using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Artportalen.Helpers;
using Kustobsar.Ap2.Data.Model;
using Kustobsar.Ap2.Data.ParseData.Model;
using Parse;
using SafeMapper;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace Kustobsar.Ap2.Data.ParseData.Storage
{
    public class ParseSightingsStorage
    {
        private readonly AttributeCalculator attributeCalculator;

        public ParseSightingsStorage(AttributeCalculator attributeCalculator)
        {
            this.attributeCalculator = attributeCalculator;
        }

        public string Save(SightingDto sighting)
        {
            var webMerc = new WebMercatorPosition(sighting.Site.SiteYCoord, sighting.Site.SiteXCoord);
            var location = PositionConverter.ToWgs84(webMerc);

            var parseSighting = new ParseSighting
            {
                ObjectId = sighting.ParseId,
                SightingId = sighting.SightingId,
                TaxonSortOrder = sighting.Taxon.SortOrder,
                TaxonPrefix = sighting.Taxon.Prefix,
                TaxonId = sighting.Taxon.TaxonId,
                TaxonName = string.IsNullOrEmpty(sighting.Taxon.CommonName) ? sighting.Taxon.ScientificName : sighting.Taxon.CommonName,
                Unsure = sighting.UnsureDetermination,
                NotRecovered = sighting.NotRecovered,
                Attribute = this.attributeCalculator.GetAttribute(sighting.Quantity, sighting.StageId, sighting.GenderId, sighting.ActivityId),
                StartDate = sighting.StartDate,
                EndDate = sighting.EndDate,
                StartTime = sighting.StartTime,
                EndTime = sighting.EndTime,
                SiteId = sighting.Site.SiteId,
                SiteName = sighting.Site.SiteName,
                Forsamling = sighting.Site.Forsamling,
                Kommun = sighting.Site.Kommun,
                Lan = sighting.Site.Lan,
                Landskap = sighting.Site.Landskap,
                Socken = sighting.Site.Socken,
                SiteXCoord = sighting.Site.SiteXCoord,
                SiteYCoord = sighting.Site.SiteYCoord,
                Location = new ParseGeoPoint(location.Latitude, location.Longitude),
                SightingObservers = sighting.SightingObservers,
                Comment = sighting.PublicComment
            };

            parseSighting.SaveAsync().Wait();

            return parseSighting.ObjectId;
        }
    }
}
