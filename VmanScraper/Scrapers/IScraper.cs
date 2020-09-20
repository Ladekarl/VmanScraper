using System.Collections.Generic;
using System.Threading.Tasks;
using VmanScraper.Models;
using VmanScraper.Printers;

namespace VmanScraper.Scrapers
{
    interface IScraper
    {
        Task<ICollection<VmanPlayer>> Run(ActionConfig actionConfig);
        ICollection<VmanPlayer> Results();
    }
}
