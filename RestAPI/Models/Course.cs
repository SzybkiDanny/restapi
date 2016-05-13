using MongoRepository;

namespace RestAPI.Models
{
    public class Course : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
    }
}
