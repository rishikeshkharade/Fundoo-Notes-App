using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FundooDBContext fundooDBContext;
        private readonly IConfiguration configuration;
        public NotesRepo(FundooDBContext fundooDBContext, IConfiguration configuration)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;
        }

        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            NotesEntity notesEntity = new NotesEntity();

            notesEntity.Title = notesModel.Title;
            notesEntity.Description = notesModel.Description;
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.LastUpdatedAt = DateTime.Now;
            notesEntity.UserId = UserId;
            fundooDBContext.Notes.Add(notesEntity);
            fundooDBContext.SaveChanges();
            return notesEntity;
        }

        public List<NotesEntity> GetNotes()
        {
            List<NotesEntity> notesEntities = fundooDBContext.Notes.ToList();
            if (notesEntities != null)
            {
                return notesEntities;
            }
            return null;
        }

        public List<NotesEntity> GetingNotesByTitleAndDescription(string title, string description)
        {
            List<NotesEntity> notesEntities = fundooDBContext.Notes.Where(t => t.Title == title && t.Description == description).ToList();
            return notesEntities;
        }

        public int CountOfNotes(int UserId)
        {
            int Notescount = fundooDBContext.Notes.Count(c => c.UserId == UserId);
            return Notescount;
        }
    }
}
