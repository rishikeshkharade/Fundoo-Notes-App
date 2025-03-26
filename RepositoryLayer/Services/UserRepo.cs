using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext context;
        public UserRepo(FundooDBContext context)
        {
            this.context = context;
        }

        public static string EncodePasswordToBased(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF32.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch(Exception ex)
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
            user.Password = model.Password;
            this.context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public bool EmailChecker(string email)
        {
            var result = this.context.Users.FirstOrDefault(x => x.Email == email);
            if(result == null)
            {
                return false;
            }
            return true;
        }
    }
}
