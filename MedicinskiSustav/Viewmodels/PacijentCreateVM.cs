using MedicinskiSustav.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class PacijentCreateVM
    {
        [Required(ErrorMessage = "OIB je obavezan")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB mora imati točno 11 znamenki")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB smije sadržavati samo brojeve")]
        public string OIB { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(50, ErrorMessage = "Ime ne smije biti duže od 50 znakova")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(50, ErrorMessage = "Prezime ne smije biti duže od 50 znakova")]
        public string Prezime { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Datum rođenja je obavezan")]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Spol je obavezan")]
        public Spol Spol { get; set; }
    }
}
