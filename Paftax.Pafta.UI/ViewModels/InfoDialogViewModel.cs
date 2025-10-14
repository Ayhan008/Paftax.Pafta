using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.UI.ViewModels.Abstracts;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class InfoDialogViewModel : ViewModel
    {
        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private IconType iconType = IconType.Info;

        [ObservableProperty]
        private string glyph = string.Empty;

        [RelayCommand]
        private void Ok()
        {
            CloseAction?.Invoke();
        }

        partial void OnIconTypeChanged(IconType value)
        {
            Glyph = GetGlyphForIcon(value);
        }

        private static string GetGlyphForIcon(IconType type) => type switch
        {
            IconType.Info => "\uE946",      // Info icon
            IconType.Warning => "\uE7BA",   // Warning icon
            IconType.Error => "\uEA39",     // Error icon
            IconType.Success => "\uE73E",   // Checkmark icon
            _ => "\uE946"
        };
    }
}
