using SalonTatuaje.Models.Validari;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models
{
    public class Programare
    {
        [Key]
        public int ProgramareCod { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime Data { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [RegularExpression(@"^(([01][0-9])|(2[0-3])):[0-5][0-9]$", ErrorMessage = "Formatul trebuie sa fie de tip hh:mm (24 ore)")]
        public string Ora { get; set; }

        [Required, Range(1, 3, ErrorMessage = "Săli existente: 1, 2, 3.")]
        public int Sala { get; set; }

        public int PretTotal { get; set; }

        //many-to-many relationship
        public virtual IList<TipTatuaj> TipuriTatuaje { get; set; }

        //many-to-many relationship
        public virtual IList<Imagine> Proiecte { get; set; }

        //one-to-many relationship
        public virtual IList<Imagine> CerinteNoi { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase[] ImageFilesCerinte { get; set; }

        //many-to-one relationship
        public int ArtistId { get; set; }
        public virtual Artist Tartist { get; set; }

        //many-to-one relationship
        public string UserId { get; set; }
        public virtual ApplicationUser Tclient { get; set; }

    }
}