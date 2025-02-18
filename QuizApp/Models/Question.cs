using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string OptionA { get; set; } = string.Empty;

        public string OptionB { get; set; } = string.Empty;

        public string OptionC { get; set; } = string.Empty;

        public string OptionD { get; set; } = string.Empty;

        // Index for the correct option (A=0, B=1, C=2, D=3)
        public int CorrectOptionIndex { get; set; }
        public string CorrectAnswer
        {
            get
            {
                return CorrectOptionIndex switch
                {
                    0 => OptionA,
                    1 => OptionB,
                    2 => OptionC,
                    3 => OptionD,
                    _ => "Invalid Option"
                };
            }
        }

        // Navigation property for the quizzes this question belongs to
        public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}
