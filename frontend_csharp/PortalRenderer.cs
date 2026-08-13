namespace MediSync.Portal;

public record SummaryDto(int count, Dictionary<string, int>? byBloodType);

public static class PortalRenderer
{
    public static string Render(SummaryDto summary)
    {
        var lines = new List<string> { "MediSync Patient Portal", $"Records: {summary.count}" };
        if (summary.byBloodType != null)
        {
            foreach (var kv in summary.byBloodType.OrderBy(k => k.Key))
                lines.Add($"{kv.Key}: {kv.Value}");
        }
        return string.Join(Environment.NewLine, lines);
    }

    public static string RenderCopy(SummaryDto summary)
    {
        var lines = new List<string> { "MediSync Patient Portal", $"Records: {summary.count}" };
        if (summary.byBloodType != null)
        {
            foreach (var kv in summary.byBloodType.OrderBy(k => k.Key))
                lines.Add($"{kv.Key}: {kv.Value}");
        }
        return string.Join(Environment.NewLine, lines);
    }
}
