using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenizenMetaWebsite.MetaObjects;
using FreneticUtilities.FreneticDataSyntax;
using FreneticUtilities.FreneticExtensions;
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

        public static List<WebsiteMetaObjectType> ObjectTypes;

        public static List<WebsiteMetaEvent> Events;

        public static List<WebsiteMetaAction> Actions;

        public static List<WebsiteMetaLanguage> Languages;

        public static List<WebsiteMetaMechanism> Mechanisms;

        public static List<WebsiteMetaObject> AllObjects;

        public static void Init()
        {
            Config = FDSUtility.ReadFile("config/config.fds");
            ReloadWebhookToken = Config.GetString("reload-webhook-token");
            if (Config.HasKey("alt-sources"))
            {
                MetaDocsLoader.SourcesToUse = Config.GetStringList("alt-sources").ToArray();
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
                MetaDocs docs = MetaDocsLoader.DownloadAll();
                Console.WriteLine("Meta loaded, HTMLizing...");
                List<WebsiteMetaCommand> _commands = new List<WebsiteMetaCommand>();
                List<WebsiteMetaTag> _tags = new List<WebsiteMetaTag>();
                List<WebsiteMetaObjectType> _objectTypes = new List<WebsiteMetaObjectType>();
                List<WebsiteMetaEvent> _events = new List<WebsiteMetaEvent>();
                List<WebsiteMetaAction> _actions = new List<WebsiteMetaAction>();
                List<WebsiteMetaLanguage> _languages = new List<WebsiteMetaLanguage>();
                List<WebsiteMetaMechanism> _mechanisms = new List<WebsiteMetaMechanism>();
                List<WebsiteMetaObject> _allObjects = new List<WebsiteMetaObject>();
                void procSet<T, T2>(ref List<T> webObjs, ICollection<T2> origObjs) where T: WebsiteMetaObject<T2>, new() where T2: MetaObject
                {
                    foreach (T2 obj in origObjs)
                    {
                        T webObj = new T() { Object = obj };
                        webObjs.Add(webObj);
                    }
                    webObjs = webObjs.OrderBy(o => string.IsNullOrWhiteSpace(o.Object.Plugin) ? 0 : 1).ThenBy(o => o.Object.Warnings.Count).ThenBy(o => o.Object.Group).ThenBy(o => o.Object.CleanName).ToList();
                    _allObjects.AddRange(webObjs);
                }
                procSet(ref _commands, docs.Commands.Values);
                procSet(ref _tags, docs.Tags.Values);
                procSet(ref _objectTypes, docs.ObjectTypes.Values);
                procSet(ref _events, docs.Events.Values);
                procSet(ref _actions, docs.Actions.Values);
                procSet(ref _languages, docs.Languages.Values);
                procSet(ref _mechanisms, docs.Mechanisms.Values);
                foreach (WebsiteMetaObject obj in _allObjects)
                {
                    obj.Docs = docs;
                    obj.LoadHTML();
                }
                Commands = _commands;
                Tags = _tags;
                ObjectTypes = _objectTypes;
                Events = _events;
                Actions = _actions;
                Languages = _languages;
                Mechanisms = _mechanisms;
                AllObjects = _allObjects;
                MetaDocs.CurrentMeta = docs;
                Console.WriteLine("Meta loaded and ready!");
            }
        }
    }
}
