using System.Net.Http.Json;
using MediSync.Portal;

var api = Environment.GetEnvironmentVariable("MEDISYNC_API") ?? "http://127.0.0.1:5080";
using var client = new HttpClient { BaseAddress = new Uri(api) };
var summary = await client.GetFromJsonAsync<SummaryDto>("/api/records/summary")
    ?? throw new Exception("empty summary");
Console.WriteLine(PortalRenderer.Render(summary));
