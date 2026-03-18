using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemo.Models;

namespace RazorPagesDemo.Pages;

public class AddItemModel : PageModel
{
    [BindProperty]
    public string Name { get; set; }

    public IActionResult OnPost()
    {
        IndexModel.Items.Add(new Item { Name = Name });

        return RedirectToPage("Index");
    }
}