using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhoneShop.ModelViews;

namespace PhoneShop.Services
{
    public class ConfigureTwilioSettings : IConfigureOptions<TwilioSettings>
    {

        private readonly IConfiguration _configuration;

        public ConfigureTwilioSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(TwilioSettings options)
        {
            _configuration.GetSection("Twilio").Bind(options);
        }
    }
}


