using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Link = RestAPI.Controllers.Links.Link;


namespace RestAPI.Models
{
    public abstract class Resource
    {
        [BsonIgnore]
        public IEnumerable<Link> Links { get; set; } = new List<Link>();
    }
}
