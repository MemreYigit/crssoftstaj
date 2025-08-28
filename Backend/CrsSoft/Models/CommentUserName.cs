namespace CrsSoft.Models
{
    public class CommentUserName
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int GameID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; } 
    }
}
