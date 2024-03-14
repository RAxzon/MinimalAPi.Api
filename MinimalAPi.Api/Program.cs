using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.DataContext;
using MinimalAPi.Api.Endpoints;

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


app.MapStudentEndpoints();
app.MapEnrollmentEndpoints();

app.MapCourseEndpoints();

app.Run();
