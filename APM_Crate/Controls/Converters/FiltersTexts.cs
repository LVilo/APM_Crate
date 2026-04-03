using APM_Crate.Models;
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters
{
    public static class FiltersTexts
    {
        public static void FilterTextCore(object? sender, Func<string?, string> filter)
        {
            if (sender is TextBox textbox)
            {
                string newstring = filter(textbox.Text);
                //if (newstring is "") newstring = "0";
                if (textbox.Text != newstring)
                {
                    var caretIndex = textbox.CaretIndex;
                    textbox.Text = newstring;
                    textbox.CaretIndex = Math.Min(caretIndex, newstring.Length);
                }
            }
        }
        public static void FilterTextOnlyFloat(object? sender, TextChangedEventArgs e) =>
            FilterTextCore(sender, FilterTextModel.OnlyFloat);

        public static void FilterTextOnlyDigits(object? sender, TextChangedEventArgs e) =>
            FilterTextCore(sender, FilterTextModel.OnlyDigits);


        public static void FilterTextOnlyDigitsOrLitteral(object? sender, TextChangedEventArgs e) =>
            FilterTextCore(sender, FilterTextModel.OnlyLetterOrDigit);

        public static void FilterTextOnlyFloatAndSignMinus(object? sender, TextChangedEventArgs e) =>
            FilterTextCore(sender, FilterTextModel.OnlyFloatAndSignMinus);
        public static void FilterTextOnlyHex(object? sender, TextChangedEventArgs e) =>
            FilterTextCore(sender, FilterTextModel.OnlyHex);
    }
}
