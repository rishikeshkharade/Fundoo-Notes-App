using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
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

        [HttpGet]
        [Route("GetUsers")]

        public ActionResult GetUsers()
        {
            try
            {
                List<UserEntity> AllUsers = userManager.GetUsers();
                if (AllUsers == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "No Users are available" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Users Which are Available :-", FullData = AllUsers });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetUserById")]

        public ActionResult GetUserById(int id)
        {
            try
            {
                var checkUserId = userManager.GetUserById(id);
                if (checkUserId == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "User Id is not Present" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "User id is Present", Data = checkUserId });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetAUsers")]

        public ActionResult GetUserByName()
        {
            try
            {
                List<UserEntity> users = userManager.GetAUsers();
                if (users == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "No User is Present whose name starts with A" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Users Whose name starts with 'A' is :- ", FullData = users });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("UserCount")]

        public IActionResult CountOfUsers()
        {
            try
            {
                int count = userManager.UserCount();
                if (count == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "No User found" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Total count of Users", Data = count });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("AscendinOrder")]

        public IActionResult UserByAscendingOrder()
        {
            try
            {
                List<UserEntity> users = userManager.AscendingOrder();
                if (users == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to Order By" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Users By Name in Ascending Order", FullData = users });
                }
            } catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("DescendingOrder")]

        public IActionResult UserByDescendingOrder()
        {
            try
            {
                List<UserEntity> users = userManager.DescendingOrder();
                if (users == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to Order By" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Users By Name in Descending Order", FullData = users });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("AverageAge")]

        public IActionResult AverageAgeOfUsers()
        {
            try
            {
                int averageAge = userManager.AverageAge();
                if (averageAge == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Unable to take the Average Age" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Average Age of Users is :-", Data = averageAge });
                }
            }
            catch (Exception ex) { throw ex; }
        }


        [HttpGet]
        [Route("YoungestAge")]

        public IActionResult YoungestAgeUser()
        {
            try
            {
                int age = userManager.YougestUserAge();
                if (age == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Unable to Find the Younger one" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = " Younger User is :- ", Data = age });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("OldestAge")]

        public IActionResult OldestAgeOfUsers()
        {
            try
            {
                int age = userManager.OldestAge();
                if (age == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Unable to Find the Older one" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Oldest Age User is :- ", Data = age });
                }
            }
            catch (Exception ex) { throw ex; }
        }

       
    }
}
