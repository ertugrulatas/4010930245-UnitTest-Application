namespace Software_Test_App.Models
{
    public class SearchLog
    {
        public int Id { get; set; }
        public string Query { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
