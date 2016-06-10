using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using RestAPI.Models;
using RestAPI.Repo;

namespace RestAPI.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:63049", headers: "*", methods: "*")]
    [RoutePrefix("api/courses")]
    public class CoursesController : ApiController
    {
        [EnableQuery]
        [HttpGet]
        public IQueryable<Course> GetAllCourses()
        {
            return Repository.GetAllCourses().AsQueryable();
        }

        [HttpGet]
        public IHttpActionResult GetCourse(string id)
        {
            var course = Repository.GetCourse(id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public IHttpActionResult CreateCourse(Course course)
        {
            if (Repository.CourseExists(course.Id))
                return Conflict();

            Repository.InsertCourse(course);
            return Created(Url.Link("DefaultApi", new { id = course.Id }), course);
        }

        [HttpPut]
        public IHttpActionResult UpdateCourse(string id, Course course)
        {
            if (!Repository.CourseExists(id))
            {
                course.Id = id;
                return CreateCourse(course);
            }

            Repository.UpdateCourse(course);
            return Ok(course);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCourse(string id)
        {
            if (!Repository.CourseExists(id))
                return NotFound();

            Repository.DeleteCourse(id);
            return StatusCode(HttpStatusCode.Accepted);
        }
    }
}
