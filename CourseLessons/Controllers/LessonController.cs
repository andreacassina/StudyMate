using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CourseLessons.Controllers
{
    public class LessonController : ControllerBase
    {

        [HttpGet("GetLessons")]
        //[HttpGet]
        public ActionResult<IEnumerable<Lesson>> Get(string degreeCourse, string courseName)
        {
            try
            {   
                string filePath = @"Courses\" + degreeCourse + @"\" + courseName + ".csv";
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Lesson>().ToList();
                    return records;
                }
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


        }

        
    }
}
