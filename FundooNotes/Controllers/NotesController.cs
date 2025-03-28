using System;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager notesManager;

        public NotesController(INotesManager notesManager)
        {
            this.notesManager = notesManager;
        }

        [HttpPost]
        [Route("CreateNotes")]

        public IActionResult CreateNote(NotesModel notesmodel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                NotesEntity notesEntity = notesManager.CreateNote(UserId, notesmodel);
                if (notesEntity == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Notes not Created" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Added Succesfully", Data = notesEntity });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
