using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public  class UserService
    {
        UserCRUD userTableActions;

        public UserService()
        {
            userTableActions = new UserCRUD(); 
        }

        public int SaveUser(User userData)
        {
            return userTableActions.SaveUser(userData);
        }
        public User GetUserByUserId(int CourseId)
        {
            return userTableActions.GetUserByUserId(CourseId);
        }
        public User GetUserByUsername(string CourseName)
        {
            return userTableActions.GetUserByUsername(CourseName);
        }

       
        public void UpdateUser(User dataTobeUpdated)
        {
            userTableActions.UpdateUser(dataTobeUpdated);
        }
        public void DeleteUser(int UserId)
        {
            User userToBeDeleted = userTableActions.GetUserByUserId(UserId);
            userTableActions.RemoveUser(userToBeDeleted);
        }
        public void RemoveUser(User dataToBeDeleted)
        {
            userTableActions.RemoveUser(dataToBeDeleted);
        }

    }
}
