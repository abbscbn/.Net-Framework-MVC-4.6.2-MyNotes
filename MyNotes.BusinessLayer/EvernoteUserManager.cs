using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.Result;
using MyNotes.Common.Helpers;
using MyNotes.DataAccessLayer.EntityFramework;
using MyNotes.Entities;
using MyNotes.Entities.Messages;
using MyNotes.Entities.ValueObjects;
using System;
using System.Linq;

namespace MyNotes.BusinessLayer
{
    public class EvernoteUserManager : ManagerBase<EverNoteUser>
    {


        Repository<Note> repo_note = new Repository<Note>(); // not tarafı için cascade delete işlemi yapılıyor
        Repository<Comment> repo_comment = new Repository<Comment>();
        Repository<Liked> repo_liked = new Repository<Liked>();
        NoteManager noteManager = new NoteManager();

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

                int dbResult = base.Insert(user);

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

                int dbResult = base.Update(res.Result);

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


            int dbResult = base.Update(res.Result);

            if (dbResult < 1)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;

        }

        public BusinessLayerResult<EverNoteUser> DeleteUser(int id)
        {

            EverNoteUser user = Find(x => x.Id == id);


            if (user != null)
            {

                foreach (Liked liked in repo_liked.List(x => x.LikedUser.Id == id).ToList())
                {
                    repo_liked.Delete(liked);
                }

                foreach (Comment comment in repo_comment.List(x => x.Owner.Id == id).ToList())
                {
                    repo_comment.Delete(comment);
                }



                int dbResult = base.Delete(user);

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





        // new anahtar kelimesi base class'taki metodu gizlemek için kullanılır
        public new BusinessLayerResult<EverNoteUser> Insert(EverNoteUser data)
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
                    Username = data.Username.Trim(),
                    Name = data.Name.Trim(),
                    Surname = data.Surname.Trim(),
                    Email = data.Email.Trim(),
                    Password = data.Password.Trim(),
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = data.IsActive,
                    IsAdmin = data.IsAdmin,
                    ProfileImageFilename = "user.webp"
                };

                int dbResult = base.Insert(user);

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Username == user.Username);

                    if (res.Result.IsActive == false)
                    {

                        string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                        string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActiveGuid}";
                        string body = $"{res.Result.Username} Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";
                        MailHelper.SendMail(body, res.Result.Email, "MyNotes Hesap Aktifleştirme");


                    }
                }
            }
            return res;
        }


        public new BusinessLayerResult<EverNoteUser> Update(EverNoteUser data)
        {
            EverNoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            if (db_user != null)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (!string.IsNullOrEmpty(data.Password))
            {
                res.Result.Password = data.Password;
            }

            int dbResult = base.Update(res.Result);
            if (dbResult < 1)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }
            return res;


        }


        public new BusinessLayerResult<EverNoteUser> Delete(EverNoteUser data)
        {
            EverNoteUser user = Find(x => x.Id == data.Id);

            if (user != null)
            {
                foreach (Liked liked in repo_liked.List(x => x.LikedUser.Id == data.Id).ToList())
                {

                    // Liked nesnesi için herbir nota ait like count sayısı azaltılır

                    Note note = repo_note.Find(x => x.Id == liked.NoteId);

                    noteManager.DecreaseLikeCount(note.Id);

                    repo_liked.Delete(liked);
                }
                foreach (Comment comment in repo_comment.List(x => x.Owner.Id == data.Id).ToList())
                {
                    repo_comment.Delete(comment);
                }

                if (base.Delete(data) < 1)
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
