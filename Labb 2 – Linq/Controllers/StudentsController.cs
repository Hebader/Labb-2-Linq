using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb_2___Linq.Data;
using Labb_2___Linq.Models;
using NuGet.DependencyResolver;
using Microsoft.AspNetCore.Mvc.Rendering;
using Labb_2___Linq.Models.Labb_2___Linq.Models;

namespace Labb_2___Linq.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Hämta alla elever med deras lärare
            var studentsWithTeachers = await _context.StudentTeachers
                .Include(st => st.Student)
                .Include(st => st.Teacher)
                .ToListAsync();

            return View(studentsWithTeachers);
        }


        public async Task<IActionResult> Programming1Students()
        {
            // Hämta alla elever som läser "Programmering1" och inkludera deras kurser och lärare
            var students = await _context.Students
                .Include(s => s.StudentTeacher)
                    .ThenInclude(st => st.Teacher)
                .Where(s => s.StudentCourses.Any(sc => sc.Course.CourseName == "Programmering1"))
                .ToListAsync();

            return View(students);
        }

        [HttpPost] // Lägg till attributet för att indikera att metoden ska hantera POST-förfrågningar
        public async Task<IActionResult> UpdateTeacher(int studentId, int teacherId, Student student)
        {
            // Hitta studenten med det angivna studentId
            var existingStudent = await _context.Students
                .Include(s => s.StudentTeacher)
                .SingleOrDefaultAsync(s => s.StudentId == studentId);

            if (existingStudent == null)
            {
                return NotFound(); // Returnera 404 om studenten inte hittades
            }

            // Hitta läraren med det angivna teacherId
            var teacher = await _context.Teachers.FindAsync(teacherId);

            if (teacher == null)
            {
                return NotFound(); // Returnera 404 om läraren inte hittades
            }

            // Uppdatera studentens lärare
            var studentTeacher = existingStudent.StudentTeacher.FirstOrDefault(); // Välj den första StudentTeacher, eller använd en annan logik för att välja vilken du vill uppdatera
            if (studentTeacher != null)
            {
                studentTeacher.Teacher = teacher;
            }
            else
            {
                // Om det inte finns någon befintlig StudentTeacher, skapa en ny
                studentTeacher = new StudentTeacher { Teacher = teacher };
                existingStudent.StudentTeacher.Add(studentTeacher);
            }

            // Spara ändringarna till databasen
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Omdirigera till studentens index efter uppdatering
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProgramming1Teacher(int studentId)
        {
            // Hämta studenten
            var student = await _context.Students.FindAsync(studentId);

            if (student == null)
            {
                return NotFound(); // Returnera 404 om studenten inte hittades
            }

            // Hämta lärarna "Reidar" och "Tobias"
            var oldTeacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherName == "Reidar");
            var newTeacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherName == "Tobias");

            if (oldTeacher == null || newTeacher == null)
            {
                return NotFound(); // Returnera 404 om någon av lärarna inte hittades
            }

            // Hämta kursen "Programmering1"
            var programming1Course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseName == "Programmering1");

            if (programming1Course == null)
            {
                return NotFound(); // Returnera 404 om kursen inte hittades
            }

            // Hämta StudentTeacher-posten för den aktuella studenten och kursen "Programmering1"
            var studentTeacherToUpdate = await _context.StudentTeachers
                .FirstOrDefaultAsync(st => st.FkStudentId == studentId && st.FkTeacherId == oldTeacher.TeacherId);

            if (studentTeacherToUpdate != null)
            {
                // Uppdatera läraren till "Tobias"
                studentTeacherToUpdate.FkTeacherId = newTeacher.TeacherId;

                // Spara ändringarna till databasen
                await _context.SaveChangesAsync();
            }
            else
            {
                // Studenten har ingen lärare "Reidar" för kursen "Programmering1"
                // Lägg till lämplig hantering här
            }

            return RedirectToAction(nameof(Index)); // Omdirigera till studentens index efter uppdatering
        }



    }

}