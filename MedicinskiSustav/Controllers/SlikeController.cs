using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MedicinskiSustav.Controllers
{
    public class SlikeController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SlikeController(IRepositoryFactory repositoryFactory, IMapper mapper, IWebHostEnvironment env)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
            _env = env;
        }

        // GET: SlikeController/Create
        public IActionResult Create(int pregledId)
        {
            SlikaCreateVM vm = new() { PregledId = pregledId };

            return View(vm);
        }

        // POST: SlikeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlikaCreateVM slikaCreateVM)
        {
            if (ModelState.IsValid && slikaCreateVM.Slika != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(slikaCreateVM.Slika.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Slika", "Dozvoljeni formati: JPG, PNG, GIF");
                    return View(slikaCreateVM);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{slikaCreateVM.Slika.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await slikaCreateVM.Slika.CopyToAsync(stream);
                }

                Slika slika = new()
                {
                    Putanja = $"/uploads/{uniqueFileName}",
                    PregledId = slikaCreateVM.PregledId
                };

                var repo = _repositoryFactory.GetRepository<Slika>();
                await repo.InsertAsync(slika);
                await repo.SaveAsync();

                TempData["Success"] = "Slika uspješno dodana!";
                return RedirectToAction("Details", "Pregledi", new { id = slikaCreateVM.PregledId });
            }

            return View(slikaCreateVM);
        }

        // POST: SlikeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var repo = _repositoryFactory.GetRepository<Slika>();
            var slika = await repo.GetByIdAsync(id);

            if (slika == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_env.WebRootPath, slika.Putanja.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await repo.DeleteAsync(id);
            await repo.SaveAsync();

            TempData["Success"] = "Slika uspješno obrisana!";
            return RedirectToAction("Details", "Pregledi", new { id = slika.PregledId });
        }
    }
}
