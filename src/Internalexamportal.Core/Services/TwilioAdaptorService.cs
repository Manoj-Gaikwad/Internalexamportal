using Internalexamportal.Core.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Internalexamportal.Core.Services
{
    public class TwilioAdaptorService : ITwilioAdaptorService
    {
        private readonly TwilioInfoConfiguration _twilioConfig;
        
     
        public TwilioAdaptorService
            (
            IOptions<TwilioInfoConfiguration> twilioConfig
            )
        {
            _twilioConfig = twilioConfig.Value;
        }

        public async Task SendAsync(string phoneNumber, string message) {

            //Initialize twilio client
            TwilioClient.Init(_twilioConfig.AccountSid, _twilioConfig.AuthToken);

            //Send Message
            await MessageResource.CreateAsync(
               to: new PhoneNumber(phoneNumber),
               from: new PhoneNumber(_twilioConfig.SenderPhoneNumber),
               body: message
               );
        }
    }
}
