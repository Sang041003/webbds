using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webbds.Models;
using System.IO;

namespace webbds.Controllers
{
    public class BatDongSanController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();

        // GET: BatDongSan
        public ActionResult Index()
        {
            var batDongSans = db.BatDongSans.Include(b => b.MoiGioi);
            return View(batDongSans.ToList());
        }

        // GET: BatDongSan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatDongSan batDongSan = db.BatDongSans.Find(id);
            if (batDongSan == null)
            {
                return HttpNotFound();
            }
            return View(batDongSan);
        }

        // GET: BatDongSan/Create
        public ActionResult Create()
        {
            ViewBag.MaDaiLy = new SelectList(db.MoiGiois, "MaDaiLy", "Ten");
            return View();
        }

        // POST: BatDongSan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBatDongSan,TieuDe,MoTa,Gia,DienTich,ViTri,Anh,TienNghi,ThongTinLienHe,NgayTao,NgaySua,MaDaiLy,urlmap")] BatDongSan batDongSan, HttpPostedFileBase Anh)
        {
            MoiGioi moigioi = Session["MoiGioi"] as MoiGioi;
            int MaMG = (int)moigioi.MaDaiLy;
            batDongSan.TrangThai = false;
            if (ModelState.IsValid)
            {
                    
                if (Anh != null && Anh.ContentLength > 0)
                {
                    // Lưu hình ảnh vào thư mục ~/Content/Images
                    var filename = Path.GetFileName(Anh.FileName);
                    var serverPath = Server.MapPath("~/Content/Images");
                    var path = Path.Combine(serverPath, filename);
                    Anh.SaveAs(path);

                    // Lưu đường dẫn của hình ảnh vào trường Anh của đối tượng BatDongSan
                    batDongSan.Anh = "~/Content/Images/" + filename;
                }
                batDongSan.MaDaiLy = MaMG;
                db.BatDongSans.Add(batDongSan);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.MaDaiLy = new SelectList(db.MoiGiois, "MaDaiLy", "Ten", batDongSan.MaDaiLy);
            return RedirectToAction("Index");
        }

        // GET: BatDongSan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatDongSan batDongSan = db.BatDongSans.Find(id);
            if (batDongSan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDaiLy = new SelectList(db.MoiGiois, "MaDaiLy", "Ten", batDongSan.MaDaiLy);
            return View(batDongSan);
        }

        // POST: BatDongSan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBatDongSan,TieuDe,MoTa,Gia,DienTich,ViTri,Anh,TienNghi,ThongTinLienHe,NgayTao,NgaySua,MaDaiLy,TrangThai,urlmap")] BatDongSan batDongSan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batDongSan).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDaiLy = new SelectList(db.MoiGiois, "MaDaiLy", "Ten", batDongSan.MaDaiLy);
            return View(batDongSan);
        }

        // GET: BatDongSan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatDongSan batDongSan = db.BatDongSans.Find(id);
            if (batDongSan == null)
            {
                return HttpNotFound();
            }
            return View(batDongSan);
        }

        // POST: BatDongSan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BatDongSan batDongSan = db.BatDongSans.Find(id);
            db.BatDongSans.Remove(batDongSan);
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


        public ActionResult MyProperties()
        {
            // Lấy thông tin người dùng đang đăng nhập từ session
            MoiGioi moigioi = Session["MoiGioi"] as MoiGioi;
            if (moigioi == null)
            {
                // Nếu người dùng không đăng nhập, chuyển hướng hoặc xử lý tùy ý
                return RedirectToAction("Create", "MoiGioi"); // Ví dụ: chuyển hướng đến trang đăng nhập
            }

            // Lấy danh sách bất động sản của người dùng từ cơ sở dữ liệu
            var properties = db.BatDongSans.Where(b => b.MaDaiLy == moigioi.MaDaiLy).ToList();

            return View(properties);
        }

        public ActionResult AllProperties()
        {
            var properties = db.BatDongSans.Where(prop => prop.TrangThai == true).ToList();
            return View(properties);
        }



    }
}
