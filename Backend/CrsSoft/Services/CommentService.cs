using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;

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

    }
}
