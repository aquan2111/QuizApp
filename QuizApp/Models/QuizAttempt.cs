using System;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime AttemptDate { get; set; }

        public int Score { get; set; }
        public List<QuizAttemptAnswer> Answers { get; set; } = new List<QuizAttemptAnswer>();

    }
}
