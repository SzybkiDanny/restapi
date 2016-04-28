using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    public class CoursesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Course> GetAllCourses()
        {
            return Repository.Repository.Courses;
        }

        [HttpGet]
        public IHttpActionResult GetCourse(int id)
        {
            var course = Repository.Repository.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();
            //course.Links = CreateLinks(course);
            return Ok(course);
        }

        [HttpPost]
        public IHttpActionResult CreateCourse(Course course)
        {
            course.Id = Repository.Repository.Courses.Count;
            while (Repository.Repository.Courses.Any(s => s.Id == course.Id))
                course.Id++;
            if (Repository.Repository.Courses.Any(c => c.Id == course.Id))
                return Conflict();
            if (!course.GradesId.All(gradeId => Repository.Repository.Grades.Any(g => g.Id == gradeId)))
                return StatusCode(HttpStatusCode.Forbidden);

            //course.Links = CreateLinks(course);
            Repository.Repository.Courses.Add(course);
            return Created(Url.Link("DefaultApi", new { id = course.Id }), course);
        }

        [HttpPut]
        public IHttpActionResult UpdateCourse(int id, Course course)
        {
            var index = Repository.Repository.Courses.FindIndex(c => c.Id == id);
            if (index == -1)
            {
                course.Id = id;
                //Repository.Repository.Courses.Add(course);
                //return Created(Url.Link("DefaultApi", new { id = course.Id }), course);
                return CreateCourse(course);
            }
            if (!course.GradesId.All(gradeId => Repository.Repository.Grades.Any(g => g.Id == gradeId)))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.Repository.Courses.RemoveAt(index);
            Repository.Repository.Courses.Add(course);
            return Ok(course);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCourse(int id)
        {
            var index = Repository.Repository.Courses.FindIndex(c => c.Id == id);
            if (index == -1)
                return NotFound();

            Repository.Repository.Courses.RemoveAt(index);
            return StatusCode(HttpStatusCode.Accepted);
        }

        private IEnumerable<Link> CreateLinks(Course course)
        {
            return new[]
            {
                new Link
                {
                    Method = "GET",
                    Rel = "self",
                    Href = Url.Link("GetById", new {id = course.Id})
                },
                new Link
                {
                    Method = "GET",
                    Rel = "parent",
                    Href = Url.Link("GetAll", null)
                },
            };
        }
    }
}
