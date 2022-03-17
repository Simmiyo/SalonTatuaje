using SalonTatuaje.Models.Validari;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models
{
    public class TipTatuaj
    {
        [Key]
        public int TipTatuajCod { get; set; }

        [DisplayName("Numele Tipului")]
        public string NumeTip { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Localizarea tatuajului trebuie să conțină cel puțin două caractere!"), 
         MaxLength(20, ErrorMessage = "Localizarea tatuajului trebuie să conțină cel mult douăzeci de caractere!")]
        public string Localizare { get; set; }

        [Required]
        [DimensiuneMin(1, ErrorMessage = "Lungimea tatuajului trebuie să fie de cel puțin 1 cm!"),
         DimensiuneMax(200, ErrorMessage = "Lungimea tatuajului trebuie să fie de cel mult 200 cm!")]
        public int Lungime { get; set; }

        [Required]
        [DimensiuneMin(1, ErrorMessage = "Lățimea tatuajului trebuie să fie de cel puțin 1 cm!"),
         DimensiuneMax(200, ErrorMessage = "Lățimea tatuajului trebuie să fie de cel mult 200 cm!")]
        public int Latime { get; set; }

        [Required]
        public string Culoare { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Preț neadecvat!")]
        public int Pret { get; set; }

        // many-to-many relationship
        public virtual ICollection<Programare> Programari { get; set; }
    }
}