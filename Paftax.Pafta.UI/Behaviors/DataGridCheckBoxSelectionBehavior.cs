using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Paftax.Pafta.UI.Behaviors
{
    public static class DataGridCheckBoxSelectionBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(DataGridCheckBoxSelectionBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
             element.SetValue(IsEnabledProperty, value);
        }
            
        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }
            
        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid grid)
                return;

            if ((bool)e.NewValue)
            {
                // Ensure extended selection for multi-row selection
                grid.SelectionMode = DataGridSelectionMode.Extended;

                // Listen to CheckBox events bubbling up from cells
                grid.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(OnCheckBoxToggled));
                grid.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(OnCheckBoxToggled));
            }
            else
            {
                grid.RemoveHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(OnCheckBoxToggled));
                grid.RemoveHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(OnCheckBoxToggled));
            }
        }

        private static void OnCheckBoxToggled(object sender, RoutedEventArgs e)
        {
            if (sender is not DataGrid grid)
                return;

            if (e.OriginalSource is not FrameworkElement sourceElement)
                return;

            object? clickedItem = sourceElement.DataContext;
            if (clickedItem is null)
                return;

            bool isChecked = e.RoutedEvent == ToggleButton.CheckedEvent;

            // If multiple rows are selected, apply the same check state to all of them
            var selectedItems = grid.SelectedItems.Cast<object>().ToList();

            if (selectedItems.Count > 1)
            {
                foreach (var selectedItem in selectedItems)
                {
                    var prop = selectedItem.GetType().GetProperty("IsChecked");
                    prop?.SetValue(selectedItem, isChecked);
                }
            }
            else
            {
                // Normal single row toggle
                var prop = clickedItem.GetType().GetProperty("IsChecked");
                prop?.SetValue(clickedItem, isChecked);
            }

            e.Handled = false; // allow bindings to update normally
        }
    }
}
