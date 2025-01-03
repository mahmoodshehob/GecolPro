using GecolPro.SmppClient.Services;
using GecolPro.SmppClient.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<HttpClient>();
builder.Services.AddTransient<IGuidService, GuidService>();

builder.Services.AddScoped<ILoggers,Loggers>();
builder.Services.AddScoped<IServiceLogic, ServiceLogic>();




//register mq in DI
builder.Services.AddHostedService<MessageWorker>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



// Configure the HTTP request pipeline.
bool enableSwagger = app.Environment.IsDevelopment() ||
                     builder.Configuration.GetValue<bool>("EnableSwaggerInProduction");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
