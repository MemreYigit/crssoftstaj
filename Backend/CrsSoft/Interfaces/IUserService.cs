using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IUserService
    {
        Task<UserWithoutPassword> GetUserDetails(int userId);
        Task AddFunds(int userId, decimal amount);
        Task EditProfile(int userId, EditProfileRequest request);
        Task ChangePassword(int userId, ChangePasswordRequest request);
    }
}
