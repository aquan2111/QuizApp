using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        // Navigation property for related questions
        public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}
