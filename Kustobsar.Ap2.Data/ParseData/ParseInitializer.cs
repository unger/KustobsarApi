using System;
using System.IO.Abstractions;
using System.Web.Hosting;
using Common.Logging;
using Kustobsar.Ap2.Data.Config;
using Kustobsar.Ap2.Data.ParseData.Model;
using Newtonsoft.Json;
using Parse;

namespace Kustobsar.Ap2.Data.ParseData
{
    public class ParseInitializer
    {
        public static void Initialize()
        {

            ParseObject.RegisterSubclass<ParseSite>();
            ParseObject.RegisterSubclass<ParseSighting>();
            ParseObject.RegisterSubclass<ParseTaxon>();
            ParseClient.Initialize(AppKeys.Current.ParseApplicationId, AppKeys.Current.ParseNetKey);
        }
    }
}
