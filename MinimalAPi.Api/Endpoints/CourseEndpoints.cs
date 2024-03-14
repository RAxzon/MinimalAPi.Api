using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Course").WithTags(nameof(Course));

        group.MapGet("/", async (StudentDbContext db) =>
        {
            return await db.Courses.ToListAsync();
        })
        .WithName("GetAllCourses")
        .WithOpenApi()
        .Produces<List<Course>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, StudentDbContext db) =>
        {
            return await db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Course model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetCourseById")
        .WithOpenApi()
        .Produces<Course>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Course course, StudentDbContext db) =>
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return Results.Created($"/api/Course/{course.Id}", course);
            })
            .WithName("CreateCourse")
            .WithOpenApi()
            .Produces<Course>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Course course, StudentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, course.Title)
                    .SetProperty(m => m.Credits, course.Credits)
                    .SetProperty(m => m.Id, course.Id)
                    .SetProperty(m => m.Created, course.Created)
                    .SetProperty(m => m.CreatedBy, course.CreatedBy)
                    .SetProperty(m => m.Modified, course.Modified)
                    .SetProperty(m => m.ModifiedBy, course.ModifiedBy)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateCourse")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", async (int id, StudentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi()
        .Produces<Course>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
