using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DORIS.Models;
using System.Data.SqlClient;
using System.Data.Sql;


namespace DORIS.Controllers
{
    public class OrderController : Controller 
    {
        private DDTrack_SandBoxEntities db = new DDTrack_SandBoxEntities();
        private static string SupplierCode = "";
        private static UserDetail UserInfo = new UserDetail();


        public ActionResult Summary()
        {
            if (!UserInfo.IsValid()  )
            {
                return RedirectToAction("Login", "Login");
            }

            List<tp_getSummary_Result> OrderSummary = db.tp_getSummary(UserInfo.getHash()).ToList();




            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();


            return View(OrderSummary);
 

        }
        // GET: Order
        public ActionResult Index(string H)
        {
            if ( H == null )
            {
                return RedirectToAction("Login", "");
            }
            string hash = H;
            if (!UserInfo.IsValid() || Session["userinfo"] == null || UserInfo.getHash() != H )
            {
                System.Data.Entity.Core.Objects.ObjectParameter validUser = new System.Data.Entity.Core.Objects.ObjectParameter("ValidUser", typeof(bool));

                try
                {
                    getUserDetails_Result ud = db.getUserDetails(hash, validUser).Single();
                    UserInfo.setDetails(ud.UserName, ud.FullName, ud.SupplierCode, ud.SupplierName,hash,ud.UserID, (int)ud.AdminLevel, ud.AdminLevelName);

                    Session["userinfo"] = UserInfo.Export();

                }
                catch(Exception e)
                {

                }
            }
            if ( ! UserInfo.IsValid())
            {
                return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("Summary");

         //   return View();
        }

        public ActionResult Display(string OS)
        {
            if ( OS == null)
            {
                if ( !String.IsNullOrEmpty((string)TempData["OS"]) )
                {
                    OS = (string)TempData["OS"];
                }
            }
            if ( OS == null || OS.Length == 0 )
            {
                return RedirectToAction("Login", "Login");
            }

            if (!UserInfo.IsValid())
            {
                return RedirectToAction("Login", "Login");
            }


            string OrderStatus = OS;

            System.Data.Entity.Core.Objects.ObjectParameter rc = new System.Data.Entity.Core.Objects.ObjectParameter("ReturnCode", typeof(int));
            System.Data.Entity.Core.Objects.ObjectParameter ct = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));


            List<getOrderStatus_Result> res = db.getOrderStatus(UserInfo.getHash(), OS, ct, rc).ToList();
            if ( res.Count() > 0 )
            {
                ViewBag.OrderStatus = res.First().UserStatusText;
            }  

            ViewBag.OrderCount = ct.Value;

            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();
            TempData["OS"] = OS;


            return View(res);

        }
        [HttpGet]
        public ActionResult Update(int? id)
        {
            ViewBag.OrderID = id;

            db.tp_updateOrder(UserInfo.getHash(), id);

            return RedirectToAction("Summary");

        }

        [HttpGet]
        public ActionResult Backout(int? id)
        {
            ViewBag.OrderID = id;

            db.tp_BackoutOrder(UserInfo.getHash(), id);

            return RedirectToAction("Summary");

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
    }
}