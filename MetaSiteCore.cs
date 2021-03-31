using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenizenMetaWebsite.MetaObjects;
using FreneticUtilities.FreneticDataSyntax;
using FreneticUtilities.FreneticToolkit;
using SharpDenizenTools.MetaHandlers;
using SharpDenizenTools.MetaObjects;

namespace DenizenMetaWebsite
{
    public class MetaSiteCore
    {
        public static FDSSection Config;

        public static string ReloadWebhookToken;

        public static List<WebsiteMetaCommand> Commands;

        public static List<WebsiteMetaTag> Tags;

        public static List<WebsiteMetaEvent> Events;

        public static List<WebsiteMetaAction> Actions;

        public static List<WebsiteMetaLanguage> Languages;

        public static List<WebsiteMetaMechanism> Mechanisms;

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

        public static LockObject ReloadTimeLock = new LockObject(), ReloadLock = new LockObject();

        public static void ReloadMeta()
        {
            lock (ReloadTimeLock)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                if (now.Subtract(LastReload).TotalSeconds < 15)
                {
                    Console.WriteLine("Ignoring too-fast reload...");
                    return;
                }
                LastReload = now;
            }
            lock (ReloadLock)
            {
                Console.WriteLine("Reloading meta...");
                MetaDocs docs = new MetaDocs();
                docs.DownloadAll();
                Console.WriteLine("Meta loaded, HTMLizing...");
                List<WebsiteMetaCommand> _commands = new List<WebsiteMetaCommand>();
                List<WebsiteMetaTag> _tags = new List<WebsiteMetaTag>();
                List<WebsiteMetaEvent> _events = new List<WebsiteMetaEvent>();
                List<WebsiteMetaAction> _actions = new List<WebsiteMetaAction>();
                List<WebsiteMetaLanguage> _languages = new List<WebsiteMetaLanguage>();
                List<WebsiteMetaMechanism> _mechanisms = new List<WebsiteMetaMechanism>();
                foreach (MetaCommand obj in docs.Commands.Values)
                {
                    WebsiteMetaCommand webObj = new WebsiteMetaCommand() { Object = obj };
                    webObj.LoadHTML();
                    _commands.Add(webObj);
                }
                foreach (MetaTag obj in docs.Tags.Values)
                {
                    WebsiteMetaTag webObj = new WebsiteMetaTag() { Object = obj };
                    webObj.LoadHTML();
                    _tags.Add(webObj);
                }
                foreach (MetaEvent obj in docs.Events.Values)
                {
                    WebsiteMetaEvent webObj = new WebsiteMetaEvent() { Object = obj };
                    webObj.LoadHTML();
                    _events.Add(webObj);
                }
                foreach (MetaAction obj in docs.Actions.Values)
                {
                    WebsiteMetaAction webObj = new WebsiteMetaAction() { Object = obj };
                    webObj.LoadHTML();
                    _actions.Add(webObj);
                }
                foreach (MetaLanguage obj in docs.Languages.Values)
                {
                    WebsiteMetaLanguage webObj = new WebsiteMetaLanguage() { Object = obj };
                    webObj.LoadHTML();
                    _languages.Add(webObj);
                }
                foreach (MetaMechanism obj in docs.Mechanisms.Values)
                {
                    WebsiteMetaMechanism webObj = new WebsiteMetaMechanism() { Object = obj };
                    webObj.LoadHTML();
                    _mechanisms.Add(webObj);
                }
                Commands = _commands;
                Tags = _tags;
                Events = _events;
                Actions = _actions;
                Languages = _languages;
                Mechanisms = _mechanisms;
                MetaDocs.CurrentMeta = docs;
                Console.WriteLine("Meta loaded and ready!");
            }
        }
    }
}
