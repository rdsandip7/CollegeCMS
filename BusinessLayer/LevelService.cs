using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class LevelService(LevelCRUD levelTableActions)
{
    public Task<PagedResult<Level>> GetLevelsAsync(ListQuery query) => levelTableActions.GetLevelsAsync(query);

    public Task<List<Level>> GetAllLevelsAsync() => levelTableActions.GetAllLevelsAsync();

    public Task<Level?> GetLevelByIdAsync(int levelId) => levelTableActions.GetLevelByIdAsync(levelId);

    public async Task SaveLevelAsync(Level level)
    {
        level.InsertedDate = DateTime.Now;
        await levelTableActions.SaveLevelAsync(level);
    }

    public async Task<OperationResult> UpdateLevelAsync(Level changes)
    {
        var level = await levelTableActions.GetLevelByIdAsync(changes.LevelId);
        if (level is null)
            return OperationResult.Fail("Level not found.");

        level.Year = changes.Year;
        level.Semester = changes.Semester;
        level.Description = changes.Description;
        level.UpdatedDate = DateTime.Now;
        await levelTableActions.UpdateLevelAsync(level);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteLevelAsync(int levelId)
    {
        var level = await levelTableActions.GetLevelByIdAsync(levelId);
        if (level is null)
            return OperationResult.Fail("Level not found.");
        if (await levelTableActions.HasDependentsAsync(levelId))
            return OperationResult.Fail($"\"{level.DisplayName}\" still has students or courses assigned.");

        await levelTableActions.RemoveLevelAsync(level);
        return OperationResult.Success;
    }
}
