using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SDColor = System.Drawing.Color;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Versioning;

namespace api.BusinessLogic.Services.Reports
{
    public class OvertimeReport : IDocument
    {
        private readonly ReportData Data;
        public OvertimeReport(ReportData data) => Data = data;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingBottom(5).Text("📊 Overtime Report")
                            .FontSize(28).Bold()
                            .FontColor("#2E7D32");
                        
                        col.Item().Text("Overtime Management System")
                            .FontSize(13)
                            .FontColor("#616161");
                        
                        col.Item().PaddingTop(10)
                            .LineHorizontal(2)
                            .LineColor("#4CAF50");
                    });
                });

                // ======= CONTENT =======
                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Spacing(20);
                    col.Item().Element(ComposeSummary);
                    col.Item().Element(ComposeTopUsers);
                    col.Item().Element(ComposeCharts);
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generated on {DateTime.Now:MM/dd/yyyy HH:mm}")
                    .FontSize(10)
                    .FontColor("#9E9E9E");
            });
        }

        void ComposeSummary(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("General Summary")
                    .FontSize(20).Bold()
                    .FontColor("#2E7D32");

                col.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Element(c => ComposeSummaryCard(c, "📅", "Total Requests", Data.TotalRequests.ToString(), "#2196F3"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => ComposeSummaryCard(c, "✅", "Approved", Data.Approved.ToString(), "#4CAF50"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => ComposeSummaryCard(c, "❌", "Rejected", Data.Rejected.ToString(), "#F44336"));
                });

                col.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Element(c => ComposeSummaryCard(c, "⏱️", "Response Time", $"{Data.AvgResponseTime:F1} hrs", "#FF9800"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => ComposeSummaryCard(c, "💰", "Total Cost", $"${Data.TotalCost:N2}", "#9C27B0"));
                });
            });
        }

        void ComposeSummaryCard(IContainer container, string icon, string label, string value, string color)
        {
            container
                .Background("#FAFAFA")
                .Border(1)
                .BorderColor("#E0E0E0")
                .Padding(15)
                .Column(col =>
                {
                    col.Item().Text(icon).FontSize(24);
                    col.Item().PaddingTop(5).Text(label)
                        .FontSize(11)
                        .FontColor("#757575");
                    col.Item().PaddingTop(2).Text(value)
                        .FontSize(18).Bold()
                        .FontColor(color);
                });
        }

        void ComposeTopUsers(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("🏅 Top 5 Users by Hours")
                    .FontSize(20).Bold()
                    .FontColor("#2E7D32");

                if (Data.TopUsers.Any())
                {
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background("#2E7D32")
                                .Padding(12)
                                .Text("#").FontColor(Colors.White).Bold();
                            header.Cell().Background("#2E7D32")
                                .Padding(12)
                                .Text("User").FontColor(Colors.White).Bold();
                            header.Cell().Background("#2E7D32")
                                .Padding(12)
                                .AlignRight()
                                .Text("Total Hours").FontColor(Colors.White).Bold();
                        });

                        int index = 1;
                        foreach (var u in Data.TopUsers)
                        {
                            var bgColor = index % 2 == 0 ? "#F5F5F5" : "#FFFFFF";
                            
                            table.Cell().Background(bgColor).Padding(10)
                                .AlignCenter()
                                .Text($"{index}").FontSize(12).Bold().FontColor("#757575");
                            
                            table.Cell().Background(bgColor).Padding(10)
                                .Text(u.UserName).FontSize(12);
                            
                            table.Cell().Background(bgColor).Padding(10)
                                .AlignRight()
                                .Text($"{u.TotalHours} h").FontSize(12).Bold().FontColor("#2E7D32");
                            
                            index++;
                        }
                    });
                }
                else
                {
                    col.Item().Text("Not enough records.")
                        .Italic().FontColor("#9E9E9E");
                }
            });
        }

        void ComposeCharts(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("📈 Visualizations")
                    .FontSize(20).Bold()
                    .FontColor("#2E7D32");

                col.Item().PaddingTop(15).Row(row =>
                {
                    row.RelativeItem(3).Padding(5).Column(c =>
                    {
                        c.Item().Text("Hours by User")
                            .FontSize(14).Bold().FontColor("#424242");
                        c.Item().PaddingTop(10)
                            .Border(1)
                            .BorderColor("#E0E0E0")
                            .Padding(10)
                            .Image(GenerateBarChart(Data.TopUsers))
                            .FitWidth();
                    });

                    row.RelativeItem(2).Padding(5).Column(c =>
                    {
                        c.Item().Text("Request Status")
                            .FontSize(14).Bold().FontColor("#424242");
                        c.Item().PaddingTop(10)
                            .Border(1)
                            .BorderColor("#E0E0E0")
                            .Padding(10)
                            .Image(GeneratePieChart(Data.Approved, Data.Rejected))
                            .FitWidth();
                    });
                });
            });
        }

        [SupportedOSPlatform("windows")]
        private byte[] GenerateBarChart(List<TopUser> users)
        {
            int width = 1200, height = 600;
            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);
            
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            
            g.Clear(SDColor.White);

            if (users.Count == 0)
            {
                using var font = new Font("Arial", 14, FontStyle.Italic);
                g.DrawString("No data available", font, Brushes.Gray, width / 2 - 80, height / 2);
                using var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }

            var max = users.Max(u => u.TotalHours);
            int barWidth = 120, spacing = 60, startX = 80, baseY = height - 100;
            int chartHeight = 400;

            using var penGrid = new Pen(SDColor.FromArgb(230, 230, 230), 1);
            for (int i = 0; i <= 5; i++)
            {
                int y = baseY - (chartHeight * i / 5);
                g.DrawLine(penGrid, startX - 20, y, width - 50, y);
                
                using var fontGrid = new Font("Arial", 10);
                string label = ((max * i / 5)).ToString("F0");
                g.DrawString(label, fontGrid, Brushes.Gray, 10, y - 8);
            }

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                int barHeight = (int)((user.TotalHours / max) * chartHeight);
                int x = startX + i * (barWidth + spacing);

                var rect = new Rectangle(x, baseY - barHeight, barWidth, barHeight);
                using var brush = new LinearGradientBrush(
                    rect,
                    SDColor.FromArgb(76, 175, 80),
                    SDColor.FromArgb(46, 125, 50),
                    LinearGradientMode.Vertical
                );
                
                g.FillRectangle(brush, rect);
                
                using var penBorder = new Pen(SDColor.FromArgb(46, 125, 50), 2);
                g.DrawRectangle(penBorder, rect);

                using var fontName = new Font("Arial", 11, FontStyle.Bold);
                string userName = user.UserName.Split('@')[0];
                if (userName.Length > 12) userName = userName.Substring(0, 10) + "..";
                
                var nameSize = g.MeasureString(userName, fontName);
                g.DrawString(userName, fontName, Brushes.Black, 
                    x + (barWidth - nameSize.Width) / 2, baseY + 10);

                using var fontValue = new Font("Arial", 12, FontStyle.Bold);
                string valueText = $"{user.TotalHours}h";
                var valueSize = g.MeasureString(valueText, fontValue);
                g.DrawString(valueText, fontValue, new SolidBrush(SDColor.FromArgb(46, 125, 50)),
                    x + (barWidth - valueSize.Width) / 2, baseY - barHeight - 30);
            }

            using var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }


        [SupportedOSPlatform("windows")]
        private byte[] GeneratePieChart(int approved, int rejected)
        {
            int width = 700, height = 700;
            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);
            
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            
            g.Clear(SDColor.White);

            float total = approved + rejected;
            if (total == 0) total = 1;

            float percentApproved = (approved / total) * 100;
            float percentRejected = (rejected / total) * 100;
            float angleApproved = (approved / total) * 360;

            int centerX = width / 2;
            int centerY = height / 2 - 50;
            int diameter = 400;
            var pieRect = new Rectangle(centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);

            using var shadowBrush = new SolidBrush(SDColor.FromArgb(50, 0, 0, 0));
            g.FillEllipse(shadowBrush, pieRect.X + 5, pieRect.Y + 5, diameter, diameter);

            using var brushApproved = new SolidBrush(SDColor.FromArgb(76, 175, 80));
            using var brushRejected = new SolidBrush(SDColor.FromArgb(244, 67, 54));
            
            g.FillPie(brushApproved, pieRect, -90, angleApproved);
            g.FillPie(brushRejected, pieRect, -90 + angleApproved, 360 - angleApproved);

            using var penWhite = new Pen(SDColor.White, 4);
            g.DrawPie(penWhite, pieRect, -90, angleApproved);
            g.DrawPie(penWhite, pieRect, -90 + angleApproved, 360 - angleApproved);

            int innerDiameter = 180;
            var innerRect = new Rectangle(centerX - innerDiameter / 2, centerY - innerDiameter / 2, 
                innerDiameter, innerDiameter);
            g.FillEllipse(Brushes.White, innerRect);

            using var fontTotal = new Font("Arial", 32, FontStyle.Bold);
            string totalText = total.ToString("F0");
            var totalSize = g.MeasureString(totalText, fontTotal);
            g.DrawString(totalText, fontTotal, Brushes.Black, 
                centerX - totalSize.Width / 2, centerY - 20);
            
            using var fontLabel = new Font("Arial", 14);
            string labelText = "TOTAL";
            var labelSize = g.MeasureString(labelText, fontLabel);
            g.DrawString(labelText, fontLabel, Brushes.Gray, 
                centerX - labelSize.Width / 2, centerY + 20);

            int legendY = height - 120;
            int legendX = 50;
            
            using var fontLegend = new Font("Arial", 16, FontStyle.Bold);
            
            g.FillRectangle(brushApproved, legendX, legendY, 40, 40);
            g.DrawString($"Approved: {approved} ({percentApproved:F1}%)", 
                fontLegend, Brushes.Black, legendX + 50, legendY + 8);
            
            g.FillRectangle(brushRejected, legendX + 350, legendY, 40, 40);
            g.DrawString($"Rejected: {rejected} ({percentRejected:F1}%)", 
                fontLegend, Brushes.Black, legendX + 400, legendY + 8);

            using var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}