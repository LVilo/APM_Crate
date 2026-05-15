using APM_Crate.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace APM_Crate.Views.DialogViews;

public partial class ParametersView : Window
{
    public ParametersView()
    {
        InitializeComponent();
    }

    private void FilterTextOnlyUint16(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textbox)
        {
            string newstring = FilterTextModel.OnlyUInt16(textbox.Text);
            if (textbox.Text != newstring)
            {
                var caretIndex = textbox.CaretIndex;
                textbox.Text = newstring;
                textbox.CaretIndex = Math.Min(caretIndex, newstring.Length);
            }
        }
    }
}