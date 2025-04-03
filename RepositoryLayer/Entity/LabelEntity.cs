using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using System.Text;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }

        [Required]
        public string LabelName { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; } = null!;

        public ICollection<LabelNoteEntity> LabelNotes { get; set; } = new List<LabelNoteEntity>();

    }
}
