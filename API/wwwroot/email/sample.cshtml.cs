using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Email.Model
{
    public class sampleModel : PageModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public void OnGet()
        {
        }
    }
}
