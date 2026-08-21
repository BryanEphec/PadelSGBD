using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Repositories;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services;
using Padel.SGBD.Api.Services.Interfaces;
using Scalar.AspNetCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration des Services & DB Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PadelSGBDContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

// Configuration CORS pour Angular / Front externe
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Injection des Dépendances (Repositories & Services)
builder.Services.AddScoped<IParticipationRepository, ParticipationRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMembreRepository, MembreRepository>();
builder.Services.AddScoped<IParticipationService, ParticipationService>();
builder.Services.AddScoped<IMatchService, MatchService>();

var app = builder.Build();

// 2. Pipeline des Middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Service des fichiers statiques (wwwroot/index.html)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "frontend")),
    RequestPath = ""
});

// CORS doit être placé AVANT les routes
app.UseCors("AllowAngular");

// 3. Routing & Endpoints
app.MapControllers();

// Redirection racine vers l'interface Joueur (ou Scalar selon préférence)
app.MapGet("/", async context =>
{
    context.Response.Redirect("/index.html");
    await Task.CompletedTask;
});

app.Run();