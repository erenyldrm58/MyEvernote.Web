using MyEvernote.Business;
using MyEvernote.Business.Results;
using MyEvernote.Entities;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    public class UserController : Controller
    {
        private UserManager um = new UserManager();
        // GET: User
        public ActionResult Index()
        {
            return View(um.List());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = um.Find(x => x.Id == id);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvernoteUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> insertResult = um.Insert(evernoteUser);

                if (insertResult.Errors.Count > 0)
                {
                    insertResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = um.Find(x => x.Id == id);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EvernoteUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> updateResult = um.Update(evernoteUser);

                if (updateResult.Errors.Count > 0)
                {
                    updateResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = um.Find(x => x.Id == id);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EvernoteUser evernoteUser = um.Find(x => x.Id == id);
            um.Delete(evernoteUser);
            return RedirectToAction("Index");
        }
    }
}