using System;
using System.Collections.Generic;
using System.Linq;
using VmanScraper.Models;
using VmanScraper.Scrapers;

namespace VmanScraper.Printers
{
    class ZeroWagePrinter : IPlayerPrinter
    {
        public void Print(IScraper scraper)
        {
            if (scraper is FreeMarketScraper)
            {
                var freeMarketPlayers = scraper.Results().Cast<FreeMarketPlayer>();
                var orderedPlayers = freeMarketPlayers
                    .Where(x => x.ThreeDaysWages == 0)
                    .OrderBy(x => x.Rating)
                    .ThenByDescending(x => x.Age).ToList();
                if (orderedPlayers.Count() > 0)
                {
                    foreach (var player in orderedPlayers)
                    {
                        Console.WriteLine(player.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No players found");
                }
            }
            else
            {
                Console.WriteLine("Method not supported");
            }
        }
    }
}
