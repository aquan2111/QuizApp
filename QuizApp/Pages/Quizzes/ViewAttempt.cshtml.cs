using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Pages.Quizzes
{
    public class ViewAttemptModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewAttemptModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Quiz Quiz { get; set; }
        public QuizAttempt Attempt { get; set; }

        public async Task<IActionResult> OnGetAsync(int attemptId)
        {
            Attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.Answers)
                .ThenInclude(ua => ua.Question)
                .FirstOrDefaultAsync(a => a.Id == attemptId);

            if (Attempt == null)
            {
                return NotFound();
            }

            Quiz = Attempt.Quiz;
            return Page();
        }
    }
}
