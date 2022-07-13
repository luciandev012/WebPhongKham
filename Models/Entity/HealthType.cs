using MongoDB.Bson.Serialization.Attributes;

namespace WebPhongKham.Models.Entity
{
    public class HealthType
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public float Price { get; set; }
        public bool Deletable { get; set; }
    }
}
