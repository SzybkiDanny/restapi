using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using RestAPI.Models;
using RestAPI.Repo;

namespace RestAPI.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        [EnableQuery]
        [HttpGet]
        public IQueryable<Student> GetAllStudents()
        {
            return Repository.GetAllStudents().AsQueryable();
        }

        [HttpGet]
        public IHttpActionResult GetStudent(int id)
        {
            var student = Repository.GetStudent(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public IHttpActionResult CreateStudent(Student student)
        {
            if (Repository.StudentExists(student.Index))
                return Conflict();

            Repository.InsertStudent(student);
            return Created(Url.Link("DefaultApi", new { id = student.Id }), student);
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(int id, Student student)
        {
            if (!Repository.StudentExists(id))
            {
                student.Index = id;
                return CreateStudent(student);
            }
            student.Id = Repository.GetStudent(id).Id;
            Repository.UpdateStudent(student);
            return Ok(student);
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            if (!Repository.StudentExists(id))
                return NotFound();
            Repository.DeleteStudent(id);
            return StatusCode(HttpStatusCode.Accepted);
        }

        [EnableQuery]
        [HttpGet, Route("{id}/grades")]
        public IQueryable<Grade> GetStudentGrades(int id)
        {
            return Repository.GetStudentsGrades(id).AsQueryable();
        }

        [EnableQuery]
        [HttpGet, Route("{id}/courses/{courseId}/grades")]
        public IQueryable<Grade> GetStudentsGradesByCourse(int id, string courseId)
        {
            return Repository.GetStudentsGradesByCourse(id, courseId).AsQueryable();
        }
    }
}
