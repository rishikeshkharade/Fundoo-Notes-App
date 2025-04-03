using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Entity
{
    public class LabelNoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [ForeignKey("Note")]
        public int NoteId {  get; set; }
        public virtual NotesEntity Note { get; set; } = null!;

        [ForeignKey("Label")]
        public int LabelId {  get; set; }
        public virtual LabelEntity Label { get; set; } = null!;
    }
}
