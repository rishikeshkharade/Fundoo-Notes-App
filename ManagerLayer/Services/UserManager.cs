using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepo userRepo;
        public UserManager(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity Register(RegisterModel model)
        {
            return userRepo.Register(model);
        }

        public bool EmailChecker(string email)
        {
            return userRepo.EmailChecker(email);
        }

        public string Login(LoginModel loginmodel)
        {
            return userRepo.Login(loginmodel);
        }

        public ForgetPasswordModel ForgetPassword(string Email)
        {
            return userRepo.ForgetPassword(Email);
        }

        public bool ResetPassword(string Email, ResetPasswordModel resetpasswordmodel)
        {
            return userRepo.ResetPassword(Email, resetpasswordmodel);
        }

        public List<UserEntity> GetUsers()
        {
            return userRepo.GetUsers();
        }

        public UserEntity GetUserById(int Userid)
        {
            return userRepo.GetUserById(Userid);
        }

        public List<UserEntity> GetAUsers()
        {
            return userRepo.GetAUsers();
        }

        public int UserCount()
        {
            return userRepo.UserCount();
        }

        public List<UserEntity> AscendingOrder()
        {
            return userRepo.AscendingOrder();
        }
        public List<UserEntity> DescendingOrder()
        {
            return userRepo.DescendingOrder();
        }

        public int AverageAge()
        {
            return userRepo.AverageAge();
        }
        public int YougestUserAge()
        {
            return userRepo.YougestUserAge();
        }
        public int OldestAge()
        {
            return userRepo.OldestAge();
        }
    }
}
