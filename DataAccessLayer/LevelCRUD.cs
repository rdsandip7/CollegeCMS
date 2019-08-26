using CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LevelCRUD
    {
        //Create Level
        public void SaveLevel(Level level)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Levels.Add(level);
                dataContext.SaveChanges();
            }
        }

        //Read Level
        public Level GetLevelById(int levelId)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from level in dataContext.Levels
                              where level.LevelId == levelId
                              select level).First();
                return result;
            }
        }
        public Level GetLevelBySemester(string Semester)
        {
            using (var dataContext = new DataContext())
            {
                var result = (from level in dataContext.Levels
                              where level.Semester == Semester
                              select level).First();
                return result;
            }
        }

        public List<Level> GetAllLevels(string sortOrder, string searchString)
        {
            using (var dataContext = new DataContext())
            {
                IEnumerable<Level> Level = (from level in dataContext.Levels select level);

                if (!string.IsNullOrEmpty(searchString))
                {
                    Level = Level.Where(x => x.Semester.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()));

                }
                
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "year_asc":
                            Level = Level.OrderBy(x => x.Year).ToList();
                            break;
                        case "year_desc":
                            Level = Level.OrderByDescending(x => x.Year).ToList();
                            break;
                        case "semester_asc":
                            Level = Level.OrderBy(x => x.Semester).ToList();
                            break;
                        case "semester_desc":
                            Level = Level.OrderByDescending(x => x.Semester).ToList();
                            break;
                        case "desc_asc":
                            Level = Level.OrderBy(x => x.Description).ToList();
                            break;
                        case "desc_desc":
                            Level = Level.OrderByDescending(x => x.Description).ToList();
                            break;
                    }
                }
                else
                {
                    Level = Level.ToList();
                }
                return Level.ToList();
            }
        }

        //Update Level
        public void UpdateLevel(Level newLevel)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(newLevel).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }

        //Delete Level
        public void RemoveLevel(Level levelToBeDeleted)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Entry(levelToBeDeleted).State = EntityState.Deleted;
                dataContext.SaveChanges();
            }
        }
    }
}
