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
                    return Ok(new ResponseModel<int> { Success = true, Message = "Count of Notes is", Data = count });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("PinNote")]

        public IActionResult PinNote(int NoteId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int pinResult = notesManager.PinNote(NoteId, UserId);

                if (pinResult == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Pin not Done" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Pinned the Note", Data = pinResult });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPut]
        [Route("ArchiveNote")]

        public IActionResult ArchiveNote(int NoteId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int archiveResult = notesManager.ArchiveNote(NoteId, UserId);
                if (archiveResult == 0)
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Archive Failed" });
                }
                else
                {
                    return Ok(new ResponseModel<int> { Success = true, Message = "Notes Archived", Data = archiveResult });
                }
            }
            catch (Exception ex) { throw ex; }
        }



        [HttpPut]
        [Route("AddColourInNote")]

        public IActionResult AddColourInNote(int NoteId, string colour)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool addingColour = notesManager.AddColourInNote(NoteId, UserId, colour);
                if (addingColour)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Colour Added" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Colour Not Added" });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("NoteInTrash")]

        public IActionResult TrashingNote(int NoteId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int trashResult = notesManager.NoteInTrash(NoteId, UserId);
                if (trashResult > 0)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Note in Trash" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed to Trash" });
                }
            }
            catch (Exception ex) { throw ex; }
        }


        [HttpPut]
        [Route("NotesReminder")]

        public IActionResult RemindingNotes(int NoteId, DateTime Reminder)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool addReminder = notesManager.AddReminder(Reminder, UserId, NoteId);
                if (addReminder)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Reminder Added", Data = addReminder });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed to Add Reminder", Data = addReminder });
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpDelete]
        [Route("DeleteNote")]
        public IActionResult DeletingNote(int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var IsNotePresent = notesManager.DeleteNote(NoteId);
            if (IsNotePresent)
            {
                return Ok(new ResponseModel<bool> { Success = true, Message = "Data deleted Successfully" });
            }
            return BadRequest(new ResponseModel<bool> { Success = false, Message = "Data Not Deleted" });
        }


        [HttpPut]
        [Route("UpdateNote")]

        public IActionResult UpdatingNote(int NoteId, UpdateNoteModel updateNote)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            NotesEntity u = notesManager.UpdateNote(NoteId, UserId, updateNote);

            if (u != null)
            {
                return Ok(u);
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed to Update" });
            }
        }


        [HttpPut]
        [Route("AddImage")]

        public IActionResult AddingImage(int NoteId, IFormFile image)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool imageAdded = notesManager.AddImage(NoteId, UserId, image);
            if (imageAdded)
            {
                return Ok(new ResponseModel<bool> { Success = true, Message = "Image Added Successfully", Data = imageAdded });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to Add Image" });
            }
        }

        [HttpPut]
        [Route("AddCollaborator")]

        public IActionResult AddCollaborator(int NoteId, string CollaboratorEmail)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool collaboratorAdded = notesManager.AddCollaborator(NoteId, UserId, CollaboratorEmail);
            if (collaboratorAdded)
            {
                return Ok(new ResponseModel<bool> { Success = true, Message = "Collaborator Added Successfully", Data = collaboratorAdded });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to Add Collaborator" });
            }
        }

        [HttpGet]
        [Route("GetCollaborators")]

        public IActionResult GetCollaborators(int NoteId)
        {
            List<CollaboratorEntity> collaborators = notesManager.GetCollaborators(NoteId);
            if (collaborators == null)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "No Collaborators are available" });
            }
            else
            {
                return Ok(new ResponseModel<CollaboratorEntity> { Success = true, Message = "Collaborators Which are Available :-", FullData = collaborators });
            }
        }

        [HttpPut]
        [Route("RemoveCollaborator")]

        public IActionResult RemoveCollaborator(int NoteId, string CollaboratorEmail)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool collaboratorRemoved = notesManager.RemoveCollaborator(NoteId, UserId, CollaboratorEmail);
            if (collaboratorRemoved)
            {
                return Ok(new ResponseModel<bool> { Success = true, Message = "Collaborator Removed Successfully", Data = collaboratorRemoved });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to Remove Collaborator" });
            }
        }

        }
    }
