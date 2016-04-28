using System;

namespace RestAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
