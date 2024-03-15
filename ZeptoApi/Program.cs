using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Zepto Application", Version = "v1" }));


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

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7182", "https://localhost:7219")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);


app.Run();
