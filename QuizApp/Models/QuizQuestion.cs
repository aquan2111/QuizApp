using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class QuizQuestion
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
