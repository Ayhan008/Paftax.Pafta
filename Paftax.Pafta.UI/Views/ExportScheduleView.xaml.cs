using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Views
{
    /// <summary>
    /// Interaction logic for ExportScheduleView.xaml
    /// </summary>
    public partial class ExportScheduleView : Window
    {
        public ExportScheduleView(string theme = "Light")
        {
            InitializeComponent();
            UIThemeService.ApplyThemeToWindow(this, theme);

            Loaded += (s, e) =>
            {
                if (DataContext is BaseViewModel vm)
                {
                    vm.CancelRequest += () =>
                    {
                        DialogResult = true;
                        Close();
                    };
                }
            };
        }
    }
}
