using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class NotesManager : INotesManager
    {
        private readonly INotesRepo notesrepo;
        public NotesManager(INotesRepo notesrepo)
        {
            this.notesrepo = notesrepo;
        }

        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            return notesrepo.CreateNote(UserId, notesModel);
        }

        public List<NotesEntity> GetNotes()
        {
            return notesrepo.GetNotes();
        }

        public List<NotesEntity> GetingNotesByTitleAndDescription(string title, string description)
        {
               return notesrepo.GetingNotesByTitleAndDescription(title, description);
        }
        public int CountOfNotes(int UserId)
        {
            return notesrepo.CountOfNotes(UserId);
        }
        public int PinNote(int NotesId, int UserId)
        {
            return notesrepo.PinNote(NotesId, UserId);
        }
        public int ArchiveNote(int NotesId, int UserId)
        {
            return notesrepo.ArchiveNote(NotesId, UserId);
        }

        public bool AddColourInNote(int NotesId, int UserId, string colour)
        {
            return notesrepo.AddColourInNote(NotesId, UserId, colour);
        }
        public int NoteInTrash(int NotesId, int UserId)
        {
            return notesrepo.NoteInTrash(NotesId, UserId);
        }
        public bool AddReminder(DateTime Reminder, int UserId, int NoteId)
        {
            return notesrepo.AddReminder(Reminder, UserId, NoteId);
        }
        public bool DeleteNote(int NoteId)
        {
            return notesrepo.DeleteNote(NoteId);
        }
        public NotesEntity UpdateNote(int NoteId, int UserId, UpdateNoteModel model)
        {
            return notesrepo.UpdateNote(NoteId, UserId, model);
        }
        public bool AddImage(int NoteId, int UserId, IFormFile image)
        {
            return notesrepo.AddImage(NoteId, UserId, image);
        }
        public bool AddCollaborator(int NoteId, int UserId, string CollaboratorEmail)
        {
            return notesrepo.AddCollaborator(NoteId, UserId, CollaboratorEmail);
        }
        public List<CollaboratorEntity> GetCollaborators(int NoteId)
        {
            return notesrepo.GetCollaborators(NoteId);
        }

        public bool RemoveCollaborator(int NoteId, int UserId, string CollaboratorEmail)
        {
            return notesrepo.RemoveCollaborator(NoteId, UserId, CollaboratorEmail);
        }
    }
}
