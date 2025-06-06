using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class ReceptCreateVM
    {
        [Required(ErrorMessage = "Odaberite lijek")]
        public int LijekId { get; set; }
        public int PregledId { get; set; }
    }
}
