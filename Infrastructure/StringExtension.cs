using System;
using System.Text;

namespace Infrastructure
{
    public class StringExtension
    {
        public static string GetRandomString(int length)
        {
            var random = new Random();
            var randomString = new StringBuilder();
            var charRange = 'z' - 'A';
            for (int i = 0; i < length; i++)
            {
                var randomChar = (char) ('A' + random.Next(0, charRange));
                randomString.Append(randomChar);
            }

            return randomString.ToString();
        }
    }
}
