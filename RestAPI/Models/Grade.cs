using System;

namespace RestAPI.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int StudentId { get; set; }
        public DateTime Issued { get; set; }
    }
}
