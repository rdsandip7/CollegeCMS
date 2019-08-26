using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class CourseService
    {
        CourseCRUD courseTableActions;

        public CourseService()
        {
            courseTableActions = new CourseCRUD(); 
        }

        public void SaveCourse(Course courseData)
        {
            courseTableActions.SaveCourse(courseData);
        }
        public Course GetCourseById(int CourseId)
        {
            return courseTableActions.GetCourseById(CourseId);
        }
        public Course GetCourseByName(string CourseName)
        {
            return courseTableActions.GetCourseByName(CourseName);
        }
        public void UpdateCourse(Course dataTobeUpdated)
        {
            courseTableActions.UpdateCourse(dataTobeUpdated);
        }
        public void DeleteCourse(int CourseId)
        {
            Course courseToBeDeleted = courseTableActions.GetCourseById(CourseId);
            courseTableActions.RemoveCourse(courseToBeDeleted);
        }
        public void RemoveCourse(Course dataToBeDeleted)
        {
            courseTableActions.RemoveCourse(dataToBeDeleted);
        }

        public IList<Course> GetAllCourses(string sortOrder, string searchString)
        {
            return courseTableActions.GetAllCourses(sortOrder,searchString);
        }

    }
}
