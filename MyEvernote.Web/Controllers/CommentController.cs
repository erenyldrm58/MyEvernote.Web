using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult ShowNoteComments()
        {
            return PartialView("_PartialComment");
        }
    }
}