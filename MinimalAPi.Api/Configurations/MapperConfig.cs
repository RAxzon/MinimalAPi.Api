using AutoMapper;
using MinimalAPi.Api.DTOs.Course;
using MinimalAPi.Api.DTOs.Enrollment;
using MinimalAPi.Api.DTOs.Student;
using Models.Data;

namespace MinimalAPi.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();

            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentCreateDto>().ReverseMap();

            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, EnrollmentCreateDto>().ReverseMap();
        }
    }
}
