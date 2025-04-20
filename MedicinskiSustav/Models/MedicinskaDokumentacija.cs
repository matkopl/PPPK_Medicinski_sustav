using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicinskiSustav.Models
{
    public class MedicinskaDokumentacija
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pacijent")]
        public int PacijentId { get; set; }
        public virtual Pacijent Pacijent { get; set; }

        public virtual ICollection<Bolest> Bolesti { get; set; } 
    }
}
