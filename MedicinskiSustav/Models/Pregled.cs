using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class Pregled
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Šifra pregleda")]
        public SifraPregleda SifraPregleda { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Vrijeme pregleda")]
        public DateTime VrijemePregleda { get; set; }

        [ForeignKey("Pacijent")]
        public int PacijentId { get; set; }
        public virtual Pacijent Pacijent { get; set; }

        public virtual ICollection<Slika> Slike { get; set; } 
        public virtual ICollection<Recept> Recepti { get; set; } 
    }
}
