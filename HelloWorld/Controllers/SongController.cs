using HelloWorld.Data;
using HelloWorld.Helpers;
using HelloWorld.Models;
using HelloWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

public class SongController(ApplicationDbContext db, CloudinaryService cloudinary) : Controller
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        var query = db.Songs
            .Where(s => s.Status == 1)
            .Include(s => s.Singer)
            .Include(s => s.Composer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(s => s.Title.Contains(searchTerm));

        ViewBag.SearchTerm = searchTerm;
        var songs = await PaginatedList<Song>.CreateAsync(query.OrderByDescending(s => s.CreatedAt), page, PageSize);
        return View(songs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var song = await db.Songs
            .Include(s => s.Singer)
            .Include(s => s.Composer)
            .FirstOrDefaultAsync(s => s.Id == id && s.Status == 1);
        if (song == null) return NotFound();
        return View(song);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(new SongViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SongViewModel vm)
    {
        if (vm.ReleaseDate.HasValue && vm.ReleaseDate.Value > DateTime.Today)
            ModelState.AddModelError("ReleaseDate", "Ngày phát hành không được lớn hơn ngày hiện tại.");

        string? thumbnailUrl = null;
        if (vm.ThumbnailFile != null)
        {
            thumbnailUrl = await cloudinary.UploadImageAsync(vm.ThumbnailFile, "songs");
            if (thumbnailUrl == null) ModelState.AddModelError("ThumbnailFile", "Tải ảnh lên thất bại.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(vm.SingerId, vm.ComposerId);
            return View(vm);
        }

        var song = new Song
        {
            Title        = vm.Title,
            Lyrics       = vm.Lyrics,
            ThumbnailUrl = thumbnailUrl,
            Mp3Link      = vm.Mp3Link,
            ReleaseDate  = vm.ReleaseDate,
            SingerId     = vm.SingerId,
            ComposerId   = vm.ComposerId,
        };

        db.Songs.Add(song);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var song = await db.Songs.FindAsync(id);
        if (song == null || song.Status == 0) return NotFound();

        var vm = new SongViewModel
        {
            Id           = song.Id,
            Title        = song.Title,
            Lyrics       = song.Lyrics,
            ThumbnailUrl = song.ThumbnailUrl,
            Mp3Link      = song.Mp3Link,
            ReleaseDate  = song.ReleaseDate,
            SingerId     = song.SingerId,
            ComposerId   = song.ComposerId,
        };

        await PopulateDropdownsAsync(vm.SingerId, vm.ComposerId);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SongViewModel vm)
    {
        if (id != vm.Id) return BadRequest();

        if (vm.ReleaseDate.HasValue && vm.ReleaseDate.Value > DateTime.Today)
            ModelState.AddModelError("ReleaseDate", "Ngày phát hành không được lớn hơn ngày hiện tại.");

        string? newThumbnailUrl = null;
        if (vm.ThumbnailFile != null)
        {
            newThumbnailUrl = await cloudinary.UploadImageAsync(vm.ThumbnailFile, "songs");
            if (newThumbnailUrl == null) ModelState.AddModelError("ThumbnailFile", "Tải ảnh lên thất bại.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(vm.SingerId, vm.ComposerId);
            return View(vm);
        }

        var song = await db.Songs.FindAsync(id);
        if (song == null) return NotFound();

        song.Title      = vm.Title;
        song.Lyrics     = vm.Lyrics;
        song.Mp3Link    = vm.Mp3Link;
        song.ReleaseDate = vm.ReleaseDate;
        song.SingerId   = vm.SingerId;
        song.ComposerId = vm.ComposerId;
        song.UpdatedAt  = DateTime.Now;

        if (newThumbnailUrl != null)
        {
            if (song.ThumbnailUrl != null) await cloudinary.DeleteImageAsync(song.ThumbnailUrl);
            song.ThumbnailUrl = newThumbnailUrl;
        }

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        var song = await db.Songs.FindAsync(id);
        if (song == null) return NotFound();
        song.Status = 0;
        await db.SaveChangesAsync();
        return Json(new { success = true });
    }

    private async Task PopulateDropdownsAsync(int? singerId = null, int? composerId = null)
    {
        ViewBag.SingerId   = new SelectList(await db.Singers.OrderBy(s => s.Name).ToListAsync(),   "Id", "Name", singerId);
        ViewBag.ComposerId = new SelectList(await db.Composers.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", composerId);
    }
}
