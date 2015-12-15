
using System;
using System.IO.Abstractions;
using System.Web.Hosting;
using Common.Logging;
using Kustobsar.Ap2.Data.ParseData;
using Newtonsoft.Json;

namespace Kustobsar.Ap2.Data.Config
{
    public class AppKeys
    {
        private static readonly ILog Log = LogManager.GetLogger<ParseInitializer>();

        public string ParseApplicationId { get; set; }
        public string ParseNetKey { get; set; }
        public string ParseWebhookKey { get; set; }

        private static IFileSystem _fileSystem;
        private static AppKeys _current;

        static AppKeys()
        {
            _fileSystem = new FileSystem();
            _current = CreateInstance();
        }

        public static AppKeys Current
        {
            get { return _current; }
        }

        private static AppKeys CreateInstance()
        {
            var filepath = HostingEnvironment.MapPath("~/appkeys.json");

            try
            {
                return JsonConvert.DeserializeObject<AppKeys>(_fileSystem.File.ReadAllText(filepath));
            }
            catch (Exception e)
            {
                Log.Error("Load AppKeys error", e);
                return new AppKeys();
            }
        }
    }

}