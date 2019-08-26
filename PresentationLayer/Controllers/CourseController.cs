using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using BusinessLayer;
using CoreLayer;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class CourseController : Controller
    {
        //
        // GET: /Course/
        CourseService courseService = new CourseService();
        DepartmentService departmentService = new DepartmentService();
        LevelService levelService = new LevelService();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_dsc" : "name_asc";
            ViewBag.deptSortParm = sortOrder == "department_asc" ? "department_dsc" : "department_asc";
            ViewBag.levelSortParm = sortOrder == "level_asc" ? "level_dsc" : "level_asc";
            ViewBag.descSortParm = sortOrder == "desc_asc" ? "desc_dsc" : "desc_asc";
            ViewBag.durationSortParm = sortOrder == "duration_asc" ? "duration_dsc" : "duration_asc";
            ViewBag.creditSortParm = sortOrder == "credit_asc" ? "credit_dsc" : "credit_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.currentFilter = searchString;
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var Courses = courseService.GetAllCourses(sortOrder, searchString);
            var departments = departmentService.GetAllDepartments(sortOrder, searchString);
            var levels = levelService.GetAllLevels(sortOrder, searchString);
            
            var CourseDetails=(from course in Courses join department in departments 
                              on course.DepartmentId equals department.DepartmentId
                              join level in levels
                              on course.LevelId equals level.LevelId
                              select new CourseViewModel()
                              {
                                CourseName = course.CourseName,
                                Department = department.DepartmentName,
                                Level = level.LevelId,
                                Description = course.Description,
                                Duration = course.Duration,
                                Credit = course.Credit
                              }).ToList();
            List<CourseViewModel> finalCourseList;
            if (!string.IsNullOrEmpty(searchString))
            {
                finalCourseList = (from courseDetail in CourseDetails
                                   where courseDetail.CourseName == searchString || courseDetail.Department == searchString
                                           || courseDetail.Description == searchString || courseDetail.Duration == searchString
                                           || courseDetail.Credit == searchString
                                   select courseDetail).ToList();
            }
            else
            {
                finalCourseList = CourseDetails;
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "name_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.CourseName).ToList();
                        break;
                    case "name_desc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.CourseName).ToList();
                        break;
                    case "description_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.Description).ToList();
                        break;
                    case "description_dsc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.Description).ToList();
                        break;
                    case "department_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.Department).ToList();
                        break;
                    case "department_dsc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.Department).ToList();
                        break;
                    case "level_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.Level).ToList();
                        break;
                    case "level_dsc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.Level).ToList();
                        break;
                    case "duration_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.Duration).ToList();
                        break;
                    case "duration_dsc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.Duration).ToList();
                        break;
                    case "credit_asc":
                        finalCourseList = finalCourseList.OrderBy(x => x.Credit).ToList();
                        break;
                    case "credit_dsc":
                        finalCourseList = finalCourseList.OrderByDescending(x => x.Credit).ToList();
                        break;
                }
            }
            return View(finalCourseList.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CreateNewCourse()
        {
            List<Department> departments = departmentService.GetAllDepartments("", "");
            List<String> departmentNames = (from department in departments select department.DepartmentName).ToList();
            foreach (var department in departments)
            {
                departmentNames.Add(department.DepartmentName);
            }
            ViewBag.departmentNames = departmentNames;

            IList<Level> levels = levelService.GetAllLevels("", "");
            List<int> levelID = (from level in levels select level.LevelId).ToList();
            foreach (var level in levels)
            {
                levelID.Add(level.LevelId);
            }
            ViewBag.levelID = levelID;
            return View();

        }
        [HttpPost]
        public ActionResult SaveCourse(CourseViewModel courseData)
        {

            courseService = new CourseService();
            Department department = departmentService.GetDepartmentByName(courseData.Department);
            Level level = levelService.GetLevelById(courseData.Level);
            

            Course Course = new Course()
            {
                CourseName = courseData.CourseName,
                DepartmentId = department.DepartmentId,
                LevelId= level.LevelId,
                Description = courseData.Description,
                Credit = courseData.Credit,
                Duration = courseData.Duration,
                InsertedDate = DateTime.Now,
            };
            courseService.SaveCourse(Course);
            return View(courseData);
        }


        public ActionResult UpdateCourse(CourseViewModel newCourseData)
        {
            Department department = departmentService.GetDepartmentByName(newCourseData.Department);
            Level level = levelService.GetLevelById(newCourseData.Level);
            

            Course course = new Course()
            {
                CourseId = newCourseData.CourseId,
                CourseName = newCourseData.CourseName,
                DepartmentId = department.DepartmentId,
                Description = newCourseData.Description,
                Credit = newCourseData.Credit,
                LevelId = level.LevelId,
                Duration = newCourseData.Duration,
                InsertedDate= DateTime.Now
            };
            courseService.UpdateCourse(course);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCourse(int CourseId)
        {
            try
            {
                courseService.DeleteCourse(CourseId);
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int CourseId)
        {
            Course course = courseService.GetCourseById(CourseId);
            CourseViewModel courseViewModel = new CourseViewModel()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                //Department = course.DepartmentId,
                Description = course.Description,
                Credit = course.Credit,
                //Level= course.LevelId,
                Duration= course.Duration
            };
            return View(courseViewModel);
        }

    }
}
