using System;
using System.Collections.Generic;
using System.Linq;
using MongoRepository;
using RestAPI.Models;

namespace RestAPI.Repo
{
    public static class Repository
    {
        private static readonly MongoRepository<Grade> Grades = new MongoRepository<Grade>();
        private static readonly MongoRepository<Course> Courses = new MongoRepository<Course>();
        private static readonly MongoRepository<Student> Students = new MongoRepository<Student>();

        static Repository()
        {
            var c1 = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Lecturer = "Jon Skeet",
                Name = "Advanced Programming"
            };
            var c2 = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Lecturer = "Elliot Alderson",
                Name = "Advanced Security"
            };

            Courses.Add(new[]
            {
                c1, c2
            });

            var s1 = new Student
            {
                Id = Guid.NewGuid().ToString(),
                Index = 100012,
                FirstName = "Adam",
                SecondName = "Niezgódka",
                BirthDate = DateTime.Now.Date.AddYears(-20),
            };
            var s2 = new Student
            {
                Id = Guid.NewGuid().ToString(),
                Index = 100026,
                FirstName = "Dade",
                SecondName = "Murphy",
                BirthDate = DateTime.Now.Date.AddYears(-21),
            };
            var s3 = new Student
            {
                Id = Guid.NewGuid().ToString(),
                Index = 100187,
                FirstName = "Van",
                SecondName = "Wilder",
                BirthDate = DateTime.Now.Date.AddYears(-19),
            };

            Students.Add(new[]
            {
                s1, s2, s3
            });

            var g1 = new Grade
            {
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now.AddMonths(-1),
                Student = s1.Index,
                Value = 4.5,
                Course = c1.Id
            };
            var g2 = new Grade
            {
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now.AddMonths(-2),
                Student = s2.Index,
                Value = 5,
                Course = c1.Id
            };
            var g3 = new Grade
            {
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now.AddMonths(-3),
                Student = s3.Index,
                Value = 4,
                Course = c2.Id
            };
            var g4 = new Grade
            {
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now.AddMonths(-3),
                Student = s3.Index,
                Value = 5,
                Course = c1.Id
            };

            Grades.Add(new[]
            {
                g1, g2, g3, g4
            });

            var manager = new MongoRepositoryManager<Student>();
            manager.EnsureIndex("Index", false, true, false);
        }

        public static ICollection<Grade> GetAllGrades()
        {
            return Grades.ToList();
        }

        public static ICollection<Student> GetAllStudents()
        {
            return Students.ToList();
        }

        public static ICollection<Course> GetAllCourses()
        {
            return Courses.ToList();
        }

        public static Grade GetGrade(string id)
        {
            return Grades.GetById(id);
        }

        public static Student GetStudent(int id)
        {
            return Students.FirstOrDefault(s => s.Index == id);
        }

        public static Course GetCourse(string id)
        {
            return Courses.GetById(id);
        }

        public static bool GradeExists(string id)
        {
            return Grades.Exists(g => g.Id == id);
        }

        public static bool StudentExists(int id)
        {
            return Students.Exists(s => s.Index == id);
        }

        public static bool CourseExists(string id)
        {
            return Courses.Exists(c => c.Id == id);
        }

        public static void InsertGrade(Grade grade)
        {
            grade.Id = Guid.NewGuid().ToString();
            try
            {
                Grades.Add(grade);
            }
            catch
            {
                // ignored
            }
        }

        public static void InsertStudent(Student student)
        {
            student.Id = Guid.NewGuid().ToString();
            student.Index = Students.Max(s => s.Index) + 1;
            try
            {
                Students.Add(student);
            }
            catch
            {
                // ignored
            }
        }

        public static void InsertCourse(Course course)
        {
            course.Id = Guid.NewGuid().ToString();
            try
            {
                Courses.Add(course);
            }
            catch
            {
                // ignored
            }
        }

        public static void UpdateGrade(Grade grade)
        {
            Grades.Update(grade);
        }

        public static void UpdateStudent(Student student)
        {
            Students.Update(student);
        }

        public static void UpdateCourse(Course course)
        {
            Courses.Update(course);
        }

        public static void DeleteGrade(string id)
        {
            Grades.Delete(id);
        }

        public static void DeleteStudent(int id)
        {
            Students.Delete(s => s.Index == id);
        }

        public static void DeleteCourse(string id)
        {
            Courses.Delete(id);
        }

        public static ICollection<Grade> GetStudentsGrades(int id)
        {
            var student = GetStudent(id);
            return student == null ? null : Grades.Where(g => g.Student == student.Index).ToList();
        }

        public static ICollection<Grade> GetStudentsGradesByCourse(int id, string courseId)
        {
            var grades = GetStudentsGrades(id);
            return grades?.Where(g => g.Student == id && g.Course == courseId).ToList();
        }
    }
}