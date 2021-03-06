using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreneticUtilities.FreneticToolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DenizenMetaWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SpecialTools.Internationalize();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}
