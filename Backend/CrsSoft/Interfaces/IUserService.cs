using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IUserService
    {
        Task<UserWithoutPasswordModel> GetUserDetails(int userId);
        Task AddFunds(int userId, decimal amount);
        Task EditProfile(int userId, EditProfileRequestModel request);
        Task ChangePassword(int userId, ChangePasswordRequestModel request);
    }
}
