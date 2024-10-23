using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNetEnv;
using CoinMarketCap.Providers;
using CoinMarketCap.Services;
using CoinMarketCap.Factories;
using CoinMarketCap.Interfaces;
using CoinMarketCap.Repositories;
using FluentValidation;
using CoinMarketCap.Helpers;
using CoinMarketCap.Dtos;
using CoinMarketCap.Validators;
using log4net.Config;
using CoinMarketCap.Middlewares;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Interfaces.Providers;
using CoinMarketCap.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

Env.Load();

var jwtSettings = new
{
    Key = Environment.GetEnvironmentVariable("JWT_KEY"),
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
    ExpireMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES"))
};
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add services to the container.
builder.Services.AddControllers();

//services
builder.Services.AddScoped<ICoinMarketCapProvider, CoinMarketCapProvider>();
builder.Services.AddScoped<ICoinMarketCapService, CoinMarketCapService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

//repositories
builder.Services.AddScoped<ICoinMarketCapRepository, CoinMarketCapRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();


//DbConnection
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
          new NpgsqlConnectionFactory(Environment.GetEnvironmentVariable("CONNECTION_STRING")));

//Validators
builder.Services.AddScoped<IValidator<UserDto>, UserValidatorFL>();
builder.Services.AddScoped<UserValidator>();

// Add All FluentValidators
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

//Other
builder.Services.AddHttpClient();
builder.Services.AddScoped<FunctionsHelper>();
builder.Services.AddScoped<Comment>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Currency API",
        Description = "Final qualifying work(CoinMarketCap integration)",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "¬ведите токен в формате: Bearer {your JWT token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});


builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<LanguageMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
