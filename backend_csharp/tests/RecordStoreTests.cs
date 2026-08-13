using MediSync.Records;

public static class RecordStoreTests
{
    public static void UpsertAndSummary()
    {
        var store = new RecordStore();
        store.Upsert("p-1", "Alice", "A+");
        var summary = store.Summary();
        if (summary is null) throw new Exception("summary null");
    }

    public static void RiskBands()
    {
        if (new RecordStore().RiskBand(10) != "pediatric")
            throw new Exception("risk band");
    }
}
