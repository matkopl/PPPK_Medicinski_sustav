using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicinskiSustav.Controllers
{
    public class DokumentacijeController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public DokumentacijeController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        // GET: DokumentacijeController
        public async Task<IActionResult> Index()
        {
            var repo = _repositoryFactory.GetRepository<MedicinskaDokumentacija>();
            var dokumentacije = await repo.GetAll()
                .Include(d => d.Pacijent)
                .Include(d => d.Bolesti)
                .ToListAsync();

            var vmList = dokumentacije.Select(d => new DokumentacijaDetailsVM
            {
                Id = d.Id,
                PacijentId = d.PacijentId,
                PacijentImePrezime = d.Pacijent.Ime + " " + d.Pacijent.Prezime,
                Bolesti = d.Bolesti.Select(b => new BolestVM
                {
                    Id = b.Id,
                    Naziv = b.Naziv,
                    Pocetak = b.Pocetak,
                    Zavrsetak = b.Zavrsetak,
                    DokumentacijaId = b.DokumentacijaId
                }).ToList()
            }).ToList();

            return View(vmList);
        }

        // GET: DokumentacijeController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var repo = _repositoryFactory.GetRepository<MedicinskaDokumentacija>();
            var dokumentacija = await repo.GetAll()
                .Include(d => d.Pacijent)
                .Include(d => d.Bolesti)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dokumentacija == null)
            {
                return NotFound();
            }

            var vm = new DokumentacijaDetailsVM
            {
                Id = dokumentacija.Id,
                PacijentId = dokumentacija.PacijentId,
                PacijentImePrezime = dokumentacija.Pacijent.Ime + " " + dokumentacija.Pacijent.Prezime,
                Bolesti = dokumentacija.Bolesti.Select(b => new BolestVM
                {
                    Id = b.Id,
                    Naziv = b.Naziv,
                    Pocetak = b.Pocetak,
                    Zavrsetak = b.Zavrsetak,
                    DokumentacijaId = b.DokumentacijaId
                }).ToList()
            };

            return View(vm);
        }

        public IActionResult CreateBolest(int dokumentacijaId)
        {
            var vm = new BolestVM { DokumentacijaId = dokumentacijaId };
            return View(vm);
        }

        // POST: DokumentacijeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBolest(BolestVM bolestVm)
        {
            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Bolest>();
                var bolest = _mapper.Map<Bolest>(bolestVm);

                await repo.InsertAsync(bolest);
                await repo.SaveAsync();

                TempData["Success"] = "Bolest uspješno dodana!";
                return RedirectToAction("Details", new { Id = bolestVm.DokumentacijaId });
            }

            return View(bolestVm);
        }

        // GET: DokumentacijeController/Edit/5
        public async Task<IActionResult> EditBolest(int id)
        {
            var repo = _repositoryFactory.GetRepository<Bolest>();
            var bolest = await repo.GetByIdAsync(id);

            if (bolest == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<BolestVM>(bolest);
            return View(vm);
        }

        // POST: DokumentacijeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBolest(int id, BolestVM bolestVM)
        {
            if (id != bolestVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Bolest>();
                var bolest = await repo.GetByIdAsync(id);

                if (bolest == null)
                {
                    return NotFound();
                }

                _mapper.Map(bolestVM, bolest);
                await repo.UpdateAsync(bolest);
                await repo.SaveAsync();
                TempData["Success"] = "Bolest uspješno ažurirana!";

                return RedirectToAction("Details", new { Id = bolestVM.DokumentacijaId });
            }

            return View(bolestVM);
        }

        // POST: DokumentacijeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBolest(int id, int dokumentacijaId)
        {
            var repo = _repositoryFactory.GetRepository<Bolest>();
            await repo.DeleteAsync(id);
            await repo.SaveAsync();
            TempData["Success"] = "Bolest uspješno obrisana!";
            return RedirectToAction("Details", new { id = dokumentacijaId });
        }
    }
}
