using System;
using System.Collections.Generic;
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
        //private readonly IConfiguration configuration;

        public NotesRepo(FundooDBContext fundooDBContext, IConfiguration configuration)
        {
            this.fundooDBContext = fundooDBContext;
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
    }
}
