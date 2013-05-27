using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.ViewModels;
using TraCuuThuatNgu.Models;
using System.Web.Security;
using System.Globalization;

namespace TraCuuThuatNgu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        UsersModel usersModel = new UsersModel();

        public ActionResult Index(int? page, string keyword)
        {

            var pageNumber = page ?? 1;
            int size = 10;

            UsersViewModel viewModel = new UsersViewModel();


            if (String.IsNullOrWhiteSpace(keyword))
            {
                viewModel.Users = usersModel.GetAllUserPaged(pageNumber, size);
            }
            else
            {
                viewModel.Users = usersModel.GetAllUserPagedContainsKeyword(pageNumber, size, keyword);
            }

            return View(viewModel);
        }


        //
        // GET: /Users/Detail
        public ActionResult Detail(string username)
        {
            try
            {
                aspnet_Users user = usersModel.GetUserByUsername(username);

                InfoViewModel userInfo = new InfoViewModel();
                userInfo.Username = user.UserName;
                userInfo.Fullname = user.Profile.Fullname;
                userInfo.Birthday = user.Profile.Birthday;
                userInfo.Email = user.aspnet_Membership.Email;
                userInfo.UserId = user.UserId;

                ViewBag.Birthday = userInfo.Birthday;
                ViewBag.UserRoles = Roles.GetRolesForUser(username);
                ViewBag.Roles = Roles.GetAllRoles();

                return View(userInfo);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /User/
        [HttpPost]
        public ActionResult Detail(InfoViewModel info, string birthday, string[] userRoles)
        {
            try
            {
                // set role
                if (userRoles != null)
                {
                    Roles.RemoveUserFromRoles(info.Username, Roles.GetRolesForUser(info.Username));
                    foreach (var role in userRoles)
                    {
                        Roles.AddUserToRole(info.Username, role);
                    }
                }
                else
                {
                    Roles.RemoveUserFromRoles(info.Username, Roles.GetRolesForUser(info.Username));
                    Roles.AddUserToRole(info.Username, "User");
                }

                ViewBag.UserRoles = Roles.GetRolesForUser(info.Username);
                ViewBag.Roles = Roles.GetAllRoles();

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

                MembershipUser user = Membership.GetUser(info.Username);
                user.Email = info.Email;
                Membership.UpdateUser(user);


                Profile profile = profileModel.GetProfileByUserId((Guid)user.ProviderUserKey);

                profile.Birthday = dt;
                profile.Fullname = info.Fullname;
                //update profile
                profileModel.UpdateProfile(profile);


                ViewBag.Birthday = dt;
                ViewBag.Success = "Cập nhật thành công!";

            }
            catch
            {

                ViewBag.Fail = "Cập nhật không thành công!";
            }

            return View(info);
        }


        // POST: /Users/Delete
        [HttpPost]
        public ActionResult Delete(string userId)
        {
            Guid GuserId = Guid.Parse(userId);
            if (usersModel.Delete(GuserId) > 0)
            {
                return Json(new { message = "SUCCESS" });
            }
            else
            {
                return Json(new { message = "FAIL" });
            }

        }

    }
}
