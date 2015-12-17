using System.Threading.Tasks;
using Kustobsar.Ap2.Data.Model;
using Kustobsar.Ap2.Data.ParseData.Model;
using Parse;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace Kustobsar.Ap2.Data.ParseData.Storage
{
    public class TaxonStorage
    {
        public async Task<string> Save(TaxonDto taxon)
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

            await parseTaxon.SaveAsync();

            return parseTaxon.ObjectId;
        }
    }
}
