using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DORIS.Models;

namespace DORIS.Controllers
{
    public class UserController : Controller
    {
        private DDTrack_SandBoxEntities db = new DDTrack_SandBoxEntities();
        private static UserDetail UserInfo = new UserDetail();

        // GET: User
        public ActionResult Index()
        {
            if (!UserInfo.IsValid() && Session["userinfo"] != null )
            {
                UserInfo.Import((UserDetail)Session["userinfo"]);
            }

            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");

            UserInfo.Import((UserDetail)(Session["userinfo"]));

            var userlist = db.usr_getUsers(UserInfo.getUserID());

            ViewBag.UserMessage = TempData["usermessage"];
            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();
            ViewBag.AdminLevel = UserInfo.getAdminLevel();
            return View(userlist.ToList());
        }

        public ActionResult Reset( long? UserID)
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.setPassword(UserID, "monkey");
            
            return RedirectToAction("Details", "User", new { id = UserID });
           
            

        }
        // GET: User/Details/5
        public ActionResult Details(long? id)
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usr_getUser_Result user = db.usr_getUser(id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();
            ViewBag.AdminLevel = UserInfo.getAdminLevel();
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            ViewBag.DefaultSupplier = UserInfo.getSupplierCode();

            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();
            ViewBag.AdminLevel = UserInfo.getAdminLevel();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string UserName, string SupplierCode, string FullName)
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            System.Data.Entity.Core.Objects.ObjectParameter rc = new System.Data.Entity.Core.Objects.ObjectParameter("rC", typeof(int));
            System.Data.Entity.Core.Objects.ObjectParameter errormessage = new System.Data.Entity.Core.Objects.ObjectParameter("errorMessage", typeof(string));

            db.usr_addUser(UserInfo.getUserID(), UserName, SupplierCode, FullName, rc,errormessage);

            TempData["usermessage"] = errormessage.Value;
            return RedirectToAction("Index");

        }

        // GET: User/Edit/5
        public ActionResult Edit(long? id)
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            usr_getUser_Result user = db.usr_getUser(id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();
            ViewBag.AdminLevel = UserInfo.getAdminLevel();

            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,UID,CreatedOn,SupplierCode,FullName")] User user)
        {
            if (!UserInfo.IsValid())
                return RedirectToAction("Login", "Login");
            //   if (ModelState.IsValid)
            //     {
            //    db.Entry(user).State = EntityState.Modified;
            System.Data.Entity.Core.Objects.ObjectParameter rc = new System.Data.Entity.Core.Objects.ObjectParameter("rc", 0);
                System.Data.Entity.Core.Objects.ObjectParameter em = new System.Data.Entity.Core.Objects.ObjectParameter("errormessage", "");
                db.usr_updateUser((int)UserInfo.getUserID(),(int) user.UserID, user.UserName, user.SupplierCode, user.FullName, rc, em);


              //  db.SaveChanges();
                return RedirectToAction("Index");
      //      }
            return View(user);
        }

     

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
