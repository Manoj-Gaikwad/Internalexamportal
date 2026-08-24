using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Internalexamportal.Core.Commons.Utils
{
    public static class Utilities
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        //Generate Cryptogrphically Secure Randoom String
        public static string GetRandomString(int length)
        {
            int maxSize = length;
            char[] chars = new char[70];
            string a;
            a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data) { result.Append(chars[b % (chars.Length)]); }
            return result.ToString();
        }

        //Get Indian Standard Time from UTC date time.
        public static DateTime GetISTDateTime()
        {
           
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,INDIAN_ZONE);
            
        }

        //Remove HTML Tags from String
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        private readonly static Random _rand = new Random();

        public static string GeneratePassword(int length = 12)
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "1234567890";
            const string special = "!@#$%^&*_-";

            // Get cryptographically random sequence of bytes
            var bytes = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            // Build up a string using random bytes and character classes
            var res = new StringBuilder();
            for(int i = 0; i<4; i++)
            {
                // Randomly select a character class for each byte
                switch (i)
                {
                    // Prefer letters 2:1
                    case 0:
                        res.Append(lower[_rand.Next(lower.Count())]);
                        break;
                    case 1:
                        res.Append(upper[_rand.Next(upper.Count())]);
                        break;
                    case 2:
                        res.Append(number[_rand.Next(number.Count())]);
                        break;
                    case 3:
                        res.Append(special[_rand.Next(special.Count())]);
                        break;
                }
            }

            return GetRandomString(length-4) + res.ToString();
        }

        public static DateTime? stringToDate(string dateString)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime? dateTime = null;
            try
            {
                dateTime = DateTime.ParseExact(dateString, "dd/MM/yyyy", provider);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return dateTime;
        }
    }
}
