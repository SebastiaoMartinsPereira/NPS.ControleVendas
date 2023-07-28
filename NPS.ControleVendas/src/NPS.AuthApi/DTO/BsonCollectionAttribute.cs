namespace NPS.AuthApi.Model
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; } = string.Empty;
        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

    }
}
