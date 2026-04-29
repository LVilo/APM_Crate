using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AutoSet_New.Models
{
    public static class FilterTextModel
    {
        public enum FilterType
        {
            None,
            OnlyFloat,
            OnlyDigits,
            OnlyLetterOrDigit,
            OnlyFloatAndSignMinus,
            OnlyHex,
            OnlyUInt16,
            OnlyStr
        }
        //public static string OnTextBoxTextChanged(FilterType filter, string str)
        //{
        //    // Применяем фильтр в зависимости от выбранного типа
        //    return filter switch
        //    {
        //        FilterType.OnlyFloat => FilterTextModel.OnlyFloat(str),
        //        FilterType.OnlyDigits => FilterTextModel.OnlyDigits(str),
        //        FilterType.OnlyLetterOrDigit => FilterTextModel.OnlyLetterOrDigit(str),
        //        FilterType.OnlyFloatAndSignMinus => FilterTextModel.OnlyFloatAndSignMinus(str),
        //        FilterType.OnlyHex => FilterTextModel.OnlyHex(str),
        //        FilterType.OnlyStr => FilterTextModel.OnlyStr(str),
        //        FilterType.None => str,
        //        _ => "",
        //    };
        //}

        public static string ConvertUint16ToHex(ushort value) => value.ToString("X");
        public static ushort ConvertHexToUint16(string hex) => Convert.ToUInt16(hex, 16);
        public static string OnlyStr(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return str;
        }
        public static string OnlyUInt16(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            str = OnlyDigits(str);
            ushort result = ushort.MaxValue;
            ulong value = Convert.ToUInt64(str);
            result = result > value ? (ushort)value : result;
            return result.ToString();
        }
        public static string OnlyHex(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = OnlyLetterOrDigit(str).ToUpper();
            string result = "";
            ushort i = 0;
            foreach(char c in str)
            {
                if((c is 'A' || c is 'B' || c is 'C' || c is 'D' || c is 'E' || c is 'F') || char.IsDigit(c))
                result += c;
                i++;
                if (i == 4) break;
            }
            return result;
        }

        public static string OnlyDigits(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            var digitsOnly = new string(str.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length > 20)
            {
                digitsOnly = str[..20];
            }
            return digitsOnly;
        }
        public static string OnlyLetterOrDigit(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            var cleaned = new string(str.Where(char.IsLetterOrDigit).ToArray());
            if (cleaned.Length > 20)
            {
                cleaned = str[..20];
            }
            return cleaned;
        }
        public static string OnlyFloat(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Replace('.', ',');
            string filteredText = "";
            bool foundComma = false;
            int commaCount = 0;
            if (str.StartsWith(',') is true) { str = str[1..]; }
            foreach (char c in str)
            {
                if (char.IsDigit(c) || (c == ',' && !foundComma))
                {
                    filteredText += c;
                    if (c == ',')
                    {
                        foundComma = true;
                        commaCount++;
                    }
                }
                if (filteredText.Length == 9) break;
            }
            int commaIndex = filteredText.IndexOf(',');
            if (commaIndex != -1 && filteredText.Length - commaIndex > 4) // 3, потому что один символ для запятой
            {
                filteredText = filteredText[..(commaIndex + 4)];
            }
            return filteredText;
        }
        public static string OnlyFloatAndSignMinus(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            str = str.Replace('.', ',');
            string filteredText = "";
            bool foundComma = false;
            bool foundMinus = false;
            int commaCount = 0;

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == '-' && i == 0 && !foundMinus)
                {
                    filteredText += c;
                    foundMinus = true;
                    continue;
                }

                if (c == ',' && i == (foundMinus ? 1 : 0))
                {
                    continue;
                }

                if (char.IsDigit(c))
                {
                    filteredText += c;
                }
                else if (c == ',' && !foundComma)
                {
                    filteredText += c;
                    foundComma = true;
                    commaCount++;
                }

                int maxLength = foundMinus ? 10 : 9;
                if (filteredText.Length >= maxLength)
                    break;
            }

            int commaIndex = filteredText.IndexOf(',');
            if (commaIndex != -1 && filteredText.Length - commaIndex > 7)
            {
                filteredText = filteredText[..(commaIndex + 7)];
            }

            if (filteredText == "-" || filteredText == "-,")
            {
                return "";
            }

            return filteredText;
        }
    }
}
