using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> LoginUserAsync (UserLoginRequest request);
        Task RegisterUserAsync (UserRegisterRequest request);
    }
}
