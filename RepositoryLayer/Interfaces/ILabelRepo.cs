using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepo
    {
       Task<LabelEntity> CreateLabel(int UserId, string Name);
       Task<List<LabelEntity>> GetAllLabels(int UserId);
       Task<bool> AssignLabelToNote(int NoteId, int LabelId);
       Task<bool> RemoveLabelFromNote(int NoteId, int LabelId);
    }
}
