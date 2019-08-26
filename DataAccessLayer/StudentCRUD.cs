using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class StudentCRUD
    {
        //Create Student
        public void SaveStudent(Student student)
        {
            using (var dataContext = new DataContext())
            {
                var addedStudent = dataContext.Students.Add(student);
                student.StudentPersonalInfo.StudentId = addedStudent.StudentId;
                addedStudent.StudentPersonalInfo = dataContext.StudentPersonalInfos.Add(student.StudentPersonalInfo);               
                dataContext.SaveChanges();
            }
        }

        //Read Student
        public Student GetStudentById(string StudentId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from student in dataContext.Students
                              where student.StudentId == StudentId
                              select student).First();
                return result;
            }
        }
        public Student GetStudentByName(string Name)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from student in dataContext.Students
                              where student.FirstName == Name
                              select student).First();
                return result;
            }
        }

        public List<Student> GetAllStudents(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<Student> Student = (from student in dataContext.Students select student);

                if (!string.IsNullOrEmpty(searchString))
                {
                    Student = Student.Where(x => x.FirstName.ToLower().Contains(searchString.ToLower()) || x.MiddleName.ToLower().Contains(searchString.ToLower()) || x.LastName.ToLower().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "fname_asc":
                            Student = Student.OrderBy(x => x.FirstName).ToList();
                            break;
                        case "fname_desc":
                            Student = Student.OrderByDescending(x => x.FirstName).ToList();
                            break;
                        case "mname_asc":
                            Student = Student.OrderBy(x => x.MiddleName).ToList();
                            break;
                        case "mname_desc":
                            Student = Student.OrderByDescending(x => x.MiddleName).ToList();
                            break;
                        case "lname_asc":
                            Student = Student.OrderBy(x => x.LastName).ToList();
                            break;
                        case "lname_desc":
                            Student = Student.OrderByDescending(x => x.LastName).ToList();
                            break;
                        case "department_asc":
                            Student = Student.OrderBy(x => x.Department).ToList();
                            break;
                        case "department_dsc":
                            Student = Student.OrderByDescending(x => x.Department).ToList();
                            break;
                        case "level_asc":
                            Student = Student.OrderBy(x => x.Level).ToList();
                            break;
                        case "level_dsc":
                            Student = Student.OrderByDescending(x => x.Level).ToList();
                            break;
                    }
                }
                else 
                {
                    Student = Student.ToList();
                }
                return Student.ToList();
            }
        }

        //Update Student
        public void UpdateStudent(Student newStudent)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newStudent).State = EntityState.Modified;
                dataContext.Entry(newStudent.StudentPersonalInfo).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Student
        public void RemoveStudent(Student studentToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(studentToBeDeleted).State = EntityState.Deleted;
                dataContext.Entry(studentToBeDeleted.StudentPersonalInfo).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
