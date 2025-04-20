using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Models
{
    public class Pacijent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "OIB je obavezan")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB mora imati 11 znamenki")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB smije sadržavati samo brojeve")]
        public string OIB { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(50, ErrorMessage = "Ime ne smije biti duže od 50 znakova")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(50, ErrorMessage = "Prezime ne smije biti duže od 50 znakova")]
        public string Prezime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja")]
        public DateTime DatumRodjenja { get; set; }

        [Required]
        [Display(Name = "Spol")]
        public Spol Spol { get; set; }

        public virtual MedicinskaDokumentacija Dokumentacija { get; set; }
        public virtual ICollection<Pregled> Pregledi { get; set; }
    }
}
