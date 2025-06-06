using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicinskiSustav.Controllers
{
    public class ReceptiController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public ReceptiController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        // GET: ReceptiController/Create
        public IActionResult Create(int pregledId)
        {
            var lijekoviRepo = _repositoryFactory.GetRepository<Lijek>();
            var lijekovi = lijekoviRepo.GetAll();

            ViewBag.Lijekovi = new SelectList(lijekovi, "Id", "Naziv");    

            var vm = new ReceptCreateVM { PregledId = pregledId };
            return View(vm);
        }

        // POST: ReceptiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceptCreateVM receptCreateVM)
        {
            if (ModelState.IsValid)
            {
                var recept = _mapper.Map<Recept>(receptCreateVM);
                var repo = _repositoryFactory.GetRepository<Recept>();

                await repo.InsertAsync(recept);
                await repo.SaveAsync();

                TempData["Success"] = "Recept uspješno dodan!";
                return RedirectToAction("Details", "Pregledi", new { id = receptCreateVM.PregledId });
            }

            var lijekoviRepo = _repositoryFactory.GetRepository<Lijek>();
            var lijekovi = lijekoviRepo.GetAll();
            ViewBag.Lijekovi = new SelectList(lijekovi, "Id", "Naziv", receptCreateVM.LijekId);

            return View(receptCreateVM);
        }

        // GET: ReceptiController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var repo = _repositoryFactory.GetRepository<Recept>();
            var recept = await repo.GetByIdAsync(id);

            if (recept == null)
            {
                return NotFound();
            }

            var lijekoviRepo = _repositoryFactory.GetRepository<Lijek>();
            var lijekovi = lijekoviRepo.GetAll();

            ViewBag.Lijekovi = new SelectList(lijekovi, "Id", "Naziv", recept.LijekId);

            var vm = _mapper.Map<ReceptEditVM>(recept);
            return View(vm);
        }

        // POST: ReceptiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReceptEditVM receptEditVM)
        {
            if (id != receptEditVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var repo = _repositoryFactory.GetRepository<Recept>();
                var recept = await repo.GetByIdAsync(id);

                if (recept == null)
                {
                    return NotFound();
                }

                _mapper.Map(receptEditVM, recept);

                await repo.UpdateAsync(recept);
                await repo.SaveAsync();

                TempData["Success"] = "Recept je uspješno ažuriran!";
                return RedirectToAction("Details", "Pregledi", new { id = recept.PregledId });
            }
            return View(receptEditVM);
        }

        // POST: ReceptiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var repo = _repositoryFactory.GetRepository<Recept>();
            var recept = await repo.GetByIdAsync(id);
            
            await repo.DeleteAsync(id);
            await repo.SaveAsync();

            return RedirectToAction("Details", "Pregledi", new { id = recept.PregledId });
        }
    }
}
