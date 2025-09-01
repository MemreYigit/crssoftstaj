using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrsSoft.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [Authorize]
        [HttpPost("add/{gameId}")]
        public async Task<IActionResult> AddComment(int gameId, [FromBody] AddCommentRequestModel req)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { success = false, message = "User not authenticated" });
                }

                await commentService.AddComment(userId, gameId, req.Text);

                return Ok(new { success = true, message = "Comment added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetComments(int gameId)
        {
            try
            {
                var comments = await commentService.GetCommentsForGame(gameId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
