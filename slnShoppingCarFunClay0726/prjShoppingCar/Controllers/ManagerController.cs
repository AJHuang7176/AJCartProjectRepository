using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class ManagerController : Controller
    {
        dbShoppingCarEntities db = new dbShoppingCarEntities();

        public ActionResult Index()
        {
            var manager = (tMember)(Session["Member"]);

            if (manager.fUserId != "Manager")
            {
                return View("Index", "MemberManagerLogin", manager);
            }

            var members = db.tMember.Where(m=>m.fUserId != "Manager").ToList();

            return View("Index", "_LayoutManager", members);
        }

        public ActionResult ManagerLogin()
        {
            return View();
        }

        //Post: Home/網頁管理人登入
        [HttpPost]
        public ActionResult ManagerLogin(string fUserId, string fPwd)
        {
            var member = db.tMember
                .Where(m => m.fUserId == fUserId && m.fUserId == "Manager" && m.fPwd == fPwd)
                .FirstOrDefault();

            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            Session["WelCome Member"] = member.fName + "歡迎光臨";
            Session["Member"] = member;

            return RedirectToAction("Index");
        }

        //刪除會員
        public ActionResult Delete(int id)
        {
            var removeMember = db.tMember.Where(m => m.fId == id).FirstOrDefault();
            db.tMember.Remove(removeMember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //修改會員資料
        public ActionResult Edit(int id)
        {
            var todo = db.tMember.Where(m => m.fId == id).FirstOrDefault();
            return View(todo);
        }

        [HttpPost]
        public ActionResult Edit(int fId, string fUserId, string fName, string fEmail)
        {
            var todo = db.tMember.Where(m => m.fId == fId).FirstOrDefault();
            todo.fUserId  = fUserId;
            todo.fName  = fName;
            todo.fEmail  = fEmail;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        //訂單管理
        public ActionResult ManagerOrderList()
        {
            var orders = db.tOrder.ToList();

            return View("ManagerOrderList", "_LayoutManager", orders);
        }

        //訂單明細
        public ActionResult ManagerOrderDetail(string fOrderGuid)
        {
            var orderDetails = db.tOrderDetail
                .Where(m => m.fOrderGuid == fOrderGuid).ToList();

            return View("ManagerOrderDetail", "_LayoutManager", orderDetails);
        }

        public ActionResult EditManagerOrderList(string fOrderGuid)
        {
            var todo = db.tOrder.Where(m => m.fOrderGuid == fOrderGuid).FirstOrDefault();
            return View(todo);
        }

        //網頁管理人明細編輯
        [HttpPost]
        public ActionResult EditManagerOrderList(string fOrderGuid, string fReceiver, string fEmail, string fAddress)
        {
            var todo = db.tOrder.Where(m => m.fOrderGuid == fOrderGuid).FirstOrDefault();
            todo.fReceiver = fReceiver;
            todo.fEmail = fEmail;
            todo.fAddress = fAddress;
            db.SaveChanges();
            return RedirectToAction("ManagerOrderList");
        }

        //網頁管理人刪除訂單
        public ActionResult DeleteManagerOrderList(string fOrderGuid)
        {
            var removeOrder = db.tOrder.Where(m => m.fOrderGuid == fOrderGuid).FirstOrDefault();
            db.tOrder.Remove(removeOrder);
            db.SaveChanges();
            return RedirectToAction("ManagerOrderList");
        }

    }
}