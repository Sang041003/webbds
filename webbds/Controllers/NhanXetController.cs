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
    public class NhanXetController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();

        // GET: NhanXet
        public ActionResult Index()
        {
            var nhanXets = db.NhanXets.Include(n => n.BatDongSan).Include(n => n.NguoiDung);
            return View(nhanXets.ToList());
        }

        // GET: NhanXet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanXet nhanXet = db.NhanXets.Find(id);
            if (nhanXet == null)
            {
                return HttpNotFound();
            }
            return View(nhanXet);
        }

        // GET: NhanXet/Create
        public ActionResult Create()
        {
            // Khởi tạo đối tượng view model
            var viewModel = new NhanXetViewModel();
            viewModel.Comments = db.NhanXets.ToList();
            viewModel.NewComment = new NhanXet();

            // Lấy thông tin người dùng hiện tại từ Session
            var currentUser = Session["TaiKhoan"] as NguoiDung;

            // Nếu có người dùng đăng nhập, thiết lập MaNguoiDung cho NewComment
            if (currentUser != null)
            {
                viewModel.NewComment.MaNguoiDung = currentUser.MaNguoiDung;
            }

            // Trả về view với view model đã thiết lập
            return View(viewModel);

        }

        // POST: NhanXet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhanXetViewModel nhanXetViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = Session["TaiKhoan"] as NguoiDung;

                if (currentUser != null && nhanXetViewModel.NewComment != null)
                {
                    int manguoidung = currentUser.MaNguoiDung;

                    // Tạo một đối tượng NhanXet mới và gán các thuộc tính
                    NhanXet newComment = new NhanXet
                    {
                        MaNguoiDung = manguoidung,
                        BinhLuan = nhanXetViewModel.NewComment.BinhLuan
                    };

                    // Thêm đối tượng NhanXet mới vào cơ sở dữ liệu và lưu thay đổi
                    db.NhanXets.Add(newComment);
                    db.SaveChanges();
                }

                // Redirect về trang hiện tại để làm mới phần hiển thị bình luận
                return RedirectToAction("Create");
            }

            // Nếu ModelState không hợp lệ, load lại danh sách bình luận và hiển thị lại trang
            nhanXetViewModel.Comments = db.NhanXets.ToList();
            return View(nhanXetViewModel);
        }

        // GET: NhanXet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanXet nhanXet = db.NhanXets.Find(id);
            if (nhanXet == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe", nhanXet.MaBatDongSan);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten", nhanXet.MaNguoiDung);
            return View(nhanXet);
        }

        // POST: NhanXet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhanXet,MaBatDongSan,MaNguoiDung,DanhGia,BinhLuan")] NhanXet nhanXet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanXet).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaBatDongSan = new SelectList(db.BatDongSans, "MaBatDongSan", "TieuDe", nhanXet.MaBatDongSan);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "Ten", nhanXet.MaNguoiDung);
            return View(nhanXet);
        }

        // GET: NhanXet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanXet nhanXet = db.NhanXets.Find(id);
            if (nhanXet == null)
            {
                return HttpNotFound();
            }
            return View(nhanXet);
        }

        // POST: NhanXet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanXet nhanXet = db.NhanXets.Find(id);
            db.NhanXets.Remove(nhanXet);
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
