using DocumentFormat.OpenXml.Spreadsheet;

namespace Paftax.Pafta.Exporters.Spreadsheet.Stylesheets
{
    public class ScheduleStylesheet : Stylesheet
    {
        public uint TitleStyleIndex { get; }
        public uint HeaderStyleIndex { get; }
        public uint BodyStyleIndex { get; }

        public ScheduleStylesheet()
        {
            var fonts = new Fonts(
                new Font( // 0: Bold Title/Header
                    new Bold(),
                    new FontName() { Val = "Arial" },
                    new FontSize() { Val = 11 }),

                new Font( // 1: Regular Body
                    new FontName() { Val = "Arial" },
                    new FontSize() { Val = 11 })
            );

            var fills = new Fills(
                new Fill(new PatternFill { PatternType = PatternValues.Solid }),
                new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),
                new Fill(
                    new PatternFill
                    {
                        PatternType = PatternValues.Solid,
                        ForegroundColor = new ForegroundColor { Rgb = "FFD9D9D9" },
                        BackgroundColor = new BackgroundColor { Rgb = "FFD9D9D9" },               
                    })
            );

            var borders = new Borders(
                new Border(),
                new Border(
                    new LeftBorder() { Style = BorderStyleValues.Thin },
                    new RightBorder() { Style = BorderStyleValues.Thin },
                    new TopBorder() { Style = BorderStyleValues.Thin },
                    new BottomBorder() { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

            var cellFormats = new CellFormats(
                new CellFormat(), // 0: required default
                new CellFormat // 1: Title
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 1,
                    ApplyFont = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Center,
                        Vertical = VerticalAlignmentValues.Center
                    }
                },
                new CellFormat // 2: Header
                {
                    FontId = 0,
                    FillId = 2,
                    BorderId = 1,
                    ApplyFont = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Center,
                        Vertical = VerticalAlignmentValues.Center
                    }
                },
                new CellFormat // 3: Body
                {
                    FontId = 1,
                    FillId = 0,
                    BorderId = 0,
                    ApplyFont = true,
                    ApplyFill = true,
                    ApplyBorder = true
                }
            );

            Append(fonts);
            Append(fills);
            Append(borders);
            Append(cellFormats);

            TitleStyleIndex = 1;
            HeaderStyleIndex = 2;
            BodyStyleIndex = 3;
        }
    }
}
