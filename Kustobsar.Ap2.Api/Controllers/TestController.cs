using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Artportalen.Helpers;
using Common.Logging;
using Kustobsar.Ap2.Api.Logic;
using Kustobsar.Ap2.Data.ParseData.Storage;
using Kustobsar.Ap2.Data.Services;

namespace Kustobsar.Ap2.Api.Controllers
{
    public class TestController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger<TestController>();

        private readonly SightingsService sightingService;
        private readonly SiteService siteService;

        public TestController()
            : this(
            new SightingsService(new ParseSiteStorage(), new ParseSightingsStorage(new AttributeCalculator()), new TaxonStorage()), 
            new SiteService(new ParseSiteStorage()))
        {
        }

        public TestController(SightingsService sightingService, SiteService siteService)
        {
            this.sightingService = sightingService;
            this.siteService = siteService;
        }
    }
}
