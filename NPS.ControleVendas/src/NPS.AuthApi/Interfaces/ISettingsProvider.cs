namespace NPS.AuthApi.Domain
{
    public interface ISettingsProvider
    {
        string Ambiente { get; }
        IMongoDbSettings MongoDbConfig { get; }
    }
}
