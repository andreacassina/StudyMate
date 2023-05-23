using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;
using System.IO;

namespace CourseLessons.Controllers
{
    public class CourseController : ControllerBase
    {
        [HttpGet("GetCourses")]
        public IEnumerable<Course> Get(string degreeCourse)
        {
            string filePath = @"Courses\" + degreeCourse + @"\_List.csv";

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Course>().ToList();
                return records;
            }
        }
    }
}
