using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        [HttpGet]
        public IEnumerable<Student> GetAllStudents()
        {
            return Repository.Repository.Students;
        }

        [HttpGet]
        public IHttpActionResult GetStudent(int id)
        {
            var student = Repository.Repository.Students.FirstOrDefault(s => s.Id == id);
            return student == null ? (IHttpActionResult) NotFound() : Ok(student);
        }

        [HttpPost]
        public IHttpActionResult CreateStudent(Student student)
        {
            student.Id = Repository.Repository.Students.Count;
            while (Repository.Repository.Students.Any(s => s.Id == student.Id))
                student.Id++;
            if (Repository.Repository.Students.Any(s => s.Id == student.Id || s.Index == student.Index))
                return Conflict();

            Repository.Repository.Students.Add(student);
            return Created(Url.Link("DefaultApi", new { id = student.Id }), student);
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(int id, Student student)
        {       
            var index = Repository.Repository.Students.FindIndex(s => s.Id == id);
            if (index == -1)
            {
                if (Repository.Repository.Students.Any(s => s.Index == student.Index))
                    return Conflict();
                student.Id = id;
                Repository.Repository.Students.Add(student);
                return Created(Url.Link("DefaultApi", new { id = student.Id }), student);
            }

            Repository.Repository.Students.RemoveAt(index);
            Repository.Repository.Students.Add(student);
            return Ok(student);
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            var index = Repository.Repository.Students.FindIndex(s => s.Id == id);
            if (index == -1)
                return NotFound();

            Repository.Repository.Students.RemoveAt(index);
            return StatusCode(HttpStatusCode.Accepted);
        }

        [HttpGet, Route("{id:int}/grades")]
        public IHttpActionResult GetStudentGrades(int id)
        {
            if (Repository.Repository.Students.FirstOrDefault(s => s.Id == id) == null)
                return NotFound();
            var grades = Repository.Repository.Grades.Where(g => g.StudentId == id);
            return Ok(grades);
        }
    }
}
