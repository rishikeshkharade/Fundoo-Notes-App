using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager _labelManager;

        public LabelController(ILabelManager labelManager)
        {
            this._labelManager = labelManager;
        }

        [HttpPost("CreateLabel")]
        public async Task<IActionResult> CreatingLabel(string createlabel)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var label = await _labelManager.CreateLabel(UserId, createlabel);
            if (label == null)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Label Not Created" });
            }
            else
            {
                return Ok(new ResponseModel<LabelEntity> { Success = true, Message = "Labels Which is created", Data = label });
            }
        }

        [HttpGet("GetAllLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var labels = await _labelManager.GetAllLabels(UserId);
            if (labels == null)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Label Not Found" });
            }
            else
            {
                return Ok(new ResponseModel<List<LabelEntity>> { Success = true, Message = "Labels Which is created", Data = labels });
            }
        }

        [HttpPost("AssignLabelToNote")]
        public async Task<IActionResult> AssignLabelToNote(int NoteId, int LabelId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var result = await _labelManager.AssignLabelToNote(NoteId, LabelId);
            if (result)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Label Assigned to Note" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Label Not Assigned to Note" });
            }
        }

        [HttpDelete("RemoveLabelFromNote")]
        public async Task<IActionResult> RemoveLabelFromNote(int NoteId, int LabelId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var result = await _labelManager.RemoveLabelFromNote(NoteId, LabelId);
            if (result)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Label Removed from Note" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Label Not Removed from Note" });
            }
        }
    }
}
