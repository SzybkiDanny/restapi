using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;
using RestAPI.Converters;

namespace RestAPI.Models
{
    public class Grade : IEntity
    {
        public string Id { get; set; }
        public double Value { get; set; }
        public int Student { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Issued { get; set; }
        public string Course { get; set; }
    }
}