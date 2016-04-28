using System;
using System.Collections.Generic;
using System.Linq;
using RestAPI.Models;

namespace RestAPI.Repository
{
    public static class Repository
    {
        public static List<Student> Students { get; } = new List<Student>
        {
            new Student
            {
                Id = 1,
                Index = 100012,
                FirstName = "Adam",
                SecondName = "Niezgódka",
                BirthDate = DateTime.Now.AddYears(-20)
            },
            new Student
            {
                Id = 2,
                Index = 100026,
                FirstName = "Dade",
                SecondName = "Murphy",
                BirthDate = DateTime.Now.AddYears(-21)
            },
            new Student
            {
                Id = 3,
                Index = 100187,
                FirstName = "Van",
                SecondName = "Wilder",
                BirthDate = DateTime.Now.AddYears(-19)
            }
        };

        public static List<Grade> Grades { get; } = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                Issued = DateTime.Now.AddMonths(-1),
                StudentId = Students.ToArray()[0].Id,
                Value = 4.5
            },
            new Grade
            {
                Id = 2,
                Issued = DateTime.Now.AddMonths(-2),
                StudentId = Students.ToArray()[1].Id,
                Value = 5
            },
            new Grade
            {
                Id = 3,
                Issued = DateTime.Now.AddMonths(-3),
                StudentId = Students.ToArray()[2].Id,
                Value = 4
            },
            new Grade
            {
                Id = 4,
                Issued = DateTime.Now.AddMonths(-3),
                StudentId = Students.ToArray()[2].Id,
                Value = 5
            }
        };

        public static List<Course> Courses { get; } = new List<Course>
        {
            new Course
            {
                GradesId = new List<int>
                {
                    Grades.ToArray()[0].Id,
                    Grades.ToArray()[1].Id
                },
                Id = 10,
                Lecturer = "Jon Skeet",
                Name = "Advanced Programming"
            },
            new Course
            {
                GradesId = new List<int>
                {
                    Grades.ToArray()[2].Id,
                    Grades.ToArray()[3].Id
                },
                Id = 15,
                Lecturer = "Elliot Alderson",
                Name = "Advanced Security"
            }
        };
    }
}