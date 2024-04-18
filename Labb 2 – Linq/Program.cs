using Labb_2___Linq.Data;
using Labb_2___Linq.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Labb_2___Linq.Models.Labb_2___Linq.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("School2Connection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        // H�mta databaskontexten
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                // H�mta databaskontexten
                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                // K�r SeedData-metoden f�r att fylla i databasen med testdata
                SeedData(dbContext);
            }
            catch (Exception ex)
            {
                // Hantera eventuella fel
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ett fel uppstod vid seedning av databasen.");
            }
        }

        app.Run();
    }

    public static void SeedData(ApplicationDbContext dbContext)
    {
        using (var transaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                if (!dbContext.Students.Any())
                {
                    // L�gg till studenter
                    var student1 = new Student { StudentName = "John Doe" };
                    var student2 = new Student { StudentName = "Jane Smith" };
                    var student3 = new Student { StudentName = "Anna P " };
                    var student4 = new Student { StudentName = "Moa L  " };

                    dbContext.Students.AddRange(student1, student2, student3, student4);

                    // L�gg till l�rare
                    var teacher1 = new Teacher { TeacherName = "Stina" };
                    var teacher2 = new Teacher { TeacherName = "Reidar" };
                    var teacher3 = new Teacher { TeacherName = "Tobias" };
                    var teacher4 = new Teacher { TeacherName = "Jessica" };

                    dbContext.Teachers.AddRange(teacher1, teacher2, teacher3, teacher4);

                    // L�gg till kurser
                    var course1 = new Course { CourseName = "Math" };
                    var course2 = new Course { CourseName = "Programmering2" };
                    var course3 = new Course { CourseName = "Programmering1" };

                    dbContext.Courses.AddRange(course1, course2, course3);

                    // Spara �ndringar f�r studenter, l�rare och kurser
                    dbContext.SaveChanges();

                    // L�gg till kopplingen mellan studenter och l�rare
                    student1.StudentTeacher = new List<StudentTeacher> { new StudentTeacher { Teacher = teacher2 } };
                    student2.StudentTeacher = new List<StudentTeacher> { new StudentTeacher { Teacher = teacher1 } };
                    student3.StudentTeacher = new List<StudentTeacher> { new StudentTeacher { Teacher = teacher3 } };
                    student4.StudentTeacher = new List<StudentTeacher> { new StudentTeacher { Teacher = teacher2 } };

                    // Skapa l�rare-kurser-relationer
                    teacher1.TeacherCourses = new List<TeacherCourse> { new TeacherCourse { Course = course1, FkTeacherId = teacher1.TeacherId } };
                    teacher3.TeacherCourses = new List<TeacherCourse> { new TeacherCourse { Course = course2, FkTeacherId = teacher3.TeacherId } };
                    teacher2.TeacherCourses = new List<TeacherCourse> { new TeacherCourse { Course = course3, FkTeacherId = teacher2.TeacherId } };
                    teacher4.TeacherCourses = new List<TeacherCourse> { new TeacherCourse { Course = course1, FkTeacherId = teacher4.TeacherId } };

                   // L�gg till kurser till studenternas kurslista genom StudentCourses - kopplingen
                    student1.StudentCourses = new List<StudentCourse> { new StudentCourse { Course = course3, FkStudentId = student1.StudentId } };
                    student2.StudentCourses = new List<StudentCourse> { new StudentCourse { Course = course2, FkStudentId = student2.StudentId } };
                    student3.StudentCourses = new List<StudentCourse> { new StudentCourse { Course = course1, FkStudentId = student3.StudentId } };
                    student4.StudentCourses = new List<StudentCourse> { new StudentCourse { Course = course3, FkStudentId = student3.StudentId } };

                    // Spara �ndringar f�r kopplingsobjekten
                    dbContext.SaveChanges();
                }
                transaction.Commit();  // Commit om allt g�r bra
            }
            catch (Exception ex)
            {
                transaction.Rollback();  // Rollback om n�got fel intr�ffar
                                         // Logga eller hantera undantaget
            }
        }
    }
}