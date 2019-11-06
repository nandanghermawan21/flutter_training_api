using InovaTrackApi_SBB.DataModel;
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
        Sales AuthenticateSales(string email, string password);
        Driver AuthenticateDriver(string email, string password);
        QcMaster AuthenticateQc(string email, string password);

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
            var hash = PasswordHasher.Hash("", password);
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

        public Customer AuthenticateCustomer(string userId, string password)
        {
            var hash = PasswordHasher.Hash("", password);

            var customer = db.Customers.SingleOrDefault(x => x.Email == userId && x.Password == hash && x.IsDeleted != true);

            // return null if user not found
            if (customer == null)
            {
                //check with no telpon
                customer = new CustomerModel(db, _appSettings).CheckPhoneExist(userId);

                if (customer == null)
                    return null;

                if (customer.Password != hash)
                    return null;

            }

            //cahange immage url
            customer.customerAvatar = $@"{_appSettings.DownloadBaseUrl}\{customer.customerAvatar}";
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiredTime = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, customer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Actor, "C" /*code untuk source customer*/),
                    new Claim(ClaimTypes.MobilePhone, customer.MobileNumber /*code untuk source customer*/),
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            customer.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            customer.Password = null;
            customer.TokenExpiredTime = expiredTime;

            return customer;
        }

        public Sales AuthenticateSales(string email, string password)
        {
            var hash = PasswordHasher.Hash("", password);
            var sales = db.Saleses.SingleOrDefault(x => x.email == email && x.password == hash && x.isdeleted != true);

            // return null if user not found
            if (sales == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiredTime = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, sales.salesId.ToString()),
                    new Claim(ClaimTypes.Actor, "S" /*code untuk source sales*/),
                    new Claim(ClaimTypes.MobilePhone, sales.phone /*code untuk source customer*/),
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            sales.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            sales.password = null;
            sales.TokenExpiredTime = expiredTime;

            return sales;
        }

        public Driver AuthenticateDriver(string email, string password)
        {
            var hash = PasswordHasher.Hash("", password);
            var driver = db.Drivers.FirstOrDefault(x => x.email == email && x.password == hash);

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
                    new Claim(ClaimTypes.Sid, driver.driverId.ToString()),
                    new Claim(ClaimTypes.Actor, "D" /*code source untuk driver*/),
                    new Claim(ClaimTypes.MobilePhone, driver.phoneNumber ?? ""),
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            driver.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            driver.password = null;
            driver.TokenExpiredTime = expiredTime;

            return driver;
        }

        public QcMaster AuthenticateQc(string email, string password)
        {
            var hash = PasswordHasher.Hash("", password);
            var qc = db.QcMasters.FirstOrDefault(x => x.email == email && x.password == hash);

            // return null if user not found
            if (qc == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiredTime = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, qc.nik.ToString()),
                    new Claim(ClaimTypes.Actor, "Q" /*code source untuk driver*/),
                    new Claim(ClaimTypes.MobilePhone, qc.phone ?? ""),
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            qc.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            qc.password = null;
            qc.TokenExpiredTime = expiredTime;

            return qc;
        }
    }}
