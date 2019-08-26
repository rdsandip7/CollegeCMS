using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CourseCRUD
    {
        //Create Course
        public void SaveCourse(Course course)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Courses.Add(course);
                dataContext.SaveChanges();
            }
        }

        //Read Course
        public Course GetCourseById(int courseId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from course in dataContext.Courses
                              where course.CourseId == courseId
                              select course).First();
                return result;
            }
        }
        public Course GetCourseByName(string CourseName)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from course in dataContext.Courses
                              where course.CourseName == CourseName
                              select course).First();
                return result;
            }
        }

        public List<Course> GetAllCourses(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<Course> Course = from course in dataContext.Courses select course;

                if (!string.IsNullOrEmpty(searchString))
                {
                    Course = Course.Where(x => x.CourseName.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()) || x.Departments.ToString().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "name_asc":
                            Course = Course.OrderBy(x => x.CourseName).ToList();
                            break;
                        case "name_desc":
                            Course = Course.OrderByDescending(x => x.CourseName).ToList();
                            break;
                        case "description_asc":
                            Course = Course.OrderBy(x => x.Description).ToList();
                            break;
                        case "description_dsc":
                            Course = Course.OrderByDescending(x => x.Description).ToList();
                            break;
                        case "department_asc":
                            Course = Course.OrderBy(x => x.Departments).ToList();
                            break;
                        case "department_dsc":
                            Course = Course.OrderByDescending(x => x.Departments).ToList();
                            break;
                        case "level_asc":
                            Course = Course.OrderBy(x => x.Levels).ToList();
                            break;
                        case "level_dsc":
                            Course = Course.OrderByDescending(x => x.Levels).ToList();
                            break;
                        case "duration_asc":
                            Course = Course.OrderBy(x => x.Duration).ToList();
                            break;
                        case "duration_dsc":
                            Course = Course.OrderByDescending(x => x.Duration).ToList();
                            break;
                        case "credit_asc":
                            Course = Course.OrderBy(x => x.Credit).ToList();
                            break;
                        case "credit_dsc":
                            Course = Course.OrderByDescending(x => x.Credit).ToList();
                            break;
                    }
                }
                else
                {
                    Course = Course.ToList();
                }


                return Course.ToList();
            }
        }

        //Update Course
        public void UpdateCourse(Course newCourse)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newCourse).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Course
        public void RemoveCourse(Course courseToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(courseToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
