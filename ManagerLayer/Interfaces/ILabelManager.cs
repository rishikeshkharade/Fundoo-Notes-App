using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Interfaces
{
    public interface ILabelManager
    {
      public Task<LabelEntity> CreateLabel(int UserId, string Name);
      public Task<List<LabelEntity>> GetAllLabels(int UserId);
      public Task<bool> AssignLabelToNote(int NoteId, int LabelId);
      public Task<bool> RemoveLabelFromNote(int NoteId, int LabelId);

    }
}
