namespace WiSave.Subscriptions.Projections.Configuration;

internal record MongoDbOptions
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}