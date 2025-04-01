using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        public NotesEntity CreateNote(int UserId, NotesModel notesModel);
        public List<NotesEntity> GetNotes();
        public List<NotesEntity> GetingNotesByTitleAndDescription(string title, string description);
        public int CountOfNotes(int UserId);
        public int PinNote(int NotesId, int UserId);
        public int ArchiveNote(int NotesId, int UserId);
        public bool AddColourInNote(int NotesId, int UserId, string colour);
        public int NoteInTrash(int NotesId, int UserId);
        public bool AddReminder(DateTime Reminder, int UserId, int NoteId);
        public bool DeleteNote(int NoteId);
        public NotesEntity UpdateNote(int NoteId, int UserId, UpdateNoteModel model);
        public bool AddImage(int NoteId, int UserId, IFormFile image);
        public bool AddCollaborator(int NoteId, int UserId, string CollaboratorEmail);
        public List<CollaboratorEntity> GetCollaborators(int NoteId);
        public bool RemoveCollaborator(int NoteId, int UserId, string CollaboratorEmail);
    }
}
