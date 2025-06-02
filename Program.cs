using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Data;
using Newtonsoft.Json;
using MyHeroAcademiaApi.Profiles;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configure Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure Services
builder.Services.AddScoped<IHeroService, HeroService>();
builder.Services.AddScoped<IQuirkService, QuirkService>();
builder.Services.AddScoped<IVillainService, VillainService>();
builder.Services.AddScoped<IItemService, ItemService>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <- Esto es fundamental

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // <- Esto genera /swagger/v1/swagger.json
    app.UseSwaggerUI(); // <- Esto genera la UI en /swagger/index.html
}

app.UseSwagger(); // <- Esto genera /swagger/v1/swagger.json
app.UseSwaggerUI(); // <- Esto genera la UI en /swagger/index.html


app.UseHttpsRedirection();
app.UseStaticFiles(); // For serving uploaded images
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();