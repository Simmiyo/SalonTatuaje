using Microsoft.AspNet.Identity;
using SalonTatuaje.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SalonTatuaje.Controllers
{
    public class ProgramareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programare
        [Authorize]
        public ActionResult Index()
        {
            List<Programare> programari = db.Programari.ToList();
            if (User.IsInRole("Editor"))
            {
                var artisti = db.Artisti.ToList();
                string editorId = GetUserId();
                Artist editor = artisti.Find(a => a.EditorId == editorId);
                programari = programari.FindAll(p => p.ArtistId == editor.ArtistCod);
            }
            else if (User.IsInRole("Client"))
            {
                string clientId = GetUserId();
                programari = programari.FindAll(p => p.UserId == clientId);
            }
            return View(programari);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Programare program = db.Programari.Find(id);
                ViewBag.ids = string.Join(",", program.Proiecte.Select(p => p.CodImagine.ToString()));
                if (program != null)
                {
                    if (User.IsInRole("Client"))
                    {
                        string clientId = GetUserId();
                        if (clientId != program.UserId)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    return View(program);
                }
                return HttpNotFound("Nu a fost gasită programarea  avand codul " + id.ToString() + "!");
            }
            return HttpNotFound("Lipseste parametrul care sa indice codul programării!");
        }

       
        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            Programare prog = new Programare();
            ViewBag.artisti = GetAllArtists();
            ViewBag.tipTatuaje = GetAllTipTatuaje();
            return View(prog);
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult New(Programare programareRequest, int[] proiecteId, int[] tipTatuajeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int counter = 0;
                    if (proiecteId != null)
                    {
                        counter += proiecteId.Length;
                    }
                    if (programareRequest.ImageFilesCerinte[0] != null)
                    {
                        counter += programareRequest.ImageFilesCerinte.Length;
                    }

                    if (counter != tipTatuajeId.Length)
                    {
                        return View(programareRequest);
                    }
                    else
                    {
                        int pret_total = 0;
                        programareRequest.Tartist = db.Artisti.Find(programareRequest.ArtistId);
                        programareRequest.TipuriTatuaje = new List<TipTatuaj>();
                        foreach(int id in tipTatuajeId)
                        {
                            var tip = db.TipuriTatuaje.Find(id);
                            pret_total += tip.Pret;
                            programareRequest.TipuriTatuaje.Add(tip);
                        }
                        
                        if (proiecteId != null)
                        {
                            programareRequest.Proiecte = new List<Imagine>();
                            foreach (int id in proiecteId)
                            {
                                var img = db.Imagini.Find(id);
                                programareRequest.Proiecte.Add(img);
                            }
                        }
                       
                        if (programareRequest.ImageFilesCerinte[0] != null)
                        {
                            programareRequest.CerinteNoi = new List<Imagine>();
                            string directory = "~/Content/Imagini/neatasat";
                            Directory.CreateDirectory(Server.MapPath(directory));
                            foreach (HttpPostedFileBase imgFile in programareRequest.ImageFilesCerinte)
                            {
                                Imagine img = new Imagine();
                                string fileName = Path.GetFileName(imgFile.FileName);
                                img.PathImagine = directory + "/" + fileName;
                                fileName = Path.Combine(Server.MapPath(directory), fileName);
                                imgFile.SaveAs(fileName);
                                programareRequest.CerinteNoi.Add(img);
                                db.Imagini.Add(img);
                            }
                        }
                        programareRequest.PretTotal = pret_total;
                        programareRequest.UserId = GetUserId();
                        programareRequest.Tclient = db.Users.Find(programareRequest.UserId);
                        db.Programari.Add(programareRequest);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.artisti = GetAllArtists();
                ViewBag.tipTatuaje = GetAllTipTatuaje();
                return View(programareRequest);
            }
            catch (Exception e)
            {
                ViewBag.artisti = GetAllArtists();
                ViewBag.tipTatuaje = GetAllTipTatuaje();
                return View(programareRequest);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Programare program = db.Programari.Find(id);

                if (program == null)
                {
                    return HttpNotFound("Nu s-a gasit programarea cu codul " + id.ToString() + "!");
                }
                if (User.IsInRole("Client"))
                {
                    string clientId = GetUserId();
                    if (clientId != program.UserId)
                    {
                        return RedirectToAction("Index");
                    }  
                }
                ViewBag.artisti = GetAllArtists();
                ViewBag.tipTatuaje = GetAllTipTatuaje();
                ViewBag.tipTatuajeAlese = program.TipuriTatuaje.Select(t => t.NumeTip);
                ViewBag.proiecteAlese = program.Proiecte.Select(p => Path.GetFileNameWithoutExtension(p.PathImagine));
                ViewBag.cerinteAlese = program.CerinteNoi.Select(p => Path.GetFileNameWithoutExtension(p.PathImagine));
                return View(program);
            }
            return HttpNotFound("Lipseste parametrul cu codul programarii!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult Edit(int id, Programare programareRequest, int[] proiecteId, int[] tipTatuajeId)
        {
            Programare program = db.Programari.SingleOrDefault(p => p.ProgramareCod.Equals(id));
            ViewBag.artisti = GetAllArtists();
            ViewBag.tipTatuaje = GetAllTipTatuaje();
            ViewBag.tipTatuajeAlese = program.TipuriTatuaje.Select(t => t.NumeTip);
            ViewBag.proiecteAlese = program.Proiecte.Select(p => Path.GetFileNameWithoutExtension(p.PathImagine));
            ViewBag.cerinteAlese = program.CerinteNoi.Select(p => Path.GetFileNameWithoutExtension(p.PathImagine));
            try
            {
                if (ModelState.IsValid)
                {

                    int counter = 0;
                    if (proiecteId != null)
                    {
                        counter += proiecteId.Length;
                    }
                    if (programareRequest.ImageFilesCerinte[0] != null)
                    {
                        counter += programareRequest.ImageFilesCerinte.Length;
                    }

                    if (tipTatuajeId != null && counter != tipTatuajeId.Length)
                    {
                        return View(programareRequest);
                    }
                    else if(TryUpdateModel(program))
                    {
                        int pret_total = 0;
                        program.Sala = programareRequest.Sala;
                        program.Ora = programareRequest.Ora;
                        program.Data = programareRequest.Data;
                        program.ArtistId = programareRequest.ArtistId;
                        program.Tartist = db.Artisti.Find(programareRequest.ArtistId);
                        if (tipTatuajeId != null)
                        {
                            program.TipuriTatuaje = new List<TipTatuaj>();
                            foreach (int idx in tipTatuajeId)
                            {
                                var tip = db.TipuriTatuaje.Find(idx);
                                pret_total += tip.Pret;
                                program.TipuriTatuaje.Add(tip);
                            }
                        }
                        if (proiecteId != null)
                        {
                            program.Proiecte = new List<Imagine>();
                            foreach (int idx in proiecteId)
                            {
                                var img = db.Imagini.Find(idx);
                                program.Proiecte.Add(img);
                            }
                        }
                        if (programareRequest.ImageFilesCerinte[0] != null)
                        {
                            program.CerinteNoi = new List<Imagine>();
                            string directory = "~/Content/Imagini/neatasat";
                            Directory.CreateDirectory(Server.MapPath(directory));
                            foreach (HttpPostedFileBase imgFile in programareRequest.ImageFilesCerinte)
                            {
                                Imagine img = new Imagine();
                                string fileName = Path.GetFileName(imgFile.FileName);
                                img.PathImagine = directory + "/" + fileName;
                                fileName = Path.Combine(Server.MapPath(directory), fileName);
                                imgFile.SaveAs(fileName);
                                program.CerinteNoi.Add(img);
                                db.Imagini.Add(img);
                            }
                        }
                        if (tipTatuajeId != null)
                        {
                            program.PretTotal = pret_total;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(programareRequest);
            }
            catch (Exception e)
            {
                return View(programareRequest);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult Delete(int? id)
        {
            Programare program = db.Programari.Include("TipuriTatuaje").Include("Proiecte").
                Include("CerinteNoi").Include("Tartist").Include("Tclient").SingleOrDefault(p => p.ProgramareCod == id);
            if (program != null)
            {
                if (User.IsInRole("Client"))
                {
                    string clientId = GetUserId();
                    if (clientId != program.UserId)
                    {
                        return RedirectToAction("Index");
                    }
                }
                db.Programari.Remove(program);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Nu am gasit programarea cu id " + id.ToString() + "!");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllArtists()
        {
            var selectList = new List<SelectListItem>();
            foreach (Artist art in db.Artisti.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = art.ArtistCod.ToString(),
                    Text = art.Nume
                });
            }
            return selectList;
        }

        [NonAction]
        public MultiSelectList GetAllTipTatuaje()
        {
            var tipTatuaje = db.TipuriTatuaje.Select(t => new {
                Id = t.TipTatuajCod,
                Nume = t.NumeTip
            }).ToList();
            return new MultiSelectList(tipTatuaje, "Id", "Nume");
        }

        [NonAction]
        public string GetUserId()
        {
            string userId = null;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
            }
            return userId;
        }

        public JsonResult GetAllImagesForArtist(string id)
        {
            var imagini = db.Artisti.Find(Convert.ToInt32(id)).PortofoliuDesene.Select(i => new {
                Id = i.CodImagine,
                Nume = Path.GetFileNameWithoutExtension(i.PathImagine)
        }).ToList();
            return Json(imagini, JsonRequestBehavior.AllowGet);
        }
    }
}