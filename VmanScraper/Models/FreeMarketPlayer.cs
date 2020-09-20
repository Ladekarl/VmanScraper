using HtmlAgilityPack;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VmanScraper.Models
{
    class FreeMarketPlayer : VmanPlayer
    {
        public double TotalPrice { get; set; }
        public double ThreeDaysWages { get; set; }
        public double SignOnBonus { get; set; }

        public FreeMarketPlayer(HttpClient client, string infoLink) : base(client, infoLink)
        {
        }

        public async Task GetFreeMarketInfo()
        {
            var response = await client.GetAsync($"https://www.virtualmanager.com{InfoLink}/free_transfer");
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var priceInfoNodes = pageDocument.DocumentNode.SelectNodes("(//tr/td[contains(@class,'amount')])");
            var priceInfos = priceInfoNodes.Where(x => x.InnerText != null).Select(x => x.InnerText).ToList();

            var totalPrice = pageDocument.DocumentNode.SelectSingleNode("(//tr/td[contains(@class,'amount total')])").InnerText;

            SignOnBonus = ParseCurrencyString(priceInfos[0]);
            ThreeDaysWages = ParseCurrencyString(priceInfos[1]);
            TotalPrice = ParseCurrencyString(totalPrice);
        }
    }
}
