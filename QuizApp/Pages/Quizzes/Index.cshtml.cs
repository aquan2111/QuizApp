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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Quiz> Quizzes { get; set; }
        public List<Question> AllQuestions { get; set; } // For the list of all available questions

        // Method to retrieve the quizzes and available questions
        public async Task<IActionResult> OnGetAsync()
        {
            // Fetch all quizzes
            Quizzes = await _context.Quizzes.Include(q => q.QuizQuestions).ToListAsync();

            // Fetch all available questions (for selecting when creating a new quiz)
            AllQuestions = await _context.Questions.ToListAsync();

            return Page();
        }

        // Method to delete a quiz
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var quiz = await _context.Quizzes.Include(q => q.QuizQuestions).FirstOrDefaultAsync(q => q.Id == id);
            if (quiz != null)
            {
                // Remove any associations with questions (if any)
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        // Method to handle quiz creation, where questions are selected
        [BindProperty]
        public Quiz NewQuiz { get; set; }

        [BindProperty]
        public List<int> SelectedQuestionIds { get; set; } // List of selected questions for the new quiz

        // Handle POST request for creating a new quiz
        public async Task<IActionResult> OnPostCreateAsync()
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
                            // Create a new QuizQuestion object and associate the quiz and question
                            var quizQuestion = new QuizQuestion
                            {
                                QuizId = quiz.Id,
                                QuestionId = question.Id,
                                Quiz = quiz,
                                Question = question
                            };

                            // Add the new QuizQuestion to the QuizQuestions collection
                            quiz.QuizQuestions.Add(quizQuestion);
                        }
                    }

                    // Save the changes (association of questions with the quiz)
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage(); // Redirect back to the quiz list page
            }

            return Page(); // If something went wrong, return to the same page
        }
    }
}
