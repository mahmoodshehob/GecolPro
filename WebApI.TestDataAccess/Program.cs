using ClassLibrary.DataAccess;
using ClassLibrary.DataAccess.Interfaces;
using ClassLibrary.DataAccess.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMeterService, MeterService>();
builder.Services.AddScoped<IRequestService, RequestService>();




var app = builder.Build();






app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
