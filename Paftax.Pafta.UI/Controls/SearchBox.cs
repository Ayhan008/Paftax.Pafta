using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Controls
{
    public class SearchBox : Control
    {
        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly RoutedEvent SearchTextChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SearchTextChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

        public event RoutedEventHandler SearchTextChanged
        {
            add { AddHandler(SearchTextChangedEvent, value); }
            remove { RemoveHandler(SearchTextChangedEvent, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
             DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_SearchTextBox") is TextBox searchTextBox)
            {
                searchTextBox.TextChanged += (s, e) => OnSearchTextBoxChanged();
                searchTextBox.MouseEnter += (s, e) => UpdateOuterBorder();
                searchTextBox.MouseLeave += (s, e) => UpdateOuterBorder();
                searchTextBox.GotFocus += (s, e) => UpdateOuterBorder();
                searchTextBox.LostFocus += (s, e) => UpdateOuterBorder();
            }
        }

        protected void UpdateOuterBorder()
        {
            if (GetTemplateChild("PART_SearchTextBox") is TextBox searchTextBox)
            {
                if (GetTemplateChild("PART_BottomBorder") is Border bottomBorder)
                {
                    if (searchTextBox.IsFocused)
                    {
                        bottomBorder.Background = TryFindResource("SearchBoxClickedBottomBackgroundBrush") as Brush;
                        bottomBorder.BorderBrush = TryFindResource("SearchBoxClickedBottomBorderBrush") as Brush;
                    }

                    else if (searchTextBox.IsMouseOver)
                    {
                        bottomBorder.Background = TryFindResource("SearchBoxHoverBottomBackgroundBrush") as Brush;
                        bottomBorder.BorderBrush = TryFindResource("SearchBoxHoverBottomBorderBrush") as Brush;
                    }

                    else
                    {
                        bottomBorder.Background = Brushes.Transparent;
                        bottomBorder.BorderBrush = Brushes.Transparent;
                    }         
                }               
            }  
        }

        protected void OnSearchTextBoxChanged()
        {
            RoutedEventArgs args = new(SearchTextChangedEvent);
            RaiseEvent(args);

            if (GetTemplateChild("PART_SearchTextBox") is TextBox searchTextBox)
            {
                if (SearchText != searchTextBox.Text)
                    SearchText = searchTextBox.Text;

                if (!string.IsNullOrEmpty(searchTextBox.Text))
                {
                    if (GetTemplateChild("PART_ClearButton") is Button clearButton)
                    {
                        clearButton.Click += (s, e) => searchTextBox.Clear();
                        clearButton.Visibility = Visibility.Visible;
                    }
                    if (GetTemplateChild("PART_PlaceholderText") is TextBlock placeholderTextBlock)
                    {
                        placeholderTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
                if (string.IsNullOrEmpty(searchTextBox.Text))
                {
                    if (GetTemplateChild("PART_ClearButton") is Button clearButton)
                    {
                        clearButton.Visibility = Visibility.Collapsed;
                    }
                    if (GetTemplateChild("PART_PlaceholderText") is TextBlock placeholderTextBlock)
                    {
                        placeholderTextBlock.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
