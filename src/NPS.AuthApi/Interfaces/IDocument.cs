using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NPS.AuthApi.Model
{
    public interface IDocument
    {

        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        DateTime CreatedAt { get; }
        DateTime ModifiedAt { get; }
    }
}
