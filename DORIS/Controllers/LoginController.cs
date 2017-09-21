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

        private static UserDetail UserInfo = new UserDetail();
      
        public ActionResult Login()
        {
            ViewBag.LoginResult = "";
            return View();
        }

        public ActionResult Logout()
        {
           
            TempData.Remove("hash");

            return RedirectToAction("Login");
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName , string Password,string SupplierCode)
        {
          
            string ipAddress = "";
            ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if(string.IsNullOrEmpty(ipAddress))
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];


            System.Data.Entity.Core.Objects.ObjectParameter hashValue = new System.Data.Entity.Core.Objects.ObjectParameter("token", typeof(string));
            int rc = 0;
            System.Data.Entity.Core.Objects.ObjectParameter change_param = new System.Data.Entity.Core.Objects.ObjectParameter("change", "N");
            if ( String.IsNullOrEmpty(Password) || Password == "monkey")
            {
                rc = db.loginUser(UserName, "monkey", SupplierCode, hashValue,ipAddress,change_param);
                
                if ( (string)hashValue.Value != "0" ) 
                {
                    TempData["hashreset"] = hashValue.Value;
                    System.Data.Entity.Core.Objects.ObjectParameter op = new System.Data.Entity.Core.Objects.ObjectParameter("validuser", 0);
                    getUserDetails_Result ud = db.getUserDetails((string)hashValue.Value, op).Single();

                    UserInfo.setDetails(ud.UserName, ud.FullName, ud.SupplierCode, ud.SupplierName, (string)hashValue.Value, ud.UserID,(int)ud.AdminLevel,ud.AdminLevelName);
                    assignViewBagDefaults();
                    return RedirectToAction("Expired");
                }
            }
           
            rc = db.loginUser(UserName, Password, SupplierCode, hashValue,ipAddress,change_param);

            ViewBag.Hash = hashValue.Value;    

            if ( (string)hashValue.Value != "0" )
            {
                System.Data.Entity.Core.Objects.ObjectParameter op = new System.Data.Entity.Core.Objects.ObjectParameter("validuser", 0);

                getUserDetails_Result ud = db.getUserDetails((string)hashValue.Value, op).Single();
                
                UserInfo.setDetails(ud.UserName, ud.FullName, ud.SupplierCode, ud.SupplierName, (string)hashValue.Value, ud.UserID, (int)ud.AdminLevel, ud.AdminLevelName);

                if ( (string)change_param.Value == "Y" )
                {
                    return RedirectToAction("Expired");
                }
                return RedirectToAction("index", "Order", new { H = hashValue.Value } );  
            }
            ViewBag.LoginResult = "Login failed";
            return View();
        }

        public ActionResult Reset()
        {
            System.Data.Entity.Core.Objects.ObjectParameter validUser = new System.Data.Entity.Core.Objects.ObjectParameter("ValidUser", typeof(bool));

            getUserDetails_Result ud = db.getUserDetails((string)TempData["hashreset"], validUser).Single();
            ViewBag.UserID = ud.UserID;
            assignViewBagDefaults();

            usr_getUser_Result usr = db.usr_getUser(ud.UserID).Single();
            if ( usr != null )
            {
                usr.Password = "";
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

        public ActionResult Expired()
        {
            if (UserInfo == null)
                return RedirectToAction("Login");
            ViewBag.ExpiredMessage = TempData["ExpiredMessage"];
            assignViewBagDefaults();
            return View();

        }
        [HttpPost]

        public ActionResult Expired ( FormCollection fc )
        {
            ViewBag.ExpiredMessage = "";
            if ( UserInfo == null)
                return RedirectToAction("Login");
            
            if ( UserInfo.IsValid() )         {
                if ( fc["password"] == fc["password2"] )
                {
                    if ( fc["password"].Length < 8 )
                    {
                        TempData["ExpiredMessage"] = "Password must be between 8 - 15 characters";
                        assignViewBagDefaults();
                        ViewBag.ExpiredMessage = TempData["ExpiredMessage"];
                        return View();
                    }
                }
                else
                {
                    TempData["ExpiredMessage"] = "Passwords do not match";
                    assignViewBagDefaults();
                    ViewBag.ExpiredMessage = TempData["ExpiredMessage"];
                    return View();

                }

                db.setPassword(UserInfo.getUserID(), fc["password"]);
                return RedirectToAction("index", "Order", new { H = UserInfo.getHash() });
            }

            return RedirectToAction("Login");
        }
        public void assignViewBagDefaults()
        {
            if (UserInfo != null)
            {
                ViewBag.SupplierCode = UserInfo.getSupplierCode();
                ViewBag.SupplierName = UserInfo.getSupplierName();
                ViewBag.UserName = UserInfo.getUserName();
                ViewBag.FullName = UserInfo.getFullName();
                ViewBag.Hash = UserInfo.getHash();
                
            }
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
