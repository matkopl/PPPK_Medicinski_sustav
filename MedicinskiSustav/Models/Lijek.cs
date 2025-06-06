using System.ComponentModel.DataAnnotations;

namespace MedicinskiSustav.Models
{
    public class Lijek
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }

        public virtual ICollection<Recept> Recepti { get; set; }
    }
}
