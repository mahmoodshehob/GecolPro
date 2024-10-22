using GecolPro.WebApi.UssdService;
using GecolPro.WebApi.Interfaces;
using GecolPro.WebApi.BusinessRules;

using GecolPro.Services;
using GecolPro.Services.IServices;

using GecolPro.GecolSystem;
using GecolPro.DCBSystem;

using GecolPro.Models.DCB;
using GecolPro.Models.Gecol;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthHeader>(builder.Configuration.GetSection("AuthHeaderOfDCB"));
builder.Services.Configure<AuthCred>(builder.Configuration.GetSection("AuthHeaderOfGecol"));


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
builder.Services.AddScoped<IMenus,Menus>();
builder.Services.AddScoped<IUssdProcessV1, UssdProcessV1>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
