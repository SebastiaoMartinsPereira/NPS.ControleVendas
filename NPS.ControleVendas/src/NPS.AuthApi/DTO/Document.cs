using MongoDB.Bson;

namespace NPS.AuthApi.Model
{
    public class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
