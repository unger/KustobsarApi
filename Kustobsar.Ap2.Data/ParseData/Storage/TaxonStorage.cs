using Kustobsar.Ap2.Data.Model;
using Kustobsar.Ap2.Data.ParseData.Model;
using Parse;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace Kustobsar.Ap2.Data.ParseData.Storage
{
    public class TaxonStorage
    {
        public string Save(TaxonDto taxon)
        {
            var parseTaxon = new ParseTaxon
            {
                ObjectId = taxon.ParseId,
                TaxonId = taxon.TaxonId,
                SortOrder = taxon.SortOrder,
                Prefix = taxon.Prefix,
                Name = taxon.CommonName,
                ScientificName = taxon.ScientificName,
                EnglishName = taxon.EnglishName,
                Type = taxon.TaxonType,
            };

            parseTaxon.SaveAsync().Wait();

            return parseTaxon.ObjectId;
        }
    }
}
