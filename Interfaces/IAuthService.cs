using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IAuthService
    {
        public Task<UserLoginResponse> LoginUserAsync (UserLoginRequest request);
        public Task RegisterUserAsync (UserRegisterRequest request);
    }
}
