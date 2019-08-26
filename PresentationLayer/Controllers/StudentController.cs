using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using CoreLayer;
using PagedList;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        StudentService studentService = new StudentService();
        DepartmentService departmentService = new DepartmentService();
        LevelService levelService = new LevelService();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = sortOrder == "fname_asc" ? "fname_dsc" : "fname_asc";
            ViewBag.MiddleNameSortParm = sortOrder == "mname_asc" ? "mname_dsc" : "mname_asc";
            ViewBag.LastNameSortParm = sortOrder == "lname_asc" ? "lname_dsc" : "lname_asc";
            ViewBag.DepartmentSortParm = sortOrder == "department_asc" ? "department_dsc" : "department_asc";
            ViewBag.LevelSortParm = sortOrder == "level_asc" ? "level_dsc" : "level_asc";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_dsc" : "email_asc";
            ViewBag.AddressSortParm = sortOrder == "address_asc" ? "address_dsc" : "address_asc";
            ViewBag.AdmissionSortParm = sortOrder == "admissiondate_asc" ? "admissiondate_dsc" : "admissiondate_asc";
            ViewBag.GenderSortParm = sortOrder == "gender_asc" ? "gender_dsc" : "gender_asc";
            ViewBag.CitizenshipSortParm = sortOrder == "citizen_asc" ? "citizen_dsc" : "citizen_asc";
            ViewBag.ContactNumberSortParm = sortOrder == "contact_asc" ? "contact_dsc" : "contact_asc";
            ViewBag.GuardianNameSortParm = sortOrder == "guardianname_asc" ? "guardianname_dsc" : "guardianname_asc";
            ViewBag.GuardianContactSortParm = sortOrder == "guardiancontact_asc" ? "guardiancontact_dsc" : "guardiancontact_asc";
            ViewBag.GuardianRelationSortParm = sortOrder == "guardianrelation_asc" ? "guardianrelation_dsc" : "guardianrelation_asc";
            
            
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


            var Students = studentService.GetAllStudents(sortOrder, searchString);
            var studentPersonalInfo = studentService.GetAllStudentPersonalInfo(sortOrder, searchString);
            var departments = departmentService.GetAllDepartments(sortOrder, searchString);
            var levels = levelService.GetAllLevels(sortOrder, searchString);

            var tempStudentDetails = (from student in Students
                                      join studentPersonal in studentPersonalInfo 
                                      on student.StudentId equals studentPersonal.StudentId
                                      select new 
                                      { 
                                        studentId= student.StudentId,
                                        FirstName = student.FirstName,
                                        MiddleName = student.MiddleName,
                                        LastName = student.LastName,
                                        level = student.LevelId,
                                        DeptId = student.DepartmentId,
                                        Address = studentPersonal.Address,
                                        ContactNumber = studentPersonal.ContactNumber,
                                        Gender = studentPersonal.Gender,
                                        GaurdianName = studentPersonal.GuardianName,
                                        Gaurdiancontact=studentPersonal.GuardianContactNumber,
                                        Gaurdianrelation = studentPersonal.GuardianRelationship,
                                        Email = studentPersonal.Email,
                                        CitizenshipNumber = studentPersonal.CitizenshipNumber,
                                        DOB = studentPersonal.DateOfBirth,
                                        Admissiondate = studentPersonal.AdmissionDate
                                      
                                      }).ToList();

            var StudentDetails = (from tempStudentDetail in tempStudentDetails
                                      join department in departments
                                      on tempStudentDetail.DeptId equals department.DepartmentId
                                      join level in levels
                                      on tempStudentDetail.level equals level.LevelId
                                      select new StudentViewModel()
                                      {
                                          StudentId = tempStudentDetail.studentId,
                                          FirstName = tempStudentDetail.FirstName,
                                          MiddleName = tempStudentDetail.MiddleName,
                                          LastName = tempStudentDetail.LastName,
                                          Level = tempStudentDetail.level,
                                          Department = department.DepartmentName,
                                          Address = tempStudentDetail.Address,
                                          Contact = tempStudentDetail.ContactNumber,
                                          Gender = tempStudentDetail.Gender,
                                          GuardianName = tempStudentDetail.GaurdianName,
                                          GuardianContact = tempStudentDetail.Gaurdiancontact,
                                          GuardianRelationship = tempStudentDetail.Gaurdianrelation,
                                          Email = tempStudentDetail.Email,
                                          CitizenshipNumber = tempStudentDetail.CitizenshipNumber,
                                          DateOfBirth = tempStudentDetail.DOB,
                                          AdmissionDate = tempStudentDetail.Admissiondate
                                      }).ToList();
            List<StudentViewModel> finalResultStudentList;
            if (!string.IsNullOrEmpty(searchString))
            {
                finalResultStudentList = (from StudentDetail in StudentDetails
                                          where StudentDetail.FirstName == searchString || StudentDetail.MiddleName == searchString
                                           || StudentDetail.LastName == searchString || StudentDetail.Email == searchString
                                           || StudentDetail.Address == searchString || StudentDetail.Contact == searchString
                                           || StudentDetail.Department == searchString || StudentDetail.Gender == searchString
                                           || StudentDetail.GuardianName == searchString || StudentDetail.GuardianContact == searchString
                                           || StudentDetail.GuardianRelationship == searchString || StudentDetail.Email == searchString
                                           || StudentDetail.CitizenshipNumber == searchString 
                                          select StudentDetail).ToList();
            }
            else
            {
                finalResultStudentList = StudentDetails;
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "fname_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.FirstName).ToList();
                        break;
                    case "fname_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.FirstName).ToList();
                        break;
                    case "mname_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.MiddleName).ToList();
                        break;
                    case "mname_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.MiddleName).ToList();
                        break;
                    case "lname_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.LastName).ToList();
                        break;
                    case "lname_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.LastName).ToList();
                        break;
                    case "department_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Department).ToList();
                        break;
                    case "department_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Department).ToList();
                        break;
                    case "level_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Level).ToList();
                        break;
                    case "level_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Level).ToList();
                        break;
                    case "email_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Email).ToList();
                        break;
                    case "email_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Email).ToList();
                        break;
                    case "address_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Address).ToList();
                        break;
                    case "address_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Address).ToList();
                        break;
                    case "admissiondate_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.AdmissionDate).ToList();
                        break;
                    case "admissiondate_desc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.AdmissionDate).ToList();
                        break;
                    case "gender_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Gender).ToList();
                        break;
                    case "gender_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Gender).ToList();
                        break;
                    case "citizen_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.CitizenshipNumber).ToList();
                        break;
                    case "citizen_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.CitizenshipNumber).ToList();
                        break;
                    case "contact_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.Contact).ToList();
                        break;
                    case "contact_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.Contact).ToList();
                        break;
                    case "guardianname_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.GuardianName).ToList();
                        break;
                    case "guardianname_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.GuardianName).ToList();
                        break;
                    case "guardiancontact_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.GuardianContact).ToList();
                        break;
                    case "guardiancontact_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.GuardianContact).ToList();
                        break;
                    case "guardianrelation_asc":
                        finalResultStudentList = finalResultStudentList.OrderBy(x => x.GuardianRelationship).ToList();
                        break;
                    case "guardianrelation_dsc":
                        finalResultStudentList = finalResultStudentList.OrderByDescending(x => x.GuardianRelationship).ToList();
                        break;
                }
            }
            return View(finalResultStudentList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateNewStudent()
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
        public ActionResult SaveStudent(StudentViewModel studentData)
        {
            Department department = departmentService.GetDepartmentByName(studentData.Department);
            Level level = levelService.GetLevelById(studentData.Level);

            StudentPersonalInfo StudentPersonalInfo = new StudentPersonalInfo()
            {
                ContactNumber = studentData.Contact,
                Address = studentData.Address,
                GuardianName = studentData.GuardianName,
                GuardianContactNumber = studentData.GuardianContact,
                GuardianRelationship = studentData.GuardianRelationship,
                Gender = studentData.Gender,
                Email = studentData.Email,
                CitizenshipNumber = studentData.CitizenshipNumber,
                DateOfBirth = studentData.DateOfBirth,
                AdmissionDate = studentData.AdmissionDate,
                InsertedDate= DateTime.Now
            };
             
             Student Student = new Student()
            {
                FirstName = studentData.FirstName,
                MiddleName = studentData.MiddleName,
                LastName = studentData.LastName,
                LevelId = level.LevelId,
                DepartmentId = department.DepartmentId,
                InsertedDate = DateTime.Now,
            };
           
            studentService.SaveStudent(Student);           
            return View(studentData);
        }
         public ActionResult UpdateStudent(StudentViewModel newStudentData)
         {
             Department department = departmentService.GetDepartmentByName(newStudentData.Department);
             Level level = levelService.GetLevelById(newStudentData.Level);
             Student student = new Student()

             {
                 StudentId = newStudentData.StudentId,
                 FirstName = newStudentData.FirstName,
                 MiddleName = newStudentData.MiddleName,
                 LastName = newStudentData.LastName,
                 DepartmentId = department.DepartmentId,
                 LevelId = level.LevelId,
                 InsertedDate = DateTime.Now

             };
             StudentPersonalInfo studentPersonalInfo = new StudentPersonalInfo()
             {
                 ContactNumber = newStudentData.Contact,
                 Gender = newStudentData.Gender,
                 Address = newStudentData.Address,
                 GuardianName = newStudentData.GuardianName,
                 GuardianContactNumber = newStudentData.GuardianContact,
                 GuardianRelationship = newStudentData.GuardianRelationship,
                 Email = newStudentData.Email,
                 CitizenshipNumber = newStudentData.CitizenshipNumber,
                 DateOfBirth = newStudentData.DateOfBirth,
                 AdmissionDate = newStudentData.AdmissionDate,
                 InsertedDate = DateTime.Now
             };
             studentService.UpdateStudent(student);             
             return RedirectToAction("Index");
         }


         public ActionResult DeleteStudent(string StudentId)
         {
             try
             {
                 studentService.DeleteStudent(StudentId);
             }
             catch
             {
                 return View();
             }

             return RedirectToAction("Index");
         }

         public ActionResult Edit(string StudentId)
         {
             Student student = studentService.GetStudentById(StudentId);
             StudentPersonalInfo studentPersonalInfo = studentService.GetStudentPersonalInfo(StudentId);
             StudentViewModel studentViewModel = new StudentViewModel()
             {
                 StudentId = student.StudentId,
                 FirstName = student.FirstName,
                 MiddleName = student.MiddleName,
                 LastName = student.LastName,
                 //Department = student.DepartmentId,
                 //Level = student.LevelId,
                 Contact = studentPersonalInfo.ContactNumber,                 
                 Gender = studentPersonalInfo.Gender,
                 Address = studentPersonalInfo.Address,
                 GuardianName = studentPersonalInfo.GuardianName,
                 GuardianContact = studentPersonalInfo.GuardianContactNumber,
                 GuardianRelationship = studentPersonalInfo.GuardianRelationship,
                 Email = studentPersonalInfo.Email,
                 CitizenshipNumber= studentPersonalInfo.CitizenshipNumber,
                 DateOfBirth= studentPersonalInfo.DateOfBirth,
                 AdmissionDate = studentPersonalInfo.AdmissionDate
             };
             return View(studentViewModel);
         }

    }
}
