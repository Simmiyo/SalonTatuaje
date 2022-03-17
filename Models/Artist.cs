using SalonTatuaje.Models.Validari;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalonTatuaje.Models
{
    public class Artist
    {
        [Key]
        public int ArtistCod { get; set; }

        [Required, MinLength(3, ErrorMessage = "Numele trebuie sa contina minim 3 caractere!"), MaxLength(40, ErrorMessage = "Numele poate contine maxim 40 de caractere!")]
        public string Nume { get; set; }

        //[RegularExpression(@"^(http(s ?):)([/|.|\w |\s | -]) *\.(?:jpg|png)$", ErrorMessage = "URL către imagine invalid!")]
        public virtual Imagine PozaArtist { get; set; }

        [NotMapped] //baza de date nu vede proprietatea
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageFilePozaArtist { get; set; }

        [MaxLength(1000, ErrorMessage = "Maxim 1000 de caractere in biografie!")]
        public string Bio { get; set; }

        //one-to-one relationship
        public virtual Contact InfoContact { get; set; }

        //one-to-many relationship
        public virtual IList<Imagine> IstoricTatuaje { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase[] ImageFilesTatuaje { get; set; }


        //one-to-many relationship
        public virtual IList<Imagine> PortofoliuDesene { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase[] ImageFilesDesene { get; set; }
        
        //one-to-many relationship
        public virtual ICollection<Programare> Programari { get; set; }

        public string EditorId { get; set; }
        public virtual ApplicationUser Editor { get; set; }
    }
}