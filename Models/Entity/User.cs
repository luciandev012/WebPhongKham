﻿using MongoDB.Bson.Serialization.Attributes;

namespace WebPhongKham.Models.Entity
{
    public class User
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public bool Status { get; set; } = true;
    }
}
