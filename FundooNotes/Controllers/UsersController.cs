using System.Net.Http;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager userManager;
        public UsersController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        //httplocal/api/Users/Reg
        [HttpPost]
        [Route("Reg")]
        public IActionResult Register(RegisterModel model)
        {
            var check = userManager.EmailChecker(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration Failed" });
            }
            else
            {
                var result = userManager.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Registration Successfull", Data = result });
                }
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration Failed", Data = result });
            }
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login(LoginModel loginmodel) {
            var result = userManager.Login(loginmodel);
            if (result != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Login Successfull", Data = result });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "Login Failed", Data = result });
        }

    }
}
