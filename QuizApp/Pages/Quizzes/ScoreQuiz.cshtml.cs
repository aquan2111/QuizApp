using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Pages.Quizzes
{
    public class ScoreQuizModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ScoreQuizModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Quiz Quiz { get; set; }
        public List<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>(); // Store multiple attempts

        public async Task<IActionResult> OnGetAsync(int quizId)
        {
            // Retrieve the quiz
            Quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId);
            if (Quiz == null)
            {
                return NotFound(); // Return 404 if the quiz doesn't exist
            }

            // Retrieve all attempts for this quiz
            QuizAttempts = await _context.QuizAttempts
                .Where(a => a.QuizId == quizId)
                .OrderByDescending(a => a.AttemptDate) // Show latest attempts first
                .ToListAsync();

            return Page();
        }
    }
}
