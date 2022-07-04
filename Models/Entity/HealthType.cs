using MongoDB.Bson.Serialization.Attributes;

namespace WebPhongKham.Models.Entity
{
    public class HealthType
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public bool Deletable { get; set; }
    }
}
