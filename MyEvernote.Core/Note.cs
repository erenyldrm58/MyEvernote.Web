using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core
{
    [Table("Notes")]
    public class Note : BaseEntity
    {
        public Note()
        {
            Comments = new List<Comment>();
            Likeds = new List<Liked>();
        }

        [Required, StringLength(60)]
        public string Title { get; set; }
        [Required, StringLength(2000)]
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount{ get; set; }
        public int CategoryId { get; set; }

        public virtual EvernoteUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likeds { get; set; }
    }
}
