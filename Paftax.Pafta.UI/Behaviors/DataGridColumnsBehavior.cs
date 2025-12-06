using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Paftax.Pafta.UI.Behaviors
{
    public static class DataGridColumnsBehavior
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty =
            DependencyProperty.RegisterAttached(
                "ColumnDefinitions",
                typeof(IEnumerable),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(null, OnColumnDefinitionsChanged));

        public static void SetColumnDefinitions(DependencyObject obj, IEnumerable value)
            => obj.SetValue(ColumnDefinitionsProperty, value);

        public static IEnumerable GetColumnDefinitions(DependencyObject obj)
            => (IEnumerable)obj.GetValue(ColumnDefinitionsProperty);

        private static void OnColumnDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid dataGrid)
                return;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= (_, _) => RefreshColumns(dataGrid);

            if (e.NewValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += (_, _) => RefreshColumns(dataGrid);

            RefreshColumns(dataGrid);
        }

        private static void RefreshColumns(DataGrid dataGrid)
        {
            var items = GetColumnDefinitions(dataGrid);
            if (items == null)
                return;

            if (dataGrid.Columns.Count > 1)
            {
                while (dataGrid.Columns.Count > 1)
                    dataGrid.Columns.RemoveAt(1);
            }

            foreach (var item in items)
            {
                Type itemType = item.GetType();

                var header = (string)itemType.GetProperty("Header")?.GetValue(item)!;
                var width = (DataGridLength)itemType.GetProperty("Width")?.GetValue(item)!;
                var canResize = (bool)itemType.GetProperty("CanResize")?.GetValue(item)!;
                var bindingFunc = itemType.GetProperty("BindingPath")?.GetValue(item);

                if (bindingFunc == null)
                    continue;

                DataGridTextColumn col = new()
                {
                    Header = header,
                    Width = width,
                    IsReadOnly = true
                };

                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Converter = new FuncValueConverter(bindingFunc)
                };

                col.Binding = binding;

                if (!canResize)
                {
                    col.CanUserResize = false;

                    if (width.UnitType != DataGridLengthUnitType.Star)
                    {
                        col.MinWidth = width.Value;
                        col.MaxWidth = width.Value;
                    }
                }

                dataGrid.Columns.Add(col);
            }
        }

        private class FuncValueConverter(object func) : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (func is Delegate del && value != null)
                {
                    return del.DynamicInvoke(value) ?? string.Empty;
                }

                return string.Empty;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                => throw new NotImplementedException();
        }
    }
}
