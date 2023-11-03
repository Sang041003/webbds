using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbds.Models;

namespace webbds.Controllers
{
    public class SearchController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchProject(string keyword)
        {
            var projects = db.BatDongSans
                            .Where(p => p.TieuDe.Contains(keyword) && p.TrangThai==true)
                            .ToList();

            if (projects.Count == 0)
            {
                // Xử lý khi không tìm thấy dự án
                ViewBag.Message = "Không tìm thấy dự án phù hợp";
                return View("NotFound"); // Tạo view "NotFound" để hiển thị thông báo không tìm thấy dự án
            }
            else
            {
                return View(projects);
            }
        }
    }
}