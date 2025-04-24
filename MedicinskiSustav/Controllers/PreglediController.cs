using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicinskiSustav.Controllers
{
    public class PreglediController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public PreglediController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        // GET: PreglediController
        public async Task<IActionResult> Index()
        {
            var repo = _repositoryFactory.GetRepository<Pregled>();
            var pregledi = await repo.GetAll().ToListAsync();

            var vm = _mapper.Map<IEnumerable<PregledVM>>(pregledi);
            return View(vm);
        }

        // GET: PreglediController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var repo = _repositoryFactory.GetRepository<Pregled>();
            var pregled = await repo.GetByIdAsync(id);

            if (pregled == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<PregledDetailsVM>(pregled);
            return View(vm);
        }

        // GET: PreglediController/Create
        public IActionResult Create(int pacijentId)
        {
            var vm = new PregledCreateVM
            {
                PacijentId = pacijentId,
                VrijemePregleda = DateTime.Now
            };

            ViewBag.SifrePregleda = Enum.GetValues(typeof(SifraPregleda));
            return View(vm);
        }

        // POST: PreglediController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PregledCreateVM pregledCreateVM)
        {
            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Pregled>();
                var pregled = _mapper.Map<Pregled>(pregledCreateVM);

                await repo.InsertAsync(pregled);
                await repo.SaveAsync();

                TempData["Success"] = "Pregled uspješno kreiran!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SifrePregleda = Enum.GetValues(typeof(SifraPregleda));
            return View(pregledCreateVM);
        }

        // GET: PreglediController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var repo = _repositoryFactory.GetRepository<Pregled>();
            var pregled = await repo.GetByIdAsync(id);

            if (pregled == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<PregledEditVM>(pregled);
            ViewBag.SifrePregleda = Enum.GetValues(typeof(SifraPregleda));
            return View(vm);
        }

        // POST: PreglediController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PregledEditVM pregledEditVM)
        {
            if (id != pregledEditVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Pregled>();
                var pregled = await repo.GetByIdAsync(id);

                if (pregled == null)
                {
                    return NotFound();
                }

                _mapper.Map(pregledEditVM, pregled);
                await repo.UpdateAsync(pregled);
                await repo.SaveAsync();

                TempData["Success"] = "Pregled uspješno ažuriran!";
                return RedirectToAction("Details", new { id });
            }

            ViewBag.SifrePregleda = Enum.GetValues(typeof(SifraPregleda));
            return View(pregledEditVM);
        }

        // POST: PreglediController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var repo = _repositoryFactory.GetRepository<Pregled>();
            
            await repo.DeleteAsync(id);
            await repo.SaveAsync();

            TempData["Success"] = "Pregled uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }
    }
}
