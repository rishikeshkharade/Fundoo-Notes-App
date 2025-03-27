using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IBus bus;
        public UsersController(IUserManager userManager, IBus bus)
        {
            this.userManager = userManager;
            this.bus = bus;
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

        public IActionResult Login(LoginModel loginmodel)
        {
            var result = userManager.Login(loginmodel);
            if (result != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Login Successfull", Data = result });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "Login Failed", Data = result });
        }

        [HttpPost]
        [Route("ForgetPass")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            try
            {
                if (userManager.EmailChecker(Email))
                {

                    SendingMailToUser send = new SendingMailToUser();
                    ForgetPasswordModel forgetPasswordModel = userManager.ForgetPassword(Email);
                    send.SendingMail(forgetPasswordModel.Email, forgetPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgetPasswordModel);

                    return Ok(new ResponseModel<string> { Success = true, Message = "Mail Sent SuccessFully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Mail is not sent" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]

        public ActionResult ResetPassword(ResetPasswordModel reset)
        {
            try
            {
                string Email = User.FindFirst("EmailId").Value;
                if (userManager.ResetPassword(Email, reset))
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Password Changed" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Password Not Cheanged" });
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }

}
