using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Viewmodels
{
    public class ReceptCreateVM
    {
        [Required]
        public string Lijek {  get; set; }

        public int PregledId { get; set; }
    }
}
