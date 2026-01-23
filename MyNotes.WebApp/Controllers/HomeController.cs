using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Result;
using MyNotes.Entities;
using MyNotes.Entities.Messages;
using MyNotes.Entities.ValueObjects;
using MyNotes.WebApp.Filters;
using MyNotes.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {

        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        public ActionResult Index()
        {


            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var category = categoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.ActiveCategoryId = id;

            return View("Index", category.Notes.OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Login()
        {
            // Hata kontrolü

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                BusinessLayerResult<EverNoteUser> res = evernoteUserManager.Login(model);

                if (res.Errors.Count > 0)
                {

                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ModelState.AddModelError("", "Hesabınız aktifleştirilmemiştir. Lütfen email adresinizi kontrol ediniz.");
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }

                Session["login"] = res.Result;

                return RedirectToAction("Index");
            }

            return View(model);

        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {


            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> res = evernoteUserManager.RegisterUser(model);


                if (res.Errors.Count > 0)
                {
                    // hata var
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                OkViewModel okViewModel = new OkViewModel();
                okViewModel.Items = new List<string>();
                okViewModel.Items.Add("Kayıt işleminiz başarıyla gerçekleştirilmiştir. Lütfen email adresinize gönderilen aktivasyon linkine tıklayarak hesabınızı aktifleştiriniz.");
                okViewModel.RedirectingUrl = "/Home/Login";

                return View("Ok", okViewModel);
            }


            return View(model);

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult UserActivate(Guid id)
        {

            BusinessLayerResult<EverNoteUser> res = evernoteUserManager.UserActivate(id);

            if (res.Errors.Count > 0)
            {

                ErrorViewModel errorViewModel = new ErrorViewModel();

                errorViewModel.Items = res.Errors;


                return View("Error", errorViewModel);

            }

            OkViewModel okViewModel = new OkViewModel();
            okViewModel.Items = new List<string>();
            okViewModel.Items.Add("Hesabınız başarıyla aktifleştirilmiştir.");

            return View("Ok", okViewModel);
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            if (Session["login"] != null)
            {

                EverNoteUser currentUser = Session["login"] as EverNoteUser;

                BusinessLayerResult<EverNoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel();
                    errorViewModel.Items = res.Errors;
                    errorViewModel.Title = "Profil Bulunamadı";
                    return View("Error", errorViewModel);
                }
                return View(res.Result);

            }

            return RedirectToAction("Index");

        }

        [Auth]
        public ActionResult EditProfile()
        {
            if (Session["login"] != null)
            {
                EverNoteUser currentUser = Session["login"] as EverNoteUser;

                BusinessLayerResult<EverNoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel();
                    errorViewModel.Items = res.Errors;
                    errorViewModel.Title = "Profil Bulunamadı";
                    return View("Error", errorViewModel);
                }
                return View(res.Result);
            }
            return RedirectToAction("Index");
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EverNoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {


                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                BusinessLayerResult<EverNoteUser> res = evernoteUserManager.UpdateUser(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel();
                    errorViewModel.Items = res.Errors;
                    errorViewModel.RedirectingUrl = "/Home/EditProfile";
                    errorViewModel.RedirectingTimeout = 10;
                    return View("Error", errorViewModel);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile(int id)
        {

            BusinessLayerResult<EverNoteUser> res = evernoteUserManager.DeleteUser(id);

            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return RedirectToAction("ShowProfile");
            }

            Session.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult AccessDenied()
        {
            ErrorViewModel error = new ErrorViewModel();



            error.Title = "Yetkisiz Erişim";
            error.Header = "Yetkisiz Erişim";
            error.Items = new List<ErrorMessageObj> { new ErrorMessageObj() { Message = "Bu Sayfaya Erişemezsiniz" } };
            error.IsRedirectingUrl = true;
            error.RedirectingUrl = "/Home/Index";
            error.RedirectingTimeout = 5;

            return View("Error", error);


        }

        public ActionResult GlobalException()
        {
            ErrorViewModel error = new ErrorViewModel();

            string msj = string.Empty;

            if (TempData["LastError"] == null)
            {
                msj = "Genel bir hata oluştu";
            }
            else
            {
                Exception exception = TempData["LastError"] as Exception;

                msj = exception.Message;
            }



            error.Title = "Genel Hata";
            error.Header = "Hata Detayı";
            error.Items = new List<ErrorMessageObj> { new ErrorMessageObj() { Message = msj } };
            error.IsRedirectingUrl = true;
            error.RedirectingUrl = "/Home/Index";
            error.RedirectingTimeout = 5;

            return View("Error", error);

        }


    }
}