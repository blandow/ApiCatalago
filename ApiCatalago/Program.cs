using ApiCatalago.Context;
using ApiCatalago.DTO.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Filters;
using ApiCatalago.Logging;
using ApiCatalago.Models;
using ApiCatalago.RateLimitOptions;
using ApiCatalago.Repositories;
using ApiCatalago.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new ArgumentException("Secret key is Invalid");

//Add Connections string and configure DbContext here
builder.Services.AddDbContext<ApiCatalagoContext>(
    options =>
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))

    );

// Add services to the container.

builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthorization();

var rateOptions = new RateLimitingGlobalOptions();

builder.Configuration.GetSection(RateLimitingGlobalOptions._name).Bind(rateOptions);


builder.Services.AddRateLimiter(rateOpt => 
{
    rateOpt.AddFixedWindowLimiter("FixedPolicyRate", options => 
    {
        options.PermitLimit = rateOptions.PermitLimit;
        options.QueueLimit = rateOptions.QueueLimit;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.Window = TimeSpan.FromSeconds(rateOptions.Window);
    });
    rateOpt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddRateLimiter(options => {

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Host.ToString(), 
        factory: partition => new FixedWindowRateLimiterOptions {
            AutoReplenishment = rateOptions.AutoReplenishment,
            PermitLimit = rateOptions.PermitLimit,
            QueueLimit = rateOptions.QueueLimit,
            Window = TimeSpan.FromSeconds(rateOptions.Window)

        }));
});

//padrão query string
builder.Services.AddApiVersioning(o => 
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    //combina os tipos url e querystring de versionamento
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader(),
        new UrlSegmentApiVersionReader());
}).AddApiExplorer(options => 
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("superAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "felipeAdmin"));
    options.AddPolicy("userOnly", policy => policy.RequireRole("User"));
    
    //politica customizada
    options.AddPolicy("exclusivePolicyOnly", policy => {
        policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id"
        && claim.Value == "felipeAdmin") || context.User.IsInRole("superAdmin"));
    });
});

//builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApiCatalagoContext>().AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(cfg => { }, typeof(ProdutoDTOMappingProfile));

builder.Services
    .AddControllers(
    options => options.Filters.Add<ApiExceptionFilter>())
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration { logLevel = LogLevel.Information }));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalago", Version = "v1" });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer Token ",
    });

   
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddOpenApi();

var PoliticaComOrigem = "_origensComAcessoPermitido";
builder.Services.AddCors(options => options.AddPolicy(name: PoliticaComOrigem,
    policy => {
        policy.WithOrigins("https://apirequest.io").WithMethods("GET","POST");
    }));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "ApiCatalago"));
    app.UseDeveloperExceptionPage();
    app.ConfigureExceptionHandler();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRateLimiter();

app.UseCors(PoliticaComOrigem);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
