using ZGecolPro.SmppClient.Services.IServices;
using ZGecolPro.SmppClient.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGuidService, GuidService>();

builder.Services.AddScoped<ILoggers, Loggers>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
bool enableSwagger = app.Environment.IsDevelopment() ||
                     builder.Configuration.GetValue<bool>("EnableSwaggerInProduction");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Use(async (context, next) =>
{
    var request = context.Request;
    Console.WriteLine($"Path: {request.Path}");
    Console.WriteLine($"QueryString: {request.QueryString}");
    await next();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
