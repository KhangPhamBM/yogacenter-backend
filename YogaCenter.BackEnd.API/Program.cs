using Microsoft.EntityFrameworkCore;
using YogaCenter.BackEnd.DAL.Data;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(p => p.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    builder.WithOrigins("http://localhost:3001").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    builder.WithOrigins("https://mon-shop-fe.vercel.app").AllowAnyMethod().AllowAnyHeader().AllowCredentials();



}));
builder.Services.AddDbContext<YogaCenterContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DB"));
});
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
