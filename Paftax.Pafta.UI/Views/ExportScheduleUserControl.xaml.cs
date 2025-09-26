using Paftax.Pafta.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Views
{
    /// <summary>
    /// Interaction logic for ExportScheduleView.xaml
    /// </summary>
    public partial class ExportScheduleUserControl : UserControl
    {
        public ExportScheduleUserControl()
        {
            InitializeComponent();

            DataContextChanged += (s, e) =>
            {
                if (e.NewValue is ExportScheduleViewModel vm)
                {
                    vm.CloseRequest += OnCloseRequest;
                }
                if (e.OldValue is ExportScheduleViewModel oldVm)
                {
                    oldVm.CloseRequest -= OnCloseRequest;
                }
            };
        }

        private void OnCloseRequest()
        {
            Window.GetWindow(this)?.Close();
        }
    }  
}
