using System;
using System.Net.Http;
using System.Threading.Tasks;
using VmanScraper.Models;
using VmanScraper.Printers;
using VmanScraper.Scrapers;

namespace VmanScraper
{
    class ActionManager
    {
        private readonly HttpClient client;

        public ActionManager(HttpClient client)
        {
            this.client = client;
        }

        public async Task ShowActionInterface(ActionConfig actionConfig)
        {
            Console.Clear();
            ConsoleKeyInfo option;
            do
            {
                Console.WriteLine("Select an option to continue (to exit press \"x\").\n 1: Scrape free market");
                option = Console.ReadKey();
                Console.WriteLine();
                try
                {
                    await SelectAction(option, actionConfig);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Something went wrong. Try again.");
                }
            } while (option.KeyChar != 'x');
        }

        private async Task SelectAction(ConsoleKeyInfo keyInfo, ActionConfig actionConfig)
        {
            IScraper scraper;
            switch (keyInfo.KeyChar)
            {
                case '1':
                    Console.WriteLine("Scrape free market selected.");
                    scraper = new FreeMarketScraper(client);
                    await RunScraper(actionConfig, scraper);
                    ShowPrinterInterface(scraper);
                    break;
                case 'x':
                    Console.WriteLine("Exiting program...");
                    break;
                default:
                    Console.WriteLine("Option not supported.");
                    break;
            }

            Console.WriteLine();
        }

        private void ShowPrinterInterface(IScraper scraper)
        {
            Console.Clear();
            ConsoleKeyInfo option;
            do
            {
                Console.WriteLine("How would you like to print your data? (to exit press \"x\")\n 1: Zero wage\n 2: Highest rating\n 3: Highest value");
                option = Console.ReadKey();
                Console.WriteLine();
                try
                {
                    SelectPrinter(option, scraper);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Something went wrong when trying to print results. Try a different option.");
                }
            } while (option.KeyChar != 'x');
        }

        private void SelectPrinter(ConsoleKeyInfo keyInfo, IScraper scraper)
        {
            Console.Clear();
            IPlayerPrinter printer;
            switch (keyInfo.KeyChar)
            {
                case '1':
                    Console.WriteLine("Zero wage selected");
                    printer = new ZeroWagePrinter();
                    printer.Print(scraper);
                    break;
                case '2':
                    Console.WriteLine("Highest rating selected");
                    printer = new HighestRatingPrinter();
                    printer.Print(scraper);
                    break;
                case '3':
                    Console.WriteLine("Highest value selected");
                    printer = new HighestValuePrinter();
                    printer.Print(scraper);
                    break;
                case 'x':
                    Console.WriteLine("Exiting to action menu...");
                    break;
                default:
                    Console.WriteLine("Option not supported.");
                    break;

            }
        }

        private async Task RunScraper(ActionConfig actionConfig, IScraper scraper)
        {
            while (actionConfig.MinimumAge < 15 || actionConfig.MinimumAge > 40)
            {
                Console.Write("Minimum Age (between 15 and 40): ");
                var minAgeString = Console.ReadLine();
                int.TryParse(minAgeString, out int minAge);
                actionConfig.MinimumAge = minAge;
            }

            while (actionConfig.MaximumAge < 15 || actionConfig.MaximumAge > 40 || actionConfig.MaximumAge < actionConfig.MinimumAge)
            {
                Console.Write("Maximum Age (between 15 and 40 and has to be larger or equal to minimum age): ");
                var maxAgeString = Console.ReadLine();
                int.TryParse(maxAgeString, out int maxAge);
                actionConfig.MaximumAge = maxAge;
            }

            while (actionConfig.Position == null)
            {
                Console.Write("Position (type in shorthand notation such as \"K\" for Keeper, leave empty for all): ");
                var playerPositionString = Console.ReadLine();
                try
                {
                    actionConfig.Position = PlayerPosition.FromString(playerPositionString);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Position not allowed.");
                }
            }

            while (actionConfig.Pages <= 0)
            {
                Console.Write("Amount of pages to search (larger than 1): ");
                var pagesString = Console.ReadLine();
                int.TryParse(pagesString, out int pages);
                actionConfig.Pages = pages;
            } while (actionConfig.Pages <= 0) ;

            Console.Clear();

            Console.WriteLine("Running scraper...");

            await scraper.Run(actionConfig);

            Console.Clear();
        }
    }
}
