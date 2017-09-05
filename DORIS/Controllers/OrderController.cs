﻿using System;
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
            if (!UserInfo.IsValid())
            {
                return RedirectToAction("Login", "Users");
            }

            List<tp_getSummary_Result> OrderSummary = db.tp_getSummary(UserInfo.getSupplierCode()).ToList();




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
                return RedirectToAction("Login", "Users");
            }
            string hash = H;
            if (!UserInfo.IsValid())
            {
                System.Data.Entity.Core.Objects.ObjectParameter validUser = new System.Data.Entity.Core.Objects.ObjectParameter("ValidUser", typeof(bool));

                try
                {
                    getUserDetails_Result ud = db.getUserDetails(hash, validUser).Single();
                    UserInfo.setDetails(ud.UserName, ud.FullName, ud.SupplierCode, ud.SupplierName,hash);
                }
                catch(Exception e)
                {

                }
            }
            if ( ! UserInfo.IsValid())
            {
                return RedirectToAction("Login", "Users");
            }

            return RedirectToAction("Summary");

         //   return View();
        }

        public ActionResult Display(string OS)
        {
            if ( OS == null || OS.Length == 0 )
            {
                return RedirectToAction("Login", "Users");
            }

            if (!UserInfo.IsValid())
            {
                return RedirectToAction("Login", "Users");
            }


            string OrderStatus = OS;

            System.Data.Entity.Core.Objects.ObjectParameter rc = new System.Data.Entity.Core.Objects.ObjectParameter("ReturnCode", typeof(int));
            System.Data.Entity.Core.Objects.ObjectParameter ct = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));


            List<getOrderStatus_Result> res = db.getOrderStatus(UserInfo.getSupplierCode(), OS, ct, rc).ToList();
            if ( res.Count() > 0 )
            {
                ViewBag.OrderStatus = res.First().UserStatusText;
            }  

            ViewBag.OrderCount = ct.Value;

            ViewBag.SupplierCode = UserInfo.getSupplierCode();
            ViewBag.SupplierName = UserInfo.getSupplierName();
            ViewBag.UserName = UserInfo.getUserName();
            ViewBag.FullName = UserInfo.getFullName();


            return View(res);

        }
    }
}