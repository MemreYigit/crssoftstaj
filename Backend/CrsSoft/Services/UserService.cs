using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Identity;

namespace CrsSoft.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext dataContext;
        private readonly IPasswordHasher<User> passwordHasher;

        public UserService(DataContext dataContext, IPasswordHasher<User> passwordHasher)
        {
            this.dataContext = dataContext;
            this.passwordHasher = passwordHasher;
        }

        public async Task<UserWithoutPasswordModel> GetUserDetails(int userId)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return new UserWithoutPasswordModel
                {
                    Name = user.Name,
                    Surname = user.Surname ?? string.Empty,
                    Email = user.Email,
                    Money = user.Money
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user details: {ex.Message}");
            }
        }

        public async Task AddFunds(int userId, decimal amount)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (amount <= 0)
                {
                    throw new Exception("Amount must be greater than zero");
                }

                user.Money += amount;
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding funds: {ex.Message}");
            }
        }

        public async Task EditProfile(int userId, EditProfileRequestModel request)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    user.Name = request.Name;
                }

                if (!string.IsNullOrWhiteSpace(request.Surname))
                {
                    user.Surname = request.Surname;
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    if (!request.Email.Contains("@"))
                    {
                        throw new Exception("Invalid email format");
                    }
                    user.Email = request.Email;
                }

                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error editing profile: {ex.Message}");
            }
        }

        public async Task ChangePassword(int userId, ChangePasswordRequestModel request)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.CurrentPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Current password is incorrect");
                }

                if (string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    throw new Exception("New password cannot be empty");
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    throw new Exception("New password and confirmation do not match");
                }

                if (request.NewPassword == request.CurrentPassword) 
                {
                    throw new Exception("New password must be different from the current password");
                }

                user.Password = passwordHasher.HashPassword(user, request.NewPassword);
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}