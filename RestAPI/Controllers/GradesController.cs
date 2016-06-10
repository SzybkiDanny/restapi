using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using RestAPI.Models;
using RestAPI.Repo;

namespace RestAPI.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:63049", headers: "*", methods: "*")]
    public class GradesController : ApiController
    {
        [EnableQuery]
        [HttpGet]
        public IQueryable<Grade> GetAllGrades()
        {
            return Repository.GetAllGrades().AsQueryable();
        }

        [HttpGet]
        public IHttpActionResult GetGrade(string id)
        {
            var grade = Repository.GetGrade(id);
            if (grade == null)
                return NotFound();
            return Ok(grade);
        }

        [HttpPost]
        public IHttpActionResult CreateGrade(Grade grade)
        {
            if (Repository.GradeExists(grade.Id))
                return Conflict();
            if (!Repository.StudentExists(grade.Student) || !Repository.CourseExists(grade.Course) || !GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.InsertGrade(grade);
            return Created(Url.Link("DefaultApi", new {id = grade.Id}), grade);
        }

        [HttpPut]
        public IHttpActionResult UpdateGrade(string id, Grade grade)
        {
            if (!Repository.GradeExists(id))
            {
                grade.Id = id;
                return CreateGrade(grade);
            }
            if (!Repository.StudentExists(grade.Student) || !Repository.CourseExists(grade.Course) || !GradeValidation(grade.Value))
                return StatusCode(HttpStatusCode.Forbidden);

            Repository.UpdateGrade(grade);
            return Ok(grade);
        }

        [HttpDelete]
        public IHttpActionResult DeleteGrade(string id)
        {
            if (!Repository.GradeExists(id))
                return NotFound();

            Repository.DeleteGrade(id);
            return StatusCode(HttpStatusCode.Accepted);
        }

        private bool GradeValidation(double grade) =>
            Regex.IsMatch(grade.ToString(CultureInfo.InvariantCulture), @"[2-5](\.[05])?") && grade != 2.5 && grade != 5.5;
    }
}