using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using PhoneShop.Data;
using System.Security.Cryptography;
using System.Text;

namespace PhoneShop.Services.MoMo
{
    public class HeplerMoMo
    {
        //mã hóa SHA256 
        public string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

       
    }
}
