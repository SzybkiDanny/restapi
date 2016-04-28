using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    public class GradesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Grade> GetAllGrades()
        {
            return Repository.Repository.Grades;
        }

        [HttpGet]
        public IHttpActionResult GetGrade(int id)
        {
            var grade = Repository.Repository.Grades.FirstOrDefault(g => g.Id == id);
            return grade == null ? (IHttpActionResult)NotFound() : Ok(grade);
        }

        [HttpPost]
        public IHttpActionResult CreateGrade(Grade grade)
        {
            grade.Id = Repository.Repository.Grades.Count;
            while (Repository.Repository.Grades.Any(s => s.Id == grade.Id))
                grade.Id++;
            if (Repository.Repository.Grades.Any(g => g.Id == grade.Id))
                return Conflict();
            if (Repository.Repository.Students.All(s => s.Id != grade.StudentId) && GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.Repository.Grades.Add(grade);
            return Created(Url.Link("DefaultApi", new { id = grade.Id }), grade);
        }

        [HttpPut]
        public IHttpActionResult UpdateGrade(int id, Grade grade)
        {
            var index = Repository.Repository.Grades.FindIndex(g => g.Id == id);
            if (index == -1)
            {
                grade.Id = id;
                Repository.Repository.Grades.Add(grade);
                return Created(Url.Link("DefaultApi", new { id = grade.Id }), grade);
            }
            if (Repository.Repository.Students.All(s => s.Id != grade.StudentId) || !GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.Repository.Grades.RemoveAt(index);
            Repository.Repository.Grades.Add(grade);
            return Ok(grade);
        }

        [HttpDelete]
        public IHttpActionResult DeleteGrade(int id)
        {
            var index = Repository.Repository.Grades.FindIndex(g => g.Id == id);
            if (index == -1)
                return NotFound();

            Repository.Repository.Grades.RemoveAt(index);
            return StatusCode(HttpStatusCode.Accepted);
        }

        private bool GradeValidation(double grade) =>
            Regex.IsMatch(grade.ToString(), @"[2-5](\.[05])?") && grade != 2.5 && grade != 5.5;
    }
}
