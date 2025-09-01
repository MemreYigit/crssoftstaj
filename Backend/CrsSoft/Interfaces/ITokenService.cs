using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface ITokenService
    {
        Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);
    }
}
