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

        // GET: Member
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

        //Get: MemberManager/Login
        public ActionResult ManagerLogin()
        {
            return View();
        }

        //Post: Home/Login
        [HttpPost]
        public ActionResult ManagerLogin(string fUserId, string fPwd)
        {
            // 依帳密取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == fUserId && m.fUserId == "Manager" && m.fPwd == fPwd)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            //使用Session變數記錄歡迎詞
            Session["WelCome Member"] = member.fName + "歡迎光臨";
            //使用Session變數記錄登入的會員物件
            Session["Member"] = member;
            //執行Home控制器的Index動作方法
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var removeMember = db.tMember.Where(m => m.fId == id).FirstOrDefault();
            db.tMember.Remove(removeMember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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

        //Get:Index/OrderDetail
        public ActionResult ManagerOrderDetail(string fOrderGuid)
        {
            //根據fOrderGuid找出和訂單主檔關聯的訂單明細，並指定給orderDetails
            var orderDetails = db.tOrderDetail
                .Where(m => m.fOrderGuid == fOrderGuid).ToList();
            //目前訂單明細
            //指定OrderDetail.cshtml套用_LayoutMember.cshtml，View使用orderDetails模型
            return View("ManagerOrderDetail", "_LayoutManager", orderDetails);
        }

        public ActionResult EditManagerOrderList(string fOrderGuid)
        {
            var todo = db.tOrder.Where(m => m.fOrderGuid == fOrderGuid).FirstOrDefault();
            return View(todo);
        }

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

        public ActionResult DeleteManagerOrderList(string fOrderGuid)
        {
            var removeOrder = db.tOrder.Where(m => m.fOrderGuid == fOrderGuid).FirstOrDefault();
            db.tOrder.Remove(removeOrder);
            db.SaveChanges();
            return RedirectToAction("ManagerOrderList");
        }

    }
}