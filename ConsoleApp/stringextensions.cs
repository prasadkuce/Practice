using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Core.Strings
{
    //public class TextUtility
    public static class StringExtensions
    {


        public static string Between(this string source, string first, string second)
        {
            return source.Split(new[] { first }, StringSplitOptions.None).Last().Split(new[] { second }, StringSplitOptions.None).First();
        }


        public static string ToXML(this string original)
        {
            if (!String.IsNullOrEmpty(original))
            {
                return System.Xml.Linq.XElement.Parse(original).ToString();
            }
            return original;
        }


        public static string ToPhoneFormat(this string original)
        {
            if (!String.IsNullOrEmpty(original) && original.Length == 10)
            {
                //return original.ToString("(###) ###-####");
                return string.Format("({0}) {1}-{2}", original.Substring(0, 3), original.Substring(3, 3), original.Substring(6, 4));
            }
            return original;
        }

        public static Int32? ToNullableInt32(this string original)
        {
            int i;
            if (Int32.TryParse(original, out i))
                return i;
            return null;
        }

        public static Int64? ToNullableInt64(this string original)
        {
            Int64 i;
            if (Int64.TryParse(original, out i))
                return i;
            return null;
        }


        //private StringExtensions() { }

        /// <summary>
        /// Cleans whitespace inside a string.
        /// </summary>
        /// <param name="original">A string to be cleaned.</param>
        /// <returns>A cleaned string.</returns>
        public static string CleanWhitespace(this string original)
        {
            if (original.Length > 0)
            {
                return Regex.Replace(original, @"[\s]", String.Empty);
            }
            return String.Empty;
        }

        public static string TabbledLine(string input)
        {
            if (input.Length > 0)
            {
                string tabs = "\t\t";

                return tabs + input + Environment.NewLine;
            }
            return String.Empty;
        }

        public static string TabbledLine(string input, int numTabs)
        {
            if (input.Length > 0)
            {
                string tabs = string.Empty;

                for (int i = 0; i < numTabs; i++)
                {
                    tabs += "\t";
                }

                return string.Format("{0}{1}{2}", tabs, input, Environment.NewLine);
            }
            return string.Empty;
        }

        public static string FilterChars(this string input, string validCharacters)
        {
            // "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz,'"
            // By Default allow Upper/lower case alphas, spaces and commas
            int intInputCharCounter;
            string filtered = "";
            char[] aryValidCharacters = validCharacters.ToCharArray();
            char[] aryInputCharacters = input.ToCharArray();
            for (intInputCharCounter = 0; (intInputCharCounter <= aryInputCharacters.GetUpperBound(0)); intInputCharCounter++)
            {
                if ((aryInputCharacters[intInputCharCounter].ToString().IndexOfAny(aryValidCharacters) != -1))
                {
                    filtered += aryInputCharacters[intInputCharCounter];
                }
            }
            return filtered;
        }


        //public static string ToPlural(this string input)
        //{
        //    return !String.IsNullOrEmpty (input)  ? PS.Core.Strings.Pluralizer.ToPlural(input): string.Empty ;
        //}

        //public static string ToPlural(string input)
        //{
        //    return PS.Core.Strings.Pluralizer.ToPlural(input);
        //}


        /// <summary>
        /// Formats a string in Pascal case (the first letter is in upper case).
        /// </summary>
        /// <param name="original">A string to be formatted.</param>
        /// <returns>A string in Pascal case.</returns>
        public static string FixVariableEndingWithId(string original)
        {
            if (original.Length > 0)
            {
                if (original.ToLower().EndsWith("id") && !original.ToLower().EndsWith("guid"))
                    return original.Substring(0, original.Length - 2) + "Id";
                return original;
            }
            return String.Empty;
        }


        public static bool IsForeignKeyField(string table, string columnName)
        {
            if (columnName.ToUpper().EndsWith("ID") && columnName.ToLower().Contains(table.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool IsDigits(this string strValue, bool blnMustHaveOneDigit = true)
        {
            Regex regex = new Regex(string.Format(@"^\d{0}$", blnMustHaveOneDigit ? "+" : "*"));
            return regex.IsMatch(strValue);
        }

        public static bool IsEmailAddress(this string strValue)
        {
            return true;
        }







        public static string ConvertToCsharpDataType(string sqlDataType, bool isNullable = false)
        {
            return string.Format("Convert.To{0}", SqlToCsharp(sqlDataType, isNullable).ToPascal().Replace("?", "").Replace("Convert.ToGuid", "new Guid"));
        }



        public static string SqlToCsharp(string sqlDataType, bool isNullable = false) //ColumnEntity column)
        {
            //string sqlDataType = column.Type.ToString().ToLower();
            string parameter;


            switch (sqlDataType.ToLower())
            {
                case "varbinary(max)":
                case "varbinary":
                case "binary":
                    parameter = "byte[]";
                    break;
                case "bit":
                    parameter = "Boolean";
                    break;
                case "bigint":
                    parameter = "Int64";
                    break;
                case "char":

                    parameter = "string";
                    break;
                case "smalldatetime":
                    goto case "datetime";
                case "datetime":
                case "date":
                    //parameter = "if (" + columnName + " <= Convert.ToDateTime(\"1/1/1900\") || " + columnName + "> DateTime.Now.AddYears(1) ) Errors.Add(string.Format(Messages.DateRangeInclusive, \"" + columnName + "\",\"1/1/1900\", DateTime.Now.AddYears(1)));";
                    parameter = "DateTime";
                    break;
                case "decimal":
                    parameter = "decimal";
                    break;
                case "float":
                    parameter = "double";
                    break;
                case "image":
                    parameter = "byte[]";
                    break;
                case "int":
                    parameter = "Int32";
                    //if (columnName.ToString().EndsWith("ID"))
                    //{
                    //    parameter = "if (0 == " + columnName + " ) Errors.Add (string.Format(Messages.ForeignKeyRequired, \"" + columnName + "\"));";
                    //}
                    break;
                case "money":
                    parameter = "decimal";
                    break;
                case "nchar":
                    parameter = "string";
                    break;
                case "ntext":
                    parameter = "string";
                    break;
                case "nvarchar":
                    parameter = "string";
                    break;
                case "numeric":
                    parameter = "decimal";
                    break;
                case "real":
                    parameter = "float";
                    break;
                case "smallint":
                    parameter = "short";
                    break;
                case "smallmoney":
                    parameter = "decimal";
                    break;
                case "sql_variant":
                    parameter = "object";
                    break;
                case "sysname":
                    parameter = "string";
                    break;
                case "text":
                    parameter = "string";
                    break;
                case "timestamp":
                    parameter = "DateTime";
                    break;
                case "tinyint":
                    parameter = "byte";
                    break;
                case "varchar":
                case "xml":
                    parameter = "string";
                    break;
                case "uniqueidentifier":
                    parameter = "Guid";
                    break;
                /////////////////////////////////

                default: // String data type
                    parameter = sqlDataType.ToPascal();

                    break;
            }
            if (isNullable && parameter != "string" && parameter != "byte[]") parameter += "?";


            return parameter;
        }

        /// <summary>
        /// Formats a string in Pascal case (the first letter is in upper case).
        /// </summary>
        /// <param name="original">A string to be formatted.</param>
        /// <returns>A string in Pascal case.</returns>
        public static string ToPascal(this string original)
        {
            if (!string.IsNullOrEmpty(original) && original.Length > 0)
            {
                if (original.Contains("_"))
                    return FixVariableEndingWithId(ConvertUnderScoreSeparatorsToPascal(original));
                return FixVariableEndingWithId(original.Substring(0, 1).ToUpper() + original.Substring(1).ToLower());
            }
            return String.Empty;
        }


        public static string ColumnNameToRoot(this string original)
        {
            if (original.Length > 0)
            {
                return original.Replace("ID", "").Replace("id", "").Replace("Id", "");
            }
            return String.Empty;
        }


        public static string ConvertUnderScoreSeparatorsToPascal(this string original)
        {
            string output = string.Empty;


            if (original.Length > 0)
            {
                string[] words = original.Split(Convert.ToChar("_"));
                foreach (string word in words)
                {
                    output += (word).ToPascal();
                }
            }

            return output;
        }

        /// <summary>
        /// Formats a string in Camel case (the first letter is in lower case).
        /// </summary>
        /// <param name="original">A string to be formatted.</param>
        /// <returns>A string in Camel case.</returns>
        public static string ToCamel(this string original)
        {
            if (!String.IsNullOrEmpty(original) && original.Length > 0)
            {
                if (original.Contains("_"))
                    original = ConvertUnderScoreSeparatorsToPascal(original);

                return FixVariableEndingWithId(original.Substring(0, 1).ToLower() + original.Substring(1));
            }
            return String.Empty;
        }

        public static string FormatSplitByCase(this string pText)
        {
            string pattern = @"(\p{Ll})(\p{Lu})|m_+";
            return Regex.Replace(pText, pattern, "$1 $2");
        }


        public static string TrimCSV(this string input)
        {
            char s = Convert.ToChar(",");
            return input.Trim().TrimStart(s).TrimEnd(s);
        }

        public static string ToSentenceCase(this string input)
        {
            return StringHelper.ToSentenceCase(input);
        }


        public static string ToTitleCase(this string input)
        {
            return StringHelper.ToTitleCase(input);
        }

        public static string ToSplitByCase(this string input)
        {
            return StringHelper.SplitByCase(input);
        }

        public static string ToShortName(this string input)
        {
            return input.Replace("Transaction", "Trans").Replace("Corporate", "Corp.").Replace("Description", "Desc");
        }

        public static string SeparateDirectionFromHouseNumber(this string input)
        {
            return StringHelper.SeparateDirectionFromHouseNumber(input);
        }

        public static string ToYesNo(this string input)
        {
            return StringHelper.GetYesNo(input);
        }

		public static bool FromYesNo(this string input)
		{
			return StringHelper.FromYesNo(input);
		}

        //    public static bool RegExMatch(object target, RuleArgs e) {
        //  RegExRuleArgs args = (RegExRuleArgs) e;
        //  Regex rx = args.RegEx;
        //  string value = (string) ReflectionFns.GetPropertyValue(target, args.PropertyName);
        //  // Contra , rule does NOT fail if there is no entry;
        //  // That test is the job of a "PropertyRequired" rule.
        //  if ( value == null || value.Trim() == String.Empty ) return true;
        //  if (!rx.IsMatch(value)) {
        //    args.Description = 
        //      String.Format(args.MessageTemplate, args.DisplayPropertyName);
        //    return false;
        //  } else
        //    return true;
        //}

        public static string Times(this string str, int times)
        {
            string result;
            if (string.IsNullOrEmpty(str))
            {
                result = string.Empty;
            }
            else
            {
                if (times <= 1)
                {
                    result = str;
                }
                else
                {
                    string text = string.Empty;
                    for (int i = 0; i < times; i++)
                    {
                        text += str;
                    }
                    result = text;
                }
            }
            return result;
        }
        public static string IncreaseTo(this string str, int maxLength, bool truncate)
        {
            string result;
            if (string.IsNullOrEmpty(str))
            {
                result = str;
            }
            else
            {
                if (str.Length == maxLength)
                {
                    result = str;
                }
                else
                {
                    if (str.Length > maxLength && truncate)
                    {
                        result = Truncate(str, maxLength);
                    }
                    else
                    {
                        string text = str;
                        while (str.Length < maxLength)
                        {
                            if (str.Length + text.Length < maxLength)
                            {
                                str += text;
                            }
                            else
                            {
                                str += str.Substring(0, maxLength - str.Length);
                            }
                        }
                        result = str;
                    }
                }
            }
            return result;
        }
        public static string IncreaseRandomly(this string str, int minLength, int maxLength, bool truncate)
        {
            Random random = new Random(minLength);
            int maxLength2 = random.Next(minLength, maxLength);
            return IncreaseTo(str, maxLength2, truncate);
        }
        public static string Truncate(this string txt, int maxChars)
        {
            string result;
            if (string.IsNullOrEmpty(txt))
            {
                result = txt;
            }
            else
            {
                if (txt.Length <= maxChars)
                {
                    result = txt;
                }
                else
                {
                    result = txt.Substring(0, maxChars);
                }
            }
            return result;
        }
        public static string TruncateWithText(this string txt, int maxChars, string suffix)
        {
            string result;
            if (string.IsNullOrEmpty(txt))
            {
                result = txt;
            }
            else
            {
                if (txt.Length <= maxChars)
                {
                    result = txt;
                }
                else
                {
                    string str0 = txt.Substring(0, maxChars);
                    result = str0 + suffix;
                }
            }
            return result;
        }
        public static byte[] ToBytesAscii(this string txt)
        {
            return ToBytesEncoding(txt, new ASCIIEncoding());
        }
        public static byte[] ToBytes(this string txt)
        {
            return ToBytesEncoding(txt, Encoding.Default);
        }
        public static byte[] ToBytesEncoding(this string txt, Encoding encoding)
        {
            byte[] result;
            if (string.IsNullOrEmpty(txt))
            {
                result = new byte[0];
            }
            else
            {
                result = encoding.GetBytes(txt);
            }
            return result;
        }
        public static string StringFromBytesASCII(this byte[] bytes)
        {
            return StringFromBytesEncoding(bytes, new ASCIIEncoding());
        }
        public static string StringFromBytes(this byte[] bytes)
        {
            return StringFromBytesEncoding(bytes, Encoding.Default);
        }

        public static string StringFromBytesEncoding(this byte[] bytes, Encoding encoding)
        {
            // TODO: revisit this
            // Question: An empty byute array semantically represents an empty string. A null array would represents a null string. we're deviating from the conventions. is this intentional and if so should that be reflected in the method name?

            string result;

            if (bytes.Length == 0)
            {
                result = null;
            }
            else
            {
                result = encoding.GetString(bytes);
            }
            return result;
        }

        #region String to Enum


        public static TEnum ParseStringToEnum<TEnum>(this string dataToMatch, bool ignorecase = default(bool))
                where TEnum : struct
        {
            return dataToMatch.IsItemInEnum<TEnum>()() ? default(TEnum) : (TEnum)Enum.Parse(typeof(TEnum), dataToMatch, default(bool));
        }


        public static Func<bool> IsItemInEnum<TEnum>(this string dataToCheck)
            where TEnum : struct
        {
            return () => { return string.IsNullOrEmpty(dataToCheck) || !Enum.IsDefined(typeof(TEnum), dataToCheck); };
        }

        #endregion



        public static object ToBoolObject(this string txt)
        {
            return ToBool(txt);
        }
        public static bool ToBool(this string txt)
        {
            bool result;
            if (string.IsNullOrEmpty(txt))
            {
                result = false;
            }
            else
            {
                string a = txt.Trim().ToLower();
                result = (a == "yes" || a == "true" || a == "1");
            }
            return result;
        }
        public static object ToIntObject(this string txt)
        {
            return ToInt(txt);
        }
        public static int ToInt(this string txt)
        {
            return ToNumber<int>(txt, (string s) => Convert.ToInt32(Convert.ToDouble(s)), 0);
        }
        public static object ToLongObject(this string txt)
        {
            return ToLong(txt);
        }
        public static long ToLong(this string txt)
        {
            return ToNumber<long>(txt, (string s) => Convert.ToInt64(s), 0L);
        }
        public static object ToDoubleObject(this string txt)
        {
            return ToDouble(txt);
        }
        public static double ToDouble(this string txt)
        {
            return ToNumber<double>(txt, (string s) => Convert.ToDouble(s), 0.0);
        }
        public static object ToFloatObject(this string txt)
        {
            return ToFloat(txt);
        }
        public static float ToFloat(this string txt)
        {
            return ToNumber<float>(txt, (string s) => Convert.ToSingle(s), 0f);
        }
        public static T ToNumber<T>(string txt, Func<string, T> callback, T defaultValue)
        {
            T result;
            if (string.IsNullOrEmpty(txt))
            {
                result = defaultValue;
            }
            else
            {
                string text = txt.Trim().ToLower();
                if (text.StartsWith("$") || text.StartsWith(Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol))
                {
                    text = text.Substring(1);
                }
                result = callback(text);
            }
            return result;
        }
        //public static object ToTimeObject(this string txt)
        //{
        //    return StringExtensions.ToTime(txt);
        //}
        //public static TimeSpan ToTime(this string txt)
        //{
        //    TimeSpan result;
        //    if (string.IsNullOrEmpty(txt))
        //    {
        //        result = TimeSpan.MinValue;
        //    }
        //    else
        //    {
        //        string strTime = txt.Trim().ToLower();
        //        result = TimeHelper.Parse(strTime).Item;
        //    }
        //    return result;
        //}
        //public static object ToDateTimeObject(this string txt)
        //{
        //    return StringExtensions.ToDateTime(txt);
        //}
        //public static DateTime ToDateTime(this string txt)
        //{
        //    DateTime result;
        //    if (string.IsNullOrEmpty(txt))
        //    {
        //        result = DateTime.MinValue;
        //    }
        //    else
        //    {
        //        string text = txt.Trim().ToLower();
        //        if (text.StartsWith("$"))
        //        {
        //            if (text == "${today}")
        //            {
        //                result = DateTime.Today;
        //            }
        //            else
        //            {
        //                if (text == "${yesterday}")
        //                {
        //                    DateTime today = DateTime.Today;
        //                    result = today.AddDays(-1.0);
        //                }
        //                else
        //                {
        //                    if (text == "${tommorrow}")
        //                    {
        //                        DateTime today = DateTime.Today;
        //                        result = today.AddDays(1.0);
        //                    }
        //                    else
        //                    {
        //                        if (text == "${t}")
        //                        {
        //                            result = DateTime.Today;
        //                        }
        //                        else
        //                        {
        //                            if (text == "${t-1}")
        //                            {
        //                                DateTime today = DateTime.Today;
        //                                result = today.AddDays(-1.0);
        //                            }
        //                            else
        //                            {
        //                                if (text == "${t+1}")
        //                                {
        //                                    DateTime today = DateTime.Today;
        //                                    result = today.AddDays(1.0);
        //                                }
        //                                else
        //                                {
        //                                    if (text == "${today+1}")
        //                                    {
        //                                        DateTime today = DateTime.Today;
        //                                        result = today.AddDays(1.0);
        //                                    }
        //                                    else
        //                                    {
        //                                        if (text == "${today-1}")
        //                                        {
        //                                            DateTime today = DateTime.Today;
        //                                            result = today.AddDays(-1.0);
        //                                        }
        //                                        else
        //                                        {
        //                                            string dateStr = text.Substring(2, text.Length - 1 - 2);
        //                                            DateTime dateTime = DateParser.ParseTPlusMinusX(dateStr);
        //                                            result = dateTime;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            DateTime dateTime2 = DateTime.Parse(text);
        //            result = dateTime2;
        //        }
        //    }
        //    return result;
        //}


        private static readonly Regex s_hexRegex = new Regex(@"^[0-9a-f]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static bool IsHex(this string txt)
        {
            return !string.IsNullOrWhiteSpace(txt) && s_hexRegex.IsMatch(txt);
        }

        private static readonly Regex s_binaryRegex = new Regex(@"^[01]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static bool IsBinary(this string txt)
        {
            return !string.IsNullOrWhiteSpace(txt) && s_binaryRegex.IsMatch(txt);
        }

        public static bool IsValidPhoneNumber(this string input)
        {
            return RegExp.HasPattern(RegExp.US_TELEPHONE, input);
        }

        public static bool IsValidNorthAmericanPhoneNumber(this string input)
        {
            return RegExp.HasPattern(RegExp.NORTH_AMERICAN_TELEPHONE, input);
        }

        public static bool IsValidSocialSecurityNumber(this string input)
        {
            return RegExp.HasPattern(RegExp.SOCIAL_SECURITY, input);
        }

        public static bool IsValidZipCode(this string input)
        {
            return RegExp.HasPattern(RegExp.US_ZIPCODE, input);
        }


        public static bool IsValidEmailAddress(this string input)
        {
            return RegExp.HasPattern(RegExp.EMAIL, input);
        }


        public static string DecimalToHex(this string txt)
        {
            return IntegerExtensions.ToHex(Convert.ToInt32(txt));
        }
        public static string DecimalToBinary(this string txt)
        {
            return IntegerExtensions.ToBinary(Convert.ToInt32(txt));
        }
        public static string HexToDecimal(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 16));
        }
        public static string HexToBinary(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 16), 2);
        }
        public static byte[] HexToByteArray(this string txt)
        {
            byte[] array = new byte[txt.Length / 2];
            for (int i = 0; i < txt.Length; i += 2)
            {
                array[i / 2] = Convert.ToByte(txt.Substring(i, 2), 16);
            }
            return array;
        }
        public static string ByteArrayToHex(this byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "");
        }
        public static string BinaryToHex(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 2), 16);
        }
        public static string BinaryToDecimal(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 2));
        }
        public static string ReplaceMSWordCharacters(this string txt)
        {
            // http://stackoverflow.com/questions/334850/c-sharp-how-to-replace-microsofts-smart-quotes-with-straight-quotation-marks
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.IndexOf('\u2013') > -1) txt = txt.Replace('\u2013', '-');
            if (txt.IndexOf('\u2014') > -1) txt = txt.Replace('\u2014', '-');
            if (txt.IndexOf('\u2015') > -1) txt = txt.Replace('\u2015', '-');
            if (txt.IndexOf('\u2017') > -1) txt = txt.Replace('\u2017', '_');
            if (txt.IndexOf('\u2018') > -1) txt = txt.Replace('\u2018', '\'');
            if (txt.IndexOf('\u2019') > -1) txt = txt.Replace('\u2019', '\'');
            if (txt.IndexOf('\u201a') > -1) txt = txt.Replace('\u201a', ',');
            if (txt.IndexOf('\u201b') > -1) txt = txt.Replace('\u201b', '\'');
            if (txt.IndexOf('\u201c') > -1) txt = txt.Replace('\u201c', '\"');
            if (txt.IndexOf('\u201d') > -1) txt = txt.Replace('\u201d', '\"');
            if (txt.IndexOf('\u201e') > -1) txt = txt.Replace('\u201e', '\"');
            if (txt.IndexOf('\u2026') > -1) txt = txt.Replace("\u2026", "...");
            if (txt.IndexOf('\u2032') > -1) txt = txt.Replace('\u2032', '\'');
            if (txt.IndexOf('\u2033') > -1) txt = txt.Replace('\u2033', '\"');

            return txt;
        }

        [Pure]
        public static IEnumerable<char> GetExtendedCharacters(this string str, int minExtendedCharByteCount = 2)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (minExtendedCharByteCount < 2) throw new ArgumentOutOfRangeException(@"Expected a value greater than 1. Extended characters are at least 2 bytes per character. value: " + minExtendedCharByteCount, "minExtendedCharByteCount");

            return str.Select(x => new { @char = x, length = Encoding.UTF8.GetBytes(new string(x, 1)).Length })
            .Where(x => x.length >= minExtendedCharByteCount)
            .Select(x => x.@char);
        }

        [Pure]
        public static bool ContainsExtendedCharacters(this string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            return str.GetExtendedCharacters().Any();
        }


        public static string ReplaceChars(this string txt, string originalChars, string newChars)
        {
            string text = "";
            for (int i = 0; i < txt.Length; i++)
            {
                int num = originalChars.IndexOf(txt.Substring(i, 1));
                if (-1 != num)
                {
                    text += newChars.Substring(num, 1);
                }
                else
                {
                    text += txt.Substring(i, 1);
                }
            }
            return text;
        }
        public static List<string> PreFixWith(this List<string> items, string prefix)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = prefix + items[i];
            }
            return items;
        }
        public static bool IsNotApplicableValue(this string val, bool useNullOrEmptyStringAsNotApplicable)
        {
            bool flag = string.IsNullOrEmpty(val);
            bool result;
            if (flag && useNullOrEmptyStringAsNotApplicable)
            {
                result = true;
            }
            else
            {
                if (flag && !useNullOrEmptyStringAsNotApplicable)
                {
                    result = false;
                }
                else
                {
                    val = val.Trim().ToLower();
                    result = (val == "na" || val == "n.a." || val == "n/a" || val == "n\\a" || val == "n.a" || val == "not applicable");
                }
            }
            return result;
        }
        public static int Levenshtein(this string source, string comparison)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Can't parse null string");
            }
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison", "Can't parse null string");
            }
            char[] array = source.ToCharArray();
            char[] array2 = comparison.ToCharArray();
            int length = source.Length;
            int length2 = comparison.Length;
            int[,] array3 = new int[length + 1, length2 + 1];
            int result;
            if (length == 0)
            {
                result = length2;
            }
            else
            {
                if (length2 == 0)
                {
                    result = length;
                }
                else
                {
                    int num = 0;
                    while (num <= length)
                    {
                        int[,] arg_A5_0 = array3;
                        int arg_A5_1 = num;
                        int arg_A5_2 = 0;
                        int expr_A0 = num;
                        num = expr_A0 + 1;
                        arg_A5_0[arg_A5_1, arg_A5_2] = expr_A0;
                    }
                    int num2 = 0;
                    while (num2 <= length2)
                    {
                        int[,] arg_CB_0 = array3;
                        int arg_CB_1 = 0;
                        int arg_CB_2 = num2;
                        int expr_C6 = num2;
                        num2 = expr_C6 + 1;
                        arg_CB_0[arg_CB_1, arg_CB_2] = expr_C6;
                    }
                    for (num = 1; num <= length; num++)
                    {
                        for (num2 = 1; num2 <= length2; num2++)
                        {
                            int num3 = array2[num2 - 1].Equals(array[num - 1]) ? 0 : 1;
                            array3[num, num2] = Math.Min(Math.Min(array3[num - 1, num2] + 1, array3[num, num2 - 1] + 1), array3[num - 1, num2 - 1] + num3);
                        }
                    }
                    result = array3[length, length2];
                }
            }
            return result;
        }
        public static string SimplifiedSoundex(this string source)
        {
            return SimplifiedSoundex(source, 4);
        }
        public static string SimplifiedSoundex(this string source, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Length < 3)
            {
                throw new ArgumentException("Source string must be at least two characters", "source");
            }
            char[] array = source.ToUpper().ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            short num = -1;
            char[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                char c = array2[i];
                short num2 = 0;
                switch (c)
                {
                    case 'A':
                    case 'E':
                    case 'H':
                    case 'I':
                    case 'O':
                    case 'U':
                    case 'W':
                    case 'Y':
                        {
                            num2 = 0;
                            break;
                        }
                    case 'B':
                    case 'F':
                    case 'P':
                    case 'V':
                        {
                            num2 = 1;
                            break;
                        }
                    case 'C':
                    case 'G':
                    case 'J':
                    case 'K':
                    case 'Q':
                    case 'S':
                    case 'X':
                    case 'Z':
                        {
                            num2 = 2;
                            break;
                        }
                    case 'D':
                    case 'T':
                        {
                            num2 = 3;
                            break;
                        }
                    case 'L':
                        {
                            num2 = 4;
                            break;
                        }
                    case 'M':
                    case 'N':
                        {
                            num2 = 5;
                            break;
                        }
                    case 'R':
                        {
                            num2 = 6;
                            break;
                        }
                    default:
                        {
                            throw new ApplicationException("Invalid state in switch statement");
                        }
                }
                if (num2 != num)
                {
                    stringBuilder.Append(num2);
                }
                num = num2;
            }
            stringBuilder.Remove(0, 1).Insert(0, Enumerable.First<char>(array));
            stringBuilder.Replace("0", "");
            while (stringBuilder.Length < length)
            {
                stringBuilder.Append('0');
            }
            return stringBuilder.ToString().Substring(0, length);
        }

        public static string Join(string separator, string conjunction, IEnumerable<string> values)
        {
            var lastItem = values.Last();
            string retVal;

            switch (values.Count())
            {
                case 0:
                case 1:
                    retVal = String.Join(separator, values);
                    break;
                case 2:
                    retVal = String.Join(String.Format(" {0} ", conjunction), values);
                    break;
                default:
                    retVal = String.Join(separator, values).Replace(lastItem, String.Format("{0} {1}", conjunction, lastItem));
                    break;
            }

            return retVal;
        }

        public static string EscapeQuotes(this string input, StringHelper.EscapeQuoteOptions options = StringHelper.EscapeQuoteOptions.DoubleQuote | StringHelper.EscapeQuoteOptions.UseBackslash)
        {
            return StringHelper.EscapeQuotes(input, options);
        }


        public static string[] Split(this string str, string seperatorPattern = @"\s*,\s*", RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant)
        {
            if (string.IsNullOrWhiteSpace(str))
                return new string[] { };

            return Regex.Split(str, seperatorPattern)
                .Select(x => x.Trim())
                .Where(x => x != string.Empty)
                .ToArray();
        }

        public static IDictionary<string, string> ToDictionary(this string input, string pairDelimiter = "&", string valueDelimiter = "=")
        {
            return StringHelper.ToDictionary(input, pairDelimiter, valueDelimiter);
        }

		// https://stackoverflow.com/questions/14655023/split-a-string-that-has-white-spaces-unless-they-are-enclosed-within-quotes
		/// <summary>
		/// Splits a string, making allowances for string groups
		/// </summary>
		/// <param name="line"></param>
		/// <param name="delimiter"></param>
		/// <param name="textQualifier"></param>
		/// <returns>For example, "oneword \"two words\"" is split into "oneword" and "two words"</returns>
		public static IEnumerable<String> SmartSplit(String line, Char delimiter, Char textQualifier)
		{

			if (line == null)
				yield break;

			else
			{
				Char prevChar = '\0';
				Char nextChar = '\0';
				Char currentChar = '\0';

				Boolean inString = false;

				StringBuilder token = new StringBuilder();

				for (int i = 0; i < line.Length; i++)
				{
					currentChar = line[i];

					if (i > 0)
						prevChar = line[i - 1];
					else
						prevChar = '\0';

					if (i + 1 < line.Length)
						nextChar = line[i + 1];
					else
						nextChar = '\0';

					if (currentChar == textQualifier && (prevChar == '\0' || prevChar == delimiter) && !inString)
					{
						inString = true;
						continue;
					}

					if (currentChar == textQualifier && (nextChar == '\0' || nextChar == delimiter) && inString)
					{
						inString = false;
						continue;
					}

					if (currentChar == delimiter && !inString)
					{
						yield return token.ToString();
						token = token.Remove(0, token.Length);
						continue;
					}

					token = token.Append(currentChar);

				}

				yield return token.ToString();

			}
		}

		public static bool CaseInsensitiveContains(this string s, string value, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
		{
			return s.IndexOf(value, comparisonType) >= 0;
		}
	}
}
