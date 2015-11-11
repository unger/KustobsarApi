using System;
using Parse;

namespace Kustobsar.Ap2.Data.ParseData.Model
{
    [ParseClassName("Sighting")]
    public class ParseSighting : ParseObject
    {
        [ParseFieldName("sightingId")]
        public long SightingId
        {
            get { return GetProperty<long>(); }
            set { SetProperty<long>(value); }
        }

        [ParseFieldName("taxonPrefix")]
        public int? TaxonPrefix
        {
            get { return GetProperty<int?>(); }
            set { SetProperty<int?>(value); }
        }

        [ParseFieldName("taxonId")]
        public int TaxonId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("taxonName")]
        public string TaxonName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("unsure")]
        public bool Unsure
        {
            get { return GetProperty<bool>(); }
            set { SetProperty<bool>(value); }
        }

        [ParseFieldName("notRecovered")]
        public bool NotRecovered
        {
            get { return GetProperty<bool>(); }
            set { SetProperty<bool>(value); }
        }

        [ParseFieldName("attribute")]
        public string Attribute
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("startDate")]
        public DateTime StartDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty<DateTime>(value); }
        }

        [ParseFieldName("endDate")]
        public DateTime EndDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty<DateTime>(value); }
        }

        [ParseFieldName("startTime")]
        public string StartTime
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("endTime")]
        public string EndTime
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("siteId")]
        public long SiteId
        {
            get { return GetProperty<long>(); }
            set { SetProperty<long>(value); }
        }

        [ParseFieldName("siteName")]
        public string SiteName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("forsamling")]
        public string Forsamling
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("kommun")]
        public string Kommun
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("landskap")]
        public string Landskap
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("lan")]
        public string Lan
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("socken")]
        public string Socken
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("location")]
        public ParseGeoPoint Location
        {
            get { return GetProperty<ParseGeoPoint>(); }
            set { SetProperty<ParseGeoPoint>(value); }
        }

        [ParseFieldName("siteYCoord")]
        public int SiteYCoord
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("siteXCoord")]
        public int SiteXCoord
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("sightingObservers")]
        public string SightingObservers
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("comment")]
        public string Comment
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
