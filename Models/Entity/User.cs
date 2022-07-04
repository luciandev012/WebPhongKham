using MongoDB.Bson.Serialization.Attributes;

namespace WebPhongKham.Models.Entity
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; } = true;
    }
}
