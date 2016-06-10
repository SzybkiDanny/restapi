using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;
using RestAPI.Converters;

namespace RestAPI.Models
{
    public class Student : IEntity
    {
        [JsonIgnore]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Id")]
        public int Index { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDate { get; set; }
    }
}
