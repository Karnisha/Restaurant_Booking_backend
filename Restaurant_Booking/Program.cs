using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Restaurant_Booking.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificationOrigin",
        builder => builder.WithOrigins("http://localhost:3000/")
        .AllowAnyHeader()
         .AllowAnyOrigin()
       .AllowAnyMethod());
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Restaurant_BookingDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("connStr"), new MySqlServerVersion(new Version())));
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
  app.UseSwagger();
  app.UseSwaggerUI();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "images")),
    RequestPath = "/wwwroot/images"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowSpecificationOrigin");
app.Run();
