using Parse;

namespace Kustobsar.Ap2.Data.ParseData.Model
{
    [ParseClassName("Taxon")]
    public class ParseTaxon : ParseObject
    {
        [ParseFieldName("taxonId")]
        public int TaxonId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("prefix")]
        public int? Prefix
        {
            get { return GetProperty<int?>(); }
            set { SetProperty<int?>(value); }
        }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("scientificName")]
        public string ScientificName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("englishName")]
        public string EnglishName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("sortOrder")]
        public int? SortOrder
        {
            get { return GetProperty<int?>(); }
            set { SetProperty<int?>(value); }
        }

        [ParseFieldName("type")]
        public string Type
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
