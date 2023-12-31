using Microsoft.EntityFrameworkCore;
using minimum_api_tutorial.Data;
using minimum_api_tutorial.model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<SuperHero>> GetAllSuperHeroes(DataContext context) =>
    await context.SuperHeroes.ToListAsync(); 

app.MapGet("/superhero", async (DataContext context) => await context.SuperHeroes.ToListAsync());
app.MapGet("/superhero/{id}", async (DataContext context, int id) =>
    await context.SuperHeroes.FindAsync(id) is SuperHero hero ?
    Results.Ok(hero) :
    Results.NotFound("Sorry!, hero not found"));

app.MapPost("/superhero", async (DataContext context, SuperHero hero) =>
{
    context.SuperHeroes.Add(hero);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllSuperHeroes(context));

});



app.Run();

