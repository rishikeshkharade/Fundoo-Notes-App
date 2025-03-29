using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserEntity Register(RegisterModel model);
        public bool EmailChecker(string email);
        public string Login(LoginModel loginModel);
        public ForgetPasswordModel ForgetPassword(string Email);
        public bool ResetPassword(string Email, ResetPasswordModel resetpasswordmodel);
        public List<UserEntity> GetUsers();
        public UserEntity GetUserById(int Userid);
        public List<UserEntity> GetAUsers();
        public int UserCount();
        public List<UserEntity> AscendingOrder();
        public List<UserEntity> DescendingOrder();
        public int AverageAge();
        public int YougestUserAge();
        public int OldestAge();
    }
}
