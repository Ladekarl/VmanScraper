using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VmanScraper.Scrapers;

namespace VmanScraper.Printers
{
    class HighestValuePrinter : IPlayerPrinter
    {
        public void Print(IScraper scraper)
        {
            var players = scraper.Results();
            var orderedPlayers = players
                .OrderBy(x => x.Value)
                .ThenByDescending(x => x.Age).ToList();
            foreach (var player in orderedPlayers)
            {
                Console.WriteLine(player.ToString());
            }
        }
    }
}
