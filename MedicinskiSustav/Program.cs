using MedicinskiSustav.Automapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Repositories;
using Microsoft.EntityFrameworkCore;
using Minio.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MedicinskiDbContext>(options => 
    options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("conStr")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddMinio(options =>
{
    options.Endpoint = builder.Configuration["Minio:Endpoint"];
    options.AccessKey = builder.Configuration["Minio:AccessKey"];
    options.SecretKey = builder.Configuration["Minio:SecretKey"];
});

// Add services to the container.
builder.Services.AddControllersWithViews();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
