using MediSync.Portal;

public static class PortalRendererTests
{
    public static void RendersCount()
    {
        var text = PortalRenderer.Render(new SummaryDto(2, new Dictionary<string, int> { ["A+"] = 1 }));
        if (!text.Contains("Records: 2")) throw new Exception("render failed");
    }
}
