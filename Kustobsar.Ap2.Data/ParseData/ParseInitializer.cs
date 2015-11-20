using System;
using System.IO.Abstractions;
using System.Web.Hosting;
using Common.Logging;
using Kustobsar.Ap2.Data.ParseData.Model;
using Newtonsoft.Json;
using Parse;

namespace Kustobsar.Ap2.Data.ParseData
{
    public class ParseInitializer
    {
        private static readonly ILog Log = LogManager.GetLogger<ParseInitializer>();

        private static IFileSystem _fileSystem;

        static ParseInitializer()
        {
            _fileSystem = new FileSystem();
        }

        public static void Initialize()
        {
            var parseKeys = GetKeys();

            ParseObject.RegisterSubclass<ParseSite>();
            ParseObject.RegisterSubclass<ParseSighting>();
            ParseObject.RegisterSubclass<ParseTaxon>();
            ParseClient.Initialize(parseKeys.ApplicationId, parseKeys.NetKey);

        }

        private static ParseKeys GetKeys()
        {
            var filepath = HostingEnvironment.MapPath("~/parsekeys.json");

            try
            {
                return JsonConvert.DeserializeObject<ParseKeys>(_fileSystem.File.ReadAllText(filepath));
            }
            catch (Exception e)
            {
                Log.Error("Load Parse keys error", e);
                return new ParseKeys();
            }
        }

        public class ParseKeys
        {
            public string ApplicationId { get; set; }
            public string NetKey { get; set; }
        }
    }
}
