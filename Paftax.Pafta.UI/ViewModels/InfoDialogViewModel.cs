using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Enums;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class InfoDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private FluentIcon iconType = FluentIcon.Info;

        [ObservableProperty]
        private string glyph = string.Empty;

        public event Action? CloseAction;

        [RelayCommand]
        private void Ok()
        {
            CloseAction?.Invoke();
        }

        partial void OnIconTypeChanged(FluentIcon value)
        {
            Glyph = GetGlyphForIcon(value);
        }

        private static string GetGlyphForIcon(FluentIcon type) => type switch
        {
            FluentIcon.Info => "\uE946",      // Info icon
            FluentIcon.Warning => "\uE7BA",   // Warning icon
            FluentIcon.Error => "\uEA39",     // Error icon
            FluentIcon.Success => "\uE73E",   // Checkmark icon
            _ => "\uE946"
        };
    }
}
