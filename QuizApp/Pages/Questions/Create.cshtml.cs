using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizApp.Data;
using QuizApp.Models;
using System.Threading.Tasks;

namespace QuizApp.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question Question { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
