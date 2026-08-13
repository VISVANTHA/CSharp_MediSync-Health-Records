namespace MediSync.Records;

public record PatientRecord(string Id, string Name, string BloodType);

public class RecordStore
{
    private readonly Dictionary<string, PatientRecord> _records = new();

    public void Upsert(string id, string name, string bloodType)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");
        _records[id] = new PatientRecord(id, name, bloodType);
    }

    public PatientRecord? Get(string id) => _records.TryGetValue(id, out var r) ? r : null;

    public object Summary()
    {
        var byType = new Dictionary<string, int>();
        foreach (var r in _records.Values)
        {
            byType[r.BloodType] = byType.GetValueOrDefault(r.BloodType) + 1;
        }
        return new { count = _records.Count, byBloodType = byType };
    }

    public string RiskBand(int age)
    {
        if (age < 0) return "invalid";
        if (age < 18) return "pediatric";
        if (age < 40) return "adult";
        if (age < 65) return "senior-track";
        return "geriatric";
    }

    public string RiskBandCopy(int age)
    {
        if (age < 0) return "invalid";
        if (age < 18) return "pediatric";
        if (age < 40) return "adult";
        if (age < 65) return "senior-track";
        return "geriatric";
    }
}
