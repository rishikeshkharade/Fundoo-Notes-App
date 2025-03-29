using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        public NotesEntity CreateNote(int UserId, NotesModel notesModel);
        public List<NotesEntity> GetNotes();
        public List<NotesEntity> GetingNotesByTitleAndDescription(string title, string description);
        public int CountOfNotes(int UserId);
    }
}
