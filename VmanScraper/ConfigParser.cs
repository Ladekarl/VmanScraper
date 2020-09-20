using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using VmanScraper.Models;

namespace VmanScraper
{
    class ConfigParser
    {
        private readonly IConfiguration configuration;

        public ConfigParser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public LoginConfig ParseLoginConfig()
        {
            var loginSection = configuration.GetSection("login");
            return new LoginConfig
            {
                Email = loginSection.GetSection("email").Exists() ? loginSection.GetSection("email").Value : null,
                Password = loginSection.GetSection("password").Exists() ? loginSection.GetSection("password").Value : null
            };
        }

        public ActionConfig ParseActionConfig()
        {
            var actionSection = configuration.GetSection("action");
            return new ActionConfig
            {
                MaximumAge = actionSection.GetSection("maximum_age").Exists() ? int.Parse(actionSection.GetSection("maximum_age").Value) : 0,
                MinimumAge = actionSection.GetSection("minimum_age").Exists() ? int.Parse(actionSection.GetSection("minimum_age").Value) : 0,
                Pages = actionSection.GetSection("pages").Exists() ? int.Parse(actionSection.GetSection("pages").Value) : 0,
                Position = actionSection.GetSection("position").Exists() ? PlayerPosition.FromString(actionSection.GetSection("position").Value) : null,
            };
        }
    }
}
