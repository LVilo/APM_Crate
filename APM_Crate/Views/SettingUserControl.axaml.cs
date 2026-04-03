using APM_Crate.Models;
using APM_Crate.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace APM_Crate.Views;

public partial class SettingUserControl : UserControl
{
    public SettingUserControl()
    {
        InitializeComponent();
    }
    private void FilterTextOnlyDigits(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textbox)
        {
            string newstring = FilterTextModel.OnlyDigits(textbox.Text);
            if (textbox.Text != newstring)
            {
                var caretIndex = textbox.CaretIndex;
                textbox.Text = newstring;
                textbox.CaretIndex = Math.Min(caretIndex, newstring.Length);
            }
        }
    }

    private void FilterTextOnlyDigitsOrLitteral(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textbox)
        {
            string newstring = FilterTextModel.OnlyLetterOrDigit(textbox.Text);
            if (textbox.Text != newstring)
            {
                var caretIndex = textbox.CaretIndex;
                textbox.Text = newstring;
                textbox.CaretIndex = Math.Min(caretIndex, newstring.Length);
            }
        }
    }
}