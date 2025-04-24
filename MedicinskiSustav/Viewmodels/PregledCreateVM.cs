using MedicinskiSustav.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class PregledCreateVM
    {
        [Required]
        [Display(Name = "Šifra pregleda")]
        public SifraPregleda SifraPregleda { get; set; }

        [Required]
        [Display(Name = "Vrijeme pregleda")]
        public DateTime VrijemePregleda { get; set; } = DateTime.Now;

        [Required]
        public int PacijentId { get; set; }
    }
}
