using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CourseLessons.Controllers
{
    public class LessonController : ControllerBase
    {

        [HttpGet("GetLessons")]
        //[HttpGet]
        public IEnumerable<Lesson> Get(string degreeCourse, string courseName)
        {
            //D:\Projects\StudyMate1\CourseLessons\Courses\
            string filePath = @"Courses\" + degreeCourse + @"\" + courseName + ".csv";
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Lesson>().ToList();
                return records;
            }


        }

        
    }
}
