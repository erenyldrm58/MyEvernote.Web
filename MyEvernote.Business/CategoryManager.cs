using MyEvernote.Business.Abstract;
using MyEvernote.Entities;
using System.Linq;

namespace MyEvernote.Business
{
    public class CategoryManager : ManagerBase<Category>
    {
        #region |Delete Cascade|
        //public override int Delete(Category category)
        //{
        //    NoteManager nm = new NoteManager();
        //    LikedManager lm = new LikedManager();
        //    CommentManager cm = new CommentManager();

        //    //Kategori ile ilişkili nesneler silinmeli önce
        //    foreach (Note note in category.Notes.ToList())
        //    {
        //        foreach (Liked liked in note.Likeds.ToList())
        //        {
        //            lm.Delete(liked);
        //        }

        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            cm.Delete(comment);
        //        }
        //        nm.Delete(note);
        //    }

        //    return base.Delete(category);
        //} 
        #endregion
    }
}
