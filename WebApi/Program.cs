using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                /*
                
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:5566")
            .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();*/
    }
}