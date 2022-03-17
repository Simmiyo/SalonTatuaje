using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ContactCod { get; set; }

        [Required, RegularExpression(@"^07(\d{8})$", ErrorMessage = "Număr de telefon invalid!")]
        public string NrTel { get; set; }

        [Required, RegularExpression(@"^(.+)@(\S+)[.](\w+)$", ErrorMessage = "Adresă de email invalidă!")]
        public string Email { get; set; } //= "sirena_de_cerneala@tattoo.salon.com";

        [RegularExpression(@"^@(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$", ErrorMessage = "Instagram invalid!")]
        public string Instagram { get; set; } = "@sirena_de_cerneala";
    }
}