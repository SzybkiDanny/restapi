using System;
using MongoRepository;

namespace RestAPI.Models
{
    public class Grade : IEntity
    {
        public string Id { get; set; }
        public double Value { get; set; }
        public int Student { get; set; }
        public DateTime Issued { get; set; }
        public string Course { get; set; }
    }
}