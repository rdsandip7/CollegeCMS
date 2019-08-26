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
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        EmployeeService employeeService = new EmployeeService();
        DepartmentService departmentService = new DepartmentService();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FNameSortParm = sortOrder == "fname_asc" ? "fname_dsc" : "fname_asc";
            ViewBag.MNameSortParm = sortOrder == "mname_asc" ? "mname_dsc" : "mname_asc";
            ViewBag.LNameSortParm = sortOrder == "lname_asc" ? "lname_dsc" : "lname_asc";
            ViewBag.deptSortParm = sortOrder == "department_asc" ? "department_dsc" : "department_asc";
            ViewBag.PhoneSortParm = sortOrder == "phone_asc" ? "phone_dsc" : "phone_asc";
            ViewBag.AddressSortParm = sortOrder == "address_asc" ? "address_dsc" : "address_asc";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_dsc" : "email_asc";
            ViewBag.PositionSortParm = sortOrder == "position_asc" ? "position_dsc" : "position_asc";
            ViewBag.GenderSortParm = sortOrder == "gender_asc" ? "gender_dsc" : "gender_asc";
            ViewBag.HiredDateSortParm = sortOrder == "hiredate_asc" ? "hiredate_dsc" : "hiredate_asc";
            ViewBag.ResignationDateSortParm = sortOrder == "regindate_asc" ? "regindate_dsc" : "regindate_asc";
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

            var Employees = employeeService.GetAllEmployees(sortOrder,searchString);
            var employeePersonalInfo = employeeService.GetAllEmployeesPerssonalInfo(sortOrder, searchString);
            var departments = departmentService.GetAllDepartments(sortOrder, searchString);

            var tempEmployeeDetails = (from employeeRecord in Employees
                                       join employeePersonRecord in employeePersonalInfo
                                       on employeeRecord.EmployeeId equals employeePersonRecord.EmployeeId
                                       select new
                                       {
                                           EmpId = employeeRecord.EmployeeId,
                                           FirstName = employeeRecord.FirstName,
                                           MiddleName = employeeRecord.MiddleName,
                                           LastName = employeeRecord.LastName,
                                           Email = employeeRecord.Email,
                                           DeptId = employeeRecord.DepartmentId,
                                           Address = employeePersonRecord.Address,
                                           ContactNumber = employeePersonRecord.Phone,
                                           Gender = employeePersonRecord.Gender,
                                           Position = employeePersonRecord.Position,
                                           HiredDate = employeePersonRecord.HiredDate,
                                           ResignationDate = employeePersonRecord.ResignationDate ?? null
                                       }).ToList();

            var employeeDetails = (from tempEmployeeDetail in tempEmployeeDetails
                                   join department in departments
                                   on tempEmployeeDetail.DeptId equals department.DepartmentId
                                   select new EmployeeViewModel()
                                   {
                                       EmployeeId = tempEmployeeDetail.EmpId,
                                       FirstName = tempEmployeeDetail.FirstName,
                                       MiddleName = tempEmployeeDetail.MiddleName,
                                       LastName = tempEmployeeDetail.LastName,
                                       Email = tempEmployeeDetail.Email,
                                       Department = department.DepartmentName,
                                       Address = tempEmployeeDetail.Address,
                                       Phone = tempEmployeeDetail.ContactNumber,
                                       Gender = tempEmployeeDetail.Gender,
                                       Position = tempEmployeeDetail.Position,
                                       HiredDate = tempEmployeeDetail.HiredDate,
                                       ResignationDate = tempEmployeeDetail.ResignationDate
                                   }).ToList();
            List<EmployeeViewModel> finalResultEmployeeList;
            if (!string.IsNullOrEmpty(searchString))
            {
                finalResultEmployeeList = (from employeeDetail in employeeDetails
                                           where employeeDetail.FirstName == searchString || employeeDetail.MiddleName == searchString
                                           || employeeDetail.LastName == searchString || employeeDetail.Email == searchString
                                           || employeeDetail.Address == searchString || employeeDetail.Phone == searchString
                                           || employeeDetail.Department == searchString || employeeDetail.Gender == searchString
                                           || employeeDetail.Position == searchString
                                           select employeeDetail).ToList();
            }
            else
            {
                finalResultEmployeeList = employeeDetails;
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "fname_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.FirstName).ToList();
                        break;
                    case "fname_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.FirstName).ToList();
                        break;
                    case "mname_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.MiddleName).ToList();
                        break;
                    case "mname_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.MiddleName).ToList();
                        break;
                    case "lname_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.LastName).ToList();
                        break;
                    case "lname_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.LastName).ToList();
                        break;
                    case "email_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.Email).ToList();
                        break;
                    case "email_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.Email).ToList();
                        break;
                    case "phone_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.Phone).ToList();
                        break;
                    case "phone_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.Phone).ToList();
                        break;
                    case "address_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.Address).ToList();
                        break;
                    case "address_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.Address).ToList();
                        break;
                    case "position_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.Position).ToList();
                        break;
                    case "position_desc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.Position).ToList();
                        break;
                    case "gender_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.Gender).ToList();
                        break;
                    case "gender_dsc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.Gender).ToList();
                        break;
                    case "hiredate_asc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderBy(x => x.HiredDate).ToList();
                        break;
                    case "hiredate_dsc":
                        finalResultEmployeeList = finalResultEmployeeList.OrderByDescending(x => x.HiredDate).ToList();
                        break;
                }
            }

            return View(finalResultEmployeeList.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CreateNewEmployee()
        {
            List<Department> departments = departmentService.GetAllDepartments("", "");
            List<String> departmentNames = (from department in departments select department.DepartmentName).ToList();
            foreach (var department in departments)
            {
                departmentNames.Add(department.DepartmentName);
            }
            ViewBag.departmentNames = departmentNames;
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployee(EmployeeViewModel employeeData)
        {
            Department department = departmentService.GetDepartmentByName(employeeData.Department);        
           
            EmployeePersonalInfo employeepersonalinfo = new EmployeePersonalInfo() 
            {
                Phone=employeeData.Phone,               
                Gender=employeeData.Gender,
                Position=employeeData.Position,
                HiredDate=employeeData.HiredDate,
                ResignationDate=employeeData.ResignationDate,
                Address=employeeData.Address,
                InsertedDate = DateTime.Now
            };

            Employee employee = new Employee()
            {
                FirstName = employeeData.FirstName,
                MiddleName = employeeData.MiddleName,
                LastName = employeeData.LastName,
                DepartmentId = department.DepartmentId,
                Email = employeeData.Email,
                EmployeePersonalInfo= employeepersonalinfo,
                InsertedDate = DateTime.Now
            };

            employeeService.SaveEmployee(employee);            
            return View(employeeData);
        }
        public ActionResult UpdateEmployee(EmployeeViewModel newEmployeeData)
        {
            Department department = departmentService.GetDepartmentByName(newEmployeeData.Department);

            Employee employee = new Employee()
            
            {
                EmployeeId = newEmployeeData.EmployeeId,
                FirstName = newEmployeeData.FirstName,
                MiddleName = newEmployeeData.MiddleName,
                LastName = newEmployeeData.LastName,
                DepartmentId = department.DepartmentId,
                InsertedDate = DateTime.Now

            };
            EmployeePersonalInfo employeePersonalInfo = new EmployeePersonalInfo()
            {
                EmployeeId = newEmployeeData.EmployeeId,
                Phone = newEmployeeData.Phone,
                
                Gender = newEmployeeData.Gender,
                Position = newEmployeeData.Position,
                Address = newEmployeeData.Address,
                HiredDate = newEmployeeData.HiredDate,
                ResignationDate = newEmployeeData.ResignationDate,
                InsertedDate = DateTime.Now
            };
            employeeService.UpdateEmployee(employee);
            
            return RedirectToAction("Index");
        }


        public ActionResult DeleteEmployee(int EmployeeId)
        {
            try
            {
                employeeService.DeleteEmployee(EmployeeId);
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int EmployeeId)
        {
            Employee employee = employeeService.GetEmployeeById(EmployeeId);
            EmployeePersonalInfo employeePersonalInfo = employeeService.GetEmployeePersonalInfoById(EmployeeId);
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                //Department = employee.Department,
                Phone = employeePersonalInfo.Phone,
                Email = employee.Email,
                Gender= employeePersonalInfo.Gender,
                Position= employeePersonalInfo.Position,
                Address= employeePersonalInfo.Address,
                HiredDate=employeePersonalInfo.HiredDate,
                ResignationDate=employeePersonalInfo.ResignationDate
            };


            return View(employeeViewModel);
        }
    }
}
