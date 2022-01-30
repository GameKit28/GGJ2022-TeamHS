using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeEngine
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove all special characters from a string. Allowed characters are A-Z (uppercase or lowercase), numbers (0-9), underscore (_), and the dot sign (.).
        /// </summary>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string sourceString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in sourceString)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
