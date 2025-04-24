using AutoMapper;
using CsvHelper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace MedicinskiSustav.Controllers
{
    public class PacijentiController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public PacijentiController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string search)
        {
            var repo = _repositoryFactory.GetRepository<Pacijent>();
            var query = repo.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => 
                p.Prezime.Contains(search) || 
                p.OIB.Contains(search));
            }

            var pacijenti = await query.ToListAsync();
            var vm = _mapper.Map<IEnumerable<PacijentVM>>(pacijenti);
            return View(vm);
        }

        public IActionResult Create()
        {
            ViewData["SpolList"] = new SelectList(Enum.GetValues(typeof(Spol)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacijentCreateVM pacijentCreateVM)
        {
            if (ModelState.IsValid)
            {
                var pacijent = _mapper.Map<Pacijent>(pacijentCreateVM);
                var repo = _repositoryFactory.GetRepository<Pacijent>();

                pacijent.Dokumentacija = new MedicinskaDokumentacija();

                await repo.InsertAsync(pacijent);
                await repo.SaveAsync();
                TempData["Success"] = "Pacijent uspješno dodan!";

                return RedirectToAction(nameof(Index));
            }

            ViewData["SpolList"] = new SelectList(Enum.GetValues(typeof(Spol)), pacijentCreateVM.Spol);
            return View(pacijentCreateVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repo = _repositoryFactory.GetRepository<Pacijent>();
            var pacijent = await repo.GetByIdAsync(id.Value);

            if (pacijent == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<PacijentEditVM>(pacijent);
            ViewData["SpolList"] = new SelectList(Enum.GetValues(typeof(Spol)), pacijent.Spol);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacijentEditVM pacijentEditVM)
        {
            if (id != pacijentEditVM.Id) 
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Pacijent>();
                var pacijent = await repo.GetByIdAsync(id);

                if (pacijent == null)
                {
                    return NotFound();
                }

                _mapper.Map(pacijentEditVM, pacijent);
                repo.UpdateAsync(pacijent);
                await repo.SaveAsync();
                TempData["Success"] = "Promjene uspješno spremljene!";

                return RedirectToAction(nameof(Index));
            }

            ViewData["SpolList"] = new SelectList(Enum.GetValues(typeof(Spol)), pacijentEditVM.Spol);
            return View(pacijentEditVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repo = _repositoryFactory.GetRepository<Pacijent>();
            var pacijent = await repo.GetAll()
                .Include(p => p.Dokumentacija)
                    .ThenInclude(d => d.Bolesti)
                .Include(p => p.Pregledi)
                    .ThenInclude(p => p.Recepti)
                .Include(p => p.Pregledi)
                    .ThenInclude(p => p.Slike)
               .FirstOrDefaultAsync(p => p.Id == id);
                

            if (pacijent == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<PacijentDetailsVM>(pacijent);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var repo = _repositoryFactory.GetRepository<Pacijent>();
            await repo.DeleteAsync(id);
            await repo.SaveAsync();
            TempData["Success"] = "Pacijent uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToCsv(int id)
        {
            var repo = _repositoryFactory.GetRepository<Pacijent>();
            var pacijent = await repo.GetAll()
                .Include(p => p.Dokumentacija)
                    .ThenInclude(d => d.Bolesti)
                .Include(p => p.Pregledi)
                    .ThenInclude(pr => pr.Recepti)
                .Include(p => p.Pregledi)
                    .ThenInclude(pr => pr.Slike)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pacijent == null)
                return NotFound();

            var exportModel = new List<PacijentExportVM> {
        new PacijentExportVM
        {
            Ime = pacijent.Ime,
            Prezime = pacijent.Prezime,
            OIB = pacijent.OIB,
            DatumRodjenja = pacijent.DatumRodjenja,
            Spol = pacijent.Spol.ToString(),
            Bolesti = string.Join(" | ", pacijent.Dokumentacija?.Bolesti?.Select(b => b.Naziv) ?? new List<string>()),
            Pregledi = string.Join(" | ", pacijent.Pregledi?.Select(pr => $"{pr.SifraPregleda} {pr.VrijemePregleda:dd.MM.yyyy}") ?? new List<string>()),
            Recepti = string.Join(" | ", pacijent.Pregledi?.SelectMany(pr => pr.Recepti).Select(r => r.Lijek) ?? new List<string>())
        }
    };

            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(exportModel);
            }

            stream.Position = 0;
            return File(stream, "text/csv", $"Pacijent {exportModel.First().Ime} {exportModel.First().Prezime} {DateTime.Now:dd.MM.yyyy}.csv");
        }
    }
}
