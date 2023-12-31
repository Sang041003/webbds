﻿using System;
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
    public class NguoiDungController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();

        // GET: NguoiDung
        public ActionResult Index()
        {
            var batDongSans = db.BatDongSans.Include(b => b.MoiGioi)
                              .Where(b => b.TrangThai == true)
                              .ToList();
            return View(batDongSans.ToList());
        }

        // GET: NguoiDung/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NguoiDung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNguoiDung,Ten,Email,DienThoai,DiaChi,Password")] NguoiDung nguoiDung, string submitButton)
        {

            if (submitButton == "Đăng ký")
            {
                var nguoiDungs = db.NguoiDungs.FirstOrDefault(k => k.Email == nguoiDung.Email);
                if (nguoiDungs != null)
                {
                    ModelState.AddModelError("CustomError", "Email này đã được đăng kí");
                    ViewBag.ms = "Email này đã được đăng kí";
                }
                if (ModelState.IsValid)
                {
                    // kiểm tra xem người ta có đăng ký với Email này chưa!
                    db.NguoiDungs.Add(nguoiDung);
                    db.SaveChanges();

                }
            }
            else if (submitButton == "Đăng nhập")
            {
                var nguoiDungLogin = db.NguoiDungs.FirstOrDefault(k => k.Email == nguoiDung.Email && k.Password == nguoiDung.Password);
                if (nguoiDungLogin != null)
                {
                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    Session["TaiKhoan"] = nguoiDungLogin;
                    return RedirectToAction("Index", "NguoiDung");
                }
                else
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập và mật khẩu không đúng!";
                    return View(nguoiDungLogin);
                }

            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNguoiDung,Ten,Email,DienThoai,DiaChi,Password")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiDung).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            db.NguoiDungs.Remove(nguoiDung);
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
