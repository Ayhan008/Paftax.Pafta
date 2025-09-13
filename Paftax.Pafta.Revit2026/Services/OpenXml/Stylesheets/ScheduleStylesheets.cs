using DocumentFormat.OpenXml.Spreadsheet;

namespace Paftax.Pafta.Revit2026.Services.OpenXml.Stylesheets
{
    internal class ScheduleStylesheets
    {
        public static Stylesheet GenericStylesheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(new Bold(), new FontName() { Val = "Arial" }, new FontSize() { Val = 11 }),
                    new Font(new FontName() { Val = "Arial" }, new FontSize() { Val = 11 })
                ),
                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None }),
                    new Fill(new PatternFill(new ForegroundColor() { Rgb = "FFD9D9D9" }) { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(
                        new LeftBorder(new Color() { Rgb = "000000" }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Rgb = "000000" }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Rgb = "000000" }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Rgb = "000000" }) { Style = BorderStyleValues.Thin }
                    )
                ),
                new CellFormats(
                    new CellFormat
                    {
                        // Index 0, Title.
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0,
                        ApplyFont = true,
                        ApplyFill = true,
                        ApplyBorder = true,
                        Alignment = new Alignment()
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center
                        }
                    },
                    new CellFormat
                    {
                        // Index 1, Column Header.
                        FontId = 0,
                        FillId = 1,
                        BorderId = 0,
                        ApplyFont = true,
                        ApplyFill = true,
                        ApplyBorder = true,
                        Alignment = new Alignment()
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center
                        }
                    },
                    new CellFormat
                    {
                        // Index 2, Body.
                        FontId = 1,
                        FillId = 0,
                        BorderId = 0,
                        ApplyFont = true,
                        ApplyFill = true,
                        ApplyBorder = true,
                    }
                )
            );
        }
    }
}
