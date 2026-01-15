using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotes.Entities
{
    [Table("EverNoteUsers")]
    public class EverNoteUser : MyEntityBase
    {
        [DisplayName("Ad"), StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Soyad"), Required, StringLength(25)]
        public string Surname { get; set; }

        [DisplayName("Email"), Required, StringLength(70)]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required, StringLength(50)]
        public string Password { get; set; }

        [DisplayName("Kullanıcı Adı"), Required, StringLength(30)]
        public string Username { get; set; }

        [StringLength(30)]
        public string ProfileImageFilename { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        [Required]
        public Guid ActiveGuid { get; set; }

        public virtual List<Note> Notes { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

    }
}
