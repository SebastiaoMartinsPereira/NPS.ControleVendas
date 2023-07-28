namespace NPS.AuthApi.Domain
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; }
    }
}