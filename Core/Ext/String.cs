namespace System
{
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods for <see cref="System.String"/>
    /// </summary>
    public static class StringExt
    {
        public static readonly Regex UrlRegex = new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", RegexOptions.Compiled);

        public static readonly Regex EmailRegex = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        

        /// <summary>
        /// Separates a given string at the first instance of the specified character
        /// </summary>
        /// <param name="string">String to split</param>
        /// <param name="until">Character at which to split</param>
        /// <param name="rest">String value beyond the separator</param>
        /// <returns>String before the instance of the until character</returns>
        public static string SeparateAt(this string @string, char until, out string rest)
        {
            if (@string.IsNullOrEmpty())
            {
                rest = string.Empty;
                return string.Empty;
            }

            int indexOfChar = @string.IndexOf(until);

            if (indexOfChar < 0)
            {
                rest = string.Empty;
                return @string;
            }

            rest = @string.Substring(indexOfChar, @string.Length - indexOfChar);
            return @string.Substring(0, indexOfChar);
        }

        /// <summary>
        /// Separates a given string at the first instance of the specified character
        /// </summary>
        /// <param name="string">String to split</param>
        /// <param name="until">Character at which to split</param>
        /// <returns>String before the instance of the until character</returns>
        public static string SeparateAt(this string @string, char until)
        {
            string rest; // unused
            return @string.SeparateAt(until, out rest);
        }

        /// <summary>
        /// Determines if a given string is a URL
        /// </summary>
        /// <param name="string">Strign to test for URL match</param>
        /// <returns>True if value is a URL, false otherwise</returns>
        public static bool IsUrl(this string @string)
        {
            return @string.IsNotNullOrEmpty() && UrlRegex.IsMatch(@string);
        }

        /// <summary>
        /// Determines if a given string is an email address
        /// </summary>
        /// <param name="string">String to test for email address match</param>
        /// <returns>True if the value is an email addres, false otherwise</returns>
        public static bool IsEmailAddress(this string @string)
        {
            return @string.IsNotNullOrEmpty() && EmailRegex.IsMatch(@string);
        }

        /// <summary>
        /// Determines if a given string is null or empty string
        /// </summary>
        /// <param name="string">String to test</param>
        /// <returns>True if null or empty. False otherwise.</returns>
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// Determines if a given string is not null or empty string
        /// </summary>
        /// <param name="string">String to test</param>
        /// <returns>False if null or empty. True otherwise.</returns>
        public static bool IsNotNullOrEmpty(this string @string)
        {
            return !string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// Removes a specified string from a string
        /// </summary>
        /// <param name="string">String from which to remove substrings</param>
        /// <param name="substrings">Strings to remove from origal string</param>
        /// <returns>String without all substrings specified</returns>
        public static string Without(this string @string, params string[] substrings)
        {
            if (@string.IsNullOrEmpty() || substrings == null || substrings.Length == 0)
                return string.Empty;

            return substrings.Where(s => s.IsNotNullOrEmpty())
                             .Aggregate(@string, (orig, without) => orig.Replace(without, string.Empty));
        }
    }
}
