using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
   public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions option) : base(option) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }
        public DbSet<LabelNoteEntity> LabelNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Many-to-Many Relationships between Note and Label
            modelBuilder.Entity<LabelNoteEntity>()
                .HasKey(ln => new {ln.NoteId, ln.LabelId});

            modelBuilder.Entity<LabelNoteEntity>()
                .HasOne(ln => ln.Note)
                .WithMany(n => n.LabelNotes)
                .HasForeignKey(n => n.NoteId);

            modelBuilder.Entity<LabelNoteEntity>()
                .HasOne(ln => ln.Label)
                .WithMany(l => l.LabelNotes)
                .HasForeignKey(l => l.LabelId);

        }
    }
}
