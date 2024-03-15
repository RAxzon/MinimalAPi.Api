using MinimalAPi.Api.DTOs.Course;
using MinimalAPi.Api.DTOs.Student;

namespace MinimalAPi.Api.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public int CourseId { get; set; }
        public int StudentID { get; set; }
        public virtual CourseDto Course { get; set; }
        public virtual StudentDto Student { get; set; }
    }
}
