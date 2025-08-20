using System.ComponentModel.DataAnnotations;

namespace esok.api.Data
{
    public class ChatbotQuestionAnswer
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public double Score { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
