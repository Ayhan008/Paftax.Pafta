using System.Windows;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Controls
{
    public class TitleBar : Control
    {
        static TitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleBar), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(TitleBar), new PropertyMetadata(15.0));

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonProperty =
            DependencyProperty.Register(nameof(CloseButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        public bool CloseButton
        {
            get { return (bool)GetValue(CloseButtonProperty); }
            set { SetValue(CloseButtonProperty, value); }
        }

        public static readonly DependencyProperty HelpButtonProperty =
            DependencyProperty.Register(nameof(HelpButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

        public bool HelpButton
        {
            get { return (bool)GetValue(HelpButtonProperty); }
            set { SetValue(HelpButtonProperty, value); }
        }

        public static readonly DependencyProperty MaximizeButtonProperty =
            DependencyProperty.Register(nameof(MaximizeButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        public bool MaximizeButton
        {
            get { return (bool)GetValue(MaximizeButtonProperty); }
            set { SetValue(MaximizeButtonProperty, value); }
        }

        public static readonly DependencyProperty MinimizeButtonProperty =
            DependencyProperty.Register(nameof(MinimizeButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        public bool MinimizeButton
        {
            get { return (bool)GetValue(MinimizeButtonProperty); }
            set { SetValue(MinimizeButtonProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("CloseButton") is Button closeButton)
            {
                closeButton.Click += (s, e) =>
                {
                    Window.GetWindow(this)?.Close();
                };
            }

            if (GetTemplateChild("TitleBarBorder") is Border titleBarBorder)
            {
                titleBarBorder.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                    {
                        Window.GetWindow(this)?.DragMove();
                    }
                };
            }            
        }
    }
}
