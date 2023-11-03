using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbds.Models;
using System.Data.Entity;
using System.Net;
namespace webbds.Controllers
{
    public class HomeController : Controller
    {
        private BDS_TestEntities db = new BDS_TestEntities();
        public ActionResult Index()
        {
            var batDongSans = db.BatDongSans.Include(b => b.MoiGioi)
                                  .Where(b => b.TrangThai == true)
                                  .ToList();
            return View(batDongSans.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: BatDongSan/Details/5
        public ActionResult CT_BDS(int id)
        {
            var ct_bds = db.BatDongSans.FirstOrDefault(s => s.MaBatDongSan == id);
            Session["MaDuAn"] = ct_bds;
            return View(ct_bds);
        }
        public ActionResult MyProperties(int id)
        {
            var properties = db.BatDongSans.Where(b => b.MaDaiLy == id).ToList();
            return View(properties);
        }

        public ActionResult MoiGioiList()
        {
            var moiGiois = db.MoiGiois.ToList();
            return View(moiGiois);
        }

        public ActionResult MyProperties1(int id)
        {
            var properties = db.BatDongSans.Where(b => b.MaDaiLy == id).ToList();

            var moigioi = db.MoiGiois.FirstOrDefault(m => m.MaDaiLy == id);
            if (moigioi != null)
            {
                ViewBag.MoiGioiTen = moigioi.Ten;
            }

            return View(properties);
        }
    }
}