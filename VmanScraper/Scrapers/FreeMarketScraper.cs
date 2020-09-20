using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VmanScraper.Models;
using VmanScraper.Printers;

namespace VmanScraper.Scrapers
{
    class FreeMarketScraper: IScraper
    {
        private readonly HttpClient client;
        public ICollection<VmanPlayer> Players { get; set; }

        public FreeMarketScraper(HttpClient client)
        {
            this.client = client;
        }

        public ICollection<VmanPlayer> Results()
        {
            return Players;
        }

        public async Task<ICollection<VmanPlayer>> Run(ActionConfig actionConfig)
        {

            Console.WriteLine($"TRACE: Processing {actionConfig.Pages} pages on free market with minimum age {actionConfig.MinimumAge}, maximum age {actionConfig.MaximumAge} and position {actionConfig.Position.Value}");

            var tasks = new List<Task<ICollection<VmanPlayer>>>();

            for (var i = 1; i <= actionConfig.Pages; i++)
            {
                tasks.Add(ProcessFreeMarketPage(actionConfig.MinimumAge, actionConfig.MaximumAge, actionConfig.Position, i));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: Could not fetch price info. Do you have available spots on your squad?");
            }

            var playersFromAllPages = new List<VmanPlayer>();
            foreach (var task in tasks)
            {
                var players = await task;
                playersFromAllPages.AddRange(players);
            }

            Console.WriteLine($"TRACE: Finished processing free market");

            Players = playersFromAllPages;

            return Players;
        }

        private async Task<ICollection<VmanPlayer>> ProcessFreeMarketPage(int mininumAge, int maximumAge, PlayerPosition position, int page)
        {
            Console.WriteLine($"TRACE: Started processing page {page}");

            var response = await client.GetAsync($"https://www.virtualmanager.com/free_transfer_listings?minimum_age={mininumAge}&maximum_age={maximumAge}&page={page}&position={position.Value}&country_id=&search=1&commit=Search");
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            Thread.Sleep(100);

            var playerInfoNodes = pageDocument.DocumentNode.SelectNodes("(//td[contains(@class,'player')]//a[@href])");
            var playerInfoLinks = playerInfoNodes
                .Select(x => x.Attributes.FirstOrDefault(x => x.Name == "href").Value)
                .Distinct()
                .ToList();

            var players = new List<VmanPlayer>();

            for (var i = 0; i < playerInfoLinks.Count(); i++)
            {
                var player = new FreeMarketPlayer(client, playerInfoLinks[i]);
                await player.GetFreeMarketInfo();
                await player.GetPlayerInfo();
                players.Add(player);
            }

            return players;
        }
    }
}
