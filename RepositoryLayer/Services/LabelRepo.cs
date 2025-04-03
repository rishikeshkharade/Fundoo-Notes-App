using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FundooDBContext dbContext;

        public LabelRepo(FundooDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<LabelEntity> CreateLabel(int UserId, string Name)
        {
            var label = new LabelEntity { LabelName = Name , UserId = UserId} ;
            dbContext.Labels.Add(label);
            await dbContext.SaveChangesAsync();
            return label;
        }

        public async Task<List<LabelEntity>> GetAllLabels(int UserId)
        {
            var labels = await dbContext.Labels.Where(x => x.UserId == UserId).ToListAsync();
            return labels;
        }

        public async Task<bool> AssignLabelToNote(int NoteId, int LabelId)
        {
            var Notes = await dbContext.Notes.FirstOrDefaultAsync(x => x.NotesId == NoteId);
            var label = await dbContext.Labels.FirstOrDefaultAsync(x => x.LabelId == LabelId);

            if (Notes == null || label == null)
                return false;

            dbContext.LabelNotes.Add(new LabelNoteEntity
            {
                NoteId = Notes.NotesId,
                LabelId = label.LabelId
            });

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveLabelFromNote(int NoteId, int LabelId)
        {
            var labelNote = await dbContext.LabelNotes
                .FirstOrDefaultAsync(x => x.NoteId == NoteId && x.LabelId == LabelId);
            if (labelNote == null)
                return false;
            dbContext.LabelNotes.Remove(labelNote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        }
}
