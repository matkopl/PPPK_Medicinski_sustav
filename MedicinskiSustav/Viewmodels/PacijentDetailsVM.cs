using MedicinskiSustav.Models;

namespace MedicinskiSustav.Viewmodels
{
    public class PacijentDetailsVM : PacijentVM
    {
        public int DokumentacijaId { get; set; }
        public List<Pregled> Pregledi { get; set; }
        public List<Bolest> Bolesti { get; set; }
    }
}
