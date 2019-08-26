using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class DepartmentService
    {
        DepartmentCRUD departmentTableActions;

        public DepartmentService()
        {
            departmentTableActions = new DepartmentCRUD(); 
        }

        public void SaveDepartment(Department departmentData)
        {
            departmentTableActions.SaveDepartment(departmentData);
        }
        public Department GetDepartmentById(int DepartmentId)
        {
            return departmentTableActions.GetDepartmentById(DepartmentId);
        }
        public Department GetDepartmentByName(string DepartmentName)
        {
            return departmentTableActions.GetDepartmentByName(DepartmentName);
        }
        public void UpdateDepartment(Department dataTobeUpdated)
        {
            departmentTableActions.UpdateDepartment(dataTobeUpdated);
        }
        public void RemoveDeparment(Department dataToBeDeleted)
        {
            departmentTableActions.RemoveDepartment(dataToBeDeleted);
        }
        public void DeleteDepartment(int DepartmentId)
        {
            Department departmentToBeDeleted = departmentTableActions.GetDepartmentById(DepartmentId);
            departmentTableActions.RemoveDepartment(departmentToBeDeleted);
        }
       
        public List<Department> GetAllDepartments(string sortOrder, string searchString)
        {
            return departmentTableActions.GetAllDepartments(sortOrder, searchString);
        }


        
    }
}
