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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz NewQuiz { get; set; }

        [BindProperty]
        public List<int> SelectedQuestionIds { get; set; } // List of selected questions for the quiz

        public List<Question> AllQuestions { get; set; } // List of all available questions

        // GET method to retrieve the quiz to be edited and all available questions
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the quiz with its associated questions
            NewQuiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(q => q.Question)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (NewQuiz == null)
            {
                return NotFound(); // If the quiz is not found, return 404
            }

            // Get all available questions
            AllQuestions = await _context.Questions.ToListAsync();

            // Pre-select the questions already associated with this quiz
            SelectedQuestionIds = NewQuiz.QuizQuestions.Select(q => q.QuestionId).ToList();

            return Page();
        }

        // POST method to handle the form submission for editing a quiz
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (NewQuiz != null && SelectedQuestionIds != null)
            {
                // Retrieve the quiz to edit
                var quizToUpdate = await _context.Quizzes
                    .Include(q => q.QuizQuestions)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (quizToUpdate == null)
                {
                    return NotFound(); // If the quiz doesn't exist
                }

                // Update the quiz title
                quizToUpdate.Title = NewQuiz.Title;

                // Remove existing question associations
                _context.QuizQuestions.RemoveRange(quizToUpdate.QuizQuestions);

                // Add new question associations
                foreach (var questionId in SelectedQuestionIds)
                {
                    var quizQuestion = new QuizQuestion
                    {
                        QuizId = quizToUpdate.Id,
                        QuestionId = questionId
                    };
                    _context.QuizQuestions.Add(quizQuestion);
                }

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return RedirectToPage("/Quizzes/Index"); // Redirect back to the quiz list page
            }

            return Page(); // If something goes wrong, return to the same page
        }
    }
}
