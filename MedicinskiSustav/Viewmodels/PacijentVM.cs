using MedicinskiSustav.Models;

namespace MedicinskiSustav.Viewmodels
{
    public class PacijentVM
    {
        public int Id { get; set; }
        public string OIB { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Spol Spol { get; set; }
    }
}
