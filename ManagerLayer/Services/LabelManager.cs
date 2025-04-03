using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class LabelManager : ILabelManager
    {
            private readonly ILabelRepo labelrepo;
            public LabelManager(ILabelRepo labelrepo)
            {
                this.labelrepo = labelrepo;
            }
            public Task<LabelEntity> CreateLabel(int UserId, string Name)
        {
            return labelrepo.CreateLabel(UserId, Name);
        }

        public Task<List<LabelEntity>> GetAllLabels(int UserId)
        {
            return labelrepo.GetAllLabels(UserId);
        }

        public Task<bool> AssignLabelToNote(int NoteId, int LabelId)
        {
            return labelrepo.AssignLabelToNote(NoteId, LabelId);
        }
        public Task<bool> RemoveLabelFromNote(int NoteId, int LabelId)
        {
            return labelrepo.RemoveLabelFromNote(NoteId, LabelId);
        }
    }
}
