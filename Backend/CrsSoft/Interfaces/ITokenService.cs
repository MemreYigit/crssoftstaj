using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface ITokenService
    {
        Task<GenerateTokenResponseModel> GenerateToken(GenerateTokenRequestModel request);
    }
}
