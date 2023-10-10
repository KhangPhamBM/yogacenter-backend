using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Data;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Mapping;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;
using YogaCenter.BackEnd.Service.Implementations;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(p => p.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));
builder.Services.AddDbContext<YogaCenterContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DB"));
});
IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
