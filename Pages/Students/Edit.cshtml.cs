using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        // Bound to the form fields in Edit.cshtml
        [BindProperty]
        public Student Student { get; set; } = default!;

        // GET: /Students/Edit?id=123
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            Student = student; // fill the bound model for the form
            return Page();
        }

        // POST: /Students/Edit
        public async Task<IActionResult> OnPostAsync()
        {
            // Block saving if validation fails
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Load the current entity from DB
            var studentToUpdate = await _context.Students
                .FirstOrDefaultAsync(s => s.ID == Student.ID);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            // Update only allowed fields (prevents overposting)
            studentToUpdate.LastName = Student.LastName;
            studentToUpdate.FirstMidName = Student.FirstMidName;
            studentToUpdate.EnrollmentDate = Student.EnrollmentDate;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Status"] = "Student updated.";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Someone else edited/deleted the row – show a friendly message
                ModelState.AddModelError(string.Empty, "Unable to save. The record was modified by another user.");
                return Page();
            }
        }
    }
}
