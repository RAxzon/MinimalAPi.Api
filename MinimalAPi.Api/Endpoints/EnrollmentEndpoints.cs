using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPi.Api.DTOs.Enrollment;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentDbContext db, IMapper mapper) =>
            {
                var enrollments = await db.Enrollments.ToListAsync();
                return mapper.Map<List<EnrollmentDto>>(enrollments);
            })
        .WithName("GetAllEnrollments")
        .WithOpenApi()
        .Produces<List<EnrollmentDto>>();

        group.MapGet("/{id}", async (int id, StudentDbContext db, IMapper mapper) =>
        {
            return await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(enrollment => enrollment.Id == id)
                is Enrollment enrollment
                    ? Results.Ok(mapper.Map<EnrollmentDto>(enrollment))
                    : Results.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi()
        .Produces<Enrollment>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (EnrollmentCreateDto enrollmentDto, StudentDbContext db, IMapper mapper) =>
            {
                var enrollment = mapper.Map<Enrollment>(enrollmentDto);
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();
                return Results.Created($"/api/Enrollment/{enrollment.Id}", enrollment);
            })
            .WithName("CreateEnrollment")
            .WithOpenApi()
            .Produces<Enrollment>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Enrollment enrollment, StudentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CourseId, enrollment.CourseId)
                    .SetProperty(m => m.StudentID, enrollment.StudentID)
                    .SetProperty(m => m.Modified, DateTime.Now)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", async (int id, StudentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi()
        .Produces<Enrollment>()
        .Produces(StatusCodes.Status404NotFound);
    }
}
