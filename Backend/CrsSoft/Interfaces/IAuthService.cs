using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponseModel> LoginUser (UserLoginRequestModel request);
        Task RegisterUser (UserRegisterRequestModel request);
    }
}
