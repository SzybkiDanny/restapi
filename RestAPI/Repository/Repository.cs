using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MongoDB.Bson;
using MongoDB.Driver;
using RestAPI.Models;

namespace RestAPI.Repository
{
    public static class Repository
    {
        private static readonly string ConnectionHost = "localhost";
        private static readonly int ConnectionPort = 8004;
        private static readonly MongoClient _client = new MongoClient($"mongodb://{ConnectionHost}:{ConnectionPort}");
        private static readonly IMongoDatabase _db;
        private static readonly IMongoCollection<Grade> _grades;
        private static readonly IMongoCollection<Course> _courses;
        private static readonly IMongoCollection<Student> _students;

        static Repository()
        {
            _db = _client.GetDatabase("university");
            _grades = _db.GetCollection<Grade>("grades");
            _courses = _db.GetCollection<Course>("courses");
            _students = _db.GetCollection<Student>("students");
        }

        public static ICollection<Grade> GetAllGrades()
        {
            return _grades.FindSync(new BsonDocument()).ToList();
        }

        public static ICollection<Student> GetAllStudents()
        {
            return _students.FindSync(new BsonDocument()).ToList();
        }

        public static ICollection<Course> GetAllCourses()
        {
            return _courses.FindSync(new BsonDocument()).ToList();
        }

        public static Grade GetGrade(string id)
        {
            return _grades.FindSync(Builders<Grade>.Filter.Eq("Id", id)).First();
        }

        public static Student GetStudent(string index)
        {
            return _students.FindSync(Builders<Student>.Filter.Eq("Index", index)).First();
        }

        public static Course GetCourse(string id)
        {
            return _courses.FindSync(Builders<Course>.Filter.Eq("Id", id)).First();
        }

        public static bool GradeExists(string id)
        {
            try
            {
                return _grades.FindSync(Builders<Grade>.Filter.Eq("Id", id)).First() != null;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static bool StudentExists(string index)
        {
            try
            {
                return _students.FindSync(Builders<Student>.Filter.Eq("Index", index)).First() != null;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static bool CourseExists(string id)
        {
            try
            {
                return _courses.FindSync(Builders<Course>.Filter.Eq("Id", id)).First() != null;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static void InsertGrade(Grade grade)
        {
            _grades.InsertOne(grade);
        }

        public static void InsertStudent(Student student)
        {
            _students.InsertOne(student);
        }

        public static void InsertCourse(Course course)
        {
            _courses.InsertOne(course);
        }

        public static bool UpdateGrade(string id, Grade grade)
        {
            return _grades.ReplaceOne(Builders<Grade>.Filter.Eq("Id", id), grade).IsAcknowledged;
        }

        public static bool UpdateStudent(string id, Student student)
        {
            return _students.ReplaceOne(Builders<Student>.Filter.Eq("Index", id), student).IsAcknowledged;
        }

        public static bool UpdateCourse(string id, Course course)
        {
            return _courses.ReplaceOne(Builders<Course>.Filter.Eq("Id", id), course).IsAcknowledged;
        }

        public static bool DeleteGrade(string id)
        {
            return _grades.DeleteOne(Builders<Grade>.Filter.Eq("Id", id)).IsAcknowledged;
        }

        public static bool DeleteStudent(string index)
        {
            return _students.DeleteOne(Builders<Student>.Filter.Eq("Index", index)).IsAcknowledged;
        }

        public static bool DeleteCourse(string id)
        {
            return _courses.DeleteOne(Builders<Course>.Filter.Eq("Id", id)).IsAcknowledged;
        }

        public static ICollection<Grade> GetStudentsGrades(string index)
        {
            return _grades.FindSync(new BsonDocument()).ToList().Where(g => g.StudentIndex.Id.ToString() == index).ToList();
        }

        public static ICollection<Grade> GetStudentsGradesByCourse(string index, string courseId)
        {
            var grades = GetStudentsGrades(index);
            var course = GetCourse(courseId);
            return grades.Where(g => course.GradesId.Exists(grade => grade.Id.ToString() == g.Id)).ToList();
        }
    }
}