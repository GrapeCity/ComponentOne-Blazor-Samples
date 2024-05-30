using C1.Document;
using C1.Report;
using GrapeCity.Documents.Word;
using System.Net;

namespace BlazorExplorer.Pages.Viewer
{
    public class DocSources
    {

        public static C1DocumentSource GetSSRS()
        {
            var c1SsrsDocumentSource1 = new C1SSRSDocumentSource();
            c1SsrsDocumentSource1.Language = new System.Globalization.CultureInfo("en-US");
            c1SsrsDocumentSource1.Credential = new NetworkCredential("ssrs_demo", "bjqveS5gh89BH1Q", "");
            c1SsrsDocumentSource1.DocumentLocation = new SSRSReportLocation("http://ssrs.componentone.com:8000/ReportServer", "/AdventureWorks/Employee Sales Summary Detail");
            return c1SsrsDocumentSource1;
        }

        public static C1DocumentSource GetPdf()
        {
            var docSource = new C1PdfDocumentSource();
            docSource.DocumentLocation = @"Data/DefaultDocument.pdf";
            return docSource;
        }

        public static C1DocumentSource GetReport()
        {
            var docSource = new C1.Report.FlexReport();
            docSource.Load(@"Data/FlexCommonTasksSQLite.flxr", "Simple List");
            return docSource;
        }
    }
}
