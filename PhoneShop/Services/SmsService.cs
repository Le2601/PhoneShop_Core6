using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Options;
using Twilio.Clients;
using PhoneShop.ModelViews;

namespace PhoneShop.Services
{
    public class SmsService
    {
        private readonly TwilioRestClient _twilioClient;
        private readonly TwilioSettings _twilioSettings;

        public SmsService(TwilioRestClient twilioClient, IOptions<TwilioSettings> twilioSettings)
        {
            _twilioClient = twilioClient;
            _twilioSettings = twilioSettings.Value;
        }

        public void SendOtpSms(string phoneNumber, string otp)
        {
            var message = MessageResource.Create(
                body: $"Your OTP is: {otp}",
                from: new PhoneNumber(_twilioSettings.PhoneNumber),
                to: new PhoneNumber(phoneNumber),
                client: _twilioClient
            );
        }


    }
}
