using ReportServer;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<ReportStorage>();
builder.Services.AddSingleton<ReportRenderer>();

var app = builder.Build();

// Store a new report
app.MapPost("/reports", (Report report, ReportStorage storage) =>
{
    var id = storage.StoreReport(report);
    return Results.Ok(new { id = id, viewUrl = $"/reports/{id}" });
});

// View a stored report
app.MapGet("/reports/{id}", (Guid id, ReportStorage storage, ReportRenderer renderer) =>
{
    var report = storage.GetReport(id);
    if (report == null)
    {
        return Results.NotFound(new { error = "Report not found" });
    }

    var html = renderer.RenderToHtml(report);
    return Results.Content(html, "text/html");
});

// View the last report
app.MapGet("/reports/last", (ReportStorage storage, ReportRenderer renderer) =>
{
    var (id, report) = storage.GetLastReport();
    if (report == null)
    {
        return Results.NotFound(new { error = "No reports have been stored yet" });
    }

    var html = renderer.RenderToHtml(report);
    return Results.Content(html, "text/html", Encoding.UTF8);
});

app.MapGet("/", () => """
    Welcome to Report Server!
    
    To create a report:
    POST /reports with JSON containing 'title' and 'contents' fields
    
    To view a specific report:
    GET /reports/{id} where id is the GUID returned from the POST request
    
    To view the last stored report:
    GET /reports/last
    """);

app.Run();
