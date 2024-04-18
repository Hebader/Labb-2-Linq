using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb_2___Linq.Models
{
    public class StudentTeacher
    {
        [Key]
        public int StudentTeacherId { get; set; }

        [ForeignKey("Student")]
        public int FkStudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Teacher")]
        public int FkTeacherId { get; set; }
        public Teacher Teacher { get; set; }

    }
}
