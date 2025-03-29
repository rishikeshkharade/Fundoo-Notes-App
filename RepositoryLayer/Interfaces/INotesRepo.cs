using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        public NotesEntity CreateNote(int UserId, NotesModel notes);
        public List<NotesEntity> GetNotes();
        public List<NotesEntity> GetingNotesByTitleAndDescription(string title, string description);
        public int CountOfNotes(int UserId);
    }
}
