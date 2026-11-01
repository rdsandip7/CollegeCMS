using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

/// <summary>Shared helpers for the list/create/edit/delete controllers.</summary>
public abstract class AppController : Controller
{
    protected const int PageSize = 8;

    protected ListQuery BuildListQuery(string? search, string? sort, int page)
    {
        ViewData["Search"] = search;
        ViewData["Sort"] = sort;
        return new ListQuery(search, sort, page, PageSize);
    }

    protected void Flash(string message, string kind = "success") =>
        TempData["Flash"] = $"{kind}|{message}";

    protected IActionResult RedirectWithResult(OperationResult result, string successMessage, string action = "Index")
    {
        if (result.Succeeded)
            Flash(successMessage);
        else
            Flash(result.Error!, "danger");
        return RedirectToAction(action);
    }
}
