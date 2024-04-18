using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb_2___Linq.Data;
using Labb_2___Linq.Models;

namespace Labb_2___Linq.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teachers/Index
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                              .Include(t => t.TeacherCourses)
                              .Where(t => t.TeacherCourses.Any(tc => tc.Course.CourseName == "Programmering1"))
                              .ToListAsync();
            return View(teachers);
        }

    }
}
