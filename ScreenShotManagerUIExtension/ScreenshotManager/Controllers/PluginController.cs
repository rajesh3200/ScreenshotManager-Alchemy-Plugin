using Alchemy4Tridion.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;


namespace ScreenShotManager.Controllers
{ 
    /// <summary>
    /// An ApiController to create web services that your plugin can interact with.
    /// </summary>
    /// <remarks>
    /// The AlchemyRoutePrefix accepts a Service Name as its first parameter.  This will be used by both
    /// the generated Url's as well as the generated JS proxy.
    /// <c>/Alchemy/Plugins/{YourPluginName}/api/{ServiceName}/{action}</c>
    /// <c>Alchemy.Plugins.YourPluginName.Api.Service.action()</c>
    /// 
    /// The attribute is optional and if you exclude it, url's and methods will be attached to "api" instead.
    /// <c>/Alchemy/Plugins/{YourPluginName}/api/{action}</c>
    /// <c>Alchemy.Plugins.YourPluginName.Api.action()</c>
    /// </remarks>
    [AlchemyRoutePrefix("Service")]
    public class PluginController : AlchemyApiController
    {
        /// // GET /Alchemy/Plugins/{YourPluginName}/api/{YourServiceName}/YourRoute
        /// <summary>
        /// Just a simple example..
        /// </summary>
        /// <returns>A string "Your Response" as the response message.</returns>
        /// </summary>
        /// <remarks>
        /// All of your action methods must have both a verb attribute as well as the RouteAttribute in
        /// order for the js proxy to work correctly.
        /// </remarks>
        [HttpGet]
        [Route("HelloSSM")]
        public string SayHello()
        {
            return string.Format("Hello Alchemist {0}!", this.User.GetName());
        }

        //http://tridion.sdldemo.com/Alchemy/Plugins/ScreenShotManager/api/Service/enablescreenshot?pageid=%22test2%22&enabled=true
        [HttpPost]
        [Route("enablescreenshot")]
        public List<ScreenShotSubscription> AddScreenshotSubscription([FromBody]ScreenShotSubscription model)
        {
            List<ScreenShotSubscription> subscriptions;
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            subscriptions = GetScreenshotSubscriptions();
            subscriptions.Add(new ScreenShotSubscription { pageid= model.pageid, enabled= model.enabled });

            JsonSerializer serializer = new JsonSerializer();            
            using (StreamWriter sw = new StreamWriter(path + @"/screenshots.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, subscriptions);                
            }
            return subscriptions;
        }
                
        [HttpPost]
        [Route("clearscreenshot")]
        public List<ScreenShotSubscription> ClearScreenshotSubscription([FromBody]string pageid)
        {
            List<ScreenShotSubscription> subscriptions=null;
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (File.Exists(path + @"/screenshots.json"))
            {
                subscriptions = GetScreenshotSubscriptions();
                var itemtoCheck = subscriptions.Find((sb) => sb.pageid == pageid);
                subscriptions.Remove(itemtoCheck);
            }
            
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(path + @"/screenshots.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, subscriptions);
            }
            return subscriptions;
        }

        //http://tridion.sdldemo.com/Alchemy/Plugins/ScreenShotManager/api/Service/getscreenshots
        [HttpGet]
        [Route("getscreenshots")]
        public List<ScreenShotSubscription> GetScreenshotSubscriptions()
        {
            JsonSerializer serializer = new JsonSerializer();
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (File.Exists(path + @"/screenshots.json"))
            {
                using (StreamReader sr = new StreamReader(path + @"/screenshots.json"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(reader, typeof(List<ScreenShotSubscription>)) as List<ScreenShotSubscription>;
                }
            }
            return new List<ScreenShotSubscription>();
        }

        private static string GetPublicationIdofItem(string id)
        {
            string pubId = null;
            Regex regex = new Regex(@"tcm:(\d+).*");
            Match match = regex.Match(id);
            if (match.Success)
            {
                pubId = match.Groups[1].Value;
            }
            return pubId;
        }
    }

    public class ScreenShotSubscription
    {
        public string pageid { get; set; }
        public bool enabled { get; set; }
    }
}
