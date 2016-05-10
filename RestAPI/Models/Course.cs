using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestAPI.Models
{
    public class Course : Resource
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
        public List<MongoDBRef> GradesId { get; set; }
    }
}
