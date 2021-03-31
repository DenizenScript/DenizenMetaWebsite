using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreneticUtilities.FreneticDataSyntax;
using FreneticUtilities.FreneticToolkit;
using SharpDenizenTools.MetaHandlers;

namespace DenizenMetaWebsite
{
    public class MetaSiteCore
    {
        public static FDSSection Config;

        public static string ReloadWebhookToken;

        public static void Init()
        {
            Config = FDSUtility.ReadFile("config/config.fds");
            ReloadWebhookToken = Config.GetString("reload-webhook-token");
            if (Config.HasKey("alt-sources"))
            {
                MetaDocs.SourcesToUse = Config.GetStringList("alt-sources").ToArray();
            }
            ReloadMeta();
        }

        public static DateTimeOffset LastReload;

        public static LockObject ReloadLock = new LockObject();

        public static void ReloadMeta()
        {
            lock (ReloadLock)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                if (now.Subtract(LastReload).TotalSeconds < 5)
                {
                    return;
                }
                LastReload = now;
                MetaDocs docs = new MetaDocs();
                docs.DownloadAll();
                MetaDocs.CurrentMeta = docs;
            }
        }
    }
}
