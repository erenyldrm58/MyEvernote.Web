using MyEvernote.Business;
using MyEvernote.Entities;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        NoteManager nm = new NoteManager();
        CategoryManager cm = new CategoryManager();
        LikedManager lm = new LikedManager();

        [Auth]
        // GET: Note
        public ActionResult Index()
        {
            var note = nm.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.User.Id)
               .OrderByDescending(x => x.ModifiedOn);

            return View(nm.ListQueryable());
        }

        [Auth]
        public ActionResult MyLikedNotes()
        {
            var note = lm.ListQueryable().Include("LikedUser").Include("Note")
                .Where(x => x.LikedUser.Id == CurrentSession.User.Id)
                .Select(x => x.Note).Include("Category").Include("Owner")
                .OrderByDescending(x => x.ModifiedOn);

            return View("Index", note.ToList());
        }

        [Auth]
        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        // POST: Note/Create
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                nm.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // POST: Note/Edit/5
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                Note dbNote = nm.Find(x => x.Id == note.Id);
                dbNote.IsDraft = note.IsDraft;
                dbNote.CategoryId = note.CategoryId;
                dbNote.Text = note.Text;
                dbNote.Title = note.Title;

                nm.Update(note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(x => x.Id == id);
            nm.Delete(note);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                List<int> likedNoteIds = lm.List(
                    x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Note.Id)).Select(
                    x => x.Note.Id).ToList();

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int noteId, bool liked)
        {
            int result = 0;

            Liked like = lm.Find(x => x.Note.Id == noteId && x.LikedUser.Id == CurrentSession.User.Id);
            Note note = nm.Find(x => x.Id == noteId);

            if (like != null && liked == false)
            {
                result = lm.Delete(like);
            }
            else if (like == null && liked == true)
            {
                result = lm.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (result > 0)
            {
                if (liked)
                    note.LikeCount++;
                else
                    note.LikeCount--;

                result = nm.Update(note);

                return Json(new { hasError = false, errorMsg = string.Empty, result = note.LikeCount });
            }
            return Json(new { hasError = true, errorMsg = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount});
        }

        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Note note = nm.Find(x => x.Id == id);

            if (id == null)
                return HttpNotFound();

            return PartialView("_PartialNoteText", note);
        }
    }
}