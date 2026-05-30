using HelloWorld.Data;
using HelloWorld.Models;
using HelloWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

public class ComposerController(ApplicationDbContext db, CloudinaryService cloudinary) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await db.Composers.ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var composer = await db.Composers
            .Include(c => c.Songs.Where(song => song.Status == 1))
            .ThenInclude(song => song.Singer)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (composer == null) return NotFound();
        return View(composer);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Composer composer, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            var url = await cloudinary.UploadImageAsync(imageFile, "composers");
            if (url == null) ModelState.AddModelError("imageFile", "Tải ảnh lên thất bại.");
            else composer.ImageUrl = url;
        }

        if (!ModelState.IsValid) return View(composer);

        db.Composers.Add(composer);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var composer = await db.Composers.FindAsync(id);
        if (composer == null) return NotFound();
        return View(composer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Composer composerForm, IFormFile? imageFile)
    {
        if (id != composerForm.Id) return BadRequest();

        string? newImageUrl = null;
        if (imageFile != null)
        {
            newImageUrl = await cloudinary.UploadImageAsync(imageFile, "composers");
            if (newImageUrl == null) ModelState.AddModelError("imageFile", "Tải ảnh lên thất bại.");
        }

        if (!ModelState.IsValid) return View(composerForm);

        var composer = await db.Composers.FindAsync(id);
        if (composer == null) return NotFound();

        composer.Name = composerForm.Name;
        composer.Biography = composerForm.Biography;

        if (newImageUrl != null)
        {
            if (composer.ImageUrl != null) await cloudinary.DeleteImageAsync(composer.ImageUrl);
            composer.ImageUrl = newImageUrl;
        }

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var composer = await db.Composers.Include(c => c.Songs).FirstOrDefaultAsync(c => c.Id == id);
        if (composer == null) return NotFound();

        if (composer.Songs.Any())
        {
            TempData["Error"] = "Không thể xóa nhạc sĩ đang có bài hát.";
            return RedirectToAction(nameof(Index));
        }

        if (composer.ImageUrl != null) await cloudinary.DeleteImageAsync(composer.ImageUrl);
        db.Composers.Remove(composer);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
