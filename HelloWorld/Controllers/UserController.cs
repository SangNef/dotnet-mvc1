using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelloWorld.Data;
using HelloWorld.Helpers;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(UserFilterViewModel filter, int? pageIndex)
        {
            var query = _context.Users.AsQueryable();

            // 1. Lọc theo từ khóa (Tên hoặc Email)
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(u => u.FullName.Contains(filter.Keyword) || u.Email.Contains(filter.Keyword));
            }

            // 2. Lọc theo thời gian (Daterange Picker)
            if (!string.IsNullOrEmpty(filter.DateRange))
            {
                var dates = filter.DateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    var startDate = DateTime.ParseExact(dates[0].Trim(), "dd/MM/yyyy", null);
                    var endDate = DateTime.ParseExact(dates[1].Trim(), "dd/MM/yyyy", null).AddDays(1);
                    query = query.Where(u => u.CreatedAt >= startDate && u.CreatedAt < endDate);
                }
            }

            // 3. Sắp xếp
            query = filter.SortOrder switch
            {
                "name_desc"  => query.OrderByDescending(u => u.FullName),
                "email_asc"  => query.OrderBy(u => u.Email),
                "email_desc" => query.OrderByDescending(u => u.Email),
                "date_asc"   => query.OrderBy(u => u.CreatedAt),
                "date_desc"  => query.OrderByDescending(u => u.CreatedAt),
                _            => query.OrderBy(u => u.FullName),
            };

            // 4. Thực thi Phân trang
            int pageSize = 10; // Số lượng bản ghi trên mỗi trang
            filter.Users = await PaginatedList<User>.CreateAsync(query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(filter);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Role,CreatedAt")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Role,CreatedAt")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken] // Nên bổ sung để bảo mật (Cần gửi kèm Token nếu dùng AJAX phức tạp hơn)
        public async Task<IActionResult> DeleteSelected(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { success = false, message = "Không có tài khoản nào được chọn." });
            }

            try
            {
                // Lấy danh sách các User có Id nằm trong list gửi lên
                var usersToDelete = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        
                if (usersToDelete.Any())
                {
                    // Xóa cứng (Hard Delete)
                    _context.Users.RemoveRange(usersToDelete);
            
                    // Hoặc Xóa mềm (nếu bảng có trường Status)
                    /*
                    foreach(var user in usersToDelete) {
                        user.Status = 0;
                    }
                    */
            
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = $"Đã xóa thành công {usersToDelete.Count} tài khoản." });
                }

                return Json(new { success = false, message = "Không tìm thấy dữ liệu phù hợp." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}
