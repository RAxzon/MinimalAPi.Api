using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPi.Api.DTOs.Student;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", async (StudentDbContext db, IMapper mapper) =>
        {
            var students = await db.Students.ToListAsync();
            return mapper.Map<List<StudentDto>>(students);
        })
        .WithName("GetAllStudents")
        .WithOpenApi()
        .Produces<List<Student>>();

        group.MapGet("/{id}", async (int id, StudentDbContext db, IMapper mapper) =>
        {
            return await db.Students.AsNoTracking()
                .FirstOrDefaultAsync(student => student.Id == id)
                is Student student
                    ? Results.Ok(mapper.Map<StudentDto>(student))
                    : Results.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi()
        .Produces<StudentDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (StudentCreateDto studentDto, StudentDbContext db, IMapper mapper) =>
            {
                var student = mapper.Map<Student>(studentDto);
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
                return Results.Created($"/api/Student/{student.Id}", student);
            })
            .WithName("CreateStudent")
            .WithOpenApi()
            .Produces<Student>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, StudentCreateDto student, StudentDbContext db) =>
        {
            var affected = await db.Students
                .Where(student => student.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FirstName, student.FirstName)
                    .SetProperty(m => m.LastName, student.LastName)
                    .SetProperty(m => m.DateOfBirth, student.DateOfBirth)
                    .SetProperty(m => m.IdNumber, student.IdNumber)
                    .SetProperty(m => m.Picture, student.Picture)
                    .SetProperty(m => m.Modified, DateTime.Now)
                    );
            return affected == 1 ? Results.Ok(student) : Results.NotFound();
        })
        .WithName("UpdateStudent")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", async (int id, StudentDbContext db) =>
        {
            var affected = await db.Students
                .Where(student => student.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok($"Successfully deleted student with id: {id}") : Results.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi()
        .Produces<Student>()
        .Produces(StatusCodes.Status404NotFound);
    }
}
