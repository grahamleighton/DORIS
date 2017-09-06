using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DORIS.Models;
using System.Data.SqlClient;
using System.Data.Sql;


namespace DORIS.Controllers
{
    public class LoginController : Controller
    {
        private DDTrack_SandBoxEntities db = new DDTrack_SandBoxEntities();

      
      
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("userinfo");

            return RedirectToAction("Login");
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName , string Password,string SupplierCode)
        {
            ViewBag.UN = UserName;
            ViewBag.PW = Password;
            ViewBag.SC = SupplierCode;

           
            System.Data.Entity.Core.Objects.ObjectParameter hashValue = new System.Data.Entity.Core.Objects.ObjectParameter("token", typeof(string));
            int rc = 0;

            if ( String.IsNullOrEmpty(Password) || Password == "monkey")
            {
                rc = db.loginUser(UserName, "monkey", SupplierCode, hashValue);
                
                if ( (string)hashValue.Value != "0" ) 
                {
                    TempData["hashreset"] = hashValue.Value;
                    return RedirectToAction("Reset");
                }
            }

            rc = db.loginUser(UserName, Password, SupplierCode, hashValue);

            ViewBag.Hash = hashValue.Value;    

            if ( ViewBag.Hash != "0" )
            {
                return RedirectToAction("index", "Order", new { H = hashValue.Value } );  
            }

            return View();
        }

        public ActionResult Reset()
        {
            System.Data.Entity.Core.Objects.ObjectParameter validUser = new System.Data.Entity.Core.Objects.ObjectParameter("ValidUser", typeof(bool));

            getUserDetails_Result ud = db.getUserDetails((string)TempData["hashreset"], validUser).Single();
            ViewBag.UserID = ud.UserID;

            usr_getUser_Result usr = db.usr_getUser(ud.UserID).Single();
            if ( usr != null )
            {
                return View(usr);
            }

            return View();
        }
        [HttpPost]

        public ActionResult Reset(int UserID , string Password)
        {
            if ( String.IsNullOrEmpty(Password) || Password == "monkey")
            {
                TempData["usermessage"] = "Invalid new password";
                return RedirectToAction("Reset");
            }
            db.setPassword(UserID, Password);
            return RedirectToAction("Login");
           
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
