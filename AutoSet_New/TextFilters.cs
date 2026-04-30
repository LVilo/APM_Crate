using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoSet_New
{
    public static class TextFilters
    {
        public static void OnlyDigits(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.TextChanged -= OnlyDigits;
                int cursor = tb.SelectionStart;
                string filteredText = "";
                uint length = 0;
                foreach (char c in tb.Text)
                {
                    if (char.IsDigit(c)) filteredText += c;
                    length++;
                    if (length is 13) break;
                }

                if (tb.Text != filteredText)
                {
                    tb.Text = filteredText;
                    tb.SelectionStart = cursor;
                }
                tb.TextChanged += OnlyDigits;
            }
        }
        public static void OnlyLittersOrDigits(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.TextChanged -= OnlyLittersOrDigits;

                string filteredText = "";
                int cursor = tb.SelectionStart;
                uint length = 0;
                foreach (char c in tb.Text)
                {
                    if (char.IsLetterOrDigit(c)) filteredText += c;
                    length++;
                    if (length is 13) break;
                }

                if (tb.Text != filteredText)
                {
                    tb.Text = filteredText;
                    tb.SelectionStart = cursor;
                }
                tb.TextChanged += OnlyLittersOrDigits;
            }
        }
        public static void OnlyFloat(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.TextChanged -= OnlyFloat;
                string filteredText = "";
                int cursor = tb.SelectionStart;
                bool foundComma = false;
                int commaCount = 0;
                foreach (char c in tb.Text)
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
                }
                int commaIndex = filteredText.IndexOf(',');
                if (commaIndex != -1 && filteredText.Length - commaIndex > 4) // 3, потому что один символ для запятой
                {
                    filteredText = filteredText.Substring(0, commaIndex + 4);
                }
                if (tb.Text != filteredText)
                {
                    tb.Text = filteredText;
                    tb.SelectionStart = cursor;
                }

                tb.TextChanged += OnlyFloat;
            }
        }
    }
}
