using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class Bolest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv {  get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Početak bolesti")]
        public DateTime Pocetak { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Završetak bolesti")]
        public DateTime? Zavrsetak { get; set; }

        [ForeignKey("MedicinskaDokumentacija")]
        public int DokumentacijaId { get; set; }

        public virtual MedicinskaDokumentacija MedicinskaDokumentacija { get; set; }
    }
}
