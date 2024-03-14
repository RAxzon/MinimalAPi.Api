using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentDbContext db) =>
        {
            return await db.Enrollments.ToListAsync();
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi()
        .Produces<List<Enrollment>>();

        group.MapGet("/{id}", async (int id, StudentDbContext db) =>
        {
            return await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Enrollment model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi()
        .Produces<Enrollment>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Enrollment enrollment, StudentDbContext db) =>
            {
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
                    .SetProperty(m => m.Id, enrollment.Id)
                    .SetProperty(m => m.Created, enrollment.Created)
                    .SetProperty(m => m.CreatedBy, enrollment.CreatedBy)
                    .SetProperty(m => m.Modified, enrollment.Modified)
                    .SetProperty(m => m.ModifiedBy, enrollment.ModifiedBy)
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
