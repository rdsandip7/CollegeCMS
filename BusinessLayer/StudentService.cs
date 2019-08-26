using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class StudentService
    {
         StudentCRUD studentTableActions;
         StudentPersonalInfoCRUD studentpersonalinfoTableActions;

         public StudentService()
        {
            studentTableActions = new StudentCRUD();
            studentpersonalinfoTableActions = new StudentPersonalInfoCRUD();
        }

        public void SaveStudent(Student studentData)
        {
            studentTableActions.SaveStudent(studentData);
        }
       
        public Student GetStudentById(string StudentId)
        {
            return studentTableActions.GetStudentById(StudentId);
        }

        public StudentPersonalInfo GetStudentPersonalInfo(string StudentId)
        {
            return studentpersonalinfoTableActions.GetStudentPersonalInfo(StudentId);
        }
        public Student GetStudentByName(string Name)
        {
            return studentTableActions.GetStudentByName(Name);
        }
        public void UpdateStudent(Student dataTobeUpdated)
        {
            studentTableActions.UpdateStudent(dataTobeUpdated);
        }
        
        public void RemoveStudent(Student dataTobeDeleted)
        {
            studentTableActions.RemoveStudent(dataTobeDeleted);
        }
        public void DeleteStudent(string StudentId)
        {
            Student studentToBeDeleted = studentTableActions.GetStudentById(StudentId);
            studentTableActions.RemoveStudent(studentToBeDeleted);
        }


        public void RemoveStudentPersonalInfo(StudentPersonalInfo dataTobeDeleted)
        {
            studentpersonalinfoTableActions.RemoveStudentPersonalInfo(dataTobeDeleted);
        }
        public void DeleteStudentPersonalInfo(string StudentId)
        {
            StudentPersonalInfo studentToBeDeleted = studentpersonalinfoTableActions.GetStudentPersonalInfo(StudentId);
            studentpersonalinfoTableActions.RemoveStudentPersonalInfo(studentToBeDeleted);
        }

        public IList<Student> GetAllStudents(string sortOrder, string searchString)
        {
            return studentTableActions.GetAllStudents(sortOrder, searchString);
        }
        public IList<StudentPersonalInfo> GetAllStudentPersonalInfo(string sortOrder, string searchString)
        {
            return studentpersonalinfoTableActions.GetAllStudentPersonalInfo(sortOrder, searchString);
        }
    }
}
