using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VmanScraper.Models;

namespace VmanScraper
{
    class LoginManager
    {
        private readonly HttpClient client;

        public LoginManager(HttpClient client)
        {
            this.client = client;
        }

        public async Task ShowLoginInterface(LoginConfig loginConfig)
        {
            Console.WriteLine("Please log in with you Vman credentials to continue:");

            bool success;
            do
            {
                if (loginConfig.Email == null)
                {
                    Console.Write("Email: ");
                    loginConfig.Email = Console.ReadLine();
                }

                if (loginConfig.Password == null)
                {
                    Console.Write("Password: ");
                    loginConfig.Password = Console.ReadLine();
                }

                success = await LoginAsync(loginConfig.Email, loginConfig.Password);

                if (!success)
                {
                    Console.Clear();
                    Console.WriteLine("Wrong email or password. Try again:");
                    loginConfig = new LoginConfig();
                }
            } while (success != true);

            Console.Clear();

        }

        private async Task<bool> LoginAsync(string email, string password)
        {
            var authenticityToken = await RetrieveAuthenticityToken();
            var response = await SendLoginRequest(email, password, authenticityToken);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private async Task<string> RetrieveAuthenticityToken()
        {
            var response = await client.GetAsync("https://www.virtualmanager.com");
            var pageContents = await response.Content.ReadAsStringAsync();
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);
            var input = pageDocument.DocumentNode.SelectSingleNode("(//input[contains(@name,'authenticity_token')]//@value)");
            var token = input.Attributes.FirstOrDefault(x => x.Name == "value");
            return token?.Value;
        }

        private async Task<HttpResponseMessage> SendLoginRequest(string email, string password, string authenticityToken)
        {
            client.DefaultRequestHeaders.Accept.Clear();

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("utf8", "%E2%9C%93"),
                new KeyValuePair<string, string>("authenticity_token", authenticityToken),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("commit", "Log in"),
            });

            var response = await client.PostAsync("https://www.virtualmanager.com/login", formContent);

            return response;
        }

    }
}
