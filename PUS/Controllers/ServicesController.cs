using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.Models;
using PUS.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PUS.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ServicesController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        public async Task<IActionResult> SimpleList()
        {
            ViewBag.Title = "Aktywne oferty";
            return PartialView("SimpleList", await _context.Services.ToListAsync());
        }

        // GET: Services/AddImage

        public IActionResult AddImage()
        {
            return View();
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsModal", service);
            //return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel vm)
        {
            var service = new Service { Title = vm.Title, Description = vm.Description, StartDate = vm.StartDate, EndDate = vm.EndDate };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Profiles.First(p => p.Id == userId);
            service.Owner = user;

            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();

                if (vm.Image.Length > 0)
                {
                    var filePath = Path.Combine(_appEnvironment.WebRootPath, "img", service.Id.ToString() + ".jpeg");

                    using var image = Image.Load(vm.Image.OpenReadStream());
                    var cropArea = new Rectangle(
                        (int)(vm.CropX / vm.CropScale), (int)(vm.CropY / vm.CropScale),
                        (int)(400 / vm.CropScale), (int)(300 / vm.CropScale)
                        );
                    image.Mutate(x => x.Crop(cropArea));
                    image.Mutate(x => x.Resize(400, 300));
                    await image.SaveAsJpegAsync(filePath);
                }

                return Json(new { success = true });
            }
            return PartialView("Create", service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
