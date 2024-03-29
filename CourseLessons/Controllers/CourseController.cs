﻿using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;
using System.IO;

namespace CourseLessons.Controllers
{
    public class CourseController : ControllerBase
    {
        [HttpGet("GetCourses")]
        public ActionResult<IEnumerable<Course>> Get(string degreeCourse)
        {
            string filePath = @"Courses\" + degreeCourse + @"\_List.csv";

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Course>().ToList();
                    return records;
                }
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetDegreeCourses")]
        public ActionResult<IEnumerable<string>> GetDegreeCourses()
        {
            string folderPath = @"Courses";
            if (Directory.Exists(folderPath))
            {
                IEnumerable<string> folderNames = GetFolderNames(folderPath);

                return Ok(folderNames);
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
                return NotFound();
            }
        }

        private IEnumerable<string> GetFolderNames(string folderPath)
        {
            // Get all subdirectories within the given folder
            string[] subdirectories = Directory.GetDirectories(folderPath);

            // Extract and return the folder names
            foreach (string subdirectory in subdirectories)
            {
                yield return new DirectoryInfo(subdirectory).Name.ToString();
            }
        }
    }
}
