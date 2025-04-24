namespace MedicinskiSustav.Viewmodels
{
    public class DokumentacijaDetailsVM
    {
        public int Id { get; set; }
        public int PacijentId { get; set; }
        public string PacijentImePrezime { get; set; }
        public List<BolestVM> Bolesti { get; set; }
    }
}
