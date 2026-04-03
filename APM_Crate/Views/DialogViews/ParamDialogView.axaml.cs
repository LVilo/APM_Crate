using APM_Crate.Models;
using APM_Crate.ViewModels.DialogViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace APM_Crate.Views.DialogViews;

public partial class ParamDialogView : Window
{
    public ParamDialogView()
    {
        InitializeComponent();
        
    }
    private void FilterTextOnlyFloat(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textbox)
        {
            string newstring = FilterTextModel.OnlyDigits(textbox.Text);
            if (newstring is null || newstring is "") newstring = "0";
            if (textbox.Text != newstring)
            {
                var caretIndex = textbox.CaretIndex;
                textbox.Text = newstring;
                textbox.CaretIndex = Math.Min(caretIndex, newstring.Length);
            }
        }
    }
    private void TapedEvent(object? sender,SizeChangedEventArgs e)
    {
        if(sender is Expander expander)
        {
            if(expander.IsExpanded && this.Height is 250) this.Height = 480;
            else this.Height = 250;
        }   
    }
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        { 
            case Key.Escape: Cancel.Command.Execute(Cancel.CommandParameter);
                 break;
            case Key.Enter: OK.Command.Execute(OK.CommandParameter);
                break;
        }
    }
    private void ClickOKWarning(object? sender, RoutedEventArgs e)
    {
    }

}