using CrsSoft.Entities;
using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface ICommentService
    {
        Task AddComment(int userId, int gameId, string content);
        Task<List<CommentUserName>> GetCommentsForGame(int gameId);
    }
}
