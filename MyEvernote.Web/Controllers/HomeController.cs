using MyEvernote.Business;
using MyEvernote.Business.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ViewModels;
using MyEvernote.Web.Models;
using MyEvernote.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager nm = new NoteManager();
        private UserManager um = new UserManager();
        private CategoryManager cm = new CategoryManager();

        public ActionResult Index()
        {
            return View(nm.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Note> notes = nm.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id.Value).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            BusinessResult<EvernoteUser> profileResult = um.GetUserById(CurrentSession.User.Id);

            if (profileResult.Errors.Count > 0)
            {
                ErrorViewModel errorModel = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = profileResult.Errors
                };

                return View("Error", errorModel);
            }

            return View(profileResult.Result);
        }

        public ActionResult EditProfile()
        {
            BusinessResult<EvernoteUser> profileResult = um.GetUserById(CurrentSession.User.Id);

            if (profileResult.Errors.Count > 0)
            {
                ErrorViewModel errorModel = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = profileResult.Errors
                };

                return View("Error", errorModel);
            }

            return View(profileResult.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                        ProfileImage.ContentType == "image/jpeg" ||
                        ProfileImage.ContentType == "image/jpg" ||
                        ProfileImage.ContentType == "image/png")
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImgFileName = filename;
                }

                BusinessResult<EvernoteUser> editResult = um.UpdateProfile(model);

                if (editResult.Errors.Count > 0)
                {
                    ErrorViewModel errorModel = new ErrorViewModel()
                    {
                        Title = "Profile Güncellenemedi",
                        Items = editResult.Errors,
                        RedirectUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorModel);
                }

                //Profil güncellendiği için session güncellendi
                CurrentSession.Set<EvernoteUser>("login", editResult.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            BusinessResult<EvernoteUser> deleteResult = um.RemoveUserById(CurrentSession.User.Id);

            if (deleteResult.Errors.Count > 0)
            {
                ErrorViewModel errorModel = new ErrorViewModel()
                {
                    Title = "Profil silinemedi",
                    Items = deleteResult.Errors,
                    RedirectUrl = "/Home/ShowProfile"
                };

                return View("Error", errorModel);
            }

            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //Giriş kontrolü
            //User aktif mi
            //Yönlendirme
            //Session a kullanıcı bilgisini atma 

            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> loginResult = um.LoginUser(model);

                if (loginResult.Errors.Count > 0)
                {
                    if (loginResult.Errors.Find(x => x.Code == ErrorMessageCode.UserIsnotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-2222-12345";
                    }
                    loginResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                CurrentSession.Set<EvernoteUser>("login", loginResult.Result);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //Username ve Email kontrolü
            //Kayıt(Insert) işlemi
            //Aktivasyon epostası gönder

            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> userResult = um.RegisterUser(model);

                if (userResult.Errors.Count > 0)
                {
                    userResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
            }

            OkViewModel okModel = new OkViewModel()
            {
                Header = "Kayıt Başarılı",
                RedirectUrl = "/Home/Login/"
            };

            okModel.Items.Add("E-posta adresinize gelen aktivasyon linkine tıklayarak hesabınızı aktive edebilirsiniz.Hesabınızı aktive etmeden not ekleme ve gönderi beğenme işlemi yapamazsınız.");

            return View("Ok", okModel);
        }
        //İptal Edildi-NotifyView
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            BusinessResult<EvernoteUser> activateResult = um.ActivateUser(id);

            if (activateResult.Errors.Count > 0)
            {
                ErrorViewModel errorModel = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = activateResult.Errors
                };

                return View("Error", errorModel);
            }

            OkViewModel okModel = new OkViewModel()
            {
                Header = "Hesap Aktifleştirildi",
                RedirectUrl = "/Home/Login/"
            };

            okModel.Items.Add("MyEvernote hesabınız aktifleştirildi.Artık not paylaşabilir ve beğeni yapabilirsiniz..");

            return View("Ok", okModel);
        }
        //İptal Edildi-NotifyView
        public ActionResult UserActivateOk()
        {
            return View();
        }
        //İptal Edildi-NotifyView
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessage> errors = null;

            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessage>;
            }
            return View(errors);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult UserActivicate(Guid activicateId)
        {
            return View();
        }
        public ActionResult TestNotify()
        {
            OkViewModel okModel = new OkViewModel()
            {
                Header = "Yönlendirme",
                Title = "OK TEST",
                RedirectTimeout = 2000,
                Items = new List<string> { "Test 1 başarılı", "Test 2 başarılı" }
            };
            return View("Ok", okModel);
        }
    }
}