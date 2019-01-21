using MyEvernote.Business;
using MyEvernote.Entities;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CommentManager cm = new CommentManager();

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Note note = nm.Find(x => x.Id == id);
            Note note = nm.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);

            if (note == null)
                return HttpNotFound();

            return PartialView("_PartialComment", note.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = cm.Find(x => x.Id == id);

            if (comment == null)
                return new HttpNotFoundResult();
            else
                comment.Text = text;

            if (cm.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = cm.Find(x => x.Id == id);

            if (comment == null)
                return new HttpNotFoundResult();

            if (cm.Delete(comment) > 0)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                if (noteId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Note note = nm.Find(x => x.Id == noteId);

                if (note == null)
                    return new HttpNotFoundResult();

                comment.Note = note;
                comment.Owner = CurrentSession.User;

                if (cm.Insert(comment) > 0)
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}