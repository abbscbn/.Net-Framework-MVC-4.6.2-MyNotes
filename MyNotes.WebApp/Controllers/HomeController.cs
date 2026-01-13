using MyNotes.BusinessLayer;
using MyNotes.Entities;
using MyNotes.Entities.Messages;
using MyNotes.Entities.ValueObjects;
using MyNotes.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyNotes.WebApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            // Bir başka controller'dan gelen notları göstermek için TempData kullanımı
            //if (TempData["categoryNotes"] != null)
            //{
            //    return View(TempData["categoryNotes"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();


            // return View(nm.getAllNotesQueryable().OrderByDescending(x => x.ModifedOn).ToList());

            return View(nm.getAllNotes().OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            var category = cm.GetCategoryById(id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("Index", category.Notes.OrderByDescending(x => x.ModifedOn).ToList());
        }


        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.getAllNotes().OrderByDescending(x => x.LikeCount).ToList());
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
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.Login(model);

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
            EvernoteUserManager eum = new EvernoteUserManager();

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> res = eum.RegisterUser(model);


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
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.UserActivate(id);

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

        public ActionResult ShowProfile()
        {
            if (Session["login"] != null)
            {

                EverNoteUser currentUser = Session["login"] as EverNoteUser;
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.GetUserById(currentUser.Id);

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

        public ActionResult EditProfile(EverNoteUser model)
        {
            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.UpdateUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }

        public ActionResult DeleteProfile(int id)
        {
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.DeleteUser(id);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return RedirectToAction("ShowProfile");
            }
            Session.Clear();
            return RedirectToAction("Index");
        }



    }
}