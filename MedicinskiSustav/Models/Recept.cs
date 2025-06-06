using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class Recept
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Lijek")]
        public int LijekId { get; set; }
        public virtual Lijek Lijek { get; set; }

        [ForeignKey("Pregled")]
        public int PregledId { get; set; }
        public virtual Pregled Pregled { get; set; }
    }
}
