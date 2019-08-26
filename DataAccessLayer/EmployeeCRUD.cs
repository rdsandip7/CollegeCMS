using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeeCRUD
    {
        //Create Employee
        public void SaveEmployee(Employee employee)
        {
            using (var dataContext = new DataContext())
            {
                var addedEmployee = dataContext.Employees.Add(employee);
                employee.EmployeePersonalInfo.EmployeeId = addedEmployee.EmployeeId;
                addedEmployee.EmployeePersonalInfo = dataContext.EmployeePersonalInfos.Add(employee.EmployeePersonalInfo);
                dataContext.SaveChanges();
            }
        }

        //Read Employee
        public Employee GetEmployeeById(int EmployeeId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from employee in dataContext.Employees
                              where employee.EmployeeId == EmployeeId
                              select employee).First();
                return result;
            }
        }
        public Employee GetEmployeeByName(string Name)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from employee in dataContext.Employees
                              where employee.FirstName == Name
                              select employee).First();
                return result;
            }
        }
        public Employee GetEmployeeByFullName(string FirstName, string MiddleName, string LastName)
        {
            using (var dataContext = new DataContext())
            {
                try 
                {
                    Employee Employee = (from employee in dataContext.Employees
                                  where employee.FirstName == FirstName &&employee.MiddleName==MiddleName && employee.LastName== LastName
                                  select employee).First();
                    return Employee;
                }
                catch(Exception ex)
                {
                    return (null);
                }
            }
        }


        public Employee GetEmployeeByEmail(string Email)
        {
            using (var dataContext = new DataContext())
            {
                try
                {
                    Employee Employee = (from employee in dataContext.Employees
                                         where employee.Email==Email
                                         select employee).First();
                    return Employee;
                }
                catch (Exception ex)
                {
                    return (null);
                }
            }
        }

        public List<Employee> GetAllEmployees(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<Employee> Employee = (from employee in dataContext.Employees select employee);

                if (!string.IsNullOrEmpty(searchString))
                {
                    Employee = Employee.Where(x => x.FirstName.ToLower().Contains(searchString.ToLower()) || x.MiddleName.ToLower().Contains(searchString.ToLower()) || x.LastName.ToLower().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "fname_asc":
                            Employee = Employee.OrderBy(x => x.FirstName).ToList();
                            break;
                        case "fname_desc":
                            Employee = Employee.OrderByDescending(x => x.FirstName).ToList();
                            break;
                        case "mname_asc":
                            Employee = Employee.OrderBy(x => x.MiddleName).ToList();
                            break;
                        case "mname_desc":
                            Employee = Employee.OrderByDescending(x => x.MiddleName).ToList();
                            break;
                        case "lname_asc":
                            Employee = Employee.OrderBy(x => x.LastName).ToList();
                            break;
                        case "lname_desc":
                            Employee = Employee.OrderByDescending(x => x.LastName).ToList();
                            break;
                        case "department_asc":
                            Employee = Employee.OrderBy(x => x.Department).ToList();
                            break;
                        case "department_dsc":
                            Employee = Employee.OrderByDescending(x => x.Department).ToList();
                            break;                       
                    }
                }
                else
                {
                    Employee = Employee.ToList();
                }

                return Employee.ToList();
            }
        }

        //Update Employee
        public void UpdateEmployee(Employee newEmployeeValue)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newEmployeeValue).State = EntityState.Modified;
                dataContext.Entry(newEmployeeValue.EmployeePersonalInfo).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Employee
        public void RemoveEmployee(Employee employeeToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(employeeToBeDeleted).State = EntityState.Deleted;
                dataContext.Entry(employeeToBeDeleted.EmployeePersonalInfo).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
