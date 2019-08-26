using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class StudentPersonalInfoCRUD
    {
 

        //Read StudentPersonalInfo
        public StudentPersonalInfo GetStudentPersonalInfo(string studentId)
        {
            using (var dataContext = new DataContext())
            {
                StudentPersonalInfo StudentPersonalInfo = (from studentPersonalInfo in dataContext.StudentPersonalInfos
                                                           where studentPersonalInfo.StudentId == studentId
                                                           select studentPersonalInfo).First();
                return StudentPersonalInfo;
            }
        }

        public IList<StudentPersonalInfo> GetAllStudentPersonalInfo(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<StudentPersonalInfo> StudentPersonalInfo = (from studentPersonalInfo in dataContext.StudentPersonalInfos select studentPersonalInfo);
                
                if (!string.IsNullOrEmpty(searchString))
                {
                    StudentPersonalInfo = StudentPersonalInfo.Where(x => x.Gender.ToLower().Contains(searchString.ToLower()) || x.CitizenshipNumber.ToLower().Contains(searchString.ToLower()) || x.Email.ToLower().Contains(searchString.ToLower()) || x.Address.ToLower().Contains(searchString.ToLower()) || x.ContactNumber.ToLower().Contains(searchString.ToLower()) || x.GuardianName.ToLower().Contains(searchString.ToLower()) || x.GuardianRelationship.ToLower().Contains(searchString.ToLower()) || x.GuardianContactNumber.ToLower().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "email_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.Email).ToList();
                            break;
                        case "email_desc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.Email).ToList();
                            break;
                        case "address_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.Address).ToList();
                            break;
                        case "address_desc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.Address).ToList();
                            break;
                        case "admissiondate_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.AdmissionDate).ToList();
                            break;
                        case "admissiondate_desc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.AdmissionDate).ToList();
                            break;
                        case "gender_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.Gender).ToList();
                            break;
                        case "gender_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.Gender).ToList();
                            break;
                        case "citizen_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.CitizenshipNumber).ToList();
                            break;
                        case "citizen_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.CitizenshipNumber).ToList();
                            break;
                        case "contact_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.ContactNumber).ToList();
                            break;
                        case "contact_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.ContactNumber).ToList();
                            break;
                        case "guardianname_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.GuardianName).ToList();
                            break;
                        case "guardianname_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.GuardianName).ToList();
                            break;
                        case "guardiancontact_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.GuardianContactNumber).ToList();
                            break;
                        case "guardiancontact_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.GuardianContactNumber).ToList();
                            break;
                        case "guardianrelation_asc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderBy(x => x.GuardianRelationship).ToList();
                            break;
                        case "guardianrelation_dsc":
                            StudentPersonalInfo = StudentPersonalInfo.OrderByDescending(x => x.GuardianRelationship).ToList();
                            break;
                    }
                }
                else
                {
                    StudentPersonalInfo = StudentPersonalInfo.ToList();
                }


                return StudentPersonalInfo.ToList();
            }
        }

      
        //Delete StudentPersonalInfo
        public void RemoveStudentPersonalInfo(StudentPersonalInfo studentPersonalInfoToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(studentPersonalInfoToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }



    }
}