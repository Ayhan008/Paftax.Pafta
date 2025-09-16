using System.Windows;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Controls
{
    public class FontIcon : Control
    {
        static FontIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIcon), new FrameworkPropertyMetadata(typeof(FontIcon)));
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.
            Register(nameof(Glyph), typeof(string), typeof(FontIcon), new PropertyMetadata(string.Empty));
        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }
    }
}
