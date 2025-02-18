using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Pages.Quizzes
{
    public class TakeQuizModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TakeQuizModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Quiz Quiz { get; set; } // Holds the quiz being taken
        public Dictionary<int, List<(string OptionText, string OptionValue)>> ShuffledOptions { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> UserAnswers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Question)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quiz == null)
            {
                return NotFound(); // Return 404 if the quiz doesn't exist
            }

            // Shuffle options for each question
            foreach (var quizQuestion in Quiz.QuizQuestions)
            {
                var question = quizQuestion.Question;
                var options = new List<(string, string)>
                {
                    (question.OptionA, "A"),
                    (question.OptionB, "B"),
                    (question.OptionC, "C"),
                    (question.OptionD, "D")
                };

                // Shuffle options
                var random = new Random();
                ShuffledOptions[question.Id] = options.OrderBy(_ => random.Next()).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Question)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            int totalQuestions = quiz.QuizQuestions.Count;
            int correctAnswers = 0;
            var attempt = new QuizAttempt
            {
                QuizId = quiz.Id,
                TotalQuestions = totalQuestions,
                AttemptDate = DateTime.UtcNow,
                Answers = new List<QuizAttemptAnswer>()
            };

            foreach (var quizQuestion in quiz.QuizQuestions)
            {
                if (UserAnswers.TryGetValue(quizQuestion.Question.Id, out string userAnswer))
                {
                    string correctAnswer = quizQuestion.Question.CorrectOptionIndex switch
                    {
                        0 => "A",
                        1 => "B",
                        2 => "C",
                        3 => "D",
                        _ => ""
                    };

                    bool isCorrect = userAnswer == correctAnswer;
                    if (isCorrect) correctAnswers++;

                    attempt.Answers.Add(new QuizAttemptAnswer
                    {
                        QuestionId = quizQuestion.Question.Id,
                        UserAnswer = userAnswer,
                        IsCorrect = isCorrect
                    });
                }
            }

            attempt.Score = correctAnswers;
            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Quizzes/ScoreQuiz", new { quizId = quiz.Id });
        }
    }
}
