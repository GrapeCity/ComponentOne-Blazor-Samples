using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

using C1.Excel;

using BlazorExplorer.Localization;

namespace BlazorExplorer
{
    public class WeatherExcelService
    {
        public Task<byte[]> GetExcelAsync(DateTime startDate)
        {
            var forecasts = WeatherForecastService.GetForecast(startDate);
            return Task.FromResult(ExcelExport(forecasts, FileFormat.OpenXml));
        }

        private byte[] ExcelExport(WeatherForecast[] forecasts, FileFormat format)
        {
            var book = new C1.Excel.C1XLBook();
            book.Author = "MESCIUS Inc.";

            var sheet = book.Sheets[0];
            sheet.Name = "Weather Forecast";

            sheet.Columns[1].Width = 1200;
            sheet.Columns[2].Width = 1000;
            sheet.Columns[3].Width = 1000;
            sheet.Columns[4].Width = 2000;

            var dateStyle = new XLStyle(book);
            dateStyle.Font = new XLFont("Helvertica", 11, Color.DarkGray);
            dateStyle.Format = XLStyle.FormatXLToDotNet("dd/MM/yyyy");

            var tempStyle = new XLStyle(book);
            tempStyle.Font = new XLFont("Helvertica", 11, Color.DarkCyan);

            var summaryStyle = new XLStyle(book);
            summaryStyle.Font = new XLFont("Helvertica", 11, Color.Black);

            var headerStyle = new XLStyle(book);
            headerStyle.BorderBottom = XLLineStyle.Thick;
            headerStyle.BorderColorBottom = Color.Black;
            headerStyle.Font = new XLFont("Helvertica", 11, true, false);

            sheet[1, 1].Style = headerStyle;
            sheet[1, 2].Style = headerStyle;
            sheet[1, 3].Style = headerStyle;
            sheet[1, 4].Style = headerStyle;

            sheet[1, 1].Value = ExcelRes.Date;
            sheet[1, 2].Value = ExcelRes.CTemp;
            sheet[1, 3].Value = ExcelRes.FTemp;
            sheet[1, 4].Value = ExcelRes.Summary;

            if (forecasts != null)
            {
                for (int r = 0; r < forecasts.Length; r++)
                {
                    int row = r + 2;

                    sheet[row, 1].Style = dateStyle;
                    sheet[row, 2].Style = tempStyle;
                    sheet[row, 3].Style = tempStyle;
                    sheet[row, 4].Style = summaryStyle;

                    sheet[row, 1].Value = forecasts[r].Date;
                    sheet[row, 2].Value = forecasts[r].TemperatureC;
                    sheet[row, 3].Value = forecasts[r].TemperatureF;
                    sheet[row, 4].Value = forecasts[r].Summary;
                }
            }

            using (var stream = new MemoryStream())
            {
                book.Save(stream, format);
                return stream.ToArray();
            }
        }
    }
}