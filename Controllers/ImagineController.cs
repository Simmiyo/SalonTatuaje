using SalonTatuaje.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonTatuaje.Controllers
{
    public class ImagineController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Imagine

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            List<Imagine> imagini = db.Imagini.ToList();
            return View(imagini);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Tatuaje()
        {
            List<Artist> artisti = db.Artisti.ToList();
            List<Imagine> tatuaje = new List<Imagine>();
            foreach(Artist art in artisti)
            {
                foreach(Imagine img in art.IstoricTatuaje)
                {
                    tatuaje.Add(img);
                }
            }
            return View(tatuaje);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Proiecte()
        {
            List<Artist> artisti = db.Artisti.ToList();
            List<Imagine> desene = new List<Imagine>();
            foreach (Artist art in artisti)
            {
                foreach (Imagine img in art.PortofoliuDesene)
                {
                    desene.Add(img);
                }
            }
            return View(desene);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Galerie(String idxs)
        {
            if (idxs != string.Empty)
            {
                List<int> ids = idxs.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                List<Imagine> imgs = new List<Imagine>();
                foreach (int id in ids)
                {
                    Imagine img = db.Imagini.Find(id);
                    if (img != null)
                    {
                        imgs.Add(img);
                    }
                    else
                    {
                        return HttpNotFound("Nu a fost gasita poza " + id.ToString() + "!");
                    }
                    
                }
                if (imgs.Count > 0)
                {
                    return View(imgs);
                }
            }
            return HttpNotFound("Lipseste parametrul care sa indice codurile pozelor!");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Imagine img = db.Imagini.Find(id);
                if (img != null)
                {
                    ViewBag.titlu = Path.GetFileNameWithoutExtension(img.PathImagine);
                    return View(img);
                }
                return HttpNotFound("Nu a fost gasita poza " + id.ToString() + "!");
            }
            return HttpNotFound("Lipseste parametrul care sa indice codul pozei!");
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New()
        {
            Imagine img = new Imagine();
            return View(img);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New(Imagine img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = Path.GetFileName(img.ImageFile.FileName);
                    img.PathImagine = "~/Content/Imagini/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Imagini/"), fileName);
                    img.ImageFile.SaveAs(fileName);
                    db.Imagini.Add(img);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(img);
            }
            catch (Exception e)
            {
                return View(img);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Imagine img = db.Imagini.Find(id);

                if (img == null)
                {
                    return HttpNotFound("Nu am gasit imaginea cu codul " + id.ToString() + "!");
                }
                return View(img);
            }
            return HttpNotFound("Lipseste parametrul cu codul imaginii!");
        }
        
        [HttpPut]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id, Imagine imgRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Imagine img = db.Imagini.SingleOrDefault(a => a.CodImagine.Equals(id));

                    string oldSource = Server.MapPath(img.PathImagine);
                    string newSource = oldSource.Replace(Path.GetFileName(img.PathImagine), imgRequest.ImageFile.FileName);
                    imgRequest.PathImagine = img.PathImagine.Replace(Path.GetFileName(img.PathImagine), imgRequest.ImageFile.FileName);
                    
                    if (TryUpdateModel(img))
                    {
                        img.PathImagine = imgRequest.PathImagine;
                        imgRequest.ImageFile.SaveAs(newSource);
                        System.IO.File.Delete(oldSource);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(imgRequest);
            }
            catch (Exception e)
            {
                return View(imgRequest);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int? id)
        {
            Imagine img = db.Imagini.SingleOrDefault(a => a.CodImagine == id);
            if (img != null)
            {
                System.IO.File.Delete(Server.MapPath(img.PathImagine));
                db.Imagini.Remove(img);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Nu s-a gasit poza " + id.ToString() + "!");
        }
    }
}