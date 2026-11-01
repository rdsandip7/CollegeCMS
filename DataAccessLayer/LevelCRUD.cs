using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class LevelCRUD(DataContext dataContext)
{
    private static readonly Dictionary<string, Expression<Func<Level, object?>>> SortColumns = new()
    {
        ["year"] = x => x.Year,
        ["semester"] = x => x.Semester,
        ["description"] = x => x.Description,
    };

    //Create Level
    public async Task SaveLevelAsync(Level level)
    {
        dataContext.Levels.Add(level);
        await dataContext.SaveChangesAsync();
    }

    //Read Level
    public Task<Level?> GetLevelByIdAsync(int levelId) =>
        dataContext.Levels.FirstOrDefaultAsync(x => x.LevelId == levelId);

    public Task<List<Level>> GetAllLevelsAsync() =>
        dataContext.Levels.AsNoTracking().OrderBy(x => x.Year).ThenBy(x => x.Semester).ToListAsync();

    public Task<PagedResult<Level>> GetLevelsAsync(ListQuery query)
    {
        IQueryable<Level> levels = dataContext.Levels.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            levels = levels.Where(x => x.Semester.ToLower().Contains(s)
                || (x.Description != null && x.Description.ToLower().Contains(s))
                || x.Year.ToString().Contains(s));
        }

        return levels.ApplySort(query, SortColumns).ToPagedResultAsync(query);
    }

    public async Task<bool> HasDependentsAsync(int levelId) =>
        await dataContext.Students.AnyAsync(x => x.LevelId == levelId)
        || await dataContext.Courses.AnyAsync(x => x.LevelId == levelId);

    //Update Level
    public async Task UpdateLevelAsync(Level level)
    {
        dataContext.Levels.Update(level);
        await dataContext.SaveChangesAsync();
    }

    //Delete Level
    public async Task RemoveLevelAsync(Level level)
    {
        dataContext.Levels.Remove(level);
        await dataContext.SaveChangesAsync();
    }
}
