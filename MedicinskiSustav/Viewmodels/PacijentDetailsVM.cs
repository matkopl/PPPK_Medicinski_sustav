using MedicinskiSustav.Models;

namespace MedicinskiSustav.Viewmodels
{
    public class PacijentDetailsVM
    {
        public string OIB { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Spol Spol { get; set; }
        public List<Pregled> Pregledi { get; set; }
        public List<Bolest> Bolesti { get; set; }
    }
}
