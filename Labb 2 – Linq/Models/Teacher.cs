using Labb_2___Linq.Models.Labb_2___Linq.Models;

namespace Labb_2___Linq.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
      
        public ICollection<StudentTeacher> StudentCourses { get; set; }
        public ICollection<TeacherCourse> TeacherCourses { get; set; }


    }
}
