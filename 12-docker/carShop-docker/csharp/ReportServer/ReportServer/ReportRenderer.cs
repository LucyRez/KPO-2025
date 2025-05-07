namespace ReportServer;

public class ReportRenderer
{
    public string RenderToHtml(Report report)
    {
        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <title>{report.Title}</title>
            </head>
            <body>
                <h1>{report.Title}</h1>
                {report.Contents.Replace("\n", "<br>")}
            </body>
            </html>
            """;
    }
} 