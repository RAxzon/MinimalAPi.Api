using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.DataContext;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("SchoolDbConnection");
builder.Services.AddDbContext<StudentDbContext>(opt => {
    opt.UseSqlServer(conn);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapGet("/api/courses", async (StudentDbContext context) => {
    return await context.Courses.ToListAsync();
});

app.MapGet("/api/courses/{id}", async (StudentDbContext context, int id) =>
{
    return await context.Courses
    .FindAsync(id) is Course course ? Results.Ok() : Results.NotFound();
});

app.MapPost("/api/courses", async (StudentDbContext context, Course course) =>
{
    await context.AddAsync(course);
    await context.SaveChangesAsync();

    return Results.Created($"/api/courses/{course.Id}", course);
});

app.MapPut("/api/courses/{id}", async (StudentDbContext context, Course course, int id) =>
{
    var record = await context.Courses.FindAsync(id);

    if(record == null) return Results.NotFound();

    record.Title = course.Title;
    record.Modified = DateTime.Now;
    record.Credits = course.Credits;

    context.Update(record);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/courses/{id}", async (StudentDbContext context, int id) =>
{
    var record = await context.Courses.FindAsync(id);

    if (record == null) return Results.NotFound();

    context.Remove(record);
    await context.SaveChangesAsync();

    return Results.NoContent();

});

app.Run();
