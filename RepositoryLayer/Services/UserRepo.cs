﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading;
using CommonLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext context;

        private readonly IConfiguration configuration;
        public UserRepo(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        private string EncodePasswordToBased(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public UserEntity Register(RegisterModel model)
        {
            UserEntity user = new UserEntity();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DOB = model.DOB;
            user.Gender = model.Gender;
            user.Email = model.Email;
            user.Password = EncodePasswordToBased(model.Password);
            this.context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public bool EmailChecker(string email)
        {
            var result = this.context.Users.FirstOrDefault(x => x.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public string Login(LoginModel loginmodel)
        {
            var checkUser = this.context.Users.FirstOrDefault(q => q.Email == loginmodel.Email && q.Password == EncodePasswordToBased(loginmodel.Password));
            if (checkUser != null)
            {
                var token = GenerateToken(checkUser.Email, checkUser.UserId);
                return token;
            }
            return null;
        }

        public ForgetPasswordModel ForgetPassword(string Email)
        {
            UserEntity User = context.Users.ToList().Find(user => user.Email == Email);
            ForgetPasswordModel forgetPassword = new ForgetPasswordModel();
            forgetPassword.Email = User.Email;
            forgetPassword.UserId = User.UserId;
            forgetPassword.Token = GenerateToken(User.Email, User.UserId);
            return forgetPassword;
        }

        public bool ResetPassword(string Email, ResetPasswordModel resetpasswordmodel)
        {
            UserEntity userEntity = context.Users.ToList().Find(user => user.Email == Email);

            if (EmailChecker(userEntity.Email))
            {
                userEntity.Password = EncodePasswordToBased(resetpasswordmodel.ConfirmPassword);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        private string GenerateToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("EmailId",email),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1.00),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public List<UserEntity> GetUsers()
        {
            List<UserEntity> userEntities = context.Users.ToList();
            if (userEntities != null)
            {
                return userEntities;
            }
            return null;

        }

        public UserEntity GetUserById(int Userid)
        {
            UserEntity userEntity = context.Users.FirstOrDefault(id => id.UserId == Userid);
            if (userEntity == null)
            {
                return null;
            }
            else
            {
                return userEntity;
            }
        }

        public List<UserEntity> GetAUsers()
        {
            List<UserEntity> user = context.Users.Where(user => user.FirstName.StartsWith("A")).ToList();
            return user;
        }

        public int UserCount()
        {
            int count = context.Users.Count();
            return count;
        }

        public List<UserEntity> AscendingOrder()
        {
            List<UserEntity> userEntities = context.Users.OrderBy(n => n.FirstName).ToList();
            return userEntities;
        }

        public List<UserEntity> DescendingOrder()
        {
            List<UserEntity> userEntities = context.Users.OrderByDescending(n => n.FirstName).ToList();
            return userEntities;
        }

        public int AverageAge()
        {
            var averageAge = context.Users.Average(user => DateTime.Today.Year - user.DOB.Year - (user.DOB.Date > DateTime.Today.AddYears(-1) ? 1 : 0));
            return (int)averageAge;
        }

        public int YougestUserAge()
        {
            var youngestAge = context.Users
        .Select(user => DateTime.Today.Year - user.DOB.Year - (user.DOB > DateTime.Today.AddYears(-1) ? 1 : 0))
        .Min();
            return youngestAge;
        }

        public int OldestAge()
        {
            var oldestAge = context.Users
        .Select(user => DateTime.Today.Year - user.DOB.Year - (user.DOB > DateTime.Today.AddYears(-1) ? 1 : 0))
        .Max();
            return oldestAge;
        }
    }
}
