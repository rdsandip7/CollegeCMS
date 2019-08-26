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
    public class DepartmentController : Controller
    {
        //
        // GET: /Department/
        DepartmentService departmentService = new DepartmentService();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int?page )
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_dsc" : "name_asc";
            ViewBag.descSortParm = sortOrder == "description_asc" ? "description_dsc" : "description_asc";
            ViewBag.deptheadSortParm = sortOrder == "departmenthead_asc" ? "departmenthead_dsc" : "departmenthead_asc";
            ViewBag.estdateSortParm = sortOrder == "estdate_asc" ? "estdate_dsc" : "estdate_asc";
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
            int pageNumber =(page ?? 1);


            var Departments = departmentService.GetAllDepartments(sortOrder, searchString);
            return View(Departments.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CreateNewDepartment()
        {
            return View();

        }
         [HttpPost]
        public ActionResult SaveDepartment(DepartmentViewModel departmentData)
        {
           
            departmentService = new DepartmentService();
            Department Department=new Department()
            {
                DepartmentName=departmentData.DepartmentName,
                Description=departmentData.Description,
                DepartmentHead = departmentData.DepartmentHead,
                InsertedDate=DateTime.Now,
                EstablishedDate = departmentData.EstablishedDate
            };
            departmentService.SaveDepartment(Department);
            return View(departmentData);
        }

        [HttpPost]
        public ActionResult UpdateDepartment(DepartmentViewModel newDepartmentData)
        {
            Department department = new Department()
            {
                DepartmentId = newDepartmentData.DeptId,
                DepartmentName = newDepartmentData.DepartmentName,
                DepartmentHead = newDepartmentData.DepartmentHead,
                Description = newDepartmentData.Description,
                EstablishedDate = newDepartmentData.EstablishedDate,
                InsertedDate=DateTime.Now

            };
            departmentService.UpdateDepartment(department);
            return RedirectToAction ("Index");
        }
        public ActionResult DeleteDepartment(int DepartmentId)
        {
            try 
            {
                departmentService.DeleteDepartment(DepartmentId);
            }
            catch
            {
                return View();
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int DepartmentId)
        {
            Department department = departmentService.GetDepartmentById(DepartmentId);
            DepartmentViewModel departmentViewModel = new DepartmentViewModel()
            {
                DeptId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentHead = department.DepartmentHead,
                Description = department.Description,
                EstablishedDate = department.EstablishedDate
            };
            return View(departmentViewModel);
        }
    }
}
