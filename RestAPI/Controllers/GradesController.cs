using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using MongoDB.Bson;
using RestAPI.Models;
using Link = RestAPI.Controllers.Links.Link;

namespace RestAPI.Controllers
{
    public class GradesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Grade> GetAllGrades()
        {
            return Repository.Repository.GetAllGrades().Select(g =>
            {
                var student = Repository.Repository.GetStudent(g.StudentIndex.Id.ToString());
                var course = Repository.Repository.GetAllCourses().FirstOrDefault(c => c.GradesId.Select(grade => grade.Id.ToString()).Contains(g.Id));
                if (student != null && course != null)
                    g.Links = CreateLinks(g, student, course);
                return g;
            });
        }

        [HttpGet]
        public IHttpActionResult GetGrade(string id)
        {
            var grade = Repository.Repository.GetGrade(id);
            if (grade == null)
                return NotFound();
            var student = Repository.Repository.GetStudent(grade.StudentIndex.Id.ToString());
            var course = Repository.Repository.GetAllCourses().FirstOrDefault(c => c.GradesId.Select(g => g.Id.ToString()).Contains(grade.Id));
            if (student == null || course == null)
                return NotFound();
            grade.Links = CreateLinks(grade, student, course);
            return Ok(grade);
        }

        [HttpPost]
        public IHttpActionResult CreateGrade(Grade grade)
        {
            if (Repository.Repository.GradeExists(grade.Id))
                return Conflict();
            if (!  Repository.Repository.StudentExists(grade.StudentIndex.Id.ToString()) || !GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.Repository.InsertGrade(grade);
            return Created(Url.Link("DefaultApi", new {id = grade.Id}), grade);
        }

        [HttpPut]
        public IHttpActionResult UpdateGrade(string id, Grade grade)
        {
            if (!Repository.Repository.GradeExists(id))
            {
                grade.Id = id;
                return CreateGrade(grade);
            }
            if (!Repository.Repository.GradeExists(grade.StudentIndex.Id.ToString()) || !GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            return Repository.Repository.UpdateGrade(id, grade)
                ? (IHttpActionResult) Ok(grade)
                : StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteGrade(string id)
        {
            if (!Repository.Repository.GradeExists(id))
                return NotFound();

            return StatusCode(Repository.Repository.DeleteGrade(id) ? HttpStatusCode.Accepted : HttpStatusCode.NoContent);
        }

        private bool GradeValidation(double grade) =>
            Regex.IsMatch(grade.ToString(CultureInfo.InvariantCulture), @"[2-5](\.[05])?") && grade != 2.5 && grade != 5.5;

        private IEnumerable<Link> CreateLinks(Grade grade, Student student, Course course)
        {
            return new []
            {
                new Link("self", $"/api/grades/{grade.Id}"),
                new Link("parent", "/api/grades"),
                new Link("related", $"/api/students/{student.Index}"),
                new Link("related", $"/api/courses/{course.Id}")
            };
        }
    }
}