using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class QuizAttemptAnswer
    {
        [Key]
        public int Id { get; set; }

        public int QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public string UserAnswer { get; set; } = string.Empty; // The option selected by the user (A, B, C, or D)
        public bool IsCorrect { get; set; } // Whether the answer is correct
    }
}
