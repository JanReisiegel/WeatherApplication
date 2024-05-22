using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Weather.Models;
using Weather.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<WeatherServices>();
builder.Services.AddScoped<LocationServices>();
builder.Services.AddScoped<LocationTransformation>();

builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("corsdef", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
    else
    {
        options.AddPolicy("corsdef", policy =>
        {
            policy.WithOrigins("https://JanReisiegel.github.io")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("corsdef");

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
