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

        public async Task<UserWithoutPassword> GetUserDetails(int userId)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user  == null)
                {
                    throw new Exception("User not found");
                }

                return new UserWithoutPassword
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

        public async Task ChangeName(int userId, string newName)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new Exception("Name cannot be empty");
                }

                user.Name = newName;
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing name: {ex.Message}");
            }
        }

        public async Task ChangeSurname(int userId, string newSurname)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (string.IsNullOrWhiteSpace(newSurname))
                {
                    throw new Exception("Surname cannot be empty");
                }

                user.Surname = newSurname;
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing surname: {ex.Message}");
            }
        }
        
        public async Task ChangeEmail(int userId, string newEmail)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    throw new Exception("Email cannot be empty");
                }
                if (!newEmail.Contains("@"))
                {
                    throw new Exception("Invalid email format");
                }

                user.Email = newEmail;
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing email: {ex.Message}");
            }
        }

        public async Task ChangePassword(int userId, string newPassword)
        {
            try
            {
                var user = await dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    throw new Exception("Password must be at least 6 characters long");
                }

                user.Password = passwordHasher.HashPassword(user, newPassword);
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing password: {ex.Message}");
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
    }
}