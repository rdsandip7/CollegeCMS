using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class EmployeeService
    {
         EmployeeCRUD employeeTableActions;
         EmployeePersonalInfoCRUD employeepersonalinfoTableActions;

         public EmployeeService()
        {
            employeeTableActions = new EmployeeCRUD();
            employeepersonalinfoTableActions = new EmployeePersonalInfoCRUD();
        }

        public void SaveEmployee(Employee employeeData)
        {
            employeeTableActions.SaveEmployee(employeeData);
        }

        
        public Employee GetEmployeeById(int EmployeeId)
        {
            return employeeTableActions.GetEmployeeById(EmployeeId);
        }
        public EmployeePersonalInfo GetEmployeePersonalInfoById(int EmployeeId)
        {
            return employeepersonalinfoTableActions.GetEmployeePersonalInfoById(EmployeeId);
        }

        public Employee GetEmployeeByName(string Name)
        {
            return employeeTableActions.GetEmployeeByName(Name);
        }

        public Employee GetEmployeeByEmail(string Email)
        {
            return employeeTableActions.GetEmployeeByEmail(Email);
        }
        public Employee GetEmployeeByFullName(string FirstName, string MiddleName, string LastName)
        {
            return employeeTableActions.GetEmployeeByFullName(FirstName,MiddleName,LastName);
        }

        public void UpdateEmployee(Employee dataTobeUpdated)
        {
            employeeTableActions.UpdateEmployee(dataTobeUpdated);
        }
       
        public void RemoveEmployee(Employee dataTobeDeleted)
        {
            employeeTableActions.RemoveEmployee(dataTobeDeleted);
        }
        public void RemoveEmployeePersonalInfo(EmployeePersonalInfo dataTobeDeleted)
        {
            employeepersonalinfoTableActions.RemoveEmployeePersonalInfo(dataTobeDeleted);
        }
        public void DeleteEmployee(int EmployeeId)
        {
            Employee employeeToBeDeleted = employeeTableActions.GetEmployeeById(EmployeeId);
            employeeTableActions.RemoveEmployee(employeeToBeDeleted);
        }
       
        public List<Employee> GetAllEmployees(string sortOrder, string searchString)
        {
            return employeeTableActions.GetAllEmployees(sortOrder,searchString);
        }

        public List<EmployeePersonalInfo> GetAllEmployeesPerssonalInfo(string sortOrder, string searchString)
        {
            return employeepersonalinfoTableActions.GetAllEmployeesPerssonalInfo(sortOrder, searchString);
        }

    }
}
