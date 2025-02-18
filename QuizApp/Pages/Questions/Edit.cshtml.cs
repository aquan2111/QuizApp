using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizApp.Data;
using QuizApp.Models;
using System.Threading.Tasks;

namespace QuizApp.Pages.Questions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Question = await _context.Questions.FindAsync(id);
            if (Question == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Questions.Update(Question);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
