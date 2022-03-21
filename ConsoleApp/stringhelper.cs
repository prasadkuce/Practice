using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.IO;
using System.Security.Cryptography;


//https://www.nuget.org/packages/Humanizer
//http://channel9.msdn.com/coding4fun/blog/Humanizer-helps-turn-geeky-strings-type-names-enum-fields-date-timespan-values-into-a-human-friendly
namespace Core.Strings
{
    public static class StringHelper
    {

        public static string GetDeveloperEmailAddress(string userName)
        {
            if (!string.IsNullOrEmpty(userName) && userName.IndexOf(@"\") != -1)

                if (!string.IsNullOrEmpty(userName) && userName.IndexOf(@"\") != -1)
                    userName = userName.Split('\\')[1].ToLower();

            string first = string.Empty;
            string last = string.Empty;
            switch (userName)
            {
                case "bdwyer":
                    first = "Barry"; last = "Dwyer";
                    break;
                case "pbailey":
                    first = "Paul"; last = "Bailey";
                    break;
                case "glackerman":
                    first = "gary"; last = "Lackerman";
                    break;
                case "apalaparty":
                    first = "Amita"; last = "Palaparty";
                    break;
                case "lespino":
                    first = "Luz"; last = "espino";
                    break;
                default:

                    break;
            }
            if (!string.IsNullOrEmpty(last))
                return string.Format("{0}.{1}@pnmac.com", first, last);
            else
                return string.Empty;
        }



        // Methods
        public static string CapitalizeFirstCharacter(string original)
        {
            if (original.Length > 0)
            {
                return (original.Substring(0, 1).ToUpper() + original.Substring(1).ToLower());
            }
            return string.Empty;
        }



        public static string FilterChars(string input, string validCharacters)
        {
            string str = "";
            char[] anyOf = validCharacters.ToCharArray();
            char[] chArray2 = input.ToCharArray();
            for (int i = 0; i <= chArray2.GetUpperBound(0); i++)
            {
                if (chArray2[i].ToString().IndexOfAny(anyOf) != -1)
                {
                    str = str + chArray2[i];
                }
            }
            return str;
        }


        public static bool IsDigits(string strValue, bool blnMustHaveOneDigit)
        {
            Regex regex = new Regex(string.Format(@"^\d{0}$", blnMustHaveOneDigit ? "+" : "*"));
            return regex.IsMatch(strValue);
        }


        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        public static bool IsDate(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(strDate, out dt);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string RemoveFinalChar(string s)
        {
            if (s.Length > 1)
                s = s.Substring(0, s.Length - 1);
            return s;
        }

        public static string RemoveFinalComma(string s)
        {
            if (s.Trim().Length > 0)
            {
                int num = s.LastIndexOf(",");
                if (num > 0)
                {
                    s = s.Substring(0, s.Length - (s.Length - num));
                }
            }
            return s;
        }

        public static string RemoveWhitespace(this string value)
        {
            if (value == null) return null;
            var result = new Regex(@"\s+").Replace(value, " ");
            return result.Trim();
        }

        public static string RemoveSpaces(this string s)
        {
            s = s.Trim();
            s = s.Replace(" ", "");
            return s;
        }

        public static string RemoveExtraSpaces(this string s)
        {
            s = s.Trim();
            s = Regex.Replace(s, @"( ) +", "$1");
            return s;
        }

        public static string RemoveHtml(this string value)
        {
            if (value == null) return null;

            return System.Text.RegularExpressions.Regex.Replace(value, @"<(.|\n)*?>", string.Empty);
        }


        public static string StripHTML(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }



                // That's it.
                return result;
            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }
        }

        public static string GetFirstWords(this string value, int numberOfWords)
        {
            var words = value.Replace("  ", " ");
            var wordCount = 0;
            var pos = 0;

            while (wordCount < numberOfWords && pos < words.Length)
            {
                if (words[pos++] == ' ')
                    wordCount++;
            }

            return words.Substring(0, pos).Trim();
        }

        public static string CleanHtml(string Html)
        {
            return ((Html.Length == 0) ? "" : Html.Replace("&", "&amp;").Replace("<", "&lt;").Replace("\r\n", "<BR>").Replace(" ", "&nbsp;"));
        }


        public static string FormatHtmlTable(System.Data.DataTable dt, string title = "")
        {
            string tab = "\t";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<h2>{0}</h2>", title);

            sb.AppendLine(tab + tab + "<table border='1'>");

            // headers.
            sb.Append(tab + tab + tab + "<tr>");

            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<td valign='top'>{0}</td>", dc.ColumnName);
            }

            sb.AppendLine("</tr>");

            // data rows
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(tab + tab + tab + "<tr>");

                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    sb.AppendFormat("<td valign='top'>{0}</td>", System.Web.HttpUtility.HtmlEncode(cellValue));
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine(tab + tab + "</table>");
            return sb.ToString();
        }


        public static string Mask(string text, char maskChar = 'X', int unmaskedCharCount = 4)
        {
            return String.IsNullOrWhiteSpace(text) ? text : text.Substring(text.Length - unmaskedCharCount).PadLeft(text.Length, maskChar);
        }

        public static string FormatHtmlTable(NameValueCollection collection, string title)
        {
            string format = "<table cellspacing=\"1\" cellpadding=\"1\" border=\"0\" style=\"width:600;\" ><tr><td colspan=2><h2>{0}</h2></td></tr>{1}</table><br>" + Environment.NewLine + Environment.NewLine;
            string str2 = "<tr><td valign=\"top\">{0}</td><td>{1}</td></tr>";
            string str3 = string.Empty;
            for (int i = 0; i < collection.Count; i++)
            {
                str3 = str3 + string.Format(str2, CleanHtml(collection.Keys[i].ToString()), CleanHtml(collection[i].ToString()));
            }
            return string.Format(format, title, str3);
        }

        public static string SplitByCase(string pText)
        {
            if (string.IsNullOrEmpty(pText)) return string.Empty;
            string pattern = @"(\p{Ll})(\p{Lu})|m_+";
            return Regex.Replace(pText, pattern, "$1 $2");
        }

        public static string ToSentenceCase(string input)
        {
            return CapitalizeFirstCharacter(input.ToLower());
        }

        public static string ToTitleCase(string input)
        {
            System.Text.StringBuilder titleCase = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(input))
            {
                string[] words = SplitByCase(input).Split(' ');

                bool first = true;
                foreach (string word in words)
                {
                    if (!first)
                        titleCase.Append(' ');
                    titleCase.Append(CapitalizeFirstCharacter(word));
                    first = false;
                }
            }

            return titleCase.ToString();
            //return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        public static string SeparateDirectionFromHouseNumber(string input)
        {
            // regex matches both "123N Main St" and "123N33rd St"
            Regex r = new Regex(@"(\d+)([a-z]+)(?<=[a-z])(\d+)?", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (r.IsMatch(input))
            {
                foreach (Match m in r.Matches(input))
                {
                    if (IsOrdinalString(m.Value))
                        continue;
                    string replacement = string.Format("{0} {1}{2}{3}",
                        m.Groups[1].Value,
                        m.Groups[2].Value,
                        !string.IsNullOrEmpty(m.Groups[3].Value) ? " " : null,
                        !string.IsNullOrEmpty(m.Groups[3].Value) ? m.Groups[3].Value : null);
                    input = input.Replace(m.Value, replacement);
                }
            }
            return input;
        }

        public static bool IsOrdinalString(string input)
        {
            Regex r = new Regex(@"\d+(st|nd|rd|th)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.IsMatch(input);
        }

        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

        public static string GetYesNo(string input)
        {
            string output = input;
            string lower = string.IsNullOrEmpty(input) ? null : input.ToLower();
            if (String.IsNullOrEmpty(input) || lower == "false" || lower == "0" || lower == "n")
                output = "No";
            else if (lower == "true" || lower == "1" || lower == "y")
                output = "Yes";

            return output;
        }

		public static bool FromYesNo(string input)
		{
			input = input.ToLowerInvariant();
			if (input == "yes" || input == "y" || input == "true" || input == "0")
				return true;
			return false;
		}

        public static string TokenizeCsvSql(string input)
        {
            string output = input
                .Replace("[", "_bracketstart_")
                .Replace("]", "_bracketend_")
                .Replace("'", "''''")
                //.Replace(",", "_comma_")
                ;
            return output;
        }

        public static string DeTokenizeCsvSql(string input)
        {
            string output = input
                .Replace("_bracketstart_", "[")
                .Replace("_bracketend_", "]")
                //.Replace("'''", "'")
                .Replace("_comma_", ",")
                ;
            return output;
        }

        public static string EscapeQuotes(string input, EscapeQuoteOptions options = EscapeQuoteOptions.DoubleQuote | EscapeQuoteOptions.UseBackslash)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string search = (options & EscapeQuoteOptions.SingleQuote) == EscapeQuoteOptions.SingleQuote ? "'" : "\"";
            string replace = string.Format("{0}{1}", (options & EscapeQuoteOptions.UseBackslash) == EscapeQuoteOptions.UseBackslash ? "\\" : search, search);
            string output = input.Replace(search, replace);
            return output;
        }

        [Flags]
        public enum EscapeQuoteOptions
        {
            SingleQuote = 1,
            DoubleQuote = 2,
            UseBackslash = 4,
            UseDoubleCharacter = 8,
        }

        public static IDictionary<string, string> ToDictionary(string input, string pairDelimiter = "&", string valueDelimiter = "=")
        {
            string[] pairs = input.Split(pairDelimiter);
            Dictionary<string, string> ret = new Dictionary<string, string>(pairs.Length);
            foreach (string pair in pairs)
            {
                string[] values = pair.Split(valueDelimiter);
                ret.Add(values[0], values[1]);
            }
            return ret;
        }

		public static string StripWindowsDomainFromUsername(string userName)
		{
			if (!string.IsNullOrEmpty(userName) && userName.Contains("\\"))
			{
				string[] arr = userName.Split('\\');
				userName = arr[arr.Length - 1];
			}
			return userName;
		}

		public static string AddWindowsDomainToUsername(string userName)
		{
			if (!userName.StartsWith("PNMAC"))
				userName = $"PNMAC\\{userName}";
			return userName;
		}

		public static string EncryptString(string plainText, string encryptionKey)
        {
            // Hash the key to ensure it is exactly 256 bits long, as required by AES-256
            using (SHA256Managed sha = new SHA256Managed())
            {
                byte[] encryptionKeyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
                return EncryptString(plainText, encryptionKeyBytes);
            }
        }

        public static string EncryptString(string plainText, byte[] encryptionKey)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(plainText);

            using (MemoryStream inputStream = new MemoryStream(buffer, false))
            using (MemoryStream outputStream = new MemoryStream())
            using (AesManaged aes = new AesManaged { Key = encryptionKey })
            {
                byte[] iv = aes.IV;  // first access generates a new IV
                outputStream.Write(iv, 0, iv.Length);
                outputStream.Flush();

                ICryptoTransform encryptor = aes.CreateEncryptor(encryptionKey, iv);
                using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                {
                    inputStream.CopyTo(cryptoStream);
                }

                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        public static string DecryptString(string cipherText, string encryptionKey)
        {
            // Hash the key to ensure it is exactly 256 bits long, as required by AES-256
            using (SHA256Managed sha = new SHA256Managed())
            {
                byte[] encryptionKeyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
                return DecryptString(cipherText, encryptionKeyBytes);
            }
        }

        public static string DecryptString(string cipherText, byte[] encryptionKey)
        {
            byte[] buffer = Convert.FromBase64String((string)cipherText);

            using (MemoryStream inputStream = new MemoryStream(buffer, false))
            using (MemoryStream outputStream = new MemoryStream())
            using (AesManaged aes = new AesManaged { Key = encryptionKey })
            {
                byte[] iv = new byte[16];
                int bytesRead = inputStream.Read(iv, 0, 16);
                if (bytesRead < 16)
                {
                    throw new CryptographicException("IV is missing or invalid.");
                }

                ICryptoTransform decryptor = aes.CreateDecryptor(encryptionKey, iv);
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(outputStream);
                }

                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }

}
