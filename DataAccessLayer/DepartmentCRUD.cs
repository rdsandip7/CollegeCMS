using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DepartmentCRUD
    {
        //Create Department
        public void SaveDepartment(Department department)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Departments.Add(department);
                dataContext.SaveChanges();
            }
        } 

        //Read Department
        public Department GetDepartmentById(int deptId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from department in dataContext.Departments
                              where department.DepartmentId == deptId
                              select department).First();
                return result;
            }
        }
        public Department GetDepartmentByName(string deptName)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from department in dataContext.Departments
                              where department.DepartmentName == deptName
                              select department).First();
                return result;
            }
        }

        public List<Department> GetAllDepartments(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<Department> Department = from department in dataContext.Departments select department;

                if (!string.IsNullOrEmpty(searchString))
                {
                    Department = Department.Where(x => x.DepartmentName.ToLower().Contains(searchString.ToLower()) || x.DepartmentHead.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()));

                }
                
                if(!string.IsNullOrEmpty(sortOrder))
                {
                    switch(sortOrder)
                    {
                        case "name_asc":
                            Department = Department.OrderBy(x => x.DepartmentName).ToList();
                            break;
                        case "name_desc":
                            Department = Department.OrderByDescending(x => x.DepartmentName).ToList();
                            break;
                        case "description_asc":
                            Department = Department.OrderBy(x => x.Description).ToList();
                            break;
                        case "description_dsc":
                            Department = Department.OrderByDescending(x => x.Description).ToList();
                            break;
                        case "departmenthead_asc":
                            Department = Department.OrderBy(x => x.DepartmentHead).ToList();
                            break;
                        case "departmenthead_dsc":
                            Department = Department.OrderByDescending(x => x.DepartmentHead).ToList();
                            break;
                        case "estdate_asc":
                            Department = Department.OrderBy(x => x.Description).ToList();
                            break;
                        case "estdate_dsc":
                            Department = Department.OrderByDescending(x => x.Description).ToList();
                            break;
                    }
                }
                else
                {
                    Department = Department.ToList();
                }
                return Department.ToList();
            }
        }

        //Update Department
        public void UpdateDepartment(Department newDepartmentValue)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newDepartmentValue).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Department
        public void RemoveDepartment(Department departmentToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(departmentToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }

        
    }
}
