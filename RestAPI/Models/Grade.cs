using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestAPI.Models
{
    public class Grade : Resource
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public double Value { get; set; }
        public MongoDBRef StudentIndex { get; set; }
        public DateTime Issued { get; set; }
    }
}