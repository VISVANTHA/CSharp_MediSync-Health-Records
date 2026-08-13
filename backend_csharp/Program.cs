using MediSync.Records;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var store = new RecordStore();
store.Upsert("p-1", "Alice", "A+");
store.Upsert("p-2", "Bob", "B");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapGet("/api/records/summary", () => Results.Ok(store.Summary()));
app.MapGet("/api/records/{id}", (string id) =>
{
    var rec = store.Get(id);
    return rec is null ? Results.NotFound() : Results.Ok(rec);
});
app.Run("http://127.0.0.1:5080");
