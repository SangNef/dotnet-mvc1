using HelloWorld.Data;
using HelloWorld.Models;
using HelloWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

public class CourseController(ApplicationDbContext db, CloudinaryService cloudinary) : Controller
{
    public async Task<IActionResult> Index()
    {
        var courses = await db.Courses
            .Where(c => c.Status == 1)
            .Include(c => c.Category)
            .ToListAsync();
        return View(courses);
    }

    public async Task<IActionResult> Details(int id)
    {
        var course = await db.Courses
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == id && c.Status == 1);
        if (course == null) return NotFound();
        return View(course);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            var url = await cloudinary.UploadImageAsync(imageFile);
            if (url == null) ModelState.AddModelError("ImageUrl", "Tải ảnh lên thất bại.");
            else course.ImageUrl = url;
        }

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(course.CategoryId);
            return View(course);
        }

        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course == null || course.Status == 0) return NotFound();
        await PopulateCategoriesAsync(course.CategoryId);
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course, IFormFile? imageFile)
    {
        if (id != course.Id) return BadRequest();

        if (imageFile != null)
        {
            var existing = await db.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existing?.ImageUrl != null)
                await cloudinary.DeleteImageAsync(existing.ImageUrl);

            var url = await cloudinary.UploadImageAsync(imageFile);
            if (url == null) ModelState.AddModelError("ImageUrl", "Tải ảnh lên thất bại.");
            else course.ImageUrl = url;
        }

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(course.CategoryId);
            return View(course);
        }

        db.Courses.Update(course);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var course = await db.Courses
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == id && c.Status == 1);
        if (course == null) return NotFound();
        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course != null)
        {
            course.Status = 0;
            await db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(int? selectedId = null)
    {
        var categories = await db.Categories.ToListAsync();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name", selectedId);
    }
}
