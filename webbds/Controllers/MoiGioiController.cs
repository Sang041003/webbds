using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webbds.Models;

namespace webbds.Controllers
{
    public class MoiGioiController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();

        // GET: MoiGioi
        public ActionResult Index()
        {
            var batDongSans = db.BatDongSans.Include(b => b.MoiGioi)
                                  .Where(b => b.TrangThai == true)
                                  .ToList();
            return View(batDongSans.ToList());
        }

        // GET: MoiGioi/Details/5
        public ActionResult Details(int? id)
        {
            var moiGiois = db.MoiGiois.ToList();
            return View(moiGiois);
        }

        // GET: MoiGioi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MoiGioi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDaiLy,Ten,Email,DienThoai,DiaChi,Passowd")] MoiGioi moiGioi, string submitButton)
        {
            if(submitButton == "Đăng ký")
            {
                var moiGiois = db.MoiGiois.FirstOrDefault(k => k.Email == moiGioi.Email || k.Email == moiGioi.DienThoai);
                if(moiGiois != null)
                {
                    ModelState.AddModelError(String.Empty, "Email hoặc số điện thoại đã được đăng ký");
                    ViewBag.ErrorMessage = 1;
                    TempData["ErrorMessage"] = "Email hoặc số điện thoại đã được đăng ký";
                    return View(moiGioi);

                }
                if (ModelState.IsValid)
                {
                    db.MoiGiois.Add(moiGioi);
                    db.SaveChanges();
                }
            }
            else if(submitButton == "Đăng nhập")
            {
                var moiGiois = db.MoiGiois.FirstOrDefault(k => k.Email == moiGioi.Email && k.Passowd == moiGioi.Passowd);
                if(moiGiois != null)
                {
                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    Session["MoiGioi"] = moiGiois;
                    return RedirectToAction("Index", "MoiGioi");
                } 
                else
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập và mật khẩu không đúng!";
                    return View(moiGioi);
                }
            }    
            return View(moiGioi);
        }

        // GET: MoiGioi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoiGioi moiGioi = db.MoiGiois.Find(id);
            if (moiGioi == null)
            {
                return HttpNotFound();
            }
            return View(moiGioi);
        }

        // POST: MoiGioi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDaiLy,Ten,Email,DienThoai,DiaChi")] MoiGioi moiGioi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moiGioi).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moiGioi);
        }

        // GET: MoiGioi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoiGioi moiGioi = db.MoiGiois.Find(id);
            if (moiGioi == null)
            {
                return HttpNotFound();
            }
            return View(moiGioi);
        }

        // POST: MoiGioi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MoiGioi moiGioi = db.MoiGiois.Find(id);
            db.MoiGiois.Remove(moiGioi);
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
