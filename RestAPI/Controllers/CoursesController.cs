using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MongoDB.Bson;
using RestAPI.Models;
using Link = RestAPI.Controllers.Links.Link;

namespace RestAPI.Controllers
{
    [RoutePrefix("api/courses")]
    public class CoursesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Course> GetAllCourses()
        {
            return Repository.Repository.GetAllCourses().Select(c =>
            {
                c.Links = CreateLinks(c);
                return c;
            });
        }

        [HttpGet]
        public IHttpActionResult GetCourse(string id)
        {
            var course = Repository.Repository.GetCourse(id);
            if (course == null)
                return NotFound();
            course.Links = CreateLinks(course);
            return Ok(course);
        }

        [HttpPost]
        public IHttpActionResult CreateCourse(Course course)
        {
            if (Repository.Repository.CourseExists(course.Id))
                return Conflict();
            if (course.GradesId.Any(gradeId => !Repository.Repository.GradeExists(gradeId.Id.ToString())))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.Repository.InsertCourse(course);
            return Created(Url.Link("DefaultApi", new { id = course.Id }), course);
        }

        [HttpPut]
        public IHttpActionResult UpdateCourse(string id, Course course)
        {
            if (!Repository.Repository.CourseExists(id))
            {
                course.Id = id;
                return CreateCourse(course);
            }
            if (!course.GradesId.All(g => Repository.Repository.GradeExists(g.Id.ToString())))
                return StatusCode(HttpStatusCode.Forbidden);

            return Repository.Repository.UpdateCourse(id, course)
                ? (IHttpActionResult) Ok(course)
                : StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCourse(string id)
        {
            if (!Repository.Repository.CourseExists(id))
                return NotFound();

            return StatusCode(Repository.Repository.DeleteCourse(id) ? HttpStatusCode.Accepted : HttpStatusCode.NoContent);
        }

        private IEnumerable<Link> CreateLinks(Course course)
        {
            return new []
            {
                new Link("self", $"/api/courses/{course.Id}"),
                new Link("parent", "/api/courses")
            };
        }
    }
}
