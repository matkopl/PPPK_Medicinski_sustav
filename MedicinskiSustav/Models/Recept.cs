using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class Recept
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Lijek { get; set; }

        [ForeignKey("Pregled")]
        public int PregledId { get; set; }
        public virtual Pregled Pregled { get; set; }
    }
}
