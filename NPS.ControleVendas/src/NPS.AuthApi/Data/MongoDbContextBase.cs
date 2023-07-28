using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NPS.AuthApi.Domain;

namespace NPS.AuthApi.Data
{
    public abstract class MongoDbContextBase : IMongoDbContextBase
    {
        public IMongoClient MongoClient { get; }
        private readonly IMongoDatabase database;

        protected MongoDbContextBase(ISettingsProvider settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var url = new MongoUrl(settings.MongoDbConfig.ConnectionString);
            MongoClient = new MongoClient(url);
            database = MongoClient.GetDatabase(url.DatabaseName);
        }

        protected abstract void OnRegisterMappers();

        public virtual void RegisterClassMap<Entity, Mapper>() where Mapper : BsonClassMap<Entity>, new()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity))) BsonClassMap.RegisterClassMap(new Mapper());

        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : class
        {
            return database.GetCollection<TDocument>(name);
        }
    }
}
