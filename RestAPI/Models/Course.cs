using System.Collections.Generic;
using System.Security.Policy;

namespace RestAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
        public IEnumerable<int> GradesId { get; set; }
        //public IEnumerable<Link> Links { get; set; }
    }
}
