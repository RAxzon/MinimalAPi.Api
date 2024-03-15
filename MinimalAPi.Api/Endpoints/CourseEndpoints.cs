using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPi.Api.DTOs.Course;
using Models.Data;
using Models.Data.DataContext;
namespace MinimalAPi.Api.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Course").WithTags(nameof(Course));

        group.MapGet("/", async (StudentDbContext db, IMapper mapper) =>
        {
            var courses = await db.Courses.ToListAsync();

            return mapper.Map<List<CourseDto>>(courses);
        })
        .WithName("GetAllCourses")
        .WithOpenApi()
        .Produces<List<CourseDto>>();

        group.MapGet("/{id}", async (int id, StudentDbContext db, IMapper mapper) =>
        {
            return await db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(course => course.Id == id)
                is Course course
                    ? Results.Ok(mapper.Map<CourseDto>(course))
                    : Results.NotFound();
        })
        .WithName("GetCourseById")
        .WithOpenApi()
        .Produces<CourseDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CourseCreateDto courseDto, StudentDbContext db, IMapper mapper) =>
            {
                var course = mapper.Map<Course>(courseDto);
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return Results.Created($"/api/Course/{course.Id}", course);
            })
            .WithName("CreateCourse")
            .WithOpenApi()
            .Produces<Course>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, CourseCreateDto course, StudentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(course => course.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, course.Title)
                    .SetProperty(m => m.Credits, course.Credits)
                    .SetProperty(m => m.Modified, DateTime.Now)
                    );
            return affected == 1 ? Results.Ok(course) : Results.NotFound();
        })
        .WithName("UpdateCourse")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", async (int id, StudentDbContext db) =>
        {
            var affected = await db.Courses
                .Where(course => course.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok($"Successfully deleted course with id: {id}") : Results.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi()
        .Produces<Course>()
        .Produces(StatusCodes.Status404NotFound);
    }
}
