using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Repositories;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services;
using Padel.SGBD.Api.Services.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PadelSGBDContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers(); 
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") // Le port de ton Angular
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMembreRepository, MembreRepository>();

builder.Services.AddScoped<IMatchService, MatchService>();
var app = builder.Build();

// 2. Middleware (Configuration du pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 3. Routes
app.MapControllers(); 

// Création automatique de la base de données au démarrage
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PadelSGBDContext>();
    // Cette ligne vérifie si la DB existe, sinon elle la crée
    context.Database.EnsureCreated();
}

app.MapGet("/", async context =>
{
    context.Response.Redirect("/scalar/v1");
    await Task.CompletedTask;
});
app.UseCors("AllowAngular");
app.Run();
