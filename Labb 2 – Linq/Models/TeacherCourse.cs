using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Labb_2___Linq.Models
{
    public class TeacherCourse
    {
        [Key]
        public int TeacherCourseId { get; set; }

        [ForeignKey("Teacher")]
       
        public int FkTeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey("Course")]
       
        public int FkCourseId { get; set; }
        public Course Course { get; set; }
    }
}
