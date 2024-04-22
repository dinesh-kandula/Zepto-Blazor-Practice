using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Repository;
using System.IdentityModel.Tokens.Jwt;
using ModelsClassLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = CreateTokenValidationParameters();
    }
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zepto Application", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddDbContext<ZeptoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"),
    b => b.MigrationsAssembly("ZeptoApi"))
   );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7182", "https://localhost:7219")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);

app.Run();



static TokenValidationParameters CreateTokenValidationParameters()
{
    var result = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt;
        },
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

    };
    result.RequireSignedTokens = false;
    return result;
}