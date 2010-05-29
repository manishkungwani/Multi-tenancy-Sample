namespace System.Collections.Generic
{
    using System.Linq;
    using System.Text;
    using MultiTenancy.Core;

    /// <summary>
    /// Extension methods for <see cref="System.Collections.Generic.IEnumerable{System.String}"/>
    /// </summary>
    public static class StringEnumerableExt
    {
        /// <summary>
        /// Concats all strings into a single string
        /// </summary>
        /// <param name="strings">Strings to concat together</param>
        /// <param name="deliminator">Deliminator between the string values</param>
        /// <returns>Composed string</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="strings"/> is null</exception>
        public static string ConcatAll(this IEnumerable<string> strings, string deliminator = "")
        {
            Ensure.Argument.NotNull(strings, "strings");

            strings = strings.Where(x => x.IsNotNullOrEmpty());

            if (!strings.Any())
            {
                return string.Empty;
            }

            if (strings.Count() == 1)
            {
                return strings.First();
            }

            return strings.Skip(1)
                          .Aggregate(new StringBuilder().Append(strings.First()), (builder, value) => builder.Append(deliminator).Append(value))
                          .ToString();
        }

        /// <summary>
        /// Concats all strings into a single string
        /// </summary>
        /// <param name="strings">Strings to concat together</param>
        /// <param name="deliminator">Deliminator between the string values</param>
        /// <returns>Composed string</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="strings"/> is null</exception>
        public static string ConcatAll(this IEnumerable<string> strings, char deliminator)
        {
            return strings.ConcatAll(deliminator.ToString());
        }
    }
}