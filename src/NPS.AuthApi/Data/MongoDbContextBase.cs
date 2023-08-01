using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NPS.AuthApi.Domain;
using NPS.AuthApi.Model;

namespace NPS.AuthApi.Data
{
    public class MongoDbContextBase : IMongoDbContextBase
    {
        public IMongoClient MongoClient { get; }
        private readonly IMongoDatabase database;

        public MongoDbContextBase(IAppSettingsProvider settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var url = new MongoUrl(settings.MongoDbConfig.ConnectionString);
            MongoClient = new MongoClient(url); 
            database = MongoClient.GetDatabase(settings.MongoDbConfig.DataBaseName);
        }

        public virtual void RegisterClassMap<Entity, Mapper>() where Mapper : BsonClassMap<Entity>, new()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity))) BsonClassMap.RegisterClassMap(new Mapper());

        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : IDocument
        {
            return database.GetCollection<TDocument>(name);
        }
    }
}
