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
            UserInfo.Import((UserDetail)(Session["userinfo"]));

            var userlist = db.usr_getUsers(UserInfo.getUserID());

            ViewBag.UserMessage = TempData["usermessage"];
            return View(userlist.ToList());
        }

        public ActionResult Reset( long? UserID)
        {
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usr_getUser_Result user = db.usr_getUser(id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.DefaultSupplier = UserInfo.getSupplierCode();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string UserName, string SupplierCode, string FullName)
        {
            System.Data.Entity.Core.Objects.ObjectParameter rc = new System.Data.Entity.Core.Objects.ObjectParameter("rC", typeof(int));
            System.Data.Entity.Core.Objects.ObjectParameter errormessage = new System.Data.Entity.Core.Objects.ObjectParameter("errorMessage", typeof(string));

            db.usr_addUser(UserInfo.getUserID(), UserName, SupplierCode, FullName, rc,errormessage);

            TempData["usermessage"] = errormessage.Value;
            return RedirectToAction("Index");

        }

        // GET: User/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = user.UserName;
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,UID,CreatedOn,SupplierCode,FullName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
