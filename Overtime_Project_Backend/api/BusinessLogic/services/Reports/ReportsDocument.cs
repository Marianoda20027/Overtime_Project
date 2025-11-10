using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace api.BusinessLogic.Services.Reports
{
    public class OvertimeReport : IDocument
    {
        private readonly ReportData Data;
        public OvertimeReport(ReportData data) => Data = data;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("📊 Overtime Report").FontSize(26).Bold().FontColor("#2E7D32");
                        col.Item().Text("Overtime Management System").FontColor("#616161");
                        col.Item().PaddingTop(10).LineHorizontal(2).LineColor("#4CAF50");
                    });
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Spacing(25);
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

        // ======= SECTIONS =======
        void ComposeSummary(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("General Summary").FontSize(18).Bold().FontColor("#2E7D32");
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Element(c => SummaryCard(c, "📅", "Total Requests", Data.TotalRequests.ToString(), "#2196F3"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => SummaryCard(c, "✅", "Approved", Data.Approved.ToString(), "#4CAF50"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => SummaryCard(c, "❌", "Rejected", Data.Rejected.ToString(), "#F44336"));
                });

                col.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Element(c => SummaryCard(c, "⏱️", "Response Time", $"{Data.AvgResponseTime:F1} hrs", "#FF9800"));
                    row.Spacing(10);
                    row.RelativeItem().Element(c => SummaryCard(c, "💰", "Total Cost", $"${Data.TotalCost:N2}", "#9C27B0"));
                });
            });
        }

        void SummaryCard(IContainer container, string icon, string label, string value, string color)
        {
            container.Background("#FAFAFA").Border(1).BorderColor("#E0E0E0").Padding(12)
                .Column(col =>
                {
                    col.Item().Text(icon).FontSize(22);
                    col.Item().PaddingTop(5).Text(label).FontSize(11).FontColor("#757575");
                    col.Item().PaddingTop(2).Text(value).FontSize(16).Bold().FontColor(color);
                });
        }

        void ComposeTopUsers(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("🏅 Top 5 Users by Hours").FontSize(18).Bold().FontColor("#2E7D32");

                if (Data.TopUsers.Any())
                {
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(40);
                            c.RelativeColumn(3);
                            c.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background("#2E7D32").Padding(10).Text("#").FontColor(Colors.White).Bold();
                            header.Cell().Background("#2E7D32").Padding(10).Text("User").FontColor(Colors.White).Bold();
                            header.Cell().Background("#2E7D32").Padding(10).AlignRight().Text("Total Hours").FontColor(Colors.White).Bold();
                        });

                        int index = 1;
                        foreach (var u in Data.TopUsers)
                        {
                            var bg = index % 2 == 0 ? "#F5F5F5" : "#FFFFFF";
                            table.Cell().Background(bg).Padding(8).AlignCenter().Text($"{index}");
                            table.Cell().Background(bg).Padding(8).Text(u.UserName);
                            table.Cell().Background(bg).Padding(8).AlignRight().Text($"{u.TotalHours} h").Bold().FontColor("#2E7D32");
                            index++;
                        }
                    });
                }
                else
                {
                    col.Item().Text("No data available").Italic().FontColor("#9E9E9E");
                }
            });
        }

        void ComposeCharts(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("📈 Visualizations").FontSize(18).Bold().FontColor("#2E7D32");

                col.Item().PaddingTop(15).Row(row =>
                {
                    row.RelativeItem(3).Padding(5).Column(c =>
                    {
                        c.Item().Text("Hours by User").FontSize(14).Bold().FontColor("#424242");
                        c.Item().PaddingTop(10).Border(1).BorderColor("#E0E0E0").Padding(10)
                            .Image(GenerateBarChart(Data.TopUsers)).FitWidth();
                    });

                    row.RelativeItem(2).Padding(5).Column(c =>
                    {
                        c.Item().Text("Request Status").FontSize(14).Bold().FontColor("#424242");
                        c.Item().PaddingTop(10).Border(1).BorderColor("#E0E0E0").Padding(10)
                            .Image(GeneratePieChart(Data.Approved, Data.Rejected)).FitWidth();
                    });
                });
            });
        }

        // ======= SKIASHARP CHARTS =======
        private byte[] GenerateBarChart(List<TopUser> users)
        {
            int width = 800, height = 400;
            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            if (!users.Any())
            {
                var paint = new SKPaint { Color = SKColors.Gray, TextSize = 24, TextAlign = SKTextAlign.Center };
                canvas.DrawText("No data available", width / 2, height / 2, paint);
            }
            else
            {
                var max = users.Max(u => u.TotalHours);
                float barWidth = width / (users.Count * 2f);
                float startX = 80;
                float chartHeight = height - 100;

                var paint = new SKPaint { IsAntialias = true };
                var text = new SKPaint { Color = SKColors.Black, TextSize = 14, IsAntialias = true };

                for (int i = 0; i < users.Count; i++)
                {
                    var u = users[i];
                    float barHeight = (float)(u.TotalHours / max) * chartHeight;
                    float x = startX + i * (barWidth * 1.5f);
                    float y = height - barHeight - 40;

                    paint.Color = new SKColor(76, 175, 80);
                    canvas.DrawRect(x, y, barWidth, barHeight, paint);

                    string label = u.UserName.Split('@')[0];
                    if (label.Length > 10) label = label[..8] + "..";
                    canvas.DrawText(label, x, height - 15, text);

                    canvas.DrawText($"{u.TotalHours}h", x, y - 5, text);
                }
            }

            using var img = surface.Snapshot();
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        private byte[] GeneratePieChart(int approved, int rejected)
        {
            int width = 400, height = 400;
            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            float total = approved + rejected;
            if (total == 0) total = 1;
            float angleApproved = (approved / total) * 360;

            var rect = new SKRect(50, 50, width - 50, height - 50);
            using var green = new SKPaint { Color = new SKColor(76, 175, 80), IsAntialias = true };
            using var red = new SKPaint { Color = new SKColor(244, 67, 54), IsAntialias = true };

            canvas.DrawArc(rect, -90, angleApproved, true, green);
            canvas.DrawArc(rect, -90 + angleApproved, 360 - angleApproved, true, red);

            var text = new SKPaint { Color = SKColors.Black, TextSize = 16, IsAntialias = true };
            canvas.DrawText($"Approved: {approved}", 30, height - 60, text);
            canvas.DrawText($"Rejected: {rejected}", 30, height - 30, text);

            using var img = surface.Snapshot();
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
    }
}
