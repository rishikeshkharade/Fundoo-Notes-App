using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
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
    }
}
