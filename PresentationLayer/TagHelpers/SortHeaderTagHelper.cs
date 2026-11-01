using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PresentationLayer.TagHelpers;

/// <summary>
/// Turns <c>&lt;th sort="name"&gt;Name&lt;/th&gt;</c> into a clickable header that toggles
/// ascending/descending order and keeps the current search term.
/// </summary>
[HtmlTargetElement("th", Attributes = "sort")]
public class SortHeaderTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{
    [HtmlAttributeName("sort")]
    public string Field { get; set; } = string.Empty;

    [ViewContext, HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var currentSort = ViewContext.ViewData["Sort"] as string;
        var search = ViewContext.ViewData["Search"] as string;
        var isActive = currentSort == Field || currentSort == Field + "_desc";
        var isDescending = currentSort == Field + "_desc";
        var nextSort = currentSort == Field ? Field + "_desc" : Field;

        var url = urlHelperFactory.GetUrlHelper(ViewContext).Action(null, new { sort = nextSort, search });
        var label = (await output.GetChildContentAsync()).GetContent();
        var icon = !isActive ? "bi-arrow-down-up opacity-25" : isDescending ? "bi-sort-down" : "bi-sort-up";

        output.Attributes.SetAttribute("aria-sort", !isActive ? "none" : isDescending ? "descending" : "ascending");
        output.Content.SetHtmlContent(
            $"<a class=\"sort-link\" href=\"{url}\">{label} <i class=\"bi {icon}\" aria-hidden=\"true\"></i></a>");
    }
}
