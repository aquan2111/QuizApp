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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz NewQuiz { get; set; }

        [BindProperty]
        public List<int> SelectedQuestionIds { get; set; } // List of selected questions for the new quiz

        public List<Question> AllQuestions { get; set; } // List of all available questions for selection

        // GET method to retrieve all questions
        public async Task<IActionResult> OnGetAsync()
        {
            AllQuestions = await _context.Questions.ToListAsync();
            return Page();
        }

        // POST method to create a new quiz
        public async Task<IActionResult> OnPostAsync()
        {
            if (NewQuiz != null && SelectedQuestionIds != null)
            {
                // Add the new quiz to the database
                _context.Quizzes.Add(NewQuiz);
                await _context.SaveChangesAsync();

                // Fetch the newly created quiz from the database to assign selected questions
                var quiz = await _context.Quizzes.Include(q => q.QuizQuestions).FirstOrDefaultAsync(q => q.Id == NewQuiz.Id);

                if (quiz != null)
                {
                    // Associate the selected questions with the new quiz using QuizQuestion
                    foreach (var questionId in SelectedQuestionIds)
                    {
                        var question = await _context.Questions.FindAsync(questionId);
                        if (question != null)
                        {
                            var quizQuestion = new QuizQuestion
                            {
                                QuizId = quiz.Id,
                                QuestionId = question.Id,
                                Quiz = quiz,
                                Question = question
                            };

                            quiz.QuizQuestions.Add(quizQuestion);
                        }
                    }

                    // Save the changes (association of questions with the quiz)
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/Quizzes/Index"); // Redirect back to the quiz list page
            }

            return Page(); // If something went wrong, return to the same page
        }
    }
}
