using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeePersonalInfoCRUD
    {
        //Read EmployeePersonalInfo
        public EmployeePersonalInfo GetEmployeePersonalInfoById(int empId)
        {
            using (var dataContext = new DataContext())
            {
                EmployeePersonalInfo EmployeePersonalInfo = (from employeePersonalInfo in dataContext.EmployeePersonalInfos
                                                             where employeePersonalInfo.EmployeeId == empId
                                                             select employeePersonalInfo).First();
                return EmployeePersonalInfo;
            }
        }

        public List<EmployeePersonalInfo> GetAllEmployeesPerssonalInfo(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<EmployeePersonalInfo> EmployeePersonalInfo = (from employeePersonalInfo in dataContext.EmployeePersonalInfos select employeePersonalInfo);

                if (!string.IsNullOrEmpty(searchString))
                {
                    EmployeePersonalInfo = EmployeePersonalInfo.Where(x => x.Phone.ToLower().Contains(searchString.ToLower()) || x.Address.ToLower().Contains(searchString.ToLower()) || x.Position.ToLower().Contains(searchString.ToLower()) || x.Gender.ToLower().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "phone_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.Phone).ToList();
                            break;
                        case "phone_desc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.Phone).ToList();
                            break;
                        case "address_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.Address).ToList();
                            break;
                        case "address_desc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.Address).ToList();
                            break;
                        case "position_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.Position).ToList();
                            break;
                        case "position_desc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.Position).ToList();
                            break;
                        case "gender_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.Gender).ToList();
                            break;
                        case "gender_dsc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.Gender).ToList();
                            break;
                        case "hiredate_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.HiredDate).ToList();
                            break;
                        case "hiredate_dsc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.HiredDate).ToList();
                            break;
                        case "resigndate_asc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderBy(x => x.ResignationDate).ToList();
                            break;
                        case "regindate_dsc":
                            EmployeePersonalInfo = EmployeePersonalInfo.OrderByDescending(x => x.ResignationDate).ToList();
                            break;
                    }
                }
                else
                {
                    EmployeePersonalInfo = EmployeePersonalInfo.ToList();
                }
                
                return EmployeePersonalInfo.ToList();
            }
        }
       

        //Delete EmployeePersonalInfo
        public void RemoveEmployeePersonalInfo(EmployeePersonalInfo employeePersonalInfoToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(employeePersonalInfoToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
