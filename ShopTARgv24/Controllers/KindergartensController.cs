using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using ShopTARgv24.Models.Kindergartens;

namespace ShopTARgv24.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly ShopTARgv24Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartensController(
            ShopTARgv24Context context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    TeacherName = x.TeacherName,
                    KindergartenName = x.KindergartenName
                });

            return View(result);
        }

        // CREATE
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            return View("CreateUpdate", new KindergartenCreateUpdateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("CreateUpdate", vm);

            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var created = await _kindergartenServices.Create(dto);
            if (created == null)
                return RedirectToAction(nameof(Index));

            // <<<<<< ВАЖНО: Guid? -> Guid >>>>>
            if (vm.Files != null && vm.Files.Count > 0 && created.Id.HasValue)
                await _fileServices.SaveToDatabaseAsync(vm.Files, created.Id.Value);

            return RedirectToAction(nameof(Update), new { id = created.Id });
        }

        // UPDATE
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

            var vm = new KindergartenCreateUpdateViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await FilesFromDatabase(id)
            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("CreateUpdate", vm);

            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var updated = await _kindergartenServices.Update(dto);
            if (updated == null)
                return RedirectToAction(nameof(Index));

            // <<<<<< fix: используем updated и распаковываем Guid? >>>>>
            if (vm.Files != null && vm.Files.Count > 0 && updated.Id.HasValue)
                await _fileServices.SaveToDatabaseAsync(vm.Files, updated.Id.Value);

            return RedirectToAction(nameof(Update), new { id = updated.Id });
        }

        // DELETE
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergarten = await _kindergartenServices.Delete(id);
            if (kindergarten != null)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }

        // DETAILS
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

            var vm = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            return View(vm);
        }

        public async Task<KindergartenImageViewModel[]> FilesFromDatabase(Guid id)
        {
            var images = await _context.KindergartenFileToDatabase
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    ImageId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return images;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto { Id = vm.ImageId };
            await _fileServices.RemoveImageFromDatabase(dto);

            return RedirectToAction(nameof(Update), new { id = vm.KindergartenId });
        }
    }
}
