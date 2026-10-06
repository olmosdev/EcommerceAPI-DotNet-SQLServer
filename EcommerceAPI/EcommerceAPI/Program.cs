using EcommerceAPI.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Repository.IRepository;
using EcommerceAPI.Repository;
using EcommerceAPI.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Environment Variables
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("SecretKey is not configured");
}

// Add services to the container.
// Add DbContext with SQL Server (Entity Framework Core)
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));
// AutoMapper
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(config => { }, typeof(Program).Assembly);

// Caching
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024;
    options.UseCaseSensitivePaths = true;
});

//Authentication Service for JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Disabling HTTPS
    options.SaveToken = true; // Saving the token in the authentication context
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validating that the token is signed with a valid key
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)), // Setting the secret key to validate the token signature.
        ValidateIssuer = false, // Not validating the token issuer
        ValidateAudience = false // There is no need to restrict it to certain clients
    };
});

// Caching Profiles
builder.Services.AddControllers( option =>
{
    // Caching Profiles
    // option.CacheProfiles.Add("Default10", new CacheProfile()
    // {
    //     Duration = 10
    // });
    // option.CacheProfiles.Add("Default20", new CacheProfile()
    // {
    //     Duration = 20
    // });
    option.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
    option.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);
} );

// Swagger/OpenAPI configuration
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Our API uses JWT authentication with the Bearer scheme.. \n\r\n\r" +
                      "Enter the token generated during login below..\n\r\n\r" +
                      "Example: \"12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ecommerce API",
        Description = "API for managing products and users",
        TermsOfService = new Uri("https://olmosdev.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "olmosdev",
            Url = new Uri("https://olmosdev.com")
        },
        License = new OpenApiLicense
        {
            Name = "Use License",
            Url = new Uri("https://olmosdev.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Ecommerce API V2",
        Description = "API for managing products and users",
        TermsOfService = new Uri("https://olmosdev.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "olmosdev",
            Url = new Uri("https://olmosdev.com")
        },
        License = new OpenApiLicense
        {
            Name = "Use License",
            Url = new Uri("https://olmosdev.com/license")
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(PolicyNames.AllowSpecificOrigin, builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

// API Versioning
var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true; // To view available API versions
    // option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version")); //?api-version
});
apiVersioningBuilder.AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV"; // v1, v2, v3...
    option.SubstituteApiVersionInUrl = true; // api/v{version}/products
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    } );
}

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
