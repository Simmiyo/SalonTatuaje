using SalonTatuaje.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonTatuaje.Controllers
{
    public class ArtistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet] 
        public ActionResult Index()
        {
            List<Artist> artisti = db.Artisti.ToList(); 
            return View(artisti);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id = 1)
        {
            if (id.HasValue)
            {
                Artist artist = db.Artisti.Find(id);
                if (artist != null)
                {
                    ViewBag.idArtist = id;
                    return View(artist);
                }
                return HttpNotFound("Nu a fost gasit artistul avand codul " + id.ToString() + "!");
            }
            return HttpNotFound("Lipseste parametrul care sa indice codul artistului!");
        }

        public ActionResult GalerieTatuaje(int idArtist, int index)
        {
            Artist tatuator = db.Artisti.Find(idArtist);
            Imagine tatv = tatuator.IstoricTatuaje[index];
            return PartialView("_GalerieTatuaje", tatv);
        }

        public ActionResult ProiectePortofoliu(int idArtist, int index)
        {
            Artist tatuator = db.Artisti.Find(idArtist);
            Imagine proiect = tatuator.PortofoliuDesene[index];
            return PartialView("_ProiectePortofoliu", proiect);
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Artist artist = new Artist();
            artist.InfoContact = new Contact();
            return View(artist);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Artist artistRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    artistRequest.IstoricTatuaje = new List<Imagine>();
                    artistRequest.PortofoliuDesene = new List<Imagine>();

                    string directoryMain = "~/Content/Imagini/" + artistRequest.Nume.ToLower();
                    Directory.CreateDirectory(Server.MapPath(directoryMain));

                    artistRequest.PozaArtist = new Imagine();
                    string fileNamePozaArtist = Path.GetFileName(artistRequest.ImageFilePozaArtist.FileName);
                    artistRequest.PozaArtist.PathImagine = directoryMain + "/" + fileNamePozaArtist;
                    fileNamePozaArtist = Path.Combine(Server.MapPath(directoryMain), fileNamePozaArtist);
                    artistRequest.ImageFilePozaArtist.SaveAs(fileNamePozaArtist);

                    string directoryTat = directoryMain + "/" + "/Tatuaje/";
                    Directory.CreateDirectory(Server.MapPath(directoryTat));
                    foreach (HttpPostedFileBase imgFile in artistRequest.ImageFilesTatuaje)
                    {
                        Imagine img = new Imagine();
                        string fileName = Path.GetFileName(imgFile.FileName);
                        img.PathImagine = directoryTat + fileName;
                        fileName = Path.Combine(Server.MapPath(directoryTat), fileName);
                        imgFile.SaveAs(fileName);
                        artistRequest.IstoricTatuaje.Add(img);
                        db.Imagini.Add(img);
                    }

                    string directoryDesen = directoryMain + "/Desene/";
                    Directory.CreateDirectory(Server.MapPath(directoryDesen));
                    foreach (HttpPostedFileBase imgFile in artistRequest.ImageFilesDesene)
                    {
                        
                        Imagine img = new Imagine();
                        string fileName = Path.GetFileName(imgFile.FileName);
                        img.PathImagine = directoryDesen + fileName;
                        fileName = Path.Combine(Server.MapPath(directoryDesen), fileName);
                        imgFile.SaveAs(fileName);
                        artistRequest.PortofoliuDesene.Add(img);
                        db.Imagini.Add(img);
                    }

                    db.Contacte.Add(artistRequest.InfoContact);
                    

                    var password = artistRequest.Nume + "$Sirena_" + artistRequest.Nume.Length;
                    AccountController accountController = DependencyResolver.Current.GetService<AccountController>();
                    //accountController.ControllerContext = this.ControllerContext;
                    var editorId = accountController.RegisterAccount(new RegisterViewModel { UserName = artistRequest.Nume, 
                        Email = artistRequest.InfoContact.Email, Password = password, Role = "Editor" });
                    db.SaveChanges();

                    artistRequest.EditorId = editorId;
                    artistRequest.Editor = db.Users.Find(editorId);
                    db.Artisti.Add(artistRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(artistRequest);
            }
            catch (Exception e)
            {
                return View(artistRequest);
            }
        }
        
        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Artist artist = db.Artisti.Find(id);

                if (artist == null)
                {
                    return HttpNotFound("Nu s-a gasit artistul cu codul " + id.ToString() + "!");
                }
                return View(artist);
            }
            return HttpNotFound("Lipseste parametrul cu codul artistului!");
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id, Artist artistRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Artist artist = db.Artisti.SingleOrDefault(a => a.ArtistCod.Equals(id));

                    string directoryMain = "~/Content/Imagini/" + artistRequest.Nume.ToLower();
                    string directoryTat = directoryMain + "/" + "/Tatuaje/";
                    string directoryDesen = directoryMain + "/Desene/";
                    if (artist.Nume != artistRequest.Nume)
                    {
                        Directory.CreateDirectory(Server.MapPath(directoryMain));
                        Directory.CreateDirectory(Server.MapPath(directoryTat));
                        Directory.CreateDirectory(Server.MapPath(directoryDesen));
                        if (artistRequest.ImageFilePozaArtist == null)
                        {
                            string source = Server.MapPath(artist.PozaArtist.PathImagine);
                            System.IO.File.Move(source,
                                source.Replace(artist.Nume.ToLower(), artistRequest.Nume.ToLower()));
                        }
                        foreach (Imagine img in artist.IstoricTatuaje)
                        {
                            string source = Server.MapPath(img.PathImagine);
                            System.IO.File.Move(source,
                                    source.Replace(artist.Nume.ToLower(), artistRequest.Nume.ToLower()));
                        }
                        foreach (Imagine proj in artist.PortofoliuDesene)
                        {
                            string source = Server.MapPath(proj.PathImagine);
                            System.IO.File.Move(source,
                                    source.Replace(artist.Nume.ToLower(), artistRequest.Nume.ToLower()));
                        }
                        Directory.Delete(Server.MapPath(directoryMain.Replace(artistRequest.Nume.ToLower(), artist.Nume.ToLower())), true);
                    }

                    if (TryUpdateModel(artist))
                    {                        
                        if (artistRequest.ImageFilePozaArtist != null)
                        {
                            Imagine img = db.Imagini.Find(artist.PozaArtist.CodImagine);
                            db.Imagini.Remove(img);

                            artist.PozaArtist = new Imagine();
                            string fileNamePozaArtist = Path.GetFileName(artistRequest.ImageFilePozaArtist.FileName);
                            artist.PozaArtist.PathImagine = directoryMain + "/" + fileNamePozaArtist;
                            fileNamePozaArtist = Path.Combine(Server.MapPath(directoryMain), fileNamePozaArtist);
                            artistRequest.ImageFilePozaArtist.SaveAs(fileNamePozaArtist);
                        }

                        if (artistRequest.ImageFilesTatuaje != null && artistRequest.ImageFilesTatuaje[0] != null)
                        { 
                            foreach (HttpPostedFileBase imgFile in artistRequest.ImageFilesTatuaje)
                            {
                                Imagine img = new Imagine();
                                string fileName = Path.GetFileName(imgFile.FileName);
                                img.PathImagine = directoryTat + fileName;
                                fileName = Path.Combine(Server.MapPath(directoryTat), fileName);
                                imgFile.SaveAs(fileName);
                                artist.IstoricTatuaje.Add(img);
                                db.Imagini.Add(img);
                            }
                        }

                        if (artistRequest.ImageFilesDesene != null && artistRequest.ImageFilesDesene[0] != null)
                        {
                            foreach (HttpPostedFileBase imgFile in artistRequest.ImageFilesDesene)
                            {
                                Imagine img = new Imagine();
                                string fileName = Path.GetFileName(imgFile.FileName);
                                img.PathImagine = directoryDesen + fileName;
                                fileName = Path.Combine(Server.MapPath(directoryDesen), fileName);
                                imgFile.SaveAs(fileName);
                                artist.PortofoliuDesene.Add(img);
                                db.Imagini.Add(img);
                            }
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(artistRequest);
            }
            catch (Exception e)
            {
                return View(artistRequest);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int ?id)
        {
            Artist artist = db.Artisti.Include("IstoricTatuaje").Include("PortofoliuDesene").Include("Editor").SingleOrDefault(a => a.ArtistCod == id);
            if (artist != null)
            {
                Imagine poza = artist.PozaArtist;
                db.Imagini.Remove(poza);
                //Directory.Delete(Server.MapPath("~/Content/Imagini/" + artist.Nume.ToLower()), true);
                db.Artisti.Remove(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Nu am gasit artistul cu id " + id.ToString() + "!");
        }

    }
}