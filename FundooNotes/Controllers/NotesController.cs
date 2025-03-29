using System;
using System.Collections.Generic;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager notesManager;

        public NotesController(INotesManager notesManager)
        {
            this.notesManager = notesManager;
        }

        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel notesModel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                NotesEntity notesEntity = notesManager.CreateNote(UserId, notesModel);
                if (notesEntity == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Note Not Created" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Note Added Successfully", Data = notesEntity });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetNotes")]
        public ActionResult GetNotes()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                List<NotesEntity> AllNotes = notesManager.GetNotes();
                if (AllNotes == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "No Notes are available" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Which are Available :-", FullData = AllNotes });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetingNotesByTitleAndDescription")]

        public IActionResult GetNoteByTitleAndDescrip(string Title, string Description)
        {
            try
            {
                List<NotesEntity> notes = notesManager.GetingNotesByTitleAndDescription(Title, Description);

                if (notes == null)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "No Notes are Available" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Fetch Successfully", FullData = notes });
                }
            }
            catch (Exception ex) { throw ex; }
        }


        [HttpGet]
        [Route("NotesCount")]
        public IActionResult CountNoteForSingleUser()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int count = notesManager.CountOfNotes(UserId);
                if (count == 0)
                {
                    return BadRequest(new ResponseModel<int> { Success = false, Message = "Failed to get notes" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Count of Notes is", Data = count});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

            
}
