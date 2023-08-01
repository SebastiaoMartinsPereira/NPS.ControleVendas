namespace NPS.AuthApi.Domain
{
    public interface IMongoDbSettings
    {
        string? ConnectionString { get; }
        public string? DataBaseName { get; set; }

    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DataBaseName { get; set; }
    }
}