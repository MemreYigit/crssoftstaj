using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.EntityFrameworkCore;

namespace CrsSoft.Services
{
    public class CommentService : ICommentService
    {
        private readonly DataContext dataContext;

        public CommentService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
        public async Task AddComment(int userId, int gameId, string content)
        {
            try
            {
                var comment = new Comment
                {
                    CommentText = content,
                    CreatedAt = DateTime.UtcNow,
                    UserID = userId,
                    GameID = gameId
                };

                await dataContext.Comments.AddAsync(comment);
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding comment by user {userId} on product {gameId}: {ex.Message}");
            }
        }

        public async Task<List<CommentUserNameModel>> GetCommentsForGame(int gameId)
        {
            try
            {
                var comments = await dataContext.Comments
                    .Where(c => c.GameID == gameId)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CommentUserNameModel
                    {
                        Id = c.Id,
                        CommentText = c.CommentText,
                        CreatedAt = c.CreatedAt,
                        GameID = c.GameID,
                        UserID = c.UserID,
                        UserName = c.User.Name
                    })
                    .ToListAsync();

                return comments;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving comments for game {gameId}: {ex.Message}");
            }
        }
    }
}
