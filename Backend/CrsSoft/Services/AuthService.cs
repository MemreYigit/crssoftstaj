using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrsSoft.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _dataContext;
        private readonly IPasswordHasher<User> _passwordHasher;


        // Constructor
        public AuthService(ITokenService tokenService, DataContext dataContext, IPasswordHasher<User> passwordHasher)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        // Kullanıcıların siteye giriş yapması için oluşturuldu.
        public async Task<UserLoginResponseModel> LoginUser(UserLoginRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new ArgumentNullException(nameof(request.Email));
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ArgumentNullException(nameof(request.Password));
                }

                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

                if (verificationResult != PasswordVerificationResult.Success)
                {
                    throw new UnauthorizedAccessException();
                }

                var tokenInfo = await _tokenService.GenerateToken(new GenerateTokenRequestModel { UserId = user.Id, Email = user.Email, Name = user.Name });

                return new UserLoginResponseModel
                {
                    AuthenticateResult = true,
                    AuthToken = tokenInfo.Token,
                    AccessTokenExpireDate = tokenInfo.TokenExpireDate,
                    UserId = user.Id,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Kullanıcıların sitede hesap oluşturması için oluşturuldu.
        public async Task RegisterUser(UserRegisterRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    throw new ArgumentNullException(nameof(request.Name));
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new ArgumentNullException(nameof(request.Email));
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ArgumentNullException(nameof(request.Password));
                }

                var existUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existUser != null)
                {
                    throw new InvalidOperationException();
                }

                var newUser = new User
                {
                    Email = request.Email,
                    Name = request.Name,
                    Password = _passwordHasher.HashPassword(null, request.Password)
                };

                _dataContext.Users.Add(newUser);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
