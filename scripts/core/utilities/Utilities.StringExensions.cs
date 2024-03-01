using System.Text;

namespace ToolShed.Utilities
{
    /// <summary>
    /// Extensions for the String class
    /// </summary>
    public static partial class Utilities
    {
        /// <summary>
        /// Multiplies a string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="multiplier">The time to repeat the string</param>
        /// <returns>The multiplied string</returns>
        public static string Multiply(this string source, int multiplier)
        {
            StringBuilder sb = new StringBuilder(multiplier * source.Length);
            for (int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }
    }
}