using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paftax.Pafta.UI.Behaviors
{
    public static class TabControlBehavior
    {
        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectionChangedCommand",
                typeof(ICommand),
                typeof(TabControlBehavior),
                new PropertyMetadata(null, OnSelectionChangedCommandChanged));

        public static ICommand GetSelectionChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetSelectionChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedCommandProperty, value);
        }

        private static void OnSelectionChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                tabControl.SelectionChanged -= OnTabControlSelectionChanged;

                if (e.NewValue is ICommand)
                {
                    tabControl.SelectionChanged += OnTabControlSelectionChanged;
                }
            }
        }

        private static void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl)
            {
                var command = GetSelectionChangedCommand(tabControl);
                if (command?.CanExecute(tabControl.SelectedIndex) == true)
                {
                    command.Execute(tabControl.SelectedIndex);
                }
            }
        }
    }
}