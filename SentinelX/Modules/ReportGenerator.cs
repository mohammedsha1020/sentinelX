using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SentinelX.Models;

namespace SentinelX.Modules
{
    public class ReportGenerator
    {
        public string GenerateWeeklyReport(List<Alert> alerts)
        {
            string reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportDir))
                Directory.CreateDirectory(reportDir);
            string fileName = $"SentinelX_WeeklyReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(reportDir, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();
                doc.Add(new Paragraph("SentinelX Weekly Forensic Report", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)));
                doc.Add(new Paragraph($"Generated: {DateTime.Now}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(5) { WidthPercentage = 100 };
                table.AddCell("Time");
                table.AddCell("Severity");
                table.AddCell("Source");
                table.AddCell("Message");
                table.AddCell("IP Address");

                foreach (var alert in alerts)
                {
                    table.AddCell(alert.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                    table.AddCell(alert.Severity);
                    table.AddCell(alert.Source);
                    table.AddCell(alert.Message);
                    table.AddCell(alert.IPAddress ?? "");
                }
                doc.Add(table);
                doc.Close();
            }
            return filePath;
        }
    }
} 