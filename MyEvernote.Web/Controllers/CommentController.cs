using MyEvernote.Business;
using MyEvernote.Entities;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CommentManager cm = new CommentManager();

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Note note = nm.Find(x => x.Id == id);
            Note note = nm.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComment", note.Comments);
        }

        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = cm.Find(x => x.Id == id);

            if (comment == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            else
                comment.Text = text;

            if (cm.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}