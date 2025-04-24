namespace MedicinskiSustav.Viewmodels
{
    public class BolestVM
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime Pocetak { get; set; }
        public DateTime? Zavrsetak { get; set; }
        public int DokumentacijaId { get; set; }
    }
}
