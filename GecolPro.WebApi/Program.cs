using GecolPro.WebApi.UssdService;
using GecolPro.WebApi.Interfaces;
using GecolPro.WebApi.BusinessRules;

using GecolPro.Services;
using GecolPro.Services.IServices;

using GecolPro.GecolSystem;
using GecolPro.DCBSystem;

using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;
using GecolPro.Models.SMPP;
using GecolPro.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthHeader>(builder.Configuration.GetSection("AuthHeaderOfDCB"));
builder.Services.Configure<AuthCred>(builder.Configuration.GetSection("AuthHeaderOfGecol"));
builder.Services.Configure<SmppInfo>(builder.Configuration.GetSection("SmmpInfo"));
builder.Services.Configure<DbApiConnection>(builder.Configuration.GetSection("DbApiConnection"));


builder.Services.AddScoped<IGecolCreateResponse , GecolPro.GecolSystem.XmlServices>();
builder.Services.AddScoped<IGecolCreateXml      , GecolPro.GecolSystem.XmlServices>();

builder.Services.AddScoped<IDcbCreateResponse   , GecolPro.DCBSystem.XmlServices>();
builder.Services.AddScoped<IDcbCreateXml        , GecolPro.DCBSystem.XmlServices>();


builder.Services.AddScoped<IDcbServices, DcbServices>();
builder.Services.AddScoped<IGecolServices, GecolServices>();


builder.Services.AddScoped<IUssdConverter, UssdConverter>();
builder.Services.AddScoped<IResponses, Responses>();
builder.Services.AddScoped<ILoggers,Loggers>();
builder.Services.AddScoped<ISendMessage, SendMessage>();
builder.Services.AddScoped<IBlackListFun,BlackListFun>();
builder.Services.AddScoped<IMenusX,MenusX>();
builder.Services.AddScoped<IUssdProcess, UssdProcess>();

builder.Services.AddScoped<IDatabaseAPIs, DatabaseAPIs>();





var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//else
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

app.MapHealthChecks("/healthz");


app.Run();
