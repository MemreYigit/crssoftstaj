namespace CrsSoft.Interfaces
{
    public interface ICommentService
    {
        public Task AddComment(int userId, int gameId, string content);
    }
}
