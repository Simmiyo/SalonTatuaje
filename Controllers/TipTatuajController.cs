using SalonTatuaje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonTatuaje.Controllers
{
    public class TipTatuajController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TipTatuaj
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<TipTatuaj> tipuri = db.TipuriTatuaje.ToList();
            return View(tipuri);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id = 1)
        {
            if (id.HasValue)
            {
                TipTatuaj tiptauaj = db.TipuriTatuaje.Find(id);
                if (tiptauaj != null)
                {
                    return View(tiptauaj);
                }
                return HttpNotFound("Nu a fost gasită tipul de tatuaj avand codul " + id.ToString() + "!");
            }
            return HttpNotFound("Lipseste parametrul care sa indice codul tipului de tatuaj!");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            TipTatuaj tt = new TipTatuaj();
            ViewBag.culori = GetAllCulori();
            return View(tt);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(TipTatuaj tt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tt.NumeTip = tt.Localizare + "-" + tt.Latime + "-" + tt.Lungime + "-" + tt.Culoare;
                    db.TipuriTatuaje.Add(tt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tt);
            }
            catch (Exception e)
            {
                return View(tt);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                TipTatuaj tt = db.TipuriTatuaje.Find(id);
                if (tt == null)
                {
                    return HttpNotFound("Nu exista tipul de tatuaj " + id.ToString() + "!");
                }
                ViewBag.culori = GetAllCulori();
                return View(tt);
            }
            return HttpNotFound("Lipseste id-ul tipului de tatuaj!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, TipTatuaj tipRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TipTatuaj tt = db.TipuriTatuaje.SingleOrDefault(b => b.TipTatuajCod.Equals(id));
                    if (TryUpdateModel(tt))
                    {
                        tt.Latime = tipRequest.Latime;
                        tt.Lungime = tipRequest.Lungime;
                        tt.Localizare = tipRequest.Localizare;
                        tt.Culoare = tipRequest.Culoare;
                        tt.NumeTip = tt.Localizare + "-" + tt.Latime + "-" + tt.Lungime + "-" + tt.Culoare;
                        tt.Pret = tipRequest.Pret;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(tipRequest);
            }
            catch (Exception)
            {
                return View(tipRequest);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            TipTatuaj tt = db.TipuriTatuaje.Include("Programari").SingleOrDefault(t => t.TipTatuajCod == id);
            if (tt != null)
            {
                db.TipuriTatuaje.Remove(tt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Nu am gasit tipul de tatuaj " + id.ToString() + "!");
        }

        [NonAction]
        public SelectList GetAllCulori()
        {
            var cuolri = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "alb-negru", Value = "alb-negru"},
                    new SelectListItem { Text = "color", Value = "color"},
                }, "Value", "Text");
            return cuolri;
        }
    }
}