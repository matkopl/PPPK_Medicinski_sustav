using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class SlikaCreateVM
    {
        [Required(ErrorMessage = "Molimo odaberite sliku")]
        [Display(Name = "Medicinska slika")]
        public IFormFile Slika { get; set; }

        public int PregledId { get; set; }
    }
}
