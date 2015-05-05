namespace Kustobsar.Ap2.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Artportalen.Helpers;
    using Artportalen.Response;

    using Common.Logging;

    using Kustobsar.Ap2.Api.Logic;
    using Kustobsar.Ap2.Api.Models;
    using Kustobsar.Ap2.Data.Services;

    public class KustobsarController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger<KustobsarController>();

        private readonly SightingsService sightingService;

        private readonly KustobsarSightingFactory kustobsarSightingsFactory;

        public KustobsarController()
            : this(new SightingsService(), new KustobsarSightingFactory(new AttributeCalculator()))
        {
        }

        public KustobsarController(SightingsService sightingService, KustobsarSightingFactory kustobsarSightingsFactory)
        {
            this.sightingService = sightingService;
            this.kustobsarSightingsFactory = kustobsarSightingsFactory;
        }

        private int StoreDays
        {
            get
            {
                int storeDays;
                if (!int.TryParse(ConfigurationManager.AppSettings["SightingsStoredDays"], out storeDays))
                {
                    storeDays = 3;
                }

                return storeDays;
            }
        }

        public ActionResult Index(string datum, string rrksort, string rrkkod, string sort, string sortorder)
        {
            Log.InfoFormat("Index: {0}", datum);
            DateTime date;
            if (!DateTime.TryParse(datum, out date))
            {
                date = DateTime.Today;
            }

            var sightings = this.sightingService.GetSightings(date);

            var kustobsarSightings =
                sightings.Where(s => s.Taxon.TaxonId != 0).Select(this.kustobsarSightingsFactory.Create);

            int kod;
            kustobsarSightings = int.TryParse(rrkkod, out kod)
                                     ? kustobsarSightings.Where(k => k.RrkKod == kod)
                                     : kustobsarSightings.Where(k => k.RrkKod != 0);

            var orderedSightings = this.OrderSightings(kustobsarSightings, rrksort, sort, sortorder).ToList();

            return this.View(orderedSightings);
        }

        public ActionResult TestRemove()
        {
            Log.Info("TestRemove");

            this.sightingService.RemoveOldSightings(this.StoreDays);

            return new ContentResult { Content = "test" };
        }

        [HttpPost]
        public ActionResult Sightings(IList<Sighting> sightings)
        {
            try
            {
                if (sightings != null && sightings.Count > 0)
                {
                    var storeDays = this.StoreDays;
                    Log.InfoFormat("RemoveOld: {0} days", storeDays);
                    this.sightingService.RemoveOldSightings(storeDays);

                    Log.InfoFormat("Sightings: {0}", sightings.Count);
                    this.sightingService.StoreSightings(sightings);
                }
                else
                {
                    Log.Info("No sightings recieved");
                }
            }
            catch (Exception e)
            {
                Log.Error("Store sightings", e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private IEnumerable<KustobsarSighting> OrderSightings(IEnumerable<KustobsarSighting> kustobsarSightings, string rrksort, string sort, string sortorder)
        {
            var sortProperties = new List<SortField<KustobsarSighting, object>>();

            if (!string.IsNullOrEmpty(rrksort))
            {
                sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.RrkKod, rrksort == "desc"));
            }

            switch (sort)
            {
                case "2":
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.SiteXCoord, sortorder == "desc"));
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.SortOrder, sortorder == "desc"));
                    break;
                case "4":
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.SightingId, sortorder == "desc"));
                    break;
                default:
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.SortOrder, sortorder == "desc"));
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.TaxonId, sortorder == "desc"));
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.SiteXCoord, sortorder == "desc"));
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.Quantity, true));
                    sortProperties.Add(new SortField<KustobsarSighting, object>(x => x.StartTime, true));
                    break;
            }

            IOrderedEnumerable<KustobsarSighting> orderedSightings = null;

            for (int i = 0; i < sortProperties.Count; i++)
            {
                var sortField = sortProperties[i];

                if (sortField.Descending)
                {
                    orderedSightings = i == 0
                                        ? kustobsarSightings.ToList().OrderByDescending(sortField.PropertyFunc)
                                        : orderedSightings.ThenByDescending(sortField.PropertyFunc);
                }
                else
                {
                    orderedSightings = i == 0
                                        ? kustobsarSightings.ToList().OrderBy(sortField.PropertyFunc)
                                        : orderedSightings.ThenBy(sortField.PropertyFunc);
                }
            }

            return orderedSightings;
        }
    }
}