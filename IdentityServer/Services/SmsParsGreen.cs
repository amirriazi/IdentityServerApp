using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PARSGREEN.RESTful.SMS;
using SharedLibrary;

namespace IdentityServer.Services
{
    public class SmsParsGreen : ISMS
    {
        
        public GeneralResult SendMessage(string[] mobiles, string text)
        {
            var result = new GeneralResult();
            var msg = new Message(apiKey: "5221EB09-DA4A-4AF3-A4F9-0153C4D26C8A");

            var smsResult = msg.SendSms(text, mobiles);
            Console.WriteLine(smsResult);

            if (!smsResult.R_Success)
            {
                result.SetError(smsResult.R_Message);
            }
            else
            {
                result.Message = smsResult.R_Message;
            }
            return result;
        }


    }
}
