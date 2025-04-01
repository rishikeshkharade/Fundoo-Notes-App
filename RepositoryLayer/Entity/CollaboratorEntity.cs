using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }

        public string Email { get; set; }
        [ForeignKey("NoteUser")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity NoteUser { get; set; }

        [ForeignKey("CollaborateNote")]
        public int NoteId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity CollaborateNote { get; set; }

    }
}
