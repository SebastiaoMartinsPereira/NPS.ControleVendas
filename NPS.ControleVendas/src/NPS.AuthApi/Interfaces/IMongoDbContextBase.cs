using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NPS.AuthApi.Model;

namespace NPS.AuthApi.Data
{
    public interface IMongoDbContextBase
    {
        IMongoClient MongoClient { get; }

        IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : IDocument;
        void RegisterClassMap<Entity, Mapper>() where Mapper : BsonClassMap<Entity>, new();
    }
}