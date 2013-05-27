using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace TraCuuThuatNgu.Controllers
{
    public class AccountController : Controller
    {


        // Forgot Password
        // GET: /Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("LogOn");
            return View();
        }

        // Forgot Password
        // POST: /Account/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            string username = Membership.GetUserNameByEmail(email);

            if (String.IsNullOrEmpty(username))
            {
                ViewBag.Fail = "Email này không tồn tại trong hệ thống";
            }
            else
            {
                try
                {
                    MembershipUser currentUser = Membership.GetUser(username);
                    string newpass = currentUser.ResetPassword();

                    MailMessage message = new MailMessage();

                    message.From = new System.Net.Mail.MailAddress("khanhsnguyen@gmail.com");
                    message.To.Add(new System.Net.Mail.MailAddress(email));

                    message.IsBodyHtml = true;
                    message.BodyEncoding = Encoding.UTF8;
                    message.Subject = "Lấy lại mật khẩu";
                    message.Body = "Chào <b>" + username + "</b>,<br />Mật khẩu mới: " + newpass
                    + "<br/><br/>Hệ thống tra cứu thuật ngữ hành chính văn phòng và hoạt động xã hội.";

                    SmtpClient client = new SmtpClient();
                    client.Send(message);
                    ViewBag.Success = "Mật khẩu mới đã được gửi về email của bạn!";

                }
                catch
                {

                    ViewBag.Fail = "Lấy lại mật khẩu không thành công, email không hợp lệ!";
                }
            }

            return View();

        }

        // Account Informations
        // GET: /Account/AccountInformation
        [Authorize]
        public ActionResult Info()
        {
            MembershipUser user = Membership.GetUser();

            ProfileModel profileModel = new ProfileModel();
            Profile profile = profileModel.GetProfileByUserId((Guid)user.ProviderUserKey);

            InfoViewModel info = new InfoViewModel();
            info.Username = user.UserName;
            info.Email = user.Email;
            info.Fullname = profile.Fullname;
            info.Birthday = profile.Birthday;
            info.UserId = (Guid)user.ProviderUserKey;

            ViewBag.Birthday = profile.Birthday;
            return View(info);
        }

        // Account Informations
        // POST: /Account/AccountInformation
        [Authorize]
        [HttpPost]
        public ActionResult Info(InfoViewModel info, string birthday)
        {
            try
            {
                DateTime dt;
                try
                {
                    string[] formats = { "dd/MM/yyyy" };
                    dt = DateTime.ParseExact(birthday, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                }
                catch
                {
                    ViewBag.Fail = "Nhập ngày sinh sai định dạng";
                    return View(info);
                }

                ProfileModel profileModel = new ProfileModel();

                Profile profile = profileModel.GetProfileByUserId(info.UserId);

                profile.Birthday = dt;
                profile.Fullname = info.Fullname;
                //update profile
                profileModel.UpdateProfile(profile);

                MembershipUser user = Membership.GetUser();
                user.Email = info.Email;
                Membership.UpdateUser(user);

                ViewBag.Birthday = dt;
                ViewBag.Success = "Cập nhật thành công!";
            }
            catch
            {

                ViewBag.Fail = "Cập nhật không thành công!";
            }

            return View(info);


        }



        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }


        }



        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // User profile
                    Guid userId = (Guid)newUser.ProviderUserKey;
                    Profile profile = new Profile();
                    profile.UserId = userId;
                    profile.Fullname = model.FullName;

                    TraCuuThuatNguEntities context = new TraCuuThuatNguEntities();
                    context.Profiles.Add(profile);
                    context.SaveChanges();
                    // --

                    // Add user in User role as default
                    if (!Roles.RoleExists("User"))
                    {
                        Roles.CreateRole("User");                        
                    }
                    Roles.AddUserToRole(model.UserName,"User");

                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    //return RedirectToAction("ChangePasswordSuccess");
                    ViewBag.Success = "Thay đổi mật khẩu thành công";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại không đúng hoặc mật khẩu mới không hợp lệ.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Tên đăng nhập này đã có người sử dụng. Xin chọn tên đăng nhập khác.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email này đã được dùng để đăng ký. Xin sử dụng một Email khác.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
