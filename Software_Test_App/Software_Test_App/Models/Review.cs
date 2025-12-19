namespace Software_Test_App.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }

        public int EntryId { get; set; }
    }
}
