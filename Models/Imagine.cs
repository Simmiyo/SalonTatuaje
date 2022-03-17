using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models
{
    public class Imagine
    {
        [Key]
        public int CodImagine { get; set; }

        [DisplayName("Titlul imaginii")]
        public string PathImagine { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageFile { get; set; }

/*        public int AutorId { get; set; }
        public virtual Artist Autor { get; set; }

        public int AutorId1 { get; set; }
        public virtual Artist Autor1 { get; set; }*/
    }
}