using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo("LMSControllerTests")]
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var query = from e in db.EnrolledIns
                        where e.Class.CourseDeptAbbreviation == subject &&
                              e.Class.CourseNum == (uint)num &&
                              e.Class.Season == season &&
                              e.Class.Year == (ushort)year
                        select new
                        {
                            fname = e.Student.FirstName,
                            lname = e.Student.LastName,
                            uid = e.StudentId,
                            dob = e.Student.Dob,
                            grade = e.Grade == "" || e.Grade == null ? "--" : e.Grade
                        };

            return Json(query.ToArray());
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            var query = from cl in db.Classes
                        where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                                cl.Season == season && cl.Year == (ushort)year
                        join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                        where category == null || ac.CategoryName == category
                        join a in db.Assignments on ac.CategoryId equals a.AssignmentCategoryId
                        select new
                        {
                            aname = a.AssignmentName,
                            cname = ac.CategoryName,
                            due = a.DueDate,
                            submissions = a.Submitteds.Count()
                        };

            return Json(query.ToArray());
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from cl in db.Classes
                        where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                                cl.Season == season && cl.Year == (ushort)year
                        join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                        select new
                        {
                            name = ac.CategoryName,
                            weight = ac.GradeWeight
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var classQuery = from cl in db.Classes
                             where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                                 cl.Season == season && cl.Year == (ushort)year
                             select cl.ClassId;

            int cid = classQuery.FirstOrDefault();
            if (cid == 0) return Json(new { success = false });

            if (db.AssignmentCategories.Any(ac => ac.ClassId == cid && ac.CategoryName == category))
                return Json(new { success = false });

            AssignmentCategory newCat = new AssignmentCategory
            {
                ClassId = cid,
                CategoryName = category,
                GradeWeight = (ushort)catweight
            };

            db.AssignmentCategories.Add(newCat);
            db.SaveChanges();
            return Json(new { success = true });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            var catQuery = from cl in db.Classes
                           where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                               cl.Season == season && cl.Year == (ushort)year
                           join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                           where ac.CategoryName == category
                           select ac.CategoryId;

            int catId = catQuery.FirstOrDefault();
            if (catId == 0) return Json(new { success = false });

            Assignment asg = new Assignment
            {
                AssignmentCategoryId = catId,
                AssignmentName = asgname,
                MaxPoints = (uint)asgpoints,
                Contents = asgcontents,
                DueDate = asgdue
            };

            db.Assignments.Add(asg);
            db.SaveChanges();
            var classId = db.AssignmentCategories
                .Where(ac => ac.CategoryId == catId)
                .Select(ac => ac.ClassId)
                .FirstOrDefault();

            UpdateAllStudentGrades(classId);
            return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from cl in db.Classes
                        where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                                cl.Season == season && cl.Year == (ushort)year
                        join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                        where ac.CategoryName == category
                        join a in db.Assignments on ac.CategoryId equals a.AssignmentCategoryId
                        where a.AssignmentName == asgname
                        join s in db.Submitteds on a.AssignmentId equals s.AssignmentId
                        select new
                        {
                            fname = s.Student.FirstName,
                            lname = s.Student.LastName,
                            uid = s.StudentId,
                            time = s.SubmissionTime,
                            score = s.Score
                        };

            return Json(query.ToArray());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            var subQuery = from cl in db.Classes
                           where cl.CourseDeptAbbreviation == subject && cl.CourseNum == (uint)num &&
                               cl.Season == season && cl.Year == (ushort)year
                           join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                           where ac.CategoryName == category
                           join a in db.Assignments on ac.CategoryId equals a.AssignmentCategoryId
                           where a.AssignmentName == asgname
                           join s in db.Submitteds on a.AssignmentId equals s.AssignmentId
                           where s.StudentId == uid
                           select s;

            var submission = subQuery.FirstOrDefault();
            if (submission == null) return Json(new { success = false });

            submission.Score = (uint)score;
            db.SaveChanges();
            var classId = db.Classes
                .Where(c => c.CourseDeptAbbreviation == subject &&
                            c.CourseNum == (uint)num &&
                            c.Season == season &&
                            c.Year == (ushort)year)
                .Select(c => c.ClassId)
                .FirstOrDefault();

            UpdateSingleStudentGrade(classId, uid);
            return Json(new { success = true });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from cl in db.Classes
                        where cl.ProfessorId == uid
                        join co in db.Courses on new { A = cl.CourseNum, B = cl.CourseDeptAbbreviation }
                                         equals new { A = co.CourseNum, B = co.CourseDeptAbbreviation }
                        select new
                        {
                            subject = cl.CourseDeptAbbreviation,
                            number = cl.CourseNum,
                            name = co.Name,
                            season = cl.Season,
                            year = cl.Year
                        };

            return Json(query.ToArray());
        }

        private string CalculateClassGrade(string uid, int classId)
        {
            var categories = db.AssignmentCategories
                .Where(c => c.ClassId == classId)
                .ToList();

            double totalWeightedScore = 0;
            double totalWeights = 0;

            foreach (var cat in categories)
            {
                var assignments = db.Assignments
                    .Where(a => a.AssignmentCategoryId == cat.CategoryId)
                    .ToList();

                if (assignments.Count == 0)
                    continue;

                double earned = 0;
                double possible = 0;

                foreach (var a in assignments)
                {
                    var submission = db.Submitteds
                        .FirstOrDefault(s => s.AssignmentId == a.AssignmentId && s.StudentId == uid);

                    double score = submission?.Score ?? 0;

                    earned += score;
                    possible += a.MaxPoints;
                }

                if (possible == 0)
                    continue;

                double categoryPercent = earned / possible;

                totalWeightedScore += categoryPercent * cat.GradeWeight;
                totalWeights += cat.GradeWeight;
            }

            if (totalWeights == 0)
                return "--";

            double finalPercent = (totalWeightedScore / totalWeights) * 100;

            return ConvertToLetterGrade(finalPercent);
        }

        //helper to update all student grades 
        private void UpdateAllStudentGrades(int classId)
        {
            var students = db.EnrolledIns
                .Where(e => e.ClassId == classId)
                .ToList();

            foreach (var s in students)
            {
                s.Grade = CalculateClassGrade(s.StudentId, classId);
            }

            db.SaveChanges();
        }

        //helper to update single student grade 
        private void UpdateSingleStudentGrade(int classId, string uid)
        {
            var enrollment = db.EnrolledIns
                .FirstOrDefault(e => e.ClassId == classId && e.StudentId == uid);

            if (enrollment == null)
                return;

            enrollment.Grade = CalculateClassGrade(uid, classId);
            db.SaveChanges();
        }

        //Helper to convert grade to letter, check that this is correct on the scale 
        private string ConvertToLetterGrade(double percent)
        {
            if (percent >= 93) return "A";
            if (percent >= 90) return "A-";
            if (percent >= 87) return "B+";
            if (percent >= 83) return "B";
            if (percent >= 80) return "B-";
            if (percent >= 77) return "C+";
            if (percent >= 73) return "C";
            if (percent >= 70) return "C-";
            if (percent >= 67) return "D+";
            if (percent >= 63) return "D";
            if (percent >= 60) return "D-";
            return "E";
        }
        /*******End code to modify********/
    }
}

