using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using MedicinskiSustav.Viewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using System.Linq;

namespace MedicinskiSustav.Controllers
{
    public class SlikeController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;
        private readonly IMinioClient _minio;
        private readonly string _bucket;

        public SlikeController(IRepositoryFactory repositoryFactory, IMapper mapper, IMinioClient minio, IConfiguration config)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
            _minio = minio;
            _bucket = config["Minio:Bucket"] ?? "slike";
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

                var uniqueFileName = $"{Guid.NewGuid()}-{slikaCreateVM.Slika.FileName}";
                var beArgs = new BucketExistsArgs().WithBucket(_bucket);
                bool found = await _minio.BucketExistsAsync(beArgs);

                if (!found)
                {
                    var mbArgs = new MakeBucketArgs().WithBucket(_bucket);
                    await _minio.MakeBucketAsync(mbArgs);
                }

                using (var stream = slikaCreateVM.Slika.OpenReadStream())
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(_bucket)
                        .WithObject(uniqueFileName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(slikaCreateVM.Slika.ContentType);

                    await _minio.PutObjectAsync(putObjectArgs);
                }

                Slika slika = new()
                {
                    Putanja = uniqueFileName,
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

            var removeObjectArgs = new RemoveObjectArgs()
                 .WithBucket(_bucket)
                 .WithObject(slika.Putanja);

            await _minio.RemoveObjectAsync(removeObjectArgs);

            await repo.DeleteAsync(id);
            await repo.SaveAsync();

            TempData["Success"] = "Slika uspješno obrisana!";
            return RedirectToAction("Details", "Pregledi", new { id = slika.PregledId });
        }
    }
}
