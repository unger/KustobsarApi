using Parse;

namespace Kustobsar.Ap2.Data.ParseData.Model
{
    [ParseClassName("Site")]
    public class ParseSite : ParseObject
    {
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

        [ParseFieldName("useCount")]
        public int UseCount
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
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

        [ParseFieldName("parentId")]
        public long? ParentId
        {
            get { return GetProperty<long?>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("accuracy")]
        public int Accuracy
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("isPublic")]
        public bool? IsPublic
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }
    }
}
