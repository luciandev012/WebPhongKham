using MongoDB.Bson.Serialization.Attributes;

namespace WebPhongKham.Models.Entity
{
    public class Patient
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string IdentityCode { get; set; }

        public string DigitalInfo { get; set; }

        public DateTime DoB { get; set; }

        public DateTime DoE { get; set; }

        public string HealthType { get; set; }

        public string ExamObject { get; set; }
        
        public bool IsPaid { get; set; }

        public bool IsTest { get; set; }

        public bool IsXray { get; set; }
        public bool IsDoneTest { get; set; } = false;
        public bool IsDoneXray { get; set; } = false;
        //public float Total { get; set; }
    }
}
