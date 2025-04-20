using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class Slika
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Putanja { get; set; }

        [ForeignKey("Pregled")]
        public int PregledId { get; set; }
        public virtual Pregled Pregled { get; set; }
    }
}
