using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Persistence;
using ECommerceAPI.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using ECommerceAPI.Infrastructure.Storages;
using ECommerceAPI.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using Serilog.Context;
using ECommerceAPI.API.ColumnWriters;
using Microsoft.AspNetCore.HttpLogging;
using ECommerceAPI.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddStorage<LocalStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

//Loglama iþlemi
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSql"), "logs", needAutoCreateTable: true, columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        {"message", new RenderedMessageColumnWriter()},
        {"message_template", new MessageTemplateColumnWriter()},
        {"level", new LevelColumnWriter()},
        {"time_stamp", new TimestampColumnWriter()},
        {"exception", new ExceptionColumnWriter()},
        {"log_event", new LogEventSerializedColumnWriter()},
        {"user_name", new UsernameColumnWriter()}
    })
    //seq üzerinde görselleþtirmek için
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});


builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<AddProductValidator>();
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //token deðerini kullanacaklar
            ValidateIssuer = true, //token deðerini daðýtan kim
            ValidateLifetime = true, //tokenýn süresi
            ValidateIssuerSigningKey = true, //tokenýn benim uygulamama ait olduðunu belirtir

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SignInKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameter) => expires != null ? expires > DateTime.UtcNow : false,

            //hangi kullanýcý istek yapýyor
            NameClaimType = ClaimTypes.Name,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
//wwrootu kullanabilmek için
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

//extension olarak oluþturuldu
app.ConfigureUse();

app.MapControllers();

app.Run();
