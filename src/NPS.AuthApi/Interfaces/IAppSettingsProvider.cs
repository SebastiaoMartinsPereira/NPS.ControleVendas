using NPS.AuthApi.Model;

namespace NPS.AuthApi.Domain
{
    public interface IAppSettingsProvider
    {
        string Ambiente { get; }
        IMongoDbSettings? MongoDbConfig { get; }
        Jwt? Jwt { get; }
    }

    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IConfiguration configuration;

        public AppSettingsProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IMongoDbSettings? MongoDbConfig => configuration.GetSection("MongoDbConfig").Get<MongoDbSettings>();

        public Jwt? Jwt => configuration.GetSection("Jwt").Get<Jwt?>();

        public string Ambiente => configuration.GetValue<string>("Ambiente") ?? "";
    }
}
