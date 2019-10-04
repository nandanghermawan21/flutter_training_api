using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace InovaTrackApi_SBB.Services
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        Customer AuthenticateCustomer(string email, string password);
        Driver AuthenticateDriver(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private ApplicationDbContext db;

        public AuthService(IOptions<AppSettings> appSettings, ApplicationDbContext db)
        {
            _appSettings = appSettings.Value;
            this.db = db;
        }

        public User Authenticate(string username, string password)
        {
            var hash = PasswordHasher.Hash(username, password);
            var user = db.Users.SingleOrDefault(x => x.UserName == username && x.Password == hash);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public Customer AuthenticateCustomer(string email, string password)
        {
            var hash = PasswordHasher.Hash(email, password);
            var customer = db.Customers.SingleOrDefault(x => x.Email == email && x.Password == hash && x.IsDeleted != true);

            // return null if user not found
            if (customer == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiredTime = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, customer.CustomerId.ToString())
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            customer.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            customer.Password = null;
            customer.TokenExpiredTime = expiredTime.ToString("yyyy-MM-dd HH:mm:ss");

            return customer;
        }

        public Driver AuthenticateDriver(string email, string password)
        {
            var hash = PasswordHasher.Hash(email, password);
            var driver = db.Drivers.SingleOrDefault(x => x.Email == email && x.Password == hash);

            // return null if user not found
            if (driver == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiredTime = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, driver.DriverId.ToString())
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            driver.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            driver.Password = null;
            driver.TokenExpiredTime = expiredTime.ToString("yyyy-MM-dd HH:mm:ss");

            return driver;
        }
    }
}
