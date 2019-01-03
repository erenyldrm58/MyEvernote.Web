using MyEvernote.Business.Abstract;
using MyEvernote.Business.Results;
using MyEvernote.Common.Helpers;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ViewModels;
using System;

namespace MyEvernote.Business
{
    public class UserManager : ManagerBase<EvernoteUser>
    {
        public BusinessResult<EvernoteUser> RegisterUser(RegisterViewModel model)
        {
            EvernoteUser user = Find(x => x.Username == model.UserName || x.Email == model.Email);
            BusinessResult<EvernoteUser> userResult = new BusinessResult<EvernoteUser>();

            if (user != null)
            {
                if (user.Username == model.UserName)
                    userResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");

                if (user.Email == model.Email)
                    userResult.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
            }
            else
            {
                int insertCount = base.Insert(new EvernoteUser()
                {
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    ActivateGuid = Guid.NewGuid(),
                    ProfileImgFileName = "user_boy.png",
                    IsActive = false,
                    IsAdmin = false
                });

                if (insertCount > 0)
                {
                    userResult.Result = Find(x => x.Username == model.UserName && x.Email == model.Email);

                    // Aktivasyon maili

                    string siteRootUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteRootUri}/Home/UserActivate/{userResult.Result.ActivateGuid}";
                    string body = $"Merhaba {userResult.Result.Username}, <br>Hesabınızı aktifleştirmek için <a href={activateUri} target='_blank'>tıklayınız</a>";
                    MailHelper.SendMail(body, userResult.Result.Email, "MyEvernote Aktivasyon");
                }
            }

            return userResult;
        }

        public BusinessResult<EvernoteUser> GetUserById(int id)
        {
            BusinessResult<EvernoteUser> profileResult = new BusinessResult<EvernoteUser>();
            profileResult.Result = Find(x => x.Id == id);

            if (profileResult.Result == null)
            {
                profileResult.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }

            return profileResult;
        }

        public BusinessResult<EvernoteUser> LoginUser(LoginViewModel model)
        {
            BusinessResult<EvernoteUser> userResult = new BusinessResult<EvernoteUser>();
            userResult.Result = Find(x => x.Username == model.UserName && x.Password == model.Password);

            if (userResult.Result != null)
            {
                if (!userResult.Result.IsActive)
                    userResult.AddError(ErrorMessageCode.UserIsnotActive, "Kullanıcı pasif durumundadır.Aktifleştirme için lütfen e-postanızı kontrol ediniz.");
            }
            else
            {
                userResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı veya şifre uyuşmuyor.");
            }

            return userResult;
        }

        public BusinessResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessResult<EvernoteUser> result = new BusinessResult<EvernoteUser>();
            result.Result = Find(x => x.ActivateGuid == activateId);

            if (result.Result != null)
            {
                if (result.Result.IsActive)
                {
                    result.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktiftir.");

                    return result;
                }
            }
            else
            {
                result.AddError(ErrorMessageCode.ActiveIdDoesNotExist, "Aktifleştirmek için kullanıcı bulunamadı!");
            }
            result.Result.IsActive = true;
            Update(result.Result);
            return result;
        }

        public BusinessResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            BusinessResult<EvernoteUser> result = new BusinessResult<EvernoteUser>();
            EvernoteUser dbUser = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));

            if (dbUser != null && dbUser.Id != data.Id)
            {
                if (dbUser.Username == data.Username)
                    result.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                if (dbUser.Email == data.Email)
                    result.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");

                return result;
            }
            result.Result = Find(x => x.Id == data.Id);
            result.Result.Name = data.Name;
            result.Result.Surname = data.Surname;
            result.Result.Email = data.Email;
            result.Result.Password = data.Password;
            result.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImgFileName) == false)
                result.Result.ProfileImgFileName = data.ProfileImgFileName;

            if (base.Update(result.Result) == 0)
            {
                result.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profile Güncellenemedi");
            }

            return result;
        }

        public BusinessResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessResult<EvernoteUser> result = new BusinessResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    result.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return result;
                }
            }
            else
            {
                result.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }

            return result;
        }

        //Metod hiding
        public new BusinessResult<EvernoteUser> Insert(EvernoteUser model)
        {
            EvernoteUser user = Find(x => x.Username == model.Username || x.Email == model.Email);
            BusinessResult<EvernoteUser> userResult = new BusinessResult<EvernoteUser>();

            userResult.Result = model;

            if (user != null)
            {
                if (user.Username == model.Username)
                    userResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");

                if (user.Email == model.Email)
                    userResult.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
            }
            else
            {
                userResult.Result.ActivateGuid = Guid.NewGuid();
                userResult.Result.ProfileImgFileName = "user_boy.png";

                int insertCount = base.Insert(userResult.Result);

                if (insertCount == 0)
                {
                    userResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return userResult;
        }

        public new BusinessResult<EvernoteUser> Update(EvernoteUser data)
        {
            BusinessResult<EvernoteUser> result = new BusinessResult<EvernoteUser>();
            EvernoteUser dbUser = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            result.Result = data;
            if (dbUser != null && dbUser.Id != data.Id)
            {
                if (dbUser.Username == data.Username)
                    result.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                if (dbUser.Email == data.Email)
                    result.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");

                return result;
            }
            result.Result = Find(x => x.Id == data.Id);
            result.Result.Name = data.Name;
            result.Result.Surname = data.Surname;
            result.Result.Email = data.Email;
            result.Result.Password = data.Password;
            result.Result.Username = data.Username;
            result.Result.IsActive = data.IsActive;
            result.Result.IsAdmin = data.IsAdmin;

            if (base.Update(result.Result) == 0)
            {
                result.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");
            }

            return result;
        }
    }
}
