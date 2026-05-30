using HelloWorld.Data;
using HelloWorld.Models;
using HelloWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

public class SingerController(ApplicationDbContext db, CloudinaryService cloudinary) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await db.Singers
            .Include(s => s.Songs.Where(song => song.Status == 1))
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var singer = await db.Singers
            .Include(s => s.Songs.Where(song => song.Status == 1))
            .ThenInclude(song => song.Composer)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (singer == null) return NotFound();
        return View(singer);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Singer singer, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            var url = await cloudinary.UploadImageAsync(imageFile, "singers");
            if (url == null) ModelState.AddModelError("imageFile", "Tải ảnh lên thất bại.");
            else singer.ImageUrl = url;
        }

        if (!ModelState.IsValid) return View(singer);

        db.Singers.Add(singer);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var singer = await db.Singers.FindAsync(id);
        if (singer == null) return NotFound();
        return View(singer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Singer singerForm, IFormFile? imageFile)
    {
        if (id != singerForm.Id) return BadRequest();

        string? newImageUrl = null;
        if (imageFile != null)
        {
            newImageUrl = await cloudinary.UploadImageAsync(imageFile, "singers");
            if (newImageUrl == null) ModelState.AddModelError("imageFile", "Tải ảnh lên thất bại.");
        }

        if (!ModelState.IsValid) return View(singerForm);

        var singer = await db.Singers.FindAsync(id);
        if (singer == null) return NotFound();

        singer.Name = singerForm.Name;
        singer.Biography = singerForm.Biography;

        if (newImageUrl != null)
        {
            if (singer.ImageUrl != null) await cloudinary.DeleteImageAsync(singer.ImageUrl);
            singer.ImageUrl = newImageUrl;
        }

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var singer = await db.Singers.Include(s => s.Songs).FirstOrDefaultAsync(s => s.Id == id);
        if (singer == null) return NotFound();

        if (singer.Songs.Any())
        {
            TempData["Error"] = "Không thể xóa ca sĩ đang có bài hát.";
            return RedirectToAction(nameof(Index));
        }

        if (singer.ImageUrl != null) await cloudinary.DeleteImageAsync(singer.ImageUrl);
        db.Singers.Remove(singer);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
