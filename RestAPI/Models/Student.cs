using System;
using MongoDB.Bson.Serialization.Attributes;

namespace RestAPI.Models
{
    public class Student : Resource
    {
        [BsonId]
        public string Index { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
