using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class LevelController(LevelService levelService) : AppController
{
    public async Task<IActionResult> Index(string? search, string? sort, int page = 1) =>
        View(await levelService.GetLevelsAsync(BuildListQuery(search, sort, page)));

    public IActionResult Create() => View(new LevelViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(LevelViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await levelService.SaveLevelAsync(model.ToEntity());
        Flash($"Level \"{model.Year} · {model.Semester}\" was created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var level = await levelService.GetLevelByIdAsync(id);
        return level is null ? NotFound() : View(LevelViewModel.From(level));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, LevelViewModel model)
    {
        model.LevelId = id;
        if (!ModelState.IsValid)
            return View(model);

        return RedirectWithResult(await levelService.UpdateLevelAsync(model.ToEntity()), "Level updated.");
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id) =>
        RedirectWithResult(await levelService.DeleteLevelAsync(id), "Level deleted.");
}
