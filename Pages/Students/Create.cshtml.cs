using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        // Form binds to this instance
        [BindProperty]
        public Student Student { get; set; } = new Student();

        // Render empty form
        public IActionResult OnGet()
        {
            return Page();
        }

        // Handle submit
        public async Task<IActionResult> OnPostAsync()
        {
            // Server-side validation check
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            // Optional: show a success banner via _Layout.cshtml aria-live region
            TempData["Status"] = "Student created.";

            return RedirectToPage("./Index");
        }
    }
}
