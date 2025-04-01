using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
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

        public int PinNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(p => p.UserId == UserId && p.NotesId == NotesId);
            if (notesEntity != null)
            {
                if (notesEntity.IsPin)
                {
                    notesEntity.IsPin = false;
                    fundooDBContext.SaveChanges();
                    return 1;
                }
                else
                {
                    notesEntity.IsPin = true;
                    fundooDBContext.SaveChanges();
                    return 2;
                }
            } else
            {
                return 3;
            }
        }

        public int ArchiveNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(notesEntity => notesEntity.NotesId == NotesId && notesEntity.UserId == UserId);
            if (notesEntity != null)
            {
                if (notesEntity.IsArchive && notesEntity.IsPin == false)
                {
                    notesEntity.IsArchive = false;
                    fundooDBContext.SaveChanges();
                    return 1;
                }
                else
                {
                    notesEntity.IsArchive = true;
                    notesEntity.IsPin = false;
                    fundooDBContext.SaveChanges();
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }


        public bool AddColourInNote(int NotesId, int UserId, string colour)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId && note.UserId == UserId);
            if (notesEntity != null)
            {
                notesEntity.Colour = colour;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int NoteInTrash(int NotesId, int UserId)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(t => t.NotesId == NotesId && t.UserId == UserId);
            if (notesEntity != null)
            {
                if (notesEntity.IsTrash)
                {
                    notesEntity.IsTrash = false;
                    fundooDBContext.SaveChanges();
                    return 1;
                }
                else
                {
                    notesEntity.IsTrash = true;
                    fundooDBContext.SaveChanges();
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }

        public bool AddReminder(DateTime Reminder, int UserId, int NoteId)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(r => r.NotesId == NoteId && r.UserId == UserId);
            if (notesEntity != null)
            {
                notesEntity.Reminder = Reminder;
                fundooDBContext.SaveChanges();
                return true;
            }
            return false;
        }


        public bool DeleteNote(int NoteId)
        {
            var dltnote = fundooDBContext.Notes.FirstOrDefault(d => d.NotesId == NoteId);
            if (dltnote != null)
            {
                fundooDBContext.Notes.Remove(dltnote);
                fundooDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public NotesEntity UpdateNote(int NoteId, int UserId, UpdateNoteModel model)
        {
            var updateNote = fundooDBContext.Notes.FirstOrDefault(u => u.NotesId == NoteId && u.UserId == UserId);
            if (updateNote != null)
            {
                updateNote.Reminder = model.Reminder;
                updateNote.UserId = UserId;
                updateNote.NotesId = NoteId;
                updateNote.Title = model.Title;
                updateNote.Description = model.Description;
                updateNote.Colour = model.Colour;
                updateNote.IsArchive = model.IsArchive;
                updateNote.Image = model.Image;
                updateNote.IsPin = model.IsPin;
                updateNote.LastUpdatedAt = DateTime.Now;
                fundooDBContext.SaveChanges();
                return updateNote;
            }
            return null;
        }

        public bool AddImage(int NoteId, int UserId, IFormFile image)
        {
            
                NotesEntity addImage = fundooDBContext.Notes.ToList().Find(user => user.UserId == UserId && user.NotesId == NoteId);
            if (addImage != null)
            {
                Account account = new Account(
                    configuration["CloudinarySettings:CloudName"],
                    configuration["CloudinarySettings:ApiKey"],
                    configuration["CloudinarySettings:ApiSecret"]
                    );
                Cloudinary cloudinary = new Cloudinary(account);
                var UploadParameter = new ImageUploadParams()
                {
                    File = new FileDescription(image.FileName, image.OpenReadStream()),
                };

                var UploadResult = cloudinary.Upload(UploadParameter);
                    string ImagePath = UploadResult.Url.ToString();
                    addImage.Image = ImagePath;
                    fundooDBContext.SaveChanges();
                    return true;
            }
            else
            {
                return false;
            }
        }


        public bool AddCollaborator(int NoteId, int UserId, string CollaboratorEmail)
        {
            NotesEntity notesEntity = fundooDBContext.Notes.FirstOrDefault(c => c.NotesId == NoteId && c.UserId == UserId);
            if (notesEntity != null)
            {
                CollaboratorEntity collaborators = new CollaboratorEntity();
                collaborators.Email = CollaboratorEmail;
                collaborators.NoteId = NoteId;
                collaborators.UserId = UserId;
                fundooDBContext.Collaborators.Add(collaborators);
                fundooDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<CollaboratorEntity> GetCollaborators(int NoteId)
        {
            List<CollaboratorEntity> collaborators = fundooDBContext.Collaborators.Where(c => c.NoteId == NoteId).ToList();
            return collaborators;
        }

        public bool RemoveCollaborator(int NoteId, int UserId, string CollaboratorEmail)
        {
            CollaboratorEntity collaborators = fundooDBContext.Collaborators.FirstOrDefault(c => c.NoteId == NoteId && c.UserId == UserId && c.Email == CollaboratorEmail);
            if (collaborators != null)
            {
                fundooDBContext.Collaborators.Remove(collaborators);
                fundooDBContext.SaveChanges();
                return true;
            }
            return false;
        }


        }
}
