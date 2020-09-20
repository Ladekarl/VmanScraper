using VmanScraper.Scrapers;

namespace VmanScraper.Printers
{
    interface IPlayerPrinter
    {   
        void Print(IScraper scraper);
    }
}
