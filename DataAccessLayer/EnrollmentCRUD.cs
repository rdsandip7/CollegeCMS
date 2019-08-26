using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EnrollmentCRUD
    {
        //Create Enrollment
        public void SaveEnrollment(Enrollment enrollment)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Enrollments.Add(enrollment);
                dataContext.SaveChanges();
            }
        }

        //Read Enrollment
        public Enrollment GetEnrollment(int enId)
        {
            using (var dataContext = new DataContext())
            {
                Enrollment Enrollment = (from enrollment in dataContext.Enrollments
                                         where enrollment.EnrollmentId == enId
                                         select enrollment).First();
                return Enrollment;
            }
        }

        //Update Enrollment
        public void UpdateEnrollment(Enrollment newEnrollment)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newEnrollment).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Enrollment
        public void RemoveEnrollment(Enrollment enrollmentToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(enrollmentToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
