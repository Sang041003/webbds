using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webbds.Models;
using System.Globalization;

namespace webbds.Controllers
{
    public class LienHeNguoiDungController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();

        // GET: LienHeNguoiDung
        public ActionResult Index()
        {
            var lienHeNguoiDungs = db.LienHeNguoiDungs.Include(l => l.BatDongSan).Include(l => l.NguoiDung).Include(l => l.TaiKhoan);
            return View(lienHeNguoiDungs.ToList());
        }

        // GET: LienHeNguoiDung/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHeNguoiDung lienHeNguoiDung = db.LienHeNguoiDungs.Find(id);
            if (lienHeNguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(lienHeNguoiDung);
        }

        // GET: LienHeNguoiDung/Create
        public ActionResult Create()
        {
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe");
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten");
            ViewBag.NguoiPhuTrach = new SelectList(db.TaiKhoans, "ID", "TaiKhoan1");
            return View();
        }

        // POST: LienHeNguoiDung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NgayLienHe,SDT,GhiChu")] LienHeNguoiDung lienHeNguoiDung)
        {
            BatDongSan batdongsan = Session["MaDuAn"] as BatDongSan;
            NguoiDung nguoidung = Session["TaiKhoan"] as NguoiDung;

            int mabds = (int)batdongsan.MaBatDongSan;
            int manguoidung = (int)nguoidung.MaNguoiDung;
            //DateTime ngayLienHe = lienHeNguoiDung.NgayLienHe ?? DateTime.Now;
            DateTime? ngaylienhe = lienHeNguoiDung.NgayLienHe;
            if (ngaylienhe.HasValue && ngaylienhe.Value.Date > DateTime.Now.Date)
            {
                // Chuyển đổi ngày từ dd/MM/yyyy sang MM/dd/yyyy trước khi Parse
                string formattedDate = lienHeNguoiDung.NgayLienHe.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                // ParseExact thành ngày tháng năm trong format MM/dd/yyyy
                //ngaylienhe = DateTime.ParseExact(formattedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                ngaylienhe = DateTime.ParseExact(lienHeNguoiDung.NgayLienHe.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                ngaylienhe = DateTime.Now;
            }
            string sdt = lienHeNguoiDung.SDT;
            string ghichu = lienHeNguoiDung.GhiChu;

            if (ModelState.IsValid)
            {
                //tạo một đối tượng mới LienHeNguoiDung và gán giá trị
                var lienhe = new LienHeNguoiDung
                {
                    MaBatDongSan = mabds,
                    MaNguoiDung = manguoidung,
                    NgayLienHe = ngaylienhe,
                    SDT = sdt,
                    GhiChu = ghichu
                };
                db.LienHeNguoiDungs.Add(lienhe);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            // Load các dữ liệu cần thiết để hiển thị lên View khi có lỗi
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe", mabds);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten", manguoidung);
            ViewBag.NguoiPhuTrach = new SelectList(db.TaiKhoans, "ID", "TaiKhoan1", lienHeNguoiDung.NguoiPhuTrach);
            return View(lienHeNguoiDung);
        }

        // GET: LienHeNguoiDung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHeNguoiDung lienHeNguoiDung = db.LienHeNguoiDungs.Find(id);
            if (lienHeNguoiDung == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe", lienHeNguoiDung.MaBatDongSan);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten", lienHeNguoiDung.MaNguoiDung);
            ViewBag.NguoiPhuTrach = new SelectList(db.TaiKhoans, "ID", "TaiKhoan1", lienHeNguoiDung.NguoiPhuTrach);
            return View(lienHeNguoiDung);
        }

        // POST: LienHeNguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLienHe,MaNguoiDung,MaBatDongSan,NgayLienHe,NguoiPhuTrach")] LienHeNguoiDung lienHeNguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lienHeNguoiDung).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe", lienHeNguoiDung.MaBatDongSan);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten", lienHeNguoiDung.MaNguoiDung);
            ViewBag.NguoiPhuTrach = new SelectList(db.TaiKhoans, "ID", "TaiKhoan1", lienHeNguoiDung.NguoiPhuTrach);
            return View(lienHeNguoiDung);
        }

        // GET: LienHeNguoiDung/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHeNguoiDung lienHeNguoiDung = db.LienHeNguoiDungs.Find(id);
            if (lienHeNguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(lienHeNguoiDung);
        }

        // POST: LienHeNguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LienHeNguoiDung lienHeNguoiDung = db.LienHeNguoiDungs.Find(id);
            db.LienHeNguoiDungs.Remove(lienHeNguoiDung);
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
