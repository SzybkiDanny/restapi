using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using RestAPI.Models;
using Tavis.UriTemplates;
using Link = RestAPI.Controllers.Links.Link;

namespace RestAPI.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        [EnableQuery]
        [HttpGet]
        public IQueryable<Student> GetAllStudents()
        {
            return Repository.Repository.GetAllStudents().Select(s =>
            {
                s.Links = CreateLinks(s);
                return s;
            }).AsQueryable();
        }

        [HttpGet]
        public IHttpActionResult GetStudent(string id)
        {
            var student = Repository.Repository.GetStudent(id);
            if (student == null)
                return NotFound();
            student.Links = CreateLinks(student);
            return Ok(student);
        }

        [HttpPost]
        public IHttpActionResult CreateStudent(Student student)
        {
            if (Repository.Repository.StudentExists(student.Index))
                return Conflict();

            Repository.Repository.InsertStudent( student);
            return Created(Url.Link("DefaultApi", new { id = student.Index }), student);
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(string id, Student student)
        {
            if (!Repository.Repository.StudentExists(id))
            {
                student.Index = id;
                return CreateStudent(student);
            }

            return Repository.Repository.UpdateStudent(id, student)
                ? (IHttpActionResult) Ok(student)
                : StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(string id)
        {
            if (!Repository.Repository.StudentExists(id))
                return NotFound();

            return StatusCode(Repository.Repository.DeleteStudent(id) ? HttpStatusCode.Accepted : HttpStatusCode.NoContent);
        }

        [EnableQuery]
        [HttpGet, Route("{id}/grades")]
        public IQueryable<Grade> GetStudentGrades(string id)
        {
            return Repository.Repository.GetStudentsGrades(id).AsQueryable();
        }

        [EnableQuery]
        [HttpGet, Route("{id}/courses/{courseId}/grades")]
        public IQueryable<Grade> GetStudentsGradesByCourse(string id, string courseId)
        {
            return Repository.Repository.GetStudentsGradesByCourse(id, courseId).AsQueryable();
        }

        private IEnumerable<Link> CreateLinks(Student student)
        {
            return new []
            {
                new Link("self", $"/api/students/{student.Index}"),
                new Link("parent", "/api/students"),
                new Link("related", "grades", $"/api/students/{student.Index}/grades"), 
            };
        }
    }
}
