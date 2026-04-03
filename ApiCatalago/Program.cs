using ApiCatalago.Context;
using ApiCatalago.DTO.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Filters;
using ApiCatalago.Logging;
using ApiCatalago.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Add Connections string and configure DbContext here
builder.Services.AddDbContext<ApiCatalagoContext>(
    options =>
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))

    );

// Add services to the container.

builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(cfg => { },typeof(ProdutoDTOMappingProfile));

builder.Services
    .AddControllers(
    options => options.Filters.Add<ApiExceptionFilter>())
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration { logLevel = LogLevel.Information }));

builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Weather API"));
    app.UseDeveloperExceptionPage();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
