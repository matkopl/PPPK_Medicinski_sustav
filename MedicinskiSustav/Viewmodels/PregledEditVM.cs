using MedicinskiSustav.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class PregledEditVM
    {
        public int Id { get; set; }
        [Required]
        public SifraPregleda SifraPregleda { get; set; }
        [Required]
        public DateTime VrijemePregleda { get; set; }
        [Required]
        public int PacijentId { get; set; }
    }
}
