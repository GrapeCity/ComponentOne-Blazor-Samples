using C1.Document;
using System.Net;
using System.Reflection;

namespace BlazorExplorer;

public class DocSources
{
    const string FlxrFileName = @"Data/FlexCommonTasksSQLite.flxr";

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
        
        var asm = Assembly.GetExecutingAssembly();
        docSource.LoadFromStream(asm.GetManifestResourceStream($"{asm.GetName().Name}.Data.DefaultDocument.pdf"));       
        return docSource;
    }

    public static string[] GetReports()
    {
        return C1.Report.FlexReport.GetReportList(FlxrFileName);
    }

    public static C1DocumentSource GetReport(string reportName)
    {
        var docSource = new C1.Report.FlexReport();
        docSource.Load(FlxrFileName, reportName);
        return docSource;
    }
}
