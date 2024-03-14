using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", async (StudentDbContext db) =>
        {
            return await db.Students.ToListAsync();
        })
        .WithName("GetAllStudents")
        .WithOpenApi()
        .Produces<List<Student>>();

        group.MapGet("/{id}", async (int id, StudentDbContext db) =>
        {
            return await db.Students.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Student model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi()
        .Produces<Student>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Student student, StudentDbContext db) =>
            {
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
                return Results.Created($"/api/Student/{student.Id}", student);
            })
            .WithName("CreateStudent")
            .WithOpenApi()
            .Produces<Student>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Student student, StudentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FirstName, student.FirstName)
                    .SetProperty(m => m.LastName, student.LastName)
                    .SetProperty(m => m.DateOfBirth, student.DateOfBirth)
                    .SetProperty(m => m.IdNumber, student.IdNumber)
                    .SetProperty(m => m.Picture, student.Picture)
                    .SetProperty(m => m.Id, student.Id)
                    .SetProperty(m => m.Created, student.Created)
                    .SetProperty(m => m.CreatedBy, student.CreatedBy)
                    .SetProperty(m => m.Modified, student.Modified)
                    .SetProperty(m => m.ModifiedBy, student.ModifiedBy)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateStudent")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", async (int id, StudentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi()
        .Produces<Student>()
        .Produces(StatusCodes.Status404NotFound);
    }
}
