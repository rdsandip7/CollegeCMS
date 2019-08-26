using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class LevelService
    {
         LevelCRUD levelTableActions;

         public LevelService()
        {
            levelTableActions = new LevelCRUD(); 
        }

        public void SaveLevel(Level levelData)
        {
            levelTableActions.SaveLevel(levelData);
        }
        public Level GetLevelById(int LevelId)
        {
            return levelTableActions.GetLevelById(LevelId);
        }
        /*public Level GetLevelBySemester(string Semester)
        {
            return levelTableActions.GetLevelBySemester(Semester);
        }*/
        public void UpdateLevel(Level dataTobeUpdated)
        {
            levelTableActions.UpdateLevel(dataTobeUpdated);
        }
        public void DeleteLevel(int LevelId)
        {
            Level levelToBeDeleted = levelTableActions.GetLevelById(LevelId);
            levelTableActions.RemoveLevel(levelToBeDeleted);
        }
        public void RemoveLevel(Level dataToBeDeleted)
        {
            levelTableActions.RemoveLevel(dataToBeDeleted);
        }

        public IList<Level> GetAllLevels(string sortOrder, string searchString)
        {
            return levelTableActions.GetAllLevels(sortOrder,searchString);
        }



        public Level GetLevelById(string p)
        {
            throw new NotImplementedException();
        }
    }
}
