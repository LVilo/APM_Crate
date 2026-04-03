using APM_Crate.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Converters;
using System;
using static APM_Crate.Models.FilterTextModel;

namespace Controls;

public partial class UserTextBox : UserControl
{
    public UserTextBox()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<UserTextBox, string>(nameof(Header), "Header:");
    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<UserTextBox, string>(nameof(Text), "Пример текста",defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
    public string Text
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    public static readonly RoutedEvent<TextChangedEventArgs> TextChangedEvent =RoutedEvent.Register<UserTextBox, TextChangedEventArgs>("TextChanged",RoutingStrategies.Bubble);
    public event EventHandler<TextChangedEventArgs> TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }
    public static readonly StyledProperty<FilterType> TextFilterTypeProperty =AvaloniaProperty.Register<UserTextBox, FilterType>(nameof(TextFilterType), FilterType.None);

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
    public FilterType TextFilterType
    {
        get => GetValue(TextFilterTypeProperty);
        set => SetValue(TextFilterTypeProperty, value);
    }
    private void OnTextBoxTextChanged(object? sender, TextChangedEventArgs e)
    {
        // Применяем фильтр в зависимости от выбранного типа
        switch (TextFilterType)
        {
            case FilterType.OnlyFloat:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyFloat);
                break;
            case FilterType.OnlyDigits:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyDigits);
                break;
            case FilterType.OnlyLetterOrDigit:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyLetterOrDigit);
                break;
            case FilterType.OnlyFloatAndSignMinus:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyFloatAndSignMinus);
                break;
            case FilterType.OnlyHex:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyHex);
                break;
            case FilterType.OnlyUInt16:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyUInt16);
                break;
            case FilterType.OnlyStr:
                FiltersTexts.FilterTextCore(sender, FilterTextModel.OnlyStr);
                break;
        }

        // Вызываем событие для внешних подписчиков
        RaiseEvent(new TextChangedEventArgs(TextChangedEvent, this));
    }
}