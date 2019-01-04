using MyEvernote.Business;
using MyEvernote.Entities;
using MyEvernote.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    public class NoteController : Controller
    {
        NoteManager nm = new NoteManager();
        CategoryManager cm = new CategoryManager();

        // GET: Note
        public ActionResult Index()
        {
            var note = nm.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.User.Id)
               .OrderByDescending(x => x.ModifiedOn);

            return View(nm.ListQueryable());
        }

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

        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title");
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                nm.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Edit/5
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
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // POST: Note/Edit/5
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
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(x => x.Id == id);
            nm.Delete(note);
            return RedirectToAction("Index");
        }
    }
}
