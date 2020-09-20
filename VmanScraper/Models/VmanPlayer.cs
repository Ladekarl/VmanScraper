using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace VmanScraper.Models
{
    class VmanPlayer
    {
        protected readonly HttpClient client;
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; }
        public double Value { get; set; }
        public string InfoLink { get; set; }

        public VmanPlayer(HttpClient client, string infoLink)
        {
            this.client = client;
            InfoLink = infoLink;
        }

        public double ParseCurrencyString(string value)
        {
            return double.Parse(value.Trim().Split(" ")[0].Replace(",", ""));
        }

        public async Task GetPlayerInfo()
        {
            var response = await client.GetAsync($"https://www.virtualmanager.com{InfoLink}");
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var name = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'big_name')]//h1)").InnerText;
            var age = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'age')]//strong)").InnerText;
            var position = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'position')]//strong)").InnerText;
            var rating = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'rating')]//div[contains(@class,'circle')])").InnerText;
            var value = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'player_value')]//strong)").InnerText;

            Name = name;
            Age = int.Parse(age.Trim().Substring(0, 2));
            Position = position;
            Rating = int.Parse(rating);
            Value = ParseCurrencyString(value);
        }

        public override string ToString()
        {
            return $"Name: {Name}\tAge: {Age}\tPosition: {Position}\tRating: {Rating}\tValue: {Value}";
        }
    }
}
