using Labb_2___Linq.Models.Labb_2___Linq.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Labb_2___Linq.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
 
        public ICollection<StudentCourse> StudentCourses { get; set; }
        
        public List<StudentTeacher> StudentTeacher { get; internal set; }
       
    }
}
