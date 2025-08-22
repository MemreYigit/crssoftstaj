using Azure;
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
        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            // Eğer emailini yazmassa 
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentNullException(nameof(request.Email));
            }

            // Eğer passwordunu yazmassa
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentNullException(nameof(request.Password));
            }

            // Kullanıcıyı databaseden buluyor
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Eğer bu email ve şifreye ait kullanıcı yoksa
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }


            // Kullanıcının veritabanındaki hashlenmiş şifresi ile istekte gelen düz şifreyi karşılaştırır
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            // Şifre yanlış ise 
            if (verificationResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException();
            }

            // Site içine girebilmesi için gerekli token oluşturuluyor
            var tokenInfo = await _tokenService.GenerateToken(new GenerateTokenRequest { UserId = user.Id, Email = user.Email, Name = user.Name});

            return new UserLoginResponse
            {
                AuthenticateResult = true,
                AuthToken = tokenInfo.Token,
                AccessTokenExpireDate = tokenInfo.TokenExpireDate,
                UserId = user.Id,
            };
        }

        // Kullanıcıların sitede hesap oluşturması için oluşturuldu.
        public async Task RegisterUserAsync(UserRegisterRequest request)
        {
            // Eğer adını yazmassa
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentNullException(nameof(request.Name));
            }

            // Eğer emaili yazmassa
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentNullException(nameof(request.Email));
            }

            // Eğer şifresini yazmassa
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentNullException(nameof(request.Password));
            }

            // Eğer bu emailde bir hesap varsa işlem gerçekleştirilemiyor
            var existUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existUser != null)
            {
                throw new InvalidOperationException();
            }

            // Yeni kullanıcımız başarılı bir şekilde oluşturuldu
            var newUser = new User { 
                Email = request.Email, 
                Name = request.Name,
                // Kullanıcının şifresi hashlenerek databasede güvenli bir şekilde saklanıyor
                Password = _passwordHasher.HashPassword(null, request.Password) 
            };

            // Yeni kullanıcımız başarılı bir şekilde database yüklendi
            _dataContext.Users.Add(newUser);
            await _dataContext.SaveChangesAsync();
        }
    }
}
