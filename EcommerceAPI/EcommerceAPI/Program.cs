using EcommerceAPI.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Repository.IRepository;
using EcommerceAPI.Repository;
using EcommerceAPI.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

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

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(
    options =>
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
  }
);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
