using System.Net.Http.Json;
using System.Text.Json;

namespace ReportServer.Client;

public class ReportServerClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ReportServerClient(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient();
    }

    public ReportServerClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public async Task<(Guid Id, string ViewUrl)> StoreReportAsync(Report report)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/reports", report);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<StoreReportResponse>();
        if (result == null)
            throw new InvalidOperationException("Failed to parse the server response");
            
        return (result.Id, result.ViewUrl);
    }

    public async Task<string> GetReportHtmlAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/reports/{id}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetLastReportHtmlAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/reports/last");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    private class StoreReportResponse
    {
        public Guid Id { get; set; }
        public string ViewUrl { get; set; } = string.Empty;
    }
} 