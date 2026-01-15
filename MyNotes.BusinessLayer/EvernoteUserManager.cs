using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.Result;
using MyNotes.Common.Helpers;
using MyNotes.DataAccessLayer.EntityFramework;
using MyNotes.Entities;
using MyNotes.Entities.Messages;
using MyNotes.Entities.ValueObjects;
using System;

namespace MyNotes.BusinessLayer
{
    public class EvernoteUserManager : ManagerBase<EverNoteUser>
    {


        Repository<Note> repo_note = new Repository<Note>();
        Repository<Comment> repo_comment = new Repository<Comment>();
        Repository<Liked> repo_liked = new Repository<Liked>();

        private BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();

        public BusinessLayerResult<EverNoteUser> RegisterUser(RegisterViewModel data)
        {
            EverNoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);

            if (user != null)
            {
                if (data.Username == user.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı çoktan kayıtlı");
                }
                if (data.Email == user.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Kullanıcı adı veya e-posta adresi kayıtlı.");
                }
            }
            else
            {
                user = new EverNoteUser()
                {
                    Username = data.Username,
                    Surname = "test",
                    Email = data.Email,
                    Password = data.Password,
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                };

                int dbResult = Insert(user);

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Username == user.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActiveGuid}";
                    string body = $"{res.Result.Username} Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";
                    MailHelper.SendMail(body, res.Result.Email, "MyNotes Hesap Aktifleştirme");


                }

            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> Login(LoginViewModel data)
        {

            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {

                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Lütfen hesabınızı aktifleştirin.");

                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.EmailOrPasswordWrong, "Kullanıcı adı veya şifre hatalı.");
            }
            return res;

        }

        public BusinessLayerResult<EverNoteUser> UserActivate(Guid id)
        {

            res.Result = Find(x => x.ActiveGuid == id);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;

                int dbResult = Update(res.Result);

                if (dbResult < 1)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotActive, "Kullanıcı aktif edilemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivationIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> GetUserById(int ıd)
        {

            res.Result = Find(x => x.Id == ıd);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<EverNoteUser> UpdateUser(EverNoteUser model)
        {

            EverNoteUser db_user = Find(x => x.Id != model.Id && (x.Username == model.Username || x.Email == model.Email));

            if (db_user != null)
            {
                if (db_user.Username == model.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (db_user.Email == model.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Surname = model.Surname;
            res.Result.Username = model.Username;

            if (!string.IsNullOrEmpty(model.Password))
            {
                res.Result.Password = model.Password;
            }

            if (!string.IsNullOrEmpty(model.ProfileImageFilename))
            {
                res.Result.ProfileImageFilename = model.ProfileImageFilename;
            }


            int dbResult = Update(res.Result);

            if (dbResult < 1)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;

        }

        public BusinessLayerResult<EverNoteUser> DeleteUser(int id)
        {

            EverNoteUser user = Find(x => x.Id == id);


            foreach (Liked liked in user.Likes)
            {
                repo_liked.Delete(liked);
            }

            foreach (Comment comment in user.Comments)
            {
                repo_comment.Delete(comment);
            }

            foreach (Note note in user.Notes)
            {
                repo_note.Delete(note);
            }



            if (user != null)
            {
                int dbResult = Delete(user);

                if (dbResult < 1)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotDeleted, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }
    }
}
