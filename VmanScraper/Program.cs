using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VmanScraper.Models;

namespace VmanScraper
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly LoginManager loginManager = new LoginManager(client);
        private static readonly ActionManager actionManager = new ActionManager(client);
        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to vman scraper.\n\n");

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var ConfigParser = new ConfigParser(Configuration);

            var loginConfig = ConfigParser.ParseLoginConfig();
            var actionConfig = ConfigParser.ParseActionConfig();

            await loginManager.ShowLoginInterface(loginConfig);
            await actionManager.ShowActionInterface(actionConfig);
        }
    }
}
