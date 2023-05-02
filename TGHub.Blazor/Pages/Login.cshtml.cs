using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TGHub.Blazor.Pages;

public class Login : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? RedirectUrl { get; set; }

    public void OnGet()
    {
        
    }
}