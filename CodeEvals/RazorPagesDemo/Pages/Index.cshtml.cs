using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemo.Models;

namespace RazorPagesDemo.Pages;

public class IndexModel : PageModel
{
    public static List<Item> Items = new();

    public void OnGet()
    {
    }
}