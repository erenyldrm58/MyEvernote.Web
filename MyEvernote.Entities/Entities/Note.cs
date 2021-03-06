﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    [Table("Notes")]
    public class Note : BaseEntity
    {
        public Note()
        {
            Comments = new List<Comment>();
            Likeds = new List<Liked>();
        }

        [DisplayName("Not Başlığı"), Required, StringLength(60)]
        public string Title { get; set; }
        [DisplayName("Not Metni"), Required, StringLength(2000)]
        public string Text { get; set; }
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }
        [DisplayName("Beğeni")]
        public int LikeCount { get; set; }
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        public virtual EvernoteUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likeds { get; set; }
    }
}
