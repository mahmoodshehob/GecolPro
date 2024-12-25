using Microsoft.EntityFrameworkCore;

//using GecolPro.WebApi.UssdService;
//using GecolPro.WebApi.Interfaces;
//using GecolPro.WebApi.BusinessRules;


using GecolPro.BusinessRules.UssdService;
using GecolPro.BusinessRules.Interfaces;
using GecolPro.BusinessRules.BusinessRules;


using GecolPro.DataAccess;
using GecolPro.DataAccess.Interfaces;
using GecolPro.DataAccess.Services;



using GecolPro.Services;
using GecolPro.Services.IServices;

using GecolPro.GecolSystem;
using GecolPro.DCBSystem;

using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;
using GecolPro.Models.SMPP;
using GecolPro.Models.Models;


using System.Globalization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthHeader>(builder.Configuration.GetSection("AuthHeaderOfDCB"));
builder.Services.Configure<AuthCred>(builder.Configuration.GetSection("AuthHeaderOfGecol"));
builder.Services.Configure<SmppInfo>(builder.Configuration.GetSection("SmmpInfo"));
builder.Services.Configure<DbApiConnection>(builder.Configuration.GetSection("DbApiConnection"));
builder.Services.Configure<MappingPKgs>(builder.Configuration.GetSection("MappingPKgs"));





//builder.Services.AddRateLimiter(options =>
//{
//    options.AddPolicy("192.168.75.170", httpContext =>
//        RateLimitPartition.GetFixedWindowLimiter(
//            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
//            factory: _ => new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 10,
//                Window = TimeSpan.FromMinutes(1)
//            }));
//});


// Database Interface

builder.Services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();


// Gecol Interface

builder.Services.AddScoped<IGecolCreateResponse , GecolPro.GecolSystem.XmlServices>();
builder.Services.AddScoped<IGecolCreateXml , GecolPro.GecolSystem.XmlServices>();
builder.Services.AddScoped<IGecolServices, GecolServices>();


// DCB Interface 

builder.Services.AddScoped<IDcbCreateResponse, GecolPro.DCBSystem.XmlServices>();
builder.Services.AddScoped<IDcbCreateXml, GecolPro.DCBSystem.XmlServices>();
builder.Services.AddScoped<IDcbServices, DcbServices>();



builder.Services.AddScoped<IUssdConverter, UssdConverter>();
builder.Services.AddScoped<IResponses, Responses>();
builder.Services.AddScoped<ISendMessage, SendMessage>();
builder.Services.AddScoped<IBlackListFun,BlackListFun>();
builder.Services.AddScoped<IMenusX,MenusX>();

builder.Services.AddScoped<ILoggers, Loggers>();
builder.Services.AddScoped<IUssdProcess, UssdProcess>();





var app = builder.Build();


// Configure the HTTP request pipeline.
bool enableSwagger = app.Environment.IsDevelopment() ||
                     builder.Configuration.GetValue<bool>("EnableSwaggerInProduction");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");


app.Run();
