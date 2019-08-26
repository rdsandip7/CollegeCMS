using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CoreLayer;

namespace DataAccessLayer
{
    public class UserCRUD
    {
        //Create User
        public int SaveUser(User user)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Users.Add(user);
                return dataContext.SaveChanges();
            }
        }
        
        //Retrive User
        public User GetUserByUserId(int userId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from user in dataContext.Users where user.UserId == userId select user).First();
                return result;
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from user in dataContext.Users where user.Username == username select user).FirstOrDefault();
                return result;
            }
        }


        public void UpdateUser(User newUser)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newUser).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete User
        public void RemoveUser(User userToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(userToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }

    }
}
